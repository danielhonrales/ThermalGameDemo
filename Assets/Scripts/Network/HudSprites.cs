using System.Collections.Generic;
using UnityEngine;

/// <summary>Procedurally generated, anti-aliased UI sprites (rounded panels, glows, stripes, vignette).</summary>
public static class HudSprites
{
    private static readonly Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => cache.Clear();

    private delegate float AlphaAt(float x, float y, int size);

    private static Sprite Make(string key, int size, AlphaAt alpha, Vector4 border,
        TextureWrapMode wrap = TextureWrapMode.Clamp)
    {
        if (cache.TryGetValue(key, out Sprite sprite) && sprite != null) return sprite;
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, true)
        {
            name = "HUD " + key, wrapMode = wrap, filterMode = FilterMode.Trilinear, anisoLevel = 4
        };
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                pixels[y * size + x] = new Color32(255, 255, 255,
                    (byte)Mathf.RoundToInt(Mathf.Clamp01(alpha(x + 0.5f, y + 0.5f, size)) * 255f));
        texture.SetPixels32(pixels);
        texture.Apply(true, true);
        sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, border);
        sprite.name = key;
        cache[key] = sprite;
        return sprite;
    }

    private static float RoundedBoxDistance(float x, float y, int size, float radius)
    {
        float half = size * 0.5f;
        float qx = Mathf.Abs(x - half) - (half - radius);
        float qy = Mathf.Abs(y - half) - (half - radius);
        float outside = new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f)).magnitude;
        return outside + Mathf.Min(Mathf.Max(qx, qy), 0f) - radius;
    }

    /// <summary>Filled rounded rectangle, 9-sliced.</summary>
    public static Sprite Panel(int radius = 18) => Make("panel" + radius, 64,
        (x, y, s) => 0.5f - RoundedBoxDistance(x, y, s, radius), new Vector4(radius + 2, radius + 2, radius + 2, radius + 2));

    /// <summary>Rounded outline only, 9-sliced.</summary>
    public static Sprite Outline(int radius = 18, float width = 2f) => Make("outline" + radius + "_" + width, 64,
        (x, y, s) =>
        {
            float d = RoundedBoxDistance(x, y, s, radius);
            return Mathf.Clamp01(0.5f - d) * Mathf.Clamp01(d + width + 0.5f);
        }, new Vector4(radius + 2, radius + 2, radius + 2, radius + 2));

    /// <summary>Soft glow halo around a rounded box, 9-sliced; place behind panels.</summary>
    public static Sprite Glow(int radius = 18) => Make("glow" + radius, 96,
        (x, y, s) =>
        {
            float d = RoundedBoxDistance(x, y, s, radius + 16);
            float t = Mathf.Clamp01(-d / 16f);
            return t * t * (3f - 2f * t);
        }, new Vector4(radius + 20, radius + 20, radius + 20, radius + 20));

    /// <summary>Radial soft dot.</summary>
    public static Sprite Dot() => Make("dot", 128, (x, y, s) =>
    {
        float d = new Vector2(x - s * 0.5f, y - s * 0.5f).magnitude / (s * 0.5f);
        return Mathf.Pow(Mathf.Clamp01(1f - d), 2.2f);
    }, Vector4.zero);

    /// <summary>Diagonal hazard stripes, tiles horizontally.</summary>
    public static Sprite Stripes() => Make("stripes", 64, (x, y, s) =>
    {
        float v = Mathf.Repeat((x + y) / s * 2f, 1f);
        return Mathf.Clamp01((Mathf.Abs(v - 0.5f) - 0.22f) * 24f);
    }, Vector4.zero, TextureWrapMode.Repeat);

    /// <summary>Full-view edge vignette: clear centre, opaque edges.</summary>
    public static Sprite Vignette() => Make("vignette", 256, (x, y, s) =>
    {
        float u = (x / s - 0.5f) * 2f, v = (y / s - 0.5f) * 2f;
        float r = Mathf.Sqrt(u * u * 0.8f + v * v);
        return Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.55f, 1.25f, r));
    }, Vector4.zero);

    /// <summary>Horizontal fade (opaque left, transparent right) for banners.</summary>
    public static Sprite FadeBand() => Make("fadeband", 128, (x, y, s) =>
    {
        float u = Mathf.Abs(x / s - 0.5f) * 2f;
        float v = Mathf.Abs(y / s - 0.5f) * 2f;
        return Mathf.Clamp01(1f - u * u) * Mathf.Clamp01((1f - v) * 6f);
    }, Vector4.zero);
}
