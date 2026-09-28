using System.Collections.Generic;
using UnityEngine;

/// <summary>Shared burn cells remove real geometry and collision, not just a decal.</summary>
public sealed class BurnableWood : MonoBehaviour
{
    public const int Columns = 10, Rows = 12, CellCount = Columns * Rows, BurnThrough = 15;
    public const float Width = 0.9f, Height = 1.25f;
    public int PanelId;
    private readonly byte[] heat = new byte[CellCount];
    private Mesh mesh;
    private MeshCollider hitbox;
    private Material[] materials;
    private Texture2D grain;
    private float nextReport, nextFlame;

    private void Awake()
    {
        var preview = transform.Find("Wood preview");
        if (preview != null) { preview.gameObject.SetActive(false); Dispose(preview.gameObject); }
        foreach (var box in GetComponents<BoxCollider>()) Dispose(box);
        grain = new Texture2D(128, 128, TextureFormat.RGB24, false);
        var pixels = new Color[128 * 128];
        for (int y = 0; y < 128; y++) for (int x = 0; x < 128; x++)
        {
            float stripe = 0.78f + 0.12f * Mathf.PerlinNoise(x * 0.32f, y * 0.018f)
                + 0.025f * Mathf.Sin(x * 0.7f + Mathf.Sin(y * 0.04f));
            pixels[y * 128 + x] = new Color(stripe * 0.65f, stripe * 0.73f, stripe * 0.69f);
        }
        grain.SetPixels(pixels); grain.Apply();
        materials = new Material[5];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            materials[i].SetTexture("_BaseMap", grain);
            materials[i].SetColor("_BaseColor", Color.Lerp(Color.white, new Color(0.09f, 0.045f, 0.018f), i / 4f));
            materials[i].SetFloat("_Smoothness", 0.18f);
            materials[i].SetFloat("_Cull", 0f);
        }
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>().sharedMaterials = materials;
        hitbox = gameObject.AddComponent<MeshCollider>();
        Rebuild();
    }

    public static void SpreadHeat(IList<byte> heat, int offset, int cell)
    {
        for (int dy = -2; dy <= 2; dy++) for (int dx = -2; dx <= 2; dx++)
        {
            int x = cell % Columns + dx, y = cell / Columns + dy;
            float radius = Mathf.Sqrt(dx * dx + dy * dy);
            if (x < 0 || x >= Columns || y < 0 || y >= Rows || radius > 2.5f) continue;
            int target = offset + y * Columns + x;
            heat[target] = (byte)Mathf.Min(BurnThrough, heat[target] + (radius < 1.5f ? 3 : 2));
        }
    }

    public static bool CellExists(int cell)
    {
        int x = cell % Columns, y = cell / Columns;
        return !(y >= Rows - 2 && (x == 0 || x == Columns - 1));
    }

    public static int CellAt(Vector3 local)
    {
        int x = Mathf.FloorToInt((local.x + Width * 0.5f) / Width * Columns);
        int y = Mathf.FloorToInt(local.y / Height * Rows);
        return x < 0 || x >= Columns || y < 0 || y >= Rows ? -1 : y * Columns + x;
    }

    public void ReportBurn(Vector3 point)
    {
        if (Time.time < nextReport) return;
        int cell = CellAt(transform.InverseTransformPoint(point));
        if (cell < 0) return;
        nextReport = Time.time + 0.1f;
        Mirror.NetworkClient.localPlayer?.GetComponent<FusionRoundDirector>()?.RequestWoodBurn(PanelId, cell);
    }

    private void Update()
    {
        var round = FusionRoundDirector.Active();
        if (round == null || round.WoodHeat.Count != CellCount * 2) return;
        bool changed = false;
        for (int i = 0; i < CellCount; i++)
        {
            byte value = round.WoodHeat[PanelId * CellCount + i];
            if (heat[i] != value) { heat[i] = value; changed = true; }
        }
        if (changed)
        {
            Rebuild();
            if (Time.time >= nextFlame)
            {
                Vector3 center = Vector3.zero; int count = 0;
                for (int i = 0; i < CellCount; i++)
                    if (heat[i] > 0 && heat[i] < BurnThrough)
                    {
                        center += new Vector3((i % Columns + 0.5f) * Width / Columns - Width / 2f,
                            (i / Columns + 0.5f) * Height / Rows, 0f); count++;
                    }
                if (count > 0)
                    ThermalFxLibrary.Spawn(ThermalFxLibrary.Instance?.fireMedium, transform.TransformPoint(center / count), 0.35f, 0.7f);
                nextFlame = Time.time + 0.25f;
            }
        }
    }

    // Five batches for progressive char; removed cells leave holes through the entire board.
    private void Rebuild()
    {
        var cube = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        var groups = new List<CombineInstance>[5];
        for (int i = 0; i < groups.Length; i++) groups[i] = new List<CombineInstance>();
        for (int i = 0; i < CellCount; i++)
        {
            if (heat[i] >= BurnThrough || !CellExists(i)) continue;
            int x = i % Columns, y = i / Columns;
            Vector3 position = new Vector3((x + 0.5f) * Width / Columns - Width / 2f, (y + 0.5f) * Height / Rows, 0.05f * Mathf.Pow((x - 4.5f) / 4.5f, 2f));
            groups[Mathf.Min(4, heat[i] / 3)].Add(new CombineInstance { mesh = cube,
                transform = Matrix4x4.TRS(position, Quaternion.identity, new Vector3(Width / Columns, Height / Rows, 0.075f)) });
        }
        var combined = new CombineInstance[5];
        for (int i = 0; i < 5; i++)
        {
            var part = new Mesh();
            if (groups[i].Count > 0) part.CombineMeshes(groups[i].ToArray(), true, true);
            else { part.vertices = new Vector3[0]; part.subMeshCount = 1; part.SetTriangles(new int[0], 0); }
            combined[i] = new CombineInstance { mesh = part, transform = Matrix4x4.identity };
        }
        var next = new Mesh { name = "Burn-through wood" }; next.CombineMeshes(combined, false, false);
        var vertices = next.vertices;
        var uv = new Vector2[vertices.Length];
        for (int i = 0; i < uv.Length; i++) uv[i] = new Vector2((vertices[i].x + Width / 2f) / Width, vertices[i].y / Height);
        next.uv = uv;
        hitbox.sharedMesh = null;
        GetComponent<MeshFilter>().sharedMesh = next;
        hitbox.sharedMesh = next.vertexCount > 0 ? next : null;
        if (mesh != null) Dispose(mesh);
        mesh = next;
        foreach (var part in combined) Dispose(part.mesh);
    }

    private static void Dispose(Object value)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) { DestroyImmediate(value); return; }
#endif
        Destroy(value);
    }

    private void OnDestroy()
    {
        if (mesh != null) Dispose(mesh);
        if (grain != null) Dispose(grain);
        if (materials != null) foreach (var material in materials) Dispose(material);
    }
}
