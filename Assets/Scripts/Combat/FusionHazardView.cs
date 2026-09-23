using UnityEngine;
using UnityEngine.Rendering;

/// <summary>Large readable warning disc followed by a short, animated fire field.</summary>
[DisallowMultipleComponent]
public sealed class FusionHazardView : MonoBehaviour
{
    private const int FlameCount = 28;
    private readonly LineRenderer[] flames = new LineRenderer[FlameCount];
    private LineRenderer outerRing;
    private LineRenderer hotRing;
    private LineRenderer sweepRing;
    private Transform fill;
    private TMPro.TextMeshProUGUI warningText;
    private RectTransform warningRoot;
    private Material lineMaterial;
    private Material fillMaterial;
    private Mesh discMesh;
    private AudioSource audioSource;
    private AudioClip warningClip;
    private AudioClip blastClip;
    private Vector3 canonicalCenter;
    private float radius;
    private int lastStage = -1;

    private void Awake()
    {
        lineMaterial = CombatVfxStyle.CreateMaterial("Danger ring", Color.white);
        fillMaterial = CombatVfxStyle.CreateMaterial("Danger fill", new Color(1f, 0.2f, 0.08f, 0.46f));
        outerRing = CombatVfxStyle.CreateLine(transform, "Outer warning ring", lineMaterial, true, 0.055f);
        hotRing = CombatVfxStyle.CreateLine(transform, "Inner warning ring", lineMaterial, true, 0.026f);
        sweepRing = CombatVfxStyle.CreateLine(transform, "Warning sweep", lineMaterial, true, 0.018f);
        for (int i = 0; i < FlameCount; i++)
            flames[i] = CombatVfxStyle.CreateLine(transform, "Animated fire tongue", lineMaterial, true,
                i % 3 == 0 ? 0.07f : 0.045f);

        GameObject disc = new GameObject("Filling danger disc");
        disc.transform.SetParent(transform, false);
        discMesh = MakeDisc();
        disc.AddComponent<MeshFilter>().sharedMesh = discMesh;
        MeshRenderer renderer = disc.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = fillMaterial;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        fill = disc.transform;
        fill.localRotation = Quaternion.Euler(90f, 0f, 0f);

        warningRoot = DemoHudElements.Canvas("Hazard warning", transform, new Vector2(560f, 130f));
        warningText = DemoHudElements.Text(warningRoot, "MOVE warning", Vector2.zero,
            new Vector2(550f, 125f), 80f, TMPro.TextAlignmentOptions.Center, CombatVfxStyle.Heat);
        warningText.text = "MOVE";

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 0.5f;
        audioSource.maxDistance = 9f;
        warningClip = Resources.Load<AudioClip>("CustomAssets/Audio/beep");
        blastClip = Resources.Load<AudioClip>("CustomAssets/Audio/FireBlast");
        gameObject.SetActive(false);
    }

    public void Begin(Vector3 center, float newRadius)
    {
        canonicalCenter = center;
        radius = newRadius;
        lastStage = -1;
    }

    public void Show(bool visible, int stage, float remaining, float warningSeconds, float burnSeconds)
    {
        if (gameObject.activeSelf != visible) gameObject.SetActive(visible);
        if (!visible) return;

        Vector3 world = NetworkPlayerAlignment.HasCalibration
            ? NetworkPlayerAlignment.TransformPoint(canonicalCenter) : canonicalCenter;
        transform.position = world + Vector3.up * 0.035f;
        if (stage != lastStage)
        {
            lastStage = stage;
            AudioClip clip = stage == 1 ? warningClip : blastClip;
            if (clip != null) CombatAudioVoice.Play(audioSource, clip, stage == 1 ? 0.8f : 1f, stage == 1 ? 0.2f : 1.1f);
        }

        float progress = stage == 1 ? 1f - Mathf.Clamp01(remaining / warningSeconds) : 1f;
        float pulse = 1f + 0.035f * Mathf.Sin(Time.time * (stage == 1 ? 14f : 23f));
        CombatVfxStyle.SetRing(outerRing, transform.position + Vector3.up * 0.01f,
            Quaternion.Euler(90f, 0f, 0f), radius * pulse, 72);
        outerRing.startColor = outerRing.endColor = stage == 1
            ? new Color(1f, 0.13f, 0.08f, 0.95f) : new Color(1f, 0.35f, 0.09f, 1f);
        CombatVfxStyle.SetRing(hotRing, transform.position + Vector3.up * 0.04f,
            Quaternion.Euler(90f, 0f, 0f), radius * (stage == 1 ? progress : 0.8f), 64);
        hotRing.startColor = hotRing.endColor = new Color(1f, 0.68f, 0.32f, 0.9f);
        CombatVfxStyle.SetRing(sweepRing, transform.position + Vector3.up * 0.055f,
            Quaternion.Euler(90f, 0f, 0f), radius * 0.94f, 20,
            Time.time * 110f, 50f);
        sweepRing.startColor = new Color(1f, 0.95f, 0.7f, 1f);
        sweepRing.endColor = new Color(1f, 0.2f, 0.08f, 0f);

        fill.localPosition = new Vector3(0f, 0.006f, 0f);
        fill.localScale = Vector3.one * Mathf.Max(0.001f, radius * progress);
        warningRoot.gameObject.SetActive(stage == 1);
        if (stage == 1)
        {
            warningRoot.position = transform.position + Vector3.up * 0.65f;
            Camera camera = Camera.main;
            if (camera != null)
                warningRoot.rotation = Quaternion.LookRotation(
                    warningRoot.position - camera.transform.position, Vector3.up);
            warningText.color = Color.Lerp(new Color(1f, 0.24f, 0.14f, 1f),
                Color.white, Mathf.Pow(progress, 3f) * 0.45f);
        }

        for (int i = 0; i < FlameCount; i++)
        {
            LineRenderer flame = flames[i];
            flame.enabled = stage == 2;
            if (stage != 2) continue;
            float angle = i * Mathf.PI * 2f / FlameCount;
            float time = Time.time * (4f + i % 4) + i * 1.74f;
            float radial = radius * (0.22f + 0.67f * Mathf.Repeat(i * 0.618034f, 1f));
            Vector3 basePoint = transform.position + new Vector3(
                Mathf.Cos(angle) * radial, 0.02f, Mathf.Sin(angle) * radial);
            float height = 0.5f + 0.46f * Mathf.Sin(time) * Mathf.Sin(time);
            Vector3 drift = new Vector3(Mathf.Sin(time * 0.7f), 0f, Mathf.Cos(time * 0.5f)) * 0.13f;
            flame.positionCount = 3;
            flame.SetPosition(0, basePoint);
            flame.SetPosition(1, basePoint + drift * 0.5f + Vector3.up * height * 0.5f);
            flame.SetPosition(2, basePoint + drift + Vector3.up * height);
            flame.startColor = new Color(1f, i % 3 == 0 ? 0.17f : 0.47f, 0.05f, 0.9f);
            flame.endColor = new Color(1f, 0.8f, 0.26f, 0f);
        }
    }

    private static Mesh MakeDisc()
    {
        const int segments = 64;
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            float a = i * Mathf.PI * 2f / segments;
            vertices[i + 1] = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f);
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 1) % segments + 1;
        }
        Mesh mesh = new Mesh { name = "Danger fill disc", vertices = vertices, triangles = triangles };
        mesh.RecalculateBounds();
        return mesh;
    }

    private void OnDestroy()
    {
        if (lineMaterial != null) Destroy(lineMaterial);
        if (fillMaterial != null) Destroy(fillMaterial);
        if (discMesh != null) Destroy(discMesh);
    }
}
