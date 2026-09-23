using Mirror;
using TMPro;
using UnityEngine;

/// <summary>Separate floating clock, announcements and unboxed local health.</summary>
[DisallowMultipleComponent]
public sealed class FusionRoundHud : MonoBehaviour
{
    private RectTransform clockRoot;
    private RectTransform announcementRoot;
    private TextMeshProUGUI timer;
    private TextMeshProUGUI phaseLabel;
    private TextMeshProUGUI status;
    private TextMeshProUGUI announcement;
    private TextMeshProUGUI announcementCaption;
    private DemoHealthBar healthBar;
    private AudioSource announcer;
    private AudioClip tickClip;
    private AudioClip startClip;
    private AudioClip finishClip;
    private int lastCountdownNumber = -1;
    private int lastFinalSecond = -1;
    private NetworkPlayerHealth localHealth;
    private float lastHitAt = -10f;
    private int lastHealth = -1;

    private void Awake() => Initialize();

    private void Initialize()
    {
        if (clockRoot != null) return;
        clockRoot = DemoHudElements.Canvas("Round clock", transform, new Vector2(430f, 190f));
        phaseLabel = DemoHudElements.Text(clockRoot, "Round phase", new Vector2(0f, 75f),
            new Vector2(480f, 44f), 26f, TextAlignmentOptions.Center, new Color(0.77f, 0.88f, 0.94f));
        phaseLabel.characterSpacing = 2f;
        timer = DemoHudElements.Text(clockRoot, "Time remaining", new Vector2(0f, 15f),
            new Vector2(270f, 100f), 62f, TextAlignmentOptions.Center, Color.white);
        timer.fontStyle = FontStyles.Bold;
        status = DemoHudElements.Text(clockRoot, "Round status", new Vector2(0f, -55f),
            new Vector2(600f, 46f), 26f, TextAlignmentOptions.Center, Color.white);
        healthBar = new DemoHealthBar(transform, "Local health");
        announcementRoot = DemoHudElements.Canvas("Round announcement", transform, new Vector2(620f, 230f));
        announcement = DemoHudElements.Text(announcementRoot, "Announcement", new Vector2(0f, 25f),
            new Vector2(610f, 140f), 90f, TextAlignmentOptions.Center, Color.white);
        announcement.fontStyle = FontStyles.Bold;
        announcementCaption = DemoHudElements.Text(announcementRoot, "Announcement caption", new Vector2(0f, -64f),
            new Vector2(610f, 40f), 23f, TextAlignmentOptions.Center, Color.white);
        announcer = gameObject.AddComponent<AudioSource>();
        announcer.playOnAwake = false;
        announcer.spatialBlend = 0f;
        announcer.volume = 0.55f;
        tickClip = Resources.Load<AudioClip>("CustomAssets/Audio/beep");
        startClip = Resources.Load<AudioClip>("CustomAssets/Audio/ChargeBlast");
        finishClip = Resources.Load<AudioClip>("CustomAssets/Audio/explosion-312361");
        announcementRoot.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        Camera eye = Camera.main;
        if (eye != null) Position(eye);
    }

    private void OnDestroy()
    {
        if (clockRoot != null) DestroyHudObject(clockRoot.gameObject);
        if (announcementRoot != null) DestroyHudObject(announcementRoot.gameObject);
        if (healthBar != null && healthBar.Root != null) DestroyHudObject(healthBar.Root.gameObject);
    }

    private static void DestroyHudObject(GameObject value)
    {
        if (Application.isPlaying) Destroy(value); else DestroyImmediate(value);
    }

    private void Position(Camera camera)
    {
        clockRoot.SetParent(camera.transform, true);
        healthBar.Root.SetParent(camera.transform, true);
        announcementRoot.SetParent(camera.transform, true);
        // Small, separated HUD anchors leave the opponent and aiming area clear.
        clockRoot.position = camera.transform.TransformPoint(new Vector3(0f, 0.43f, 1.8f));
        clockRoot.rotation = camera.transform.rotation;
        clockRoot.localScale = Vector3.one * 0.00125f;
        healthBar.Root.position = camera.transform.TransformPoint(new Vector3(-0.38f, -0.30f, 1.4f));
        healthBar.Root.rotation = camera.transform.rotation;
        announcementRoot.position = camera.transform.TransformPoint(new Vector3(0f, 0.06f, 1.8f));
        announcementRoot.rotation = camera.transform.rotation;
    }

    public void Show(FusionRoundDirector round)
    {
        Initialize();
        Camera camera = Camera.main;
        if (camera == null) return;
        Position(camera);
        if (localHealth == null)
            foreach (var found in FindObjectsByType<NetworkPlayerHealth>(FindObjectsSortMode.None))
                if (found.IsLocalPlayer) { localHealth = found; break; }
        if (localHealth != null)
        {
            if (lastHealth >= 0 && localHealth.CurrentHealth < lastHealth) lastHitAt = Time.time;
            lastHealth = localHealth.CurrentHealth;
            healthBar.SetHealth(lastHealth, localHealth.Health01);
            float pulse = 1f + 0.055f * Mathf.Clamp01(1f - (Time.time - lastHitAt) / 0.22f);
            healthBar.Root.localScale = Vector3.one * (0.001f * pulse);
        }
        healthBar.Root.gameObject.SetActive(localHealth != null);
        announcementRoot.gameObject.SetActive(false);
        timer.color = Color.white;
        status.color = Color.white;
        switch (round.Phase)
        {
            case FusionRoundDirector.RoundPhase.Waiting:
                phaseLabel.text = "THERMAL DUEL";
                timer.text = "2:00";
                status.text = NetworkPlayerAlignment.HasCalibration ? "WAITING FOR PLAYER" : "ALIGN AT START";
                lastCountdownNumber = lastFinalSecond = -1;
                break;
            case FusionRoundDirector.RoundPhase.Countdown:
                int number = Mathf.Max(1, Mathf.CeilToInt(round.PhaseRemaining));
                phaseLabel.text = "GET READY";
                timer.text = "2:00";
                status.text = "";
                announcementRoot.gameObject.SetActive(true);
                announcement.text = number.ToString();
                announcement.color = Color.white;
                announcementCaption.text = "BATTLE STARTS";
                float countdownScale = 1f + 0.12f * Mathf.Repeat(round.PhaseRemaining, 1f);
                announcement.rectTransform.localScale = Vector3.one * countdownScale;
                if (number != lastCountdownNumber) { lastCountdownNumber = number; Play(tickClip, 0.65f); }
                break;
            case FusionRoundDirector.RoundPhase.Fighting:
                int seconds = Mathf.CeilToInt(round.PhaseRemaining);
                phaseLabel.text = seconds <= 20 ? "FINAL SURGE" : "DUEL";
                timer.text = $"{seconds / 60}:{seconds % 60:00}";
                status.text = round.HazardStage == 1 ? "MOVE · FIRE INCOMING"
                    : round.HazardStage == 2 ? "FIRE ZONE ACTIVE" : "";
                if (round.HazardStage > 0) status.color = CombatVfxStyle.Heat;
                if (seconds <= 20)
                {
                    timer.color = CombatVfxStyle.Heat;
                    if (seconds <= 10 && seconds != lastFinalSecond) { lastFinalSecond = seconds; Play(tickClip, 0.4f); }
                }
                break;
            case FusionRoundDirector.RoundPhase.Result:
                int localId = NetworkClient.localPlayer != null ? (int)NetworkClient.localPlayer.netId : -1;
                bool won = round.WinnerPlayerId == localId;
                phaseLabel.text = "ROUND COMPLETE";
                timer.text = "0:00";
                status.text = "";
                announcementRoot.gameObject.SetActive(true);
                announcement.rectTransform.localScale = Vector3.one;
                announcement.fontSize = 68f;
                announcement.text = round.WinnerPlayerId < 0 ? "DRAW" : won ? "VICTORY" : "DEFEAT";
                announcement.color = won ? DemoHudElements.Green : Color.white;
                announcementCaption.text = "NEXT ROUND  " + Mathf.CeilToInt(round.PhaseRemaining);
                break;
        }
        if (round.Phase == FusionRoundDirector.RoundPhase.Countdown) announcement.fontSize = 90f;
    }

    public void AnnouncePhase(FusionRoundDirector round)
    {
        if (round.Phase == FusionRoundDirector.RoundPhase.Fighting) Play(startClip, 0.8f);
        else if (round.Phase == FusionRoundDirector.RoundPhase.Result) Play(finishClip, 0.8f);
    }

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null && announcer != null)
            CombatAudioVoice.Play(announcer, clip, volume, clip == tickClip ? 0.18f : clip == startClip ? 0.8f : 1.4f,
                clip == startClip ? 5.4f : 0f);
    }

#if UNITY_EDITOR
    public void Preview(Camera camera)
    {
        Initialize();
        Position(camera);
        phaseLabel.text = "DUEL";
        timer.text = "1:42";
        status.text = "";
        healthBar.SetHealth(240, 0.8f);
        Canvas.ForceUpdateCanvases();
    }
#endif
}
