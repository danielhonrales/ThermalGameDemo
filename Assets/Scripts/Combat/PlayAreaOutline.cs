using UnityEngine;

/// <summary>
/// A neon outline on the floor around the real play space. It sits under ArenaRoot, so every
/// headset sees it in the same place once calibrated, and the spectator PC draws it too. Its centre
/// is this transform and its height the floor; cover and spawn points should fit inside
/// (Thermal → Arena → List Objects Outside Play Area).
/// </summary>
[DisallowMultipleComponent]
public sealed class PlayAreaOutline : MonoBehaviour
{
    [Tooltip("Metres along this transform's x and z.")]
    [SerializeField] private Vector2 size = new Vector2(3f, 3f);
    [SerializeField] private Color color = new Color(0.72f, 1f, 0.16f, 1f);
    [SerializeField, Min(0.005f)] private float lineWidth = 0.025f;
    [SerializeField, Min(0f)] private float glowWidth = 0.12f;
    [Tooltip("Lifts the line just off the floor so it doesn't flicker into it.")]
    [SerializeField] private float heightAboveFloor = 0.01f;

    private Material material;

    public Vector2 Size => size;

    /// <summary>Whether a point in this transform's space is over the play area, at any height.</summary>
    public bool Contains(Vector3 local) => Mathf.Abs(local.x) <= size.x * 0.5f && Mathf.Abs(local.z) <= size.y * 0.5f;

    private void Awake()
    {
        material = CombatVfxStyle.CreateMaterial("Play area outline", Color.white);
        CreateLine("Play area glow", glowWidth, new Color(color.r, color.g, color.b, 0.22f));
        CreateLine("Play area outline", lineWidth, color);
    }

    private void CreateLine(string name, float width, Color tint)
    {
        if (width <= 0f) return;
        LineRenderer line = CombatVfxStyle.CreateLine(transform, name, material, false, width);
        // Lie flat on the floor instead of turning to face the viewer: the child's z points up, so
        // its point (x, y, h) is (x, h, -y) here.
        line.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        line.alignment = LineAlignment.TransformZ;
        line.loop = true;
        line.startColor = line.endColor = tint;
        float x = size.x * 0.5f, z = size.y * 0.5f;
        line.positionCount = 4;
        line.SetPositions(new[]
        {
            new Vector3(-x, -z, heightAboveFloor), new Vector3(x, -z, heightAboveFloor),
            new Vector3(x, z, heightAboveFloor), new Vector3(-x, z, heightAboveFloor),
        });
        line.enabled = true;
    }

    private void OnDestroy()
    {
        if (material != null) Destroy(material);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.up * heightAboveFloor, new Vector3(size.x, 0f, size.y));
    }
}
