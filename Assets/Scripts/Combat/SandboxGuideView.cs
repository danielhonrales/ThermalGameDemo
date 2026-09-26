using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>World-anchored practice guide; each headset billboards it toward its own player.</summary>
[DisallowMultipleComponent]
public sealed class SandboxGuideView : MonoBehaviour
{
    private RectTransform board;
    private bool showing;
    private static readonly Color Fire = new Color(1f, 0.38f, 0.20f, 1f);
    private static readonly Color Ice = new Color(0.37f, 0.85f, 1f, 1f);
    private static readonly Color Shield = new Color(0.75f, 0.55f, 1f, 1f);

    private void Awake()
    {
        board = HudKit.Canvas("Practice ability cards", transform, new Vector2(1280f, 960f), 30);
        board.localScale = Vector3.one * 0.0011f;
        HudKit.Image(board, "Board", HudSprites.Panel(18), new Color(0.015f, 0.025f, 0.04f, 0.82f),
            Vector2.zero, new Vector2(1250f, 940f), true, 1f);
        Label(board, "PRACTICE ARENA", 56f, Color.white, new Vector2(0f, 375f), new Vector2(1150f, 90f));
        Label(board, "TRY EACH HAND POSE", 28f, FusionRoundHud.Friendly,
            new Vector2(0f, 320f), new Vector2(1100f, 55f));
        Card("FIRE BEAM", "FINGER GUN  ·  HOLD TO BURN", Fire, 180f, 0);
        Card("ICE BOMB", "OPEN PALM  ·  AUTO RECHARGE", Ice, -5f, 1);
        Card("SHIELD", "FIST  ·  HOLD TO BLOCK", Shield, -190f, 2);
        Label(board, "EITHER PLAYER: THUMBS UP FOR 3 SECONDS TO START", 28f,
            Color.white, new Vector2(0f, -390f), new Vector2(1180f, 65f));
        board.gameObject.SetActive(false);
    }

    private void Card(string title, string instruction, Color accent, float y, int kind)
    {
        var row = HudKit.Rect(board, title + " card", new Vector2(0f, y), new Vector2(1160f, 165f));
        HudKit.Image(row, "Panel", HudSprites.Panel(12), new Color(0.09f, 0.13f, 0.18f, 0.95f),
            Vector2.zero, new Vector2(1160f, 165f), true, 1f);
        HudKit.Image(row, "Accent", HudSprites.Panel(4), accent, new Vector2(-562f, 0f), new Vector2(9f, 140f), true, 1f);
        DrawHand(row, kind, accent);
        Label(row, title, 44f, accent, new Vector2(-50f, 27f), new Vector2(650f, 75f));
        Label(row, instruction, 23f, Color.white, new Vector2(-50f, -29f), new Vector2(680f, 55f));
        DrawAction(row, kind, accent);
    }

    private static TextMeshProUGUI Label(Transform parent, string text, float size, Color color,
        Vector2 position, Vector2 dimensions)
    {
        var label = HudKit.Text(parent, text, HudKit.Heavy, size, color, position, dimensions, TextAlignmentOptions.Center);
        label.text = text;
        label.characterSpacing = size < 30f ? 4f : 7f;
        return label;
    }

    private static Image Bar(Transform parent, string name, Color color, Vector2 position,
        Vector2 size, float angle = 0f)
    {
        Image image = HudKit.Image(parent, name, HudSprites.Panel(7), color, position, size, true, 1f);
        image.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        return image;
    }

    private static void DrawHand(Transform row, int kind, Color accent)
    {
        var hand = HudKit.Rect(row, "Hand pose", new Vector2(-445f, -3f), new Vector2(145f, 145f));
        Color skin = new Color(0.88f, 0.93f, 1f, 1f);
        Bar(hand, "Palm", skin, new Vector2(0f, -24f), new Vector2(72f, kind == 2 ? 65f : 74f));
        Bar(hand, "Thumb", skin, new Vector2(-47f, -12f), new Vector2(22f, 58f), 42f);
        for (int finger = 0; finger < 4; finger++)
        {
            bool extended = kind == 1 || (kind == 0 && finger == 1);
            float height = extended ? 67f : kind == 2 ? 20f : 26f;
            float x = -28f + finger * 19f;
            Bar(hand, "Finger " + finger, skin, new Vector2(x, 13f + height * 0.5f - 14f),
                new Vector2(15f, height));
        }
        if (kind == 0) Bar(hand, "Aim line", accent, new Vector2(61f, 33f), new Vector2(55f, 7f));
        if (kind == 1) HudKit.Image(hand, "Ice charge", HudSprites.Dot(), accent,
            new Vector2(0f, 62f), new Vector2(52f, 52f));
        if (kind == 2) HudKit.Image(hand, "Shield glow", HudSprites.Dot(),
            HudKit.A(accent, 0.3f), new Vector2(0f, 12f), new Vector2(140f, 115f));
    }

    private static void DrawAction(Transform row, int kind, Color accent)
    {
        var art = HudKit.Rect(row, "Ability effect", new Vector2(465f, 0f), new Vector2(165f, 130f));
        if (kind == 0)
        {
            Bar(art, "Burning beam", accent, Vector2.zero, new Vector2(135f, 14f));
            for (int i = 0; i < 3; i++)
                HudKit.Image(art, "Flame " + i, HudSprites.Dot(),
                    HudKit.A(accent, 0.8f), new Vector2(-32f + i * 43f, 20f), new Vector2(30f, 35f));
        }
        else if (kind == 1)
        {
            HudKit.Image(art, "Ice orb", HudSprites.Dot(), accent, Vector2.zero, new Vector2(82f, 82f));
            for (int i = 0; i < 4; i++)
                Bar(art, "Ice shard " + i, Color.white,
                    Quaternion.Euler(0f, 0f, i * 90f) * new Vector2(0f, 56f),
                    new Vector2(13f, 28f), i * 90f);
        }
        else
        {
            HudKit.Image(art, "Shield field", HudSprites.Panel(18), HudKit.A(accent, 0.35f),
                Vector2.zero, new Vector2(108f, 110f), true, 1f);
            Bar(art, "Shield crest", accent, new Vector2(0f, 4f), new Vector2(77f, 12f));
            Bar(art, "Shield spine", accent, new Vector2(0f, -15f), new Vector2(12f, 72f));
        }
    }

    public void Show(bool visible)
    {
        if (board == null) return;
        if (showing != visible) { showing = visible; board.gameObject.SetActive(visible); }
        if (!visible) return;
        board.position = NetworkPlayerAlignment.TransformPoint(new Vector3(0.275f, 2.12f, 0.71f));
        Camera eye = Camera.main;
        if (eye == null) return;
        Vector3 away = board.position - eye.transform.position;
        away.y = 0f;
        if (away.sqrMagnitude > 0.01f) board.rotation = Quaternion.LookRotation(away.normalized, Vector3.up);
    }

#if UNITY_EDITOR
    public void Preview(Camera eye)
    {
        if (board == null) Awake();
        Show(true);
    }
#endif

    private void OnDestroy()
    {
        if (board != null) Destroy(board.gameObject);
    }
}
