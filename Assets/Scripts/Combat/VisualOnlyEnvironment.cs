using System.Collections;
using System.Collections.Generic;
using Meta.XR.EnvironmentDepth;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class VisualOnlyEnvironment : MonoBehaviour
{
    [SerializeField] private bool applyLayerToChildren = true;
    [SerializeField] private bool disableColliders = true;

    [Header("Mixed Reality Playfield")]
    [SerializeField] private bool cutOutPassthroughPlayfield = true;
    [SerializeField] private Transform playfieldRoot;
    [SerializeField] private Vector3 playfieldLocalCenter = new Vector3(0f, 1.1f, 0f);
    [SerializeField] private Vector3 playfieldSize = new Vector3(7f, 3.4f, 7f);
    [SerializeField] private bool useEnvironmentDepthOcclusion = true;
    [SerializeField, Range(-0.2f, 0.2f)] private float environmentDepthBias = 0f;
    [Tooltip("Meta's official Environment Depth shader. Assigned explicitly so it cannot be stripped from Quest builds.")]
    [SerializeField] private Shader environmentOcclusionShader;
    [Tooltip("Extra roots that should receive depth-occlusion materials without changing their layers, colliders, or transforms.")]
    [SerializeField] private Transform[] additionalOcclusionRoots;

    private readonly List<Material> runtimeMaterials = new List<Material>();
    private bool mixedRealityMaterialsApplied;
    private string activeOcclusionShaderName;

    private static readonly int PlayfieldWorldToLocalId = Shader.PropertyToID("_MRPlayfieldWorldToLocal");
    private static readonly int PlayfieldHalfExtentsId = Shader.PropertyToID("_MRPlayfieldHalfExtents");
    private static readonly int PlayfieldCutoutEnabledId = Shader.PropertyToID("_MREnablePlayfieldCutout");

    private void Reset()
    {
        ApplySetup();
    }

    private void Awake()
    {
        ApplySetup();
        ApplyMixedRealityMaterials();
        UpdatePlayfieldShaderGlobals();
        StartCoroutine(LogOcclusionStatus());
    }

    private void LateUpdate()
    {
        UpdatePlayfieldShaderGlobals();
    }

    [ContextMenu("Apply Visual Only Setup")]
    public void ApplySetup()
    {
        int layer = CombatLayers.BackgroundEnvironmentLayer;
        if (layer >= 0)
        {
            if (applyLayerToChildren)
            {
                CombatLayers.SetLayerRecursively(gameObject, layer);
            }
            else
            {
                gameObject.layer = layer;
            }
        }

        if (!disableColliders)
        {
            return;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>(true);
        foreach (Collider environmentCollider in colliders)
        {
            environmentCollider.enabled = false;
        }
    }

    private void ApplyMixedRealityMaterials()
    {
        if (mixedRealityMaterialsApplied || !Application.isPlaying)
        {
            return;
        }

        Shader mixedRealityShader = environmentOcclusionShader;
        if (mixedRealityShader == null)
        {
            mixedRealityShader = Shader.Find("EnvironmentDepth/URP/OcclusionUnlit");
        }

        if (mixedRealityShader == null)
        {
            mixedRealityShader = Shader.Find("ThermalGame/MixedRealityEnvironment");
        }

        if (mixedRealityShader == null)
        {
            Debug.LogWarning("Neither Meta's reference Environment Depth shader nor the fallback MR shader was found. The environment will render with its original materials.", this);
            return;
        }

        activeOcclusionShaderName = mixedRealityShader.name;

        Dictionary<Material, Material> replacements = new Dictionary<Material, Material>();
        Renderer[] renderers = GetOcclusionRenderers();
        int replacedRendererCount = 0;
        foreach (Renderer environmentRenderer in renderers)
        {
            if (environmentRenderer is ParticleSystemRenderer)
            {
                continue;
            }

            Material[] sourceMaterials = environmentRenderer.sharedMaterials;
            Material[] replacementMaterials = new Material[sourceMaterials.Length];
            bool replacedAny = false;
            for (int i = 0; i < sourceMaterials.Length; i++)
            {
                Material source = sourceMaterials[i];
                if (source == null)
                {
                    replacementMaterials[i] = null;
                    continue;
                }

                if (!replacements.TryGetValue(source, out Material replacement))
                {
                    replacement = CreateMixedRealityMaterial(source, mixedRealityShader);
                    replacements.Add(source, replacement);
                    runtimeMaterials.Add(replacement);
                }

                replacementMaterials[i] = replacement;
                replacedAny = true;
            }

            if (replacedAny)
            {
                environmentRenderer.sharedMaterials = replacementMaterials;
                replacedRendererCount++;
            }
        }

        mixedRealityMaterialsApplied = true;
        Debug.Log(
            $"MR environment setup replaced {replacedRendererCount}/{renderers.Length} renderers and {replacements.Count} unique materials. " +
            $"Shader supported: {mixedRealityShader.isSupported}.",
            this);
    }

    private Material CreateMixedRealityMaterial(Material source, Shader shader)
    {
        Material material = new Material(shader)
        {
            name = $"{source.name} (MR Occlusion)"
        };

        string textureProperty = source.HasProperty("_BaseMap") ? "_BaseMap" : "_MainTex";
        Texture mainTexture = source.HasProperty(textureProperty) ? source.GetTexture(textureProperty) : source.mainTexture;
        Vector2 textureScale = source.HasProperty(textureProperty) ? source.GetTextureScale(textureProperty) : Vector2.one;
        Vector2 textureOffset = source.HasProperty(textureProperty) ? source.GetTextureOffset(textureProperty) : Vector2.zero;
        Color color = source.HasProperty("_BaseColor")
            ? source.GetColor("_BaseColor")
            : source.HasProperty("_Color") ? source.GetColor("_Color") : Color.white;

        material.SetTexture("_BaseMap", mainTexture);
        material.SetTextureScale("_BaseMap", textureScale);
        material.SetTextureOffset("_BaseMap", textureOffset);
        material.SetColor("_BaseColor", color);
        if (material.HasProperty("_EnvironmentDepthBias"))
        {
            material.SetFloat("_EnvironmentDepthBias", environmentDepthBias);
        }

        if (material.HasProperty("_UseEnvironmentDepth"))
        {
            material.SetFloat("_UseEnvironmentDepth", useEnvironmentDepthOcclusion ? 1f : 0f);
        }
        return material;
    }

    private void UpdatePlayfieldShaderGlobals()
    {
        if (playfieldRoot == null)
        {
            GameObject foundArenaRoot = GameObject.Find("ArenaRoot");
            playfieldRoot = foundArenaRoot != null ? foundArenaRoot.transform : transform.root;
        }

        Vector3 worldCenter = playfieldRoot.TransformPoint(playfieldLocalCenter);
        Matrix4x4 playfieldMatrix = Matrix4x4.TRS(worldCenter, playfieldRoot.rotation, Vector3.one).inverse;
        Shader.SetGlobalMatrix(PlayfieldWorldToLocalId, playfieldMatrix);
        Shader.SetGlobalVector(PlayfieldHalfExtentsId, Vector3.Max(playfieldSize * 0.5f, Vector3.one * 0.01f));
        Shader.SetGlobalFloat(PlayfieldCutoutEnabledId, cutOutPassthroughPlayfield ? 1f : 0f);
    }

    private IEnumerator LogOcclusionStatus()
    {
        yield return new WaitForSeconds(2f);

        int shaderRendererCount = 0;
        Renderer[] renderers = GetOcclusionRenderers();
        foreach (Renderer environmentRenderer in renderers)
        {
            foreach (Material material in environmentRenderer.sharedMaterials)
            {
                if (material != null && material.shader != null
                    && material.shader.name == activeOcclusionShaderName)
                {
                    shaderRendererCount++;
                    break;
                }
            }
        }

        Debug.Log(
            $"MR occlusion status: SOFT={Shader.IsKeywordEnabled(EnvironmentDepthManager.SoftOcclusionKeyword)}, " +
            $"HARD={Shader.IsKeywordEnabled(EnvironmentDepthManager.HardOcclusionKeyword)}, " +
            $"shader={activeOcclusionShaderName}, MR shader renderers={shaderRendererCount}, cutout={cutOutPassthroughPlayfield}, " +
            $"playfield size={playfieldSize}, bias={environmentDepthBias}.",
            this);
    }

    private Renderer[] GetOcclusionRenderers()
    {
        HashSet<Renderer> uniqueRenderers = new HashSet<Renderer>();
        AddRenderersFromRoot(transform, uniqueRenderers);

        if (additionalOcclusionRoots != null)
        {
            foreach (Transform additionalRoot in additionalOcclusionRoots)
            {
                AddRenderersFromRoot(additionalRoot, uniqueRenderers);
            }
        }

        Renderer[] renderers = new Renderer[uniqueRenderers.Count];
        uniqueRenderers.CopyTo(renderers);
        return renderers;
    }

    private static void AddRenderersFromRoot(Transform root, HashSet<Renderer> renderers)
    {
        if (root == null)
        {
            return;
        }

        foreach (Renderer foundRenderer in root.GetComponentsInChildren<Renderer>(true))
        {
            if (foundRenderer != null)
            {
                renderers.Add(foundRenderer);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Material runtimeMaterial in runtimeMaterials)
        {
            if (runtimeMaterial != null)
            {
                Destroy(runtimeMaterial);
            }
        }
    }
}
