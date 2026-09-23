using UnityEngine;
using UnityEngine.Rendering;

/// <summary>Shared palette and lightweight geometry for the Quest combat effects.</summary>
internal static class CombatVfxStyle
{
    internal static readonly Color Heat = new Color(1f, 0.21f, 0.13f, 1f);
    internal static readonly Color HeatCore = new Color(1f, 0.78f, 0.7f, 1f);
    internal static readonly Color Cold = new Color(0.31f, 0.79f, 0.9f, 1f);
    internal static readonly Color ColdCore = new Color(0.83f, 0.96f, 1f, 1f);
    internal static readonly Color Shield = new Color(0.65f, 0.43f, 0.85f, 1f);
    internal static readonly Color Neutral = new Color(0.67f, 0.77f, 0.82f, 1f);
    internal static readonly Color Critical = new Color(1f, 0.23f, 0.16f, 1f);

    internal static Color WithAlpha(Color color, float alpha)
    {
        // A modest shared visibility lift; zero-alpha fades still reach zero.
        color.a = Mathf.Clamp01(alpha * 1.18f);
        return color;
    }

    internal static Material CreateMaterial(string name, Color tint)
    {
        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
        {
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        }

        Material material = new Material(shader) { name = name };
        material.color = tint;
        material.renderQueue = (int)RenderQueue.Transparent;
        return material;
    }

    internal static LineRenderer CreateLine(
        Transform parent, string name, Material material, bool worldSpace, float width)
    {
        GameObject lineObject = new GameObject(name);
        lineObject.transform.SetParent(parent, false);
        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.useWorldSpace = worldSpace;
        line.alignment = LineAlignment.View;
        line.textureMode = LineTextureMode.Stretch;
        line.widthMultiplier = width;
        line.numCapVertices = 4;
        line.numCornerVertices = 3;
        line.shadowCastingMode = ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.sharedMaterial = material;
        line.enabled = false;
        return line;
    }

    internal static void SetRing(
        LineRenderer line, Vector3 center, Quaternion rotation, float radius,
        int segments = 48, float startDegrees = 0f, float sweepDegrees = 360f)
    {
        if (line == null)
        {
            return;
        }

        segments = Mathf.Max(3, segments);
        line.positionCount = segments + 1;
        for (int i = 0; i <= segments; i++)
        {
            float angle = (startDegrees + sweepDegrees * i / segments) * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            line.SetPosition(i, center + rotation * point);
        }

        line.enabled = true;
    }

    internal static void SetBezier(
        Vector3[] buffer, Vector3 start, Vector3 controlA, Vector3 controlB, Vector3 end)
    {
        int last = buffer.Length - 1;
        for (int i = 0; i <= last; i++)
        {
            float t = i / (float)last;
            float u = 1f - t;
            buffer[i] = u * u * u * start
                + 3f * u * u * t * controlA
                + 3f * u * t * t * controlB
                + t * t * t * end;
        }
    }

    internal static void SetHelix(LineRenderer line, float length, float radius,
        float turns, float phase, int segments = 36)
    {
        line.positionCount = segments + 1;
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float angle = t * turns * Mathf.PI * 2f + phase;
            float envelope = Mathf.Sin(Mathf.PI * Mathf.Lerp(0.06f, 0.94f, t));
            line.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius * envelope,
                Mathf.Sin(angle) * radius * envelope, -length * t));
        }
        line.enabled = true;
    }

    internal static Vector3 CatmullRom(
        Vector3 before, Vector3 start, Vector3 end, Vector3 after, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * ((2f * start)
            + (-before + end) * t
            + (2f * before - 5f * start + 4f * end - after) * t2
            + (-before + 3f * start - 3f * end + after) * t3);
    }

    internal static Mesh CreateHexField(float radius)
    {
        const int segments = 48;
        Vector3[] vertices = new Vector3[1 + segments * 2];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[segments * 9];
        vertices[0] = Vector3.zero;
        colors[0] = WithAlpha(Shield, 0.26f);

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float sector = Mathf.Repeat(angle + Mathf.PI / 6f, Mathf.PI / 3f) - Mathf.PI / 6f;
            float edgeRadius = radius * Mathf.Cos(Mathf.PI / 6f) / Mathf.Cos(sector);
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            vertices[1 + i] = direction * edgeRadius * 0.78f;
            vertices[1 + segments + i] = direction * edgeRadius;
            colors[1 + i] = WithAlpha(Shield, 0.42f);
            colors[1 + segments + i] = WithAlpha(Shield, 0.20f);

            int next = (i + 1) % segments;
            int triangle = i * 9;
            triangles[triangle] = 0;
            triangles[triangle + 1] = 1 + i;
            triangles[triangle + 2] = 1 + next;
            triangles[triangle + 3] = 1 + i;
            triangles[triangle + 4] = 1 + segments + i;
            triangles[triangle + 5] = 1 + segments + next;
            triangles[triangle + 6] = 1 + i;
            triangles[triangle + 7] = 1 + segments + next;
            triangles[triangle + 8] = 1 + next;
        }

        Mesh mesh = new Mesh { name = "Shield field" };
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
    }

    internal static Mesh CreateRadialField(float radius, Color color)
    {
        const int segments = 48;
        Vector3[] vertices = new Vector3[segments + 1];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[segments * 3];
        vertices[0] = Vector3.zero;
        colors[0] = WithAlpha(color, 0.32f);
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius, 0f);
            colors[i + 1] = WithAlpha(color, 0.055f);
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 1) % segments + 1;
        }

        Mesh mesh = new Mesh { name = "Energy radius field" };
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
    }

    internal static Mesh CreateShard()
    {
        Mesh mesh = new Mesh { name = "Energy shard" };
        mesh.vertices = new[]
        {
            new Vector3(0f, 0.55f, 0f), new Vector3(-0.18f, 0f, 0f),
            new Vector3(0f, 0f, 0.18f), new Vector3(0.18f, 0f, 0f),
            new Vector3(0f, 0f, -0.18f), new Vector3(0f, -0.55f, 0f)
        };
        mesh.triangles = new[]
        {
            0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1,
            5, 2, 1, 5, 3, 2, 5, 4, 3, 5, 1, 4
        };
        mesh.RecalculateNormals();
        return mesh;
    }
}
