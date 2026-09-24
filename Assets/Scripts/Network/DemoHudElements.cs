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

/// <summary>Compact floating health bar shown over the opponent: chip trail, hit flash and low-health pulse.</summary>
public sealed class DemoHealthBar
{
    public RectTransform Root { get; }
    private readonly UnityEngine.UI.Image fill, chip, glow;
    private readonly TextMeshProUGUI number;
    private float shown = 1f, trail = 1f, droppedAt = -10f;
    private int lastHp = -1;
    private const float Width = 300f;

    public DemoHealthBar(Transform parent, string name)
    {
        Root = DemoHudElements.Canvas(name, parent, new Vector2(320f, 76f));
        glow = HudKit.Image(Root, "Bloom", HudSprites.Dot(), HudKit.A(FusionRoundHud.Enemy, 0.2f), new Vector2(0f, -8f), new Vector2(Width + 60f, 56f));
        HudKit.Image(Root, "Track", HudSprites.Panel(4), new Color(1f, 1f, 1f, 0.12f), new Vector2(0f, -8f), new Vector2(Width, 3f), true, 1f);
        chip = Bar("Chip", new Color(1f, 0.93f, 0.8f, 0.9f));
        fill = Bar("Fill", FusionRoundHud.Enemy);
        number = HudKit.Text(Root, "Health value", HudKit.Heavy, 30f, Color.white, new Vector2(0f, 20f), new Vector2(300f, 40f), TextAlignmentOptions.Center);
    }

    private UnityEngine.UI.Image Bar(string name, Color color)
    {
        var image = HudKit.Image(Root, name, HudSprites.Panel(6), color, new Vector2(-Width / 2f, -8f), new Vector2(Width, 10f), true, 1f);
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
        number.text = Mathf.Max(0, hp).ToString();
        number.rectTransform.localScale = Vector3.one * (1f + 0.25f * flash);
        fill.color = Color.Lerp(FusionRoundHud.Enemy, Color.white, 0.7f * flash);
        fill.rectTransform.sizeDelta = new Vector2(Width * shown, 10f);
        chip.rectTransform.sizeDelta = new Vector2(Width * trail, 10f);
        float low = fraction < 0.3f ? 0.5f + 0.5f * Mathf.Sin(Time.time * 9f) : 0f;
        glow.color = HudKit.A(FusionRoundHud.Enemy, 0.1f + 0.4f * flash + 0.15f * low);
    }
}
