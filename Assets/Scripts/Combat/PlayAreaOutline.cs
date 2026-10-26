using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A neon outline on the floor around the real play space. It sits under ArenaRoot, so every
/// headset sees it in the same place once calibrated, and the spectator PC draws it too. Its centre
/// is this transform and its height the floor; cover and spawn points should fit inside
/// (Thermal → Arena → List Objects Outside Play Area). It also shows while editing the scene; those
/// lines are hidden from the Hierarchy and never saved.
/// </summary>
[ExecuteAlways]
[DisallowMultipleComponent]
public sealed class PlayAreaOutline : MonoBehaviour
{
    [Tooltip("Metres along this transform's x and z.")]
    [SerializeField] private Vector2 size = new Vector2(3.5f, 3.5f);
    [SerializeField] private Color color = new Color(0.72f, 1f, 0.16f, 1f);
    [SerializeField, Min(0.005f)] private float lineWidth = 0.025f;
    [SerializeField, Min(0f)] private float glowWidth = 0.12f;
    [Tooltip("Lifts the line just off the floor so it doesn't flicker into it.")]
    [SerializeField] private float heightAboveFloor = 0.01f;

    private Material material;
    private readonly List<GameObject> lines = new List<GameObject>();

    public Vector2 Size => size;

    /// <summary>Whether a point in this transform's space is over the play area, at any height.</summary>
    public bool Contains(Vector3 local) => Mathf.Abs(local.x) <= size.x * 0.5f && Mathf.Abs(local.z) <= size.y * 0.5f;

    private void OnEnable() => Build();

    private void OnDisable() => Clear();

#if UNITY_EDITOR
    // Inspector changes redraw the lines; objects can't be created or destroyed inside OnValidate itself.
    private void OnValidate() => UnityEditor.EditorApplication.delayCall += () =>
    {
        if (this == null || !isActiveAndEnabled) return;
        Clear();
        Build();
    };
#endif

    private void Build()
    {
        if (lines.Count > 0) return;
        material = CombatVfxStyle.CreateMaterial("Play area outline", Color.white);
        material.hideFlags = HideFlags.DontSave;
        CreateLine("Play area glow", glowWidth, new Color(color.r, color.g, color.b, 0.22f));
        CreateLine("Play area outline", lineWidth, color);
    }

    private void CreateLine(string name, float width, Color tint)
    {
        if (width <= 0f) return;
        LineRenderer line = CombatVfxStyle.CreateLine(transform, name, material, false, width);
        line.gameObject.hideFlags = HideFlags.HideAndDontSave;
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
        lines.Add(line.gameObject);
    }

    private void Clear()
    {
        foreach (GameObject line in lines) Release(line);
        lines.Clear();
        Release(material);
        material = null;
    }

    private static void Release(Object target)
    {
        if (target == null) return;
        if (Application.isPlaying) Destroy(target);
        else DestroyImmediate(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.up * heightAboveFloor, new Vector3(size.x, 0f, size.y));
#if UNITY_EDITOR
        Vector3 corner = transform.TransformPoint(new Vector3(-size.x * 0.5f, heightAboveFloor, size.y * 0.5f));
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.Label(corner, $"Play area {size.x:0.##} × {size.y:0.##} m");
#endif
    }
}
