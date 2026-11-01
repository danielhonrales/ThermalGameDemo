using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The round clock, written large on four boards just outside the play area, one past each side and
/// each facing the middle, so a player sees one whichever way they face. They hang under the
/// <see cref="PlayAreaOutline"/> (inside ArenaRoot), so they line up with the arena on every headset
/// once calibrated, and the spectator's cameras show them too.
/// </summary>
public sealed class ArenaClockBoards
{
    // Metres: how far past the outline each board stands (in front of the room's pillars), and the height
    // of its middle above the floor (its top stays under the 2.45 m ceiling).
    public const float Outside = 0.28f, Height = 1.85f;
    private const float Scale = 0.001f;   // canvas units are millimetres
    private static readonly Vector2 BoardSize = new Vector2(1400f, 620f);

    private readonly List<RectTransform> boards = new List<RectTransform>();
    private readonly List<TextMeshProUGUI> times = new List<TextMeshProUGUI>();
    private readonly List<Image> accents = new List<Image>(), glows = new List<Image>();
    private string shownText;

    /// <summary>Builds the boards around the scene's play area, or returns null when it has none.</summary>
    public static ArenaClockBoards Create()
    {
        var area = Object.FindFirstObjectByType<PlayAreaOutline>();
        return area != null ? new ArenaClockBoards(area) : null;
    }

    /// <summary>Where each board's middle stands in the play area's frame, and the way its face looks (outward).</summary>
    public static IEnumerable<(Vector3 position, Vector3 outward)> Placements(Vector2 areaSize)
    {
        float x = areaSize.x * 0.5f + Outside, z = areaSize.y * 0.5f + Outside;
        yield return (new Vector3(0f, Height, z), Vector3.forward);
        yield return (new Vector3(x, Height, 0f), Vector3.right);
        yield return (new Vector3(0f, Height, -z), Vector3.back);
        yield return (new Vector3(-x, Height, 0f), Vector3.left);
    }

    private ArenaClockBoards(PlayAreaOutline area)
    {
        string[] sides = { "North", "East", "South", "West" };
        int i = 0;
        foreach (var (position, outward) in Placements(area.Size))
        {
            RectTransform board = HudKit.Canvas("Round clock " + sides[i++], area.transform, BoardSize, 0);
            // A canvas reads correctly from behind its forward axis, so it looks outward to face the middle.
            board.localPosition = position;
            board.localRotation = Quaternion.LookRotation(outward, Vector3.up);
            board.localScale = Vector3.one * Scale;
            glows.Add(HudKit.Image(board, "Bloom", HudSprites.Dot(), Color.clear, Vector2.zero, BoardSize + new Vector2(400f, 300f)));
            HudKit.Image(board, "Plate", HudSprites.Panel(18), HudKit.A(FusionRoundHud.Ink, 0.55f), Vector2.zero, BoardSize, true, 4f);
            accents.Add(HudKit.Image(board, "Underline", HudSprites.FadeBand(), Color.clear, new Vector2(0f, -250f), new Vector2(1100f, 16f)));
            TextMeshProUGUI time = HudKit.Text(board, "Time", HudKit.Heavy, 470f, Color.white, new Vector2(0f, 20f), new Vector2(1360f, 560f), TextAlignmentOptions.Center);
            time.characterSpacing = 4f;
            time.outlineWidth = 0.08f;
            times.Add(time);
            boards.Add(board);
        }
    }

    /// <summary>Shows a time (or a word) in a colour, with an accent for the glow and underline.</summary>
    public void Show(string text, Color color, Color accent, float glow)
    {
        bool changed = text != shownText;
        shownText = text;
        for (int i = 0; i < boards.Count; i++)
        {
            if (boards[i] == null) continue;
            if (changed) times[i].text = text;
            times[i].color = color;
            accents[i].color = HudKit.A(accent, 0.9f);
            glows[i].color = HudKit.A(accent, glow);
        }
    }

    public void Destroy()
    {
        foreach (RectTransform board in boards)
            if (board != null)
            {
                if (Application.isPlaying) Object.Destroy(board.gameObject);
                else Object.DestroyImmediate(board.gameObject);
            }
        boards.Clear();
    }
}
