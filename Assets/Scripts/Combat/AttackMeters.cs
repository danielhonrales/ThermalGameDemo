using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Two small bars just up the forearm from the attack hand's wrist, always shown while the hand is
/// tracked: fire and ice. Each is full when ready, drains while its attack is used (the beam fires,
/// the bomb charges) and refills, greyed, through the cooldown, with a flash when it is ready again.
/// </summary>
[DisallowMultipleComponent]
public sealed class AttackMeters : MonoBehaviour
{
    // Canvas units are millimetres.
    private const float Scale = 0.001f;
    private static readonly Vector2 BoardSize = new Vector2(132f, 40f);
    private static readonly Vector2 BarSize = new Vector2(90f, 9f);
    private const float BarLeft = -32f;
    // Metres from the wrist: back along the forearm, up, and to the viewer's right, so the bars sit
    // above the forearm and the hand never covers them.
    private const float TowardElbow = 0.06f, Above = 0.095f, Right = 0.05f;

    private sealed class Bar
    {
        public Color color;
        public Image fill, glow;
        public TextMeshProUGUI label;
        public float shown = 1f, last = 1f, readyAt = -10f;
    }

    private PalmBeamShooter beam;
    private IceGrenadeLauncher ice;
    private HandPoseRouter pose;
    private RectTransform board;
    private CanvasGroup group;
    private Bar fire, cold;
    private bool placed;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        if (SpectatorSession.IsSpectator) return;
        foreach (PalmBeamShooter shooter in FindObjectsByType<PalmBeamShooter>(FindObjectsSortMode.None))
            if (shooter.GetComponent<AttackMeters>() == null) shooter.gameObject.AddComponent<AttackMeters>();
    }

    private void Awake()
    {
        beam = GetComponent<PalmBeamShooter>();
        ice = GetComponent<IceGrenadeLauncher>();
        pose = GetComponent<HandPoseRouter>();
        board = HudKit.Canvas("Attack meters", null, BoardSize, 25);
        board.localScale = Vector3.one * Scale;
        group = board.gameObject.AddComponent<CanvasGroup>();
        group.alpha = 0f;
        fire = Row("FIRE", CombatVfxStyle.Heat, 9f);
        cold = Row("ICE", CombatVfxStyle.Cold, -9f);
    }

    private Bar Row(string name, Color color, float y)
    {
        var bar = new Bar { color = color };
        // Right-aligned, ending just short of the bar's track.
        bar.label = HudKit.Text(board, name + " label", HudKit.Display, 10f, color, new Vector2(BarLeft - 24f, y), new Vector2(36f, 12f),
            TextAlignmentOptions.Right);
        bar.label.text = name;
        bar.label.characterSpacing = 4f;
        HudKit.Image(board, name + " track", HudSprites.Panel(4), new Color(0.03f, 0.05f, 0.08f, 0.75f),
            new Vector2(BarLeft + BarSize.x * 0.5f, y), BarSize + new Vector2(4f, 4f), true, 1f);
        bar.glow = HudKit.Image(board, name + " glow", HudSprites.Dot(), HudKit.A(color, 0f),
            new Vector2(BarLeft, y), new Vector2(BarSize.x + 24f, BarSize.y + 22f));
        bar.glow.rectTransform.pivot = new Vector2(0f, 0.5f);
        bar.fill = HudKit.Image(board, name + " fill", HudSprites.Panel(3), color, new Vector2(BarLeft, y), BarSize, true, 1f);
        bar.fill.rectTransform.pivot = new Vector2(0f, 0.5f);
        return bar;
    }

    private void LateUpdate()
    {
        Camera eye = Camera.main;
        if (eye == null) return;
        bool tracked = pose != null && pose.TryGetForearmPose(eye.transform, out Vector3 wrist, out Quaternion arm)
            && Place(eye.transform, wrist, arm);
        group.alpha = Mathf.MoveTowards(group.alpha, tracked ? 1f : 0f, Time.deltaTime * 6f);
        Show(fire, beam != null ? beam.Meter : 1f, beam != null && beam.IsRecharging);
        Show(cold, ice != null ? ice.Meter : 1f, ice != null && ice.IsRecharging);
    }

    private bool Place(Transform head, Vector3 wrist, Quaternion arm)
    {
        Vector3 right = Vector3.ProjectOnPlane(head.right, Vector3.up).normalized;
        Vector3 target = wrist - arm * Vector3.forward * TowardElbow + Vector3.up * Above + right * Right;
        Vector3 view = target - head.position;
        if (view.sqrMagnitude < 0.0001f) return false;
        // Turned to the eye and kept upright; the board's front faces away from the viewer.
        Quaternion facing = Quaternion.LookRotation(view, Vector3.up);
        if (!placed) { board.SetPositionAndRotation(target, facing); placed = true; return true; }
        float follow = 1f - Mathf.Exp(-Time.deltaTime * 18f);
        board.SetPositionAndRotation(Vector3.Lerp(board.position, target, follow), Quaternion.Slerp(board.rotation, facing, follow));
        return true;
    }

    private static void Show(Bar bar, float level, bool recharging)
    {
        float now = Time.time;
        // Follows the weapon's own timing; only sudden changes (a cancelled charge) are eased.
        bar.shown = Mathf.MoveTowards(bar.shown, level, Time.deltaTime * 4f);
        if (bar.last < 0.999f && level >= 0.999f) bar.readyAt = now;
        bar.last = level;
        float ready = Mathf.Exp(-(now - bar.readyAt) * 5f);
        bar.fill.rectTransform.sizeDelta = new Vector2(BarSize.x * bar.shown, BarSize.y);
        Color colour = recharging ? HudKit.A(Color.Lerp(CombatVfxStyle.Neutral, bar.color, 0.25f), 0.75f) : bar.color;
        bar.fill.color = Color.Lerp(colour, Color.white, 0.6f * ready);
        bar.glow.rectTransform.sizeDelta = new Vector2(BarSize.x * bar.shown + 24f, BarSize.y + 22f);
        bar.glow.color = HudKit.A(bar.color, recharging ? 0.05f : 0.22f + 0.5f * ready);
        bar.label.color = recharging ? HudKit.A(Color.Lerp(CombatVfxStyle.Neutral, bar.color, 0.3f), 0.8f) : bar.color;
    }

    private void OnDestroy()
    {
        if (board != null) Destroy(board.gameObject);
    }
}
