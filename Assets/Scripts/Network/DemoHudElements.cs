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

public sealed class DemoHealthBar
{
    public RectTransform Root { get; }
    private readonly UnityEngine.UI.Image[] cells = new UnityEngine.UI.Image[12];
    private readonly TextMeshProUGUI number;

    public DemoHealthBar(Transform parent, string name)
    {
        Root = DemoHudElements.Canvas(name, parent, new Vector2(320f, 76f));
        DemoHudElements.Strip(Root, "Health backing", new Vector2(0f, 18f),
            new Vector2(344f, 70f), new Color(0.012f, 0.025f, 0.035f, 0.72f));
        DemoHudElements.Strip(Root, "Health accent", new Vector2(-171f, 18f),
            new Vector2(3f, 70f), DemoHudElements.Green);
        var label = DemoHudElements.Text(Root, "HP BAR", new Vector2(-75f, 22f),
            new Vector2(170f, 42f), 26f, TextAlignmentOptions.Left, DemoHudElements.Green);
        label.text = "HP BAR";
        label.characterSpacing = 1f;
        number = DemoHudElements.Text(Root, "Health value", new Vector2(106f, 22f),
            new Vector2(108f, 46f), 30f, TextAlignmentOptions.Right, DemoHudElements.Green);
        for (int i = 0; i < cells.Length; i++)
        {
            float x = -160f + 12f + i * (320f / cells.Length);
            DemoHudElements.Strip(Root, "Empty health segment", new Vector2(x, 0f),
                new Vector2(23f, 11f), new Color(0.025f, 0.12f, 0.065f, 0.65f));
            cells[i] = DemoHudElements.Strip(Root, "Health segment", new Vector2(x, 0f),
                new Vector2(23f, 11f), DemoHudElements.Green);
            cells[i].rectTransform.localPosition += Vector3.back * 0.3f;
        }
    }

    public void SetHealth(int hp, float fraction)
    {
        number.text = Mathf.Max(0, hp).ToString();
        for (int i = 0; i < cells.Length; i++)
        {
            float amount = Mathf.Clamp01(fraction * cells.Length - i);
            var rect = cells[i].rectTransform;
            rect.sizeDelta = new Vector2(23f * amount, 11f);
            rect.anchoredPosition = new Vector2(-160f + 12f + i * (320f / cells.Length) - 11.5f * (1f - amount), 0f);
            cells[i].enabled = amount > 0f;
        }
    }
}
