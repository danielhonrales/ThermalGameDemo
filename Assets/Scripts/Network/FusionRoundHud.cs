using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Head-locked match HUD: player health cards (top left), round clock with a match timeline,
/// centre banners (countdown, FIGHT, SUDDEN DEATH, result), toasts and a full-view feedback vignette.
/// </summary>
[DisallowMultipleComponent]
public sealed class FusionRoundHud : MonoBehaviour
{
    // Palette.
    internal static readonly Color Ink = new Color(0.02f, 0.03f, 0.05f, 0.78f);
    internal static readonly Color Friendly = new Color(0.25f, 0.95f, 0.85f, 1f);
    internal static readonly Color Enemy = new Color(1f, 0.28f, 0.24f, 1f);
    internal static readonly Color Warn = new Color(1f, 0.16f, 0.12f, 1f);
    internal static readonly Color Soft = new Color(0.72f, 0.8f, 0.86f, 1f);

    private const float HudDistance = 1.5f;
    private const float HudScale = 0.00095f;

    private RectTransform root;
    private RectTransform vignetteRoot;
    private Image vignette;
    private HealthCard localCard;
    private HealthCard opponentCard;
    private RectTransform clock;
    private Image clockGlow;
    private TextMeshProUGUI timer, phaseLabel;
    private Image timelineFill, timelineHead;
    private RectTransform timelineRoot;
    private RectTransform banner;
    private Image bannerBand, bannerFlash;
    private RectTransform bannerStripes;
    private CanvasGroup bannerGroup;
    private TextMeshProUGUI bannerText, bannerCaption;
    private RectTransform toast;
    private CanvasGroup toastGroup;
    private Image toastAccent;
    private TextMeshProUGUI toastText;
    private float toastUntil = -1f, toastShownAt;
    private string bannerKey = "";
    private float bannerShownAt;
    private float fightStartedAt = -10f;
    private int lastCountdownNumber = -1, lastFinalSecond = -1, lastHealState = -1, lastPhase = -1;
    private NetworkPlayerHealth localHealth, opponentHealth;
    private Quaternion smoothedRotation = Quaternion.identity;
    private float roundLength = 90f, suddenDeathAt = 60f;
    private float suddenDeathAmount;
    private float previousHit;

    public static FusionRoundHud Current { get; private set; }

    private void Awake()
    {
        Current = this;
        Initialize();
    }

    private void OnDestroy()
    {
        if (Current == this) Current = null;
        if (root != null) DestroyHudObject(root.gameObject);
        if (vignetteRoot != null) DestroyHudObject(vignetteRoot.gameObject);
    }

    private static void DestroyHudObject(GameObject value)
    {
        if (Application.isPlaying) Destroy(value); else DestroyImmediate(value);
    }

    private void Initialize()
    {
        if (root != null) return;
        root = HudKit.Canvas("Arena HUD", transform, new Vector2(2800f, 1700f), 20);

        localCard = new HealthCard(root, "YOU", Friendly, new Vector2(-1000f, 600f), true);
        opponentCard = new HealthCard(root, "OPPONENT", Enemy, new Vector2(-1022f, 478f), false);

        // Round clock with a match timeline underneath (heal at 30 s, sudden death at 60 s).
        clock = HudKit.Rect(root, "Clock", new Vector2(0f, 600f), new Vector2(260f, 100f));
        clockGlow = HudKit.Image(clock, "Bloom", HudSprites.Dot(), HudKit.A(Friendly, 0.1f), Vector2.zero, new Vector2(420f, 170f));
        timer = HudKit.Text(clock, "Time", HudKit.Heavy, 76f, Color.white, new Vector2(0f, 2f), new Vector2(260f, 100f), TextAlignmentOptions.Center);
        phaseLabel = HudKit.Text(root, "Phase", HudKit.Display, 26f, Soft, new Vector2(0f, 528f), new Vector2(600f, 40f), TextAlignmentOptions.Center);
        phaseLabel.characterSpacing = 18f;

        timelineRoot = HudKit.Rect(root, "Timeline", new Vector2(0f, 488f), new Vector2(440f, 30f));
        HudKit.Image(timelineRoot, "Track", HudSprites.Panel(4), new Color(1f, 1f, 1f, 0.12f), Vector2.zero, new Vector2(440f, 6f), true, 1f);
        timelineFill = HudKit.Image(timelineRoot, "Fill", HudSprites.Panel(4), HudKit.A(Soft, 0.85f), new Vector2(-220f, 0f), new Vector2(0f, 6f), true, 1f);
        timelineFill.rectTransform.pivot = new Vector2(0f, 0.5f);
        Marker(timelineRoot, 30f / 90f, HealPickupView.Green, "HEAL");
        Marker(timelineRoot, 60f / 90f, Warn, "SUDDEN DEATH");
        timelineHead = HudKit.Image(timelineRoot, "Head", HudSprites.Dot(), Color.white, new Vector2(-220f, 0f), new Vector2(26f, 26f));

        // Centre banner.
        banner = HudKit.Rect(root, "Banner", new Vector2(0f, 70f), new Vector2(1500f, 260f));
        bannerGroup = banner.gameObject.AddComponent<CanvasGroup>();
        bannerBand = HudKit.Image(banner, "Bloom", HudSprites.Dot(), HudKit.A(Color.black, 0f), Vector2.zero, new Vector2(1500f, 420f));
        var stripesMask = HudKit.Rect(banner, "Stripes mask", new Vector2(0f, -108f), new Vector2(700f, 6f));
        stripesMask.gameObject.AddComponent<RectMask2D>();
        bannerStripes = HudKit.Image(stripesMask, "Stripes", HudSprites.Stripes(), HudKit.A(Warn, 0.9f), Vector2.zero, new Vector2(900f, 6f)).rectTransform;
        bannerStripes.GetComponent<Image>().type = Image.Type.Tiled;
        bannerStripes.GetComponent<Image>().pixelsPerUnitMultiplier = 4f;
        bannerFlash = HudKit.Image(banner, "Flash", HudSprites.Dot(), new Color(1f, 1f, 1f, 0f), Vector2.zero, new Vector2(1300f, 360f));
        bannerText = HudKit.Text(banner, "Title", HudKit.Heavy, 150f, Color.white, new Vector2(0f, 14f), new Vector2(1500f, 300f), TextAlignmentOptions.Center);
        bannerText.characterSpacing = 4f;
        bannerCaption = HudKit.Text(banner, "Caption", HudKit.Display, 34f, Soft, new Vector2(0f, -70f), new Vector2(1200f, 50f), TextAlignmentOptions.Center);
        bannerCaption.characterSpacing = 16f;
        bannerGroup.alpha = 0f;

        // Toast under the clock.
        toast = HudKit.Rect(root, "Toast", new Vector2(0f, 400f), new Vector2(620f, 64f));
        toastGroup = toast.gameObject.AddComponent<CanvasGroup>();
        toastAccent = HudKit.Image(toast, "Underline", HudSprites.FadeBand(), Friendly, new Vector2(0f, -30f), new Vector2(420f, 4f));
        toastText = HudKit.Text(toast, "Text", HudKit.Display, 32f, Color.white, new Vector2(8f, 0f), new Vector2(580f, 60f), TextAlignmentOptions.Center);
        toastText.characterSpacing = 8f;
        toastGroup.alpha = 0f;

        // Feedback vignette sits closer than the HUD and covers the whole view.
        vignetteRoot = HudKit.Canvas("Feedback vignette", transform, new Vector2(2000f, 1700f), 10);
        vignette = HudKit.Image(vignetteRoot, "Vignette", HudSprites.Vignette(), new Color(1f, 0f, 0f, 0f), Vector2.zero, new Vector2(2000f, 1700f));
    }

    private static void Marker(RectTransform parent, float at, Color color, string label)
    {
        float x = -220f + 440f * at;
        var diamond = HudKit.Image(parent, label + " marker", HudSprites.Panel(3), color, new Vector2(x, 0f), new Vector2(12f, 12f), true, 1f);
        diamond.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f);
        var text = HudKit.Text(parent, label + " label", HudKit.Display, 15f, HudKit.A(color, 0.9f), new Vector2(x, -22f), new Vector2(300f, 26f), TextAlignmentOptions.Center);
        text.text = label;
        text.characterSpacing = 6f;
    }

    private void LateUpdate()
    {
        Camera eye = Camera.main;
        if (eye == null) return;
        Position(eye);
        UpdateVignette();
        UpdateToast();
    }

    private void Position(Camera camera)
    {
        Transform head = camera.transform;
        if (root.parent != head)
        {
            root.SetParent(head, false);
            vignetteRoot.SetParent(head, false);
            smoothedRotation = head.rotation;
        }
        // Slight rotational lag makes the HUD feel mounted in a helmet rather than glued to the eyes.
        smoothedRotation = Quaternion.Slerp(smoothedRotation, head.rotation, 1f - Mathf.Exp(-Time.deltaTime * 14f));
        if (Quaternion.Angle(smoothedRotation, head.rotation) > 25f) smoothedRotation = head.rotation;
        Quaternion lag = Quaternion.Inverse(head.rotation) * smoothedRotation;
        root.localPosition = lag * new Vector3(0f, 0f, HudDistance);
        root.localRotation = lag;
        root.localScale = Vector3.one * HudScale;
        vignetteRoot.localPosition = new Vector3(0f, 0f, 0.45f);
        vignetteRoot.localRotation = Quaternion.identity;
        vignetteRoot.localScale = Vector3.one * 0.0012f;
    }

    // ---------- Match state ----------

    public void Show(FusionRoundDirector round)
    {
        Initialize();
        roundLength = round.RoundLength;
        suddenDeathAt = round.SuddenDeathStartsAt;
        FindPlayers();
        localCard.Update(localHealth);
        opponentCard.Update(opponentHealth);

        float remaining = round.PhaseRemaining;
        var phase = round.Phase;
        if ((int)phase != lastPhase)
        {
            if (phase == FusionRoundDirector.RoundPhase.Fighting) fightStartedAt = Time.time;
            lastPhase = (int)phase;
        }

        float sd = round.SuddenDeathClock;
        suddenDeathAmount = Mathf.MoveTowards(suddenDeathAmount,
            round.IsFighting && sd >= 0f ? 1f : 0f, Time.deltaTime * 2f);
        Color accent = Color.Lerp(Friendly, Warn, suddenDeathAmount);
        float beat = HeartbeatPulse();
        clockGlow.color = HudKit.A(accent, 0.08f + suddenDeathAmount * 0.3f * beat);
        timer.color = Color.white;
        clock.localScale = Vector3.one;

        switch (phase)
        {
            case FusionRoundDirector.RoundPhase.Waiting:
                timer.text = Clock(roundLength);
                phaseLabel.text = NetworkPlayerAlignment.HasCalibration ? "WAITING FOR OPPONENT" : "ALIGN AT START";
                SetTimeline(0f);
                ShowBanner("waiting", "", "", Color.white, 0f);
                lastCountdownNumber = lastFinalSecond = lastHealState = -1;
                break;

            case FusionRoundDirector.RoundPhase.Countdown:
            {
                int number = Mathf.Max(1, Mathf.CeilToInt(remaining));
                timer.text = Clock(roundLength);
                phaseLabel.text = "GET READY";
                SetTimeline(0f);
                if (number != lastCountdownNumber)
                {
                    lastCountdownNumber = number;
                    SynthAudio.Play2D(SynthAudio.CountBeat(), 0.8f);
                }
                ShowBanner("count" + number, number.ToString(), "BATTLE STARTS IN", Friendly, 1f);
                break;
            }

            case FusionRoundDirector.RoundPhase.Fighting:
            {
                float elapsed = round.FightElapsed;
                int seconds = Mathf.CeilToInt(remaining);
                timer.text = Clock(remaining);
                SetTimeline(elapsed / Mathf.Max(1f, roundLength));
                if (Time.time - fightStartedAt < 1.1f)
                    ShowBanner("fight", "FIGHT", "", Friendly, 1f);
                else if (sd >= -5f && sd < 0f)
                {
                    int left = Mathf.CeilToInt(-sd);
                    ShowBanner("sdwarn" + left, left.ToString(), "SUDDEN DEATH INCOMING", Warn, 1f);
                }
                else if (sd >= 0f && sd < 2.6f)
                    ShowBanner("suddendeath", "SUDDEN DEATH", "DAMAGE ×1.5 · NO RETREAT", Warn, 1f);
                else ShowBanner("none", "", "", Color.white, 0f);

                phaseLabel.text = sd >= 0f ? "SUDDEN DEATH"
                    : round.HazardStage == 1 ? "FIRE STRIKE INCOMING"
                    : round.HazardStage == 2 ? "FIRE ZONE ACTIVE" : "DUEL";
                phaseLabel.color = sd >= 0f || round.HazardStage > 0
                    ? Color.Lerp(Warn, Color.white, 0.25f * beat) : Soft;
                if (sd >= 0f)
                {
                    timer.color = Color.Lerp(Color.white, Warn, 0.35f + 0.65f * beat);
                    clock.localScale = Vector3.one * (1f + 0.06f * beat);
                }
                if (seconds <= 10 && seconds != lastFinalSecond)
                {
                    lastFinalSecond = seconds;
                    SynthAudio.Play2D(SynthAudio.CountBeat(), 0.9f, seconds <= 3 ? 1.25f : 1f);
                }
                UpdateHealToasts(round);
                break;
            }

            case FusionRoundDirector.RoundPhase.Result:
            {
                int localId = NetworkClient.localPlayer != null ? (int)NetworkClient.localPlayer.netId : -1;
                bool won = round.WinnerPlayerId == localId;
                bool draw = round.WinnerPlayerId < 0;
                timer.text = "0:00";
                phaseLabel.text = "ROUND OVER";
                phaseLabel.color = Soft;
                SetTimeline(1f);
                ShowBanner(draw ? "draw" : won ? "win" : "lose", draw ? "DRAW" : won ? "VICTORY" : "DEFEAT",
                    "NEXT ROUND IN " + Mathf.CeilToInt(remaining), draw ? Soft : won ? Friendly : Enemy, 1f);
                lastFinalSecond = -1;
                break;
            }
        }
        AnimateBanner();
    }

    private void UpdateHealToasts(FusionRoundDirector round)
    {
        if (round.HealState == lastHealState) return;
        int previous = lastHealState;
        lastHealState = round.HealState;
        if (previous < 0) return;
        if (round.HealState == 1)
        {
            Toast("MED-KIT DEPLOYED · CENTER", HealPickupView.Green, 3f);
            SynthAudio.Play2D(SynthAudio.HealChime(), 0.7f);
        }
        else if (round.HealState == 2)
        {
            bool mine = NetworkClient.localPlayer != null && round.HealTakerId == (int)NetworkClient.localPlayer.netId;
            Toast(mine ? "+ HEALTH RESTORED" : "OPPONENT HEALED", mine ? HealPickupView.Green : Enemy, 2.4f);
            if (mine) localCard.FlashHeal();
            else opponentCard.FlashHeal();
        }
    }

    public void AnnouncePhase(FusionRoundDirector round)
    {
        if (round.Phase == FusionRoundDirector.RoundPhase.Fighting)
            SynthAudio.Play2D(SynthAudio.Stinger(), 0.45f, 1.6f);
        else if (round.Phase == FusionRoundDirector.RoundPhase.Result)
            SynthAudio.Play2D(SynthAudio.Stinger(), 0.8f, 0.9f);
    }

    private void FindPlayers()
    {
        if (localHealth != null && opponentHealth != null) return;
        foreach (var found in FindObjectsByType<NetworkPlayerHealth>(FindObjectsSortMode.None))
        {
            if (found.IsLocalPlayer) localHealth = found;
            else opponentHealth = found;
        }
    }

    private void SetTimeline(float progress)
    {
        progress = Mathf.Clamp01(progress);
        timelineFill.rectTransform.sizeDelta = new Vector2(440f * progress, 6f);
        timelineFill.color = HudKit.A(Color.Lerp(Soft, Warn, suddenDeathAmount), 0.9f);
        timelineHead.rectTransform.anchoredPosition = new Vector2(-220f + 440f * progress, 0f);
        timelineHead.color = Color.Lerp(Color.white, Warn, suddenDeathAmount);
    }

    private static string Clock(float seconds)
    {
        int s = Mathf.Max(0, Mathf.CeilToInt(seconds));
        return $"{s / 60}:{s % 60:00}";
    }

    // ---------- Banner ----------

    private void ShowBanner(string key, string title, string caption, Color color, float targetAlpha)
    {
        if (key != bannerKey)
        {
            bannerKey = key;
            bannerShownAt = Time.time;
            if (targetAlpha > 0f)
            {
                bannerText.text = title;
                bannerCaption.text = caption;
                bannerText.color = color;
                bannerText.fontSize = title.Length <= 2 ? 220f : title.Length > 9 ? 140f : 160f;
                bool danger = color == Warn || color == Enemy;
                bannerStripes.parent.gameObject.SetActive(danger);
                bannerBand.color = HudKit.A(color, danger ? 0.22f : 0.12f);
                if (key == "suddendeath") SynthAudio.Play2D(SynthAudio.Stinger(), 1f);
                if (key == "fight") SynthAudio.Play2D(SynthAudio.Clunk(), 0.9f, 0.8f);
            }
        }
        bannerTarget = targetAlpha;
    }

    private float bannerTarget;

    private void AnimateBanner()
    {
        float age = Time.time - bannerShownAt;
        bannerGroup.alpha = Mathf.MoveTowards(bannerGroup.alpha, bannerTarget, Time.deltaTime * (bannerTarget > 0f ? 10f : 4f));
        // Slam in: overshoot scale, then settle; digits punch once per second.
        float slam = 1f + 0.6f * Mathf.Exp(-age * 9f) * Mathf.Cos(age * 18f);
        bannerText.rectTransform.localScale = Vector3.one * slam;
        bannerFlash.color = new Color(1f, 1f, 1f, 0.5f * Mathf.Exp(-age * 7f) * bannerGroup.alpha);
        float shake = bannerKey == "suddendeath" ? 14f * Mathf.Exp(-age * 3f) : 0f;
        banner.anchoredPosition = new Vector2(Random.Range(-shake, shake), 70f + Random.Range(-shake, shake));
        bannerStripes.anchoredPosition = new Vector2(Mathf.Repeat(Time.time * 60f, 64f) - 32f, 0f);
    }

    // ---------- Toast ----------

    public void Toast(string message, Color color, float seconds = 2.2f)
    {
        Initialize();
        toastText.text = message;
        toastText.color = Color.Lerp(color, Color.white, 0.35f);
        toastAccent.color = color;
        toastShownAt = Time.time;
        toastUntil = Time.time + seconds;
    }

    private void UpdateToast()
    {
        float age = Time.time - toastShownAt;
        bool visible = Time.time < toastUntil;
        toastGroup.alpha = Mathf.MoveTowards(toastGroup.alpha, visible ? 1f : 0f, Time.deltaTime * (visible ? 8f : 3f));
        float slide = 1f - Mathf.Exp(-age * 12f);
        toast.anchoredPosition = new Vector2(0f, 400f + 30f * (1f - slide));
    }

    // ---------- Vignette ----------

    private void UpdateVignette()
    {
        float now = Time.time;
        float hit = ArmActivationSignal.Level(ArmActivationSignal.Kind.Hit, now);
        if (hit > 0.6f && previousHit < 0.3f)
        {
            SynthAudio.Play2D(SynthAudio.Explosion(), 0.35f, 1.4f);
            SynthAudio.Play2D(SynthAudio.Clunk(), 0.8f, 0.6f);
        }
        previousHit = hit;
        float block = ArmActivationSignal.Level(ArmActivationSignal.Kind.Shield, now, burstsOnly: true);
        float heal = ArmActivationSignal.Level(ArmActivationSignal.Kind.Heal, now);
        float lead = ArmActivationSignal.Anticipation(ArmActivationSignal.Kind.Hit, now);
        float low = localHealth != null && localHealth.IsAlive && localHealth.Health01 < 0.3f
            ? (0.25f + 0.35f * HeartbeatPulse()) * Mathf.InverseLerp(0.3f, 0.05f, localHealth.Health01) : 0f;
        float danger = suddenDeathAmount * (0.14f + 0.12f * HeartbeatPulse());

        Color color = Warn;
        float alpha = Mathf.Max(Mathf.Min(1f, hit * 0.75f) + lead * 0.2f, low, danger);
        if (block > alpha) { color = CombatVfxStyle.Shield; alpha = Mathf.Min(0.8f, block * 0.6f); }
        if (heal > alpha) { color = HealPickupView.Green; alpha = Mathf.Min(0.7f, heal * 0.5f); }
        vignette.color = HudKit.A(color, alpha);
        vignetteRoot.gameObject.SetActive(alpha > 0.005f);
        localCard.Shake(hit);
    }

    /// <summary>0..1 lub-dub pulse at ~80 bpm, faster in sudden death.</summary>
    internal float HeartbeatPulse()
    {
        float bpm = Mathf.Lerp(80f, 128f, suddenDeathAmount);
        float t = Mathf.Repeat(Time.time * bpm / 60f, 1f);
        return Mathf.Max(Mathf.Exp(-t * 14f), 0.7f * Mathf.Exp(-Mathf.Max(0f, t - 0.24f) * 14f) * (t > 0.24f ? 1f : 0f));
    }

    // ---------- Hit confirmation (attacker side) ----------

    public void HitConfirmed(Vector3 worldPosition, int damage, bool shielded)
    {
        SynthAudio.Play2D(SynthAudio.HitConfirm(), shielded ? 0.4f : 0.8f, shielded ? 0.7f : 1f);
        HitMarkerFx.Spawn(worldPosition, damage, shielded ? CombatVfxStyle.Shield : Enemy);
        if (!shielded) opponentCard.Punch();
    }

#if UNITY_EDITOR
    public void Preview(Camera camera)
    {
        Initialize();
        Position(camera);
        timer.text = "1:12";
        phaseLabel.text = "DUEL";
        localCard.Preview(240, 0.8f);
        opponentCard.Preview(110, 0.37f);
        SetTimeline(0.2f);
        Canvas.ForceUpdateCanvases();
    }

    internal float PreviewLocalFill => localCard.Fill;
#endif

    // ---------- Health card ----------

    private sealed class HealthCard
    {
        private readonly RectTransform rect;
        private readonly Vector2 home;
        private readonly Image fill, chip, heal, glow, tip;
        private readonly float barLeft;
        private readonly TextMeshProUGUI number;
        private readonly float barWidth;
        private readonly Color accent;
        private float shownFraction = 1f, chipFraction = 1f, lastDropAt = -10f, healAt = -10f, punchAt = -10f;
        private int lastHp = -1;
        private float shake;

        public float Fill => shownFraction;

        public HealthCard(RectTransform parent, string label, Color accentColor, Vector2 position, bool large)
        {
            accent = accentColor;
            Vector2 size = large ? new Vector2(500f, 124f) : new Vector2(456f, 90f);
            barWidth = size.x - 56f;
            float barHeight;
            home = position;
            rect = HudKit.Rect(parent, label + " card", position, size);
            var name = HudKit.Text(rect, "Label", HudKit.Display, large ? 26f : 21f, accent,
                new Vector2(-size.x / 2f + 28f + 110f, large ? 30f : 20f), new Vector2(220f, 36f), TextAlignmentOptions.Left);
            name.text = label;
            name.characterSpacing = 14f;
            number = HudKit.Text(rect, "HP", HudKit.Heavy, large ? 58f : 38f, Color.white,
                new Vector2(size.x / 2f - 28f - 100f, large ? 24f : 17f), new Vector2(200f, 90f), TextAlignmentOptions.Right);
            float barY = large ? -26f : -20f;
            float left = -size.x / 2f + 28f;
            barHeight = large ? 12f : 8f;
            HudKit.Image(rect, "Track", HudSprites.Panel(4), new Color(1f, 1f, 1f, 0.1f), new Vector2(left + barWidth / 2f, barY), new Vector2(barWidth, 3f), true, 1f);
            // Amorphous bloom that follows the fill, osu!lazer style.
            glow = HudKit.Image(rect, "Bloom", HudSprites.Glow(8), HudKit.A(accent, 0.3f), new Vector2(left - 40f, barY), new Vector2(barWidth + 80f, barHeight + 80f), true, 1f);
            glow.rectTransform.pivot = new Vector2(0f, 0.5f);
            chip = Bar(rect, "Chip", new Color(1f, 0.93f, 0.8f, 0.9f), left, barY, barHeight);
            fill = Bar(rect, "Fill", accent, left, barY, barHeight);
            heal = Bar(rect, "Heal", HealPickupView.Green, left, barY, barHeight);
            tip = HudKit.Image(rect, "Tip", HudSprites.Dot(), Color.white, new Vector2(left, barY), new Vector2(barHeight * 5f, barHeight * 5f));
            barLeft = left;
            var shine = HudKit.Image(fill.rectTransform, "Shine", HudSprites.Panel(3), new Color(1f, 1f, 1f, 0.22f), Vector2.zero, Vector2.zero, true, 1f);
            shine.rectTransform.anchorMin = new Vector2(0f, 0.55f);
            shine.rectTransform.anchorMax = new Vector2(1f, 0.9f);
            shine.rectTransform.offsetMin = new Vector2(3f, 0f);
            shine.rectTransform.offsetMax = new Vector2(-3f, 0f);
            for (int i = 1; i < 10; i++)
                HudKit.Image(rect, "Tick", null, new Color(0f, 0f, 0f, 0.35f),
                    new Vector2(left + barWidth * i / 10f, barY), new Vector2(2f, barHeight));
        }

        private Image Bar(RectTransform parent, string name, Color color, float left, float y, float height)
        {
            var image = HudKit.Image(parent, name, HudSprites.Panel(6), color, new Vector2(left, y), new Vector2(barWidth, height), true, 1f);
            image.rectTransform.pivot = new Vector2(0f, 0.5f);
            return image;
        }

        public void Update(NetworkPlayerHealth health)
        {
            rect.gameObject.SetActive(health != null);
            if (health == null) return;
            int hp = health.CurrentHealth;
            float target = health.Health01;
            if (lastHp >= 0 && hp < lastHp) { lastDropAt = Time.time; punchAt = Time.time; }
            if (lastHp >= 0 && hp > lastHp) healAt = Time.time;
            lastHp = hp;
            Apply(hp, target);
        }

        private void Apply(int hp, float target)
        {
            shownFraction = Mathf.MoveTowards(shownFraction, target, Time.deltaTime * (target < shownFraction ? 6f : 1.2f));
            if (Time.time - lastDropAt > 0.5f) chipFraction = Mathf.MoveTowards(chipFraction, shownFraction, Time.deltaTime * 0.9f);
            chipFraction = Mathf.Max(chipFraction, shownFraction);
            number.text = Mathf.Max(0, hp).ToString();
            Color barColor = target > 0.5f ? accent : target > 0.25f
                ? Color.Lerp(new Color(1f, 0.78f, 0.2f), accent, (target - 0.25f) * 4f)
                : Color.Lerp(Warn, new Color(1f, 0.78f, 0.2f), target * 4f);
            float healGlow = Mathf.Exp(-(Time.time - healAt) * 2.5f);
            fill.color = Color.Lerp(barColor, Color.white, 0.6f * Mathf.Exp(-(Time.time - lastDropAt) * 12f));
            fill.rectTransform.sizeDelta = new Vector2(barWidth * shownFraction, fill.rectTransform.sizeDelta.y);
            chip.rectTransform.sizeDelta = new Vector2(barWidth * chipFraction, chip.rectTransform.sizeDelta.y);
            heal.rectTransform.sizeDelta = new Vector2(barWidth * shownFraction, heal.rectTransform.sizeDelta.y);
            heal.color = HudKit.A(HealPickupView.Green, 0.8f * healGlow * (0.6f + 0.4f * Mathf.Sin(Time.time * 20f)));
            float lowPulse = target < 0.25f ? 0.5f + 0.5f * Mathf.Sin(Time.time * 10f) : 0f;
            glow.color = HudKit.A(Color.Lerp(barColor, HealPickupView.Green, healGlow),
                0.28f + 0.4f * healGlow + 0.45f * Mathf.Exp(-(Time.time - lastDropAt) * 5f) + 0.25f * lowPulse);
            glow.rectTransform.sizeDelta = new Vector2(barWidth * shownFraction + 80f, glow.rectTransform.sizeDelta.y);
            tip.rectTransform.anchoredPosition = new Vector2(barLeft + barWidth * shownFraction, fill.rectTransform.anchoredPosition.y);
            tip.color = HudKit.A(Color.Lerp(Color.white, barColor, 0.4f), shownFraction > 0.001f ? 0.9f : 0f);
            tip.rectTransform.localScale = Vector3.one * (1f + 0.25f * Mathf.Sin(Time.time * 6f) + 0.8f * Mathf.Exp(-(Time.time - lastDropAt) * 8f));
            float punch = 1f + 0.08f * Mathf.Exp(-(Time.time - punchAt) * 10f);
            number.rectTransform.localScale = Vector3.one * punch;
            number.color = Color.Lerp(Color.white, Warn, Mathf.Exp(-(Time.time - lastDropAt) * 6f));
            Vector2 jitter = shake > 0.01f ? Random.insideUnitCircle * 12f * shake : Vector2.zero;
            rect.anchoredPosition = home + jitter;
        }

        public void Shake(float amount) => shake = Mathf.Clamp01(amount);
        public void FlashHeal() => healAt = Time.time;
        public void Punch() => punchAt = Time.time;

        public void Preview(int hp, float fraction)
        {
            shownFraction = chipFraction = fraction;
            rect.gameObject.SetActive(true);
            Apply(hp, fraction);
        }
    }
}

/// <summary>Tiny builder helpers for the runtime HUD.</summary>
internal static class HudKit
{
    private static TMP_FontAsset display, heavy;
    internal static TMP_FontAsset Display => display != null ? display
        : display = FontOrDefault(ThermalFxLibrary.Instance != null ? ThermalFxLibrary.Instance.displayFont : null);
    internal static TMP_FontAsset Heavy => heavy != null ? heavy
        : heavy = FontOrDefault(ThermalFxLibrary.Instance != null ? ThermalFxLibrary.Instance.heavyFont : null);

    private static TMP_FontAsset FontOrDefault(TMP_FontAsset font)
        => font != null ? font : Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");

    internal static Color A(Color color, float alpha) { color.a = alpha; return color; }

    internal static RectTransform Canvas(string name, Transform parent, Vector2 size, int order)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        var rect = (RectTransform)go.transform;
        rect.SetParent(parent, false);
        rect.sizeDelta = size;
        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = order;
        go.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 3f;
        return rect;
    }

    internal static RectTransform Rect(Transform parent, string name, Vector2 position, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform));
        var rect = (RectTransform)go.transform;
        rect.SetParent(parent, false);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        return rect;
    }

    internal static Image Image(Transform parent, string name, Sprite sprite, Color color, Vector2 position,
        Vector2 size, bool sliced = false, float cornerScale = 1f)
    {
        var rect = Rect(parent, name, position, size);
        var image = rect.gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.raycastTarget = false;
        if (sliced && sprite != null)
        {
            image.type = UnityEngine.UI.Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 1f / Mathf.Max(0.05f, cornerScale);
        }
        return image;
    }

    internal static TextMeshProUGUI Text(Transform parent, string name, TMP_FontAsset font, float size, Color color,
        Vector2 position, Vector2 box, TextAlignmentOptions alignment)
    {
        // Heavy display faces have tall line heights; never let a box clip its own font.
        box.y = Mathf.Max(box.y, size * 1.5f);
        var rect = Rect(parent, name, position, box);
        var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
        text.font = font;
        text.fontSize = size;
        text.color = color;
        text.alignment = alignment;
        text.enableWordWrapping = false;
        text.overflowMode = TextOverflowModes.Overflow;
        text.raycastTarget = false;
        text.outlineColor = new Color32(0, 0, 0, 170);
        text.outlineWidth = 0.12f;
        return text;
    }
}
