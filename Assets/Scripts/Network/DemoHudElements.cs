using TMPro;
using UnityEngine;

/// <summary>Shared, unboxed world-space typography and green health bars.</summary>
public static class DemoHudElements
{
    public static readonly Color Green = new Color(0.22f, 1f, 0.45f, 1f);
    private static TMP_FontAsset font;

    public static RectTransform Canvas(string name, Transform parent, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler));
        var rect = (RectTransform)go.transform;
        rect.SetParent(parent, false);
        rect.sizeDelta = size;
        rect.localScale = Vector3.one * 0.001f;
        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        go.GetComponent<UnityEngine.UI.CanvasScaler>().dynamicPixelsPerUnit = 2f;
        return rect;
    }

    public static TextMeshProUGUI Text(Transform parent, string name, Vector2 position,
        Vector2 size, float fontSize, TextAlignmentOptions alignment, Color color)
    {
        if (font == null) font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var text = go.AddComponent<TextMeshProUGUI>();
        text.font = font;
        text.fontSize = fontSize;
        text.fontStyle = FontStyles.Bold;
        text.alignment = alignment;
        text.enableWordWrapping = false;
        text.overflowMode = TextOverflowModes.Ellipsis;
        text.raycastTarget = false;
        text.color = color;
        text.outlineColor = new Color32(6, 13, 20, 230);
        text.outlineWidth = 0.2f;
        text.rectTransform.anchoredPosition = position;
        text.rectTransform.sizeDelta = size;
        return text;
    }

    public static UnityEngine.UI.Image Strip(Transform parent, string name, Vector2 position, Vector2 size, Color tint)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(UnityEngine.UI.Image));
        go.transform.SetParent(parent, false);
        var image = go.GetComponent<UnityEngine.UI.Image>();
        image.raycastTarget = false;
        image.color = tint;
        image.rectTransform.anchoredPosition = position;
        image.rectTransform.sizeDelta = size;
        return image;
    }
}

/// <summary>
/// Floating health bar shown over the opponent: a thick red bar on a dark, red-framed plate with the
/// number above it; chip trail, hit flash and low-health pulse. It is drawn over everything, so it stays
/// visible while the opponent is behind cover.
/// </summary>
public sealed class DemoHealthBar
{
    public RectTransform Root { get; }
    private readonly UnityEngine.UI.Image fill, chip, glow, frame;
    private readonly TextMeshProUGUI number;
    private float shown = 1f, trail = 1f, droppedAt = -10f;
    private int lastHp = -1;
    private const float Width = 300f, BarHeight = 24f, BarY = -14f;

    public DemoHealthBar(Transform parent, string name)
    {
        Root = DemoHudElements.Canvas(name, parent, new Vector2(360f, 130f));
        Color enemy = FusionRoundHud.Enemy;
        glow = HudKit.Image(Root, "Bloom", HudSprites.Dot(), HudKit.A(enemy, 0.3f), new Vector2(0f, BarY), new Vector2(Width + 140f, 120f));
        frame = HudKit.Image(Root, "Frame", HudSprites.Panel(8), enemy, new Vector2(0f, BarY), new Vector2(Width + 20f, BarHeight + 20f), true, 1f);
        HudKit.Image(Root, "Plate", HudSprites.Panel(6), new Color(0.02f, 0.03f, 0.05f, 0.88f), new Vector2(0f, BarY), new Vector2(Width + 10f, BarHeight + 10f), true, 1f);
        chip = Bar("Chip", new Color(1f, 0.93f, 0.8f, 0.9f));
        fill = Bar("Fill", enemy);
        for (int i = 1; i < 4; i++)
            HudKit.Image(Root, "Tick", null, new Color(0f, 0f, 0f, 0.45f), new Vector2(-Width / 2f + Width * i / 4f, BarY), new Vector2(3f, BarHeight));
        number = HudKit.Text(Root, "Health value", HudKit.Heavy, 46f, Color.white, new Vector2(0f, 26f), new Vector2(200f, 60f), TextAlignmentOptions.Center);
        DrawOverEverything();
    }

    // Shared by every bar: images and the number drawn without a depth test, so cover never hides them.
    private static Material overlayImage, overlayText;

    private void DrawOverEverything()
    {
        // Under the HUD banners (order 20), over the arena's other transparent effects.
        Root.GetComponent<Canvas>().sortingOrder = 18;
        if (overlayImage == null)
        {
            overlayImage = new Material(Canvas.GetDefaultCanvasMaterial()) { name = "Opponent health overlay image" };
            overlayImage.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);
        }
        foreach (UnityEngine.UI.Image image in Root.GetComponentsInChildren<UnityEngine.UI.Image>(true))
            image.material = overlayImage;
        Shader shader = Shader.Find("TextMeshPro/Distance Field Overlay");
        if (overlayText == null && shader != null && number.fontSharedMaterial != null)
            overlayText = new Material(number.fontSharedMaterial) { name = "Opponent health overlay text", shader = shader };
        if (overlayText != null) number.fontSharedMaterial = overlayText;
    }

    private UnityEngine.UI.Image Bar(string name, Color color)
    {
        var image = HudKit.Image(Root, name, HudSprites.Panel(6), color, new Vector2(-Width / 2f, BarY), new Vector2(Width, BarHeight), true, 1f);
        image.rectTransform.pivot = new Vector2(0f, 0.5f);
        return image;
    }

    public void SetHealth(int hp, float fraction)
    {
        if (lastHp >= 0 && hp < lastHp) droppedAt = Time.time;
        lastHp = hp;
        shown = Mathf.MoveTowards(shown, fraction, Time.deltaTime * 6f);
        if (Time.time - droppedAt > 0.45f) trail = Mathf.MoveTowards(trail, shown, Time.deltaTime * 0.9f);
        trail = Mathf.Max(trail, shown);
        float flash = Mathf.Exp(-(Time.time - droppedAt) * 10f);
        float low = fraction < 0.3f ? 0.5f + 0.5f * Mathf.Sin(Time.time * 9f) : 0f;
        Color enemy = FusionRoundHud.Enemy;
        number.text = Mathf.Max(0, hp).ToString();
        number.rectTransform.localScale = Vector3.one * (1f + 0.3f * flash);
        fill.color = Color.Lerp(enemy, Color.white, 0.7f * flash);
        fill.rectTransform.sizeDelta = new Vector2(Width * shown, BarHeight);
        chip.rectTransform.sizeDelta = new Vector2(Width * trail, BarHeight);
        // The frame flares on each hit and throbs when the opponent is nearly down.
        frame.color = Color.Lerp(enemy, Color.white, Mathf.Max(0.8f * flash, 0.45f * low));
        glow.color = HudKit.A(enemy, 0.3f + 0.45f * flash + 0.25f * low);
    }
}
