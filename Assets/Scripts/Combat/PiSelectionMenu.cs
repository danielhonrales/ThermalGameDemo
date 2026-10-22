using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>Startup menu that floats in front of the player until a fingertip presses one of the
/// Raspberry Pis found on the LAN; this headset's combat output then goes to that Pi. Each Pi's
/// receiver broadcasts "THERMAL-PI-1:&lt;name&gt;" to UDP <see cref="DiscoveryPort"/> about once a second,
/// so no Pi IPs live in Unity. The expected names always have a button, greyed until that Pi is
/// heard; any other Pi that announces itself is added after them. It appears once per launch, so
/// restart the app to choose again. Keys 1-9 also select, for editor testing.</summary>
[DisallowMultipleComponent]
public sealed class PiSelectionMenu : MonoBehaviour
{
    public const int DiscoveryPort = 7780;
    public const string BeaconPrefix = "THERMAL-PI-1:";
    public const int MaxNameLength = 24;
    // A Pi that has not announced itself for this long is greyed out until it is heard again.
    private const float StaleAfter = 3f;

    [SerializeField] private string[] expectedPis = { "Player1", "Player2" };
    [SerializeField, Min(0.25f)] private float distance = 0.42f;
    [SerializeField] private float belowEyes = 0.12f;

    // Canvas units are millimetres.
    private const float Scale = 0.001f;
    private const float ButtonGap = 20f;
    private static readonly Vector2 ButtonSize = new Vector2(160f, 100f);
    private static readonly Color Idle = new Color(0.045f, 0.075f, 0.11f, 0.95f);
    private static readonly Color Disabled = new Color(0.05f, 0.05f, 0.06f, 0.8f);

    private sealed class PiButton
    {
        public string name, host;
        public float lastSeen = float.NegativeInfinity;
        public Vector2 center;
        public Image panel;
        public TextMeshProUGUI title, address;
        public bool shown;
        public bool Valid => host != null && Time.unscaledTime - lastSeen < StaleAfter;
    }
    private sealed class Finger
    {
        public OVRPlugin.Hand side;
        public OVRHand hand;
        public OVRSkeleton skeleton;
        public bool armed;
    }

    private RectTransform board;
    private Vector2 boardSize;
    private readonly List<PiButton> buttons = new List<PiButton>();
    private TextMeshProUGUI status;
    private UdpClient discovery;
    private string discoveryError;
    private readonly Finger[] fingers = { new Finger { side = OVRPlugin.Hand.HandLeft }, new Finger { side = OVRPlugin.Hand.HandRight } };
    private float nextHandSearch, hideAt = -1f;
    private bool placed, selected;

    private void Awake()
    {
        if (SpectatorSession.IsSpectator) { enabled = false; return; }
        CombatEventOutput.AwaitHostSelection();
        foreach (string name in expectedPis)
            if (!string.IsNullOrWhiteSpace(name) && Find(name.Trim()) == null)
                buttons.Add(new PiButton { name = name.Trim() });
        try
        {
            discovery = new UdpClient(AddressFamily.InterNetwork);
            discovery.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            discovery.Client.Bind(new IPEndPoint(IPAddress.Any, DiscoveryPort));
        }
        catch (SocketException e)
        {
            discovery?.Dispose();
            discovery = null;
            discoveryError = e.Message;
            Debug.LogWarning($"[PiSelectionMenu] Cannot listen for Pis on UDP {DiscoveryPort}: {e.Message}");
        }
        Build();
    }

    /// <summary>Reads a Pi announcement; false for anything else on the discovery port.</summary>
    public static bool TryParseBeacon(string packet, out string name)
    {
        name = null;
        if (packet == null || !packet.StartsWith(BeaconPrefix, StringComparison.Ordinal)) return false;
        string value = packet.Substring(BeaconPrefix.Length).Trim();
        if (value.Length == 0 || value.Length > MaxNameLength) return false;
        foreach (char c in value)
            if (char.IsControl(c)) return false;
        name = value;
        return true;
    }

    private PiButton Find(string name)
    {
        foreach (var button in buttons)
            if (string.Equals(button.name, name, StringComparison.OrdinalIgnoreCase)) return button;
        return null;
    }

    private void Build()
    {
        float rowWidth = buttons.Count * ButtonSize.x + Mathf.Max(0, buttons.Count - 1) * ButtonGap;
        boardSize = new Vector2(Mathf.Max(380f, rowWidth + 40f), 270f);
        board = HudKit.Canvas("Pi selection menu", transform, boardSize, 40);
        board.localScale = Vector3.one * Scale;
        HudKit.Image(board, "Board", HudSprites.Panel(18), new Color(0.015f, 0.025f, 0.04f, 0.88f),
            Vector2.zero, boardSize, true, 1f);
        Label(board, "SELECT PI", 26f, Color.white, new Vector2(0f, 100f), boardSize.x - 20f);
        Label(board, "PRESS A BUTTON WITH YOUR FINGERTIP", 11f, FusionRoundHud.Friendly, new Vector2(0f, 70f), boardSize.x - 20f);
        status = Label(board, "", 10f, FusionRoundHud.Soft, new Vector2(0f, -105f), boardSize.x - 20f);

        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];
            button.center = new Vector2(-rowWidth * 0.5f + ButtonSize.x * 0.5f + i * (ButtonSize.x + ButtonGap), -15f);
            var root = HudKit.Rect(board, $"Pi {i + 1} button", button.center, ButtonSize);
            button.panel = HudKit.Image(root, "Panel", HudSprites.Panel(12), Disabled, Vector2.zero, ButtonSize, true, 1f);
            button.title = Label(root, button.name, 26f, FusionRoundHud.Soft, new Vector2(0f, 14f), ButtonSize.x - 10f);
            button.address = Label(root, "", 12f, FusionRoundHud.Warn, new Vector2(0f, -26f), ButtonSize.x - 10f);
            Refresh(button);
        }
        RefreshStatus();
    }

    /// <summary>Rebuilds the board for a newly found Pi, keeping it where the player sees it.</summary>
    private void Rebuild()
    {
        Transform parent = board.parent;
        board.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        Destroy(board.gameObject);
        Build();
        board.SetParent(parent, false);
        board.SetPositionAndRotation(position, rotation);
    }

    private static void Refresh(PiButton button)
    {
        button.shown = button.Valid;
        button.panel.color = button.shown ? Idle : Disabled;
        button.title.color = button.shown ? Color.white : FusionRoundHud.Soft;
        button.address.text = button.shown ? button.host : button.host == null ? "SEARCHING" : "NOT RESPONDING";
        button.address.color = button.shown ? FusionRoundHud.Friendly : FusionRoundHud.Warn;
    }

    private void RefreshStatus()
    {
        if (selected) return;
        bool any = buttons.Exists(button => button.Valid);
        status.text = discoveryError != null ? "PI DISCOVERY UNAVAILABLE: CHECK WI-FI"
            : any ? "RESTART THE APP TO CHOOSE AGAIN" : "SEARCHING FOR PIS ON THE NETWORK";
        status.color = discoveryError != null ? FusionRoundHud.Warn : FusionRoundHud.Soft;
    }

    private static TextMeshProUGUI Label(Transform parent, string text, float size, Color color, Vector2 position, float width)
    {
        var label = HudKit.Text(parent, text, HudKit.Heavy, size, color, position, new Vector2(width, size * 1.5f),
            TextAlignmentOptions.Center);
        label.text = text;
        label.characterSpacing = 2f;
        return label;
    }

    private void Update()
    {
        if (selected)
        {
            if (Time.unscaledTime >= hideAt) { board.gameObject.SetActive(false); enabled = false; }
            return;
        }
        Listen();
        bool changed = false;
        foreach (var button in buttons)
            if (button.Valid != button.shown) { Refresh(button); changed = true; }
        if (changed) RefreshStatus();
        var keys = Keyboard.current;
        if (keys != null)
            for (int i = 0; i < buttons.Count && i < 9; i++)
                if (keys[Key.Digit1 + i].wasPressedThisFrame || keys[Key.Numpad1 + i].wasPressedThisFrame) { Select(buttons[i]); return; }
    }

    private void Listen()
    {
        if (discovery == null) return;
        bool added = false;
        try
        {
            while (discovery.Available > 0)
            {
                IPEndPoint from = null;
                byte[] packet = discovery.Receive(ref from);
                if (!TryParseBeacon(Encoding.UTF8.GetString(packet), out string name)) continue;
                var button = Find(name);
                if (button == null)
                {
                    buttons.Add(button = new PiButton { name = name });
                    added = true;
                }
                button.lastSeen = Time.unscaledTime;
                string host = from.Address.ToString();
                if (button.host == host) continue;
                Debug.Log(button.host == null ? $"[PiSelectionMenu] Found {name} at {host}"
                    : $"[PiSelectionMenu] {name} moved from {button.host} to {host}");
                button.host = host;
                // A Pi added just now gets its labels when the board is rebuilt below.
                if (button.panel != null) { Refresh(button); RefreshStatus(); }
            }
        }
        catch (SocketException) { /* Wi-Fi may drop; keep listening when it returns. */ }
        catch (ObjectDisposedException) { }
        if (added) Rebuild();
    }

    private void LateUpdate()
    {
        Camera eye = Camera.main;
        if (eye == null) return;
        if (board.parent != eye.transform.parent && eye.transform.parent != null)
            board.SetParent(eye.transform.parent, true);
        Vector3?[] tips = FingerTips();
        Follow(eye.transform, tips);
        if (!selected) Press(tips);
    }

    private void Follow(Transform head, Vector3?[] tips)
    {
        Vector3 forward = Vector3.ProjectOnPlane(head.forward, Vector3.up);
        if (forward.sqrMagnitude < 0.04f) forward = Vector3.Cross(head.right, Vector3.up);
        forward.Normalize();
        Vector3 position = head.position + forward * distance - Vector3.up * belowEyes;
        Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
        if (!placed) { board.SetPositionAndRotation(position, rotation); placed = true; return; }
        // Hold still while a finger is reaching for a button so it does not slide away.
        foreach (Vector3? tip in tips)
        {
            if (tip == null) continue;
            Vector3 local = board.InverseTransformPoint(tip.Value);
            if (Mathf.Abs(local.x) < boardSize.x * 0.5f + 60f && Mathf.Abs(local.y) < boardSize.y * 0.5f + 60f
                && local.z > -150f && local.z < 60f) return;
        }
        float follow = 1f - Mathf.Exp(-Time.deltaTime * 4f);
        board.SetPositionAndRotation(Vector3.Lerp(board.position, position, follow),
            Quaternion.Slerp(board.rotation, rotation, follow));
    }

    private void Press(Vector3?[] tips)
    {
        foreach (var button in buttons)
            if (button.shown) button.panel.color = Idle;
        for (int f = 0; f < fingers.Length; f++)
        {
            var finger = fingers[f];
            if (tips[f] == null) { finger.armed = false; continue; }
            // The board faces away from the player, so the near side is negative local z.
            Vector3 local = board.InverseTransformPoint(tips[f].Value);
            float depth = -local.z;
            PiButton over = null;
            foreach (var button in buttons)
                if (button.shown && Mathf.Abs(local.x - button.center.x) < ButtonSize.x * 0.5f
                    && Mathf.Abs(local.y - button.center.y) < ButtonSize.y * 0.5f) over = button;
            if (over == null || depth > 120f || depth < -40f) { finger.armed = false; continue; }
            // A press needs an approach from the front; sliding in sideways behind the panel does nothing.
            if (depth > 12f) finger.armed = true;
            over.panel.color = Color.Lerp(Idle, FusionRoundHud.Friendly, depth <= 12f ? 0.7f : 0.3f);
            if (finger.armed && depth <= 0f) { Select(over); return; }
        }
    }

    private void Select(PiButton button)
    {
        if (selected || !button.Valid) return;
        selected = true;
        discovery?.Dispose();
        discovery = null;
        bool sent = CombatEventOutput.SelectHost(button.host);
        button.panel.color = sent ? FusionRoundHud.Friendly : FusionRoundHud.Warn;
        status.text = sent ? $"SENDING TO {button.name} ({button.host})" : "PI OUTPUT DISABLED IN COMBAT-OUTPUT.JSON";
        status.color = sent ? FusionRoundHud.Friendly : FusionRoundHud.Warn;
        hideAt = Time.unscaledTime + 1.2f;
        Debug.Log($"[PiSelectionMenu] Selected {button.name} ({button.host})");
    }

    private Vector3?[] FingerTips()
    {
        if (Time.unscaledTime >= nextHandSearch) FindHands();
        var tips = new Vector3?[fingers.Length];
        for (int f = 0; f < fingers.Length; f++)
        {
            var finger = fingers[f];
            var skeleton = finger.skeleton;
            if (finger.hand == null || !finger.hand.IsTracked || skeleton == null
                || !skeleton.IsInitialized || !skeleton.IsDataValid || skeleton.Bones == null) continue;
            var type = skeleton.GetSkeletonType();
            bool xr = type == OVRSkeleton.SkeletonType.XRHandLeft || type == OVRSkeleton.SkeletonType.XRHandRight;
            var tipId = xr ? OVRSkeleton.BoneId.XRHand_IndexTip : OVRSkeleton.BoneId.Hand_IndexTip;
            foreach (OVRBone bone in skeleton.Bones)
                if (bone.Id == tipId && bone.Transform != null) { tips[f] = bone.Transform.position; break; }
        }
        return tips;
    }

    private void FindHands()
    {
        nextHandSearch = Time.unscaledTime + 0.75f;
        OVRHand[] hands = FindObjectsByType<OVRHand>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var finger in fingers)
        {
            if (finger.hand != null && finger.hand.isActiveAndEnabled && finger.skeleton != null) continue;
            foreach (OVRHand hand in hands)
                if (hand.GetHand() == finger.side && (finger.hand == null || hand.IsTracked)) finger.hand = hand;
            if (finger.hand != null) finger.skeleton = CombatHandSkeleton.For(finger.hand);
        }
    }

    private void OnDestroy()
    {
        discovery?.Dispose();
        if (board != null) Destroy(board.gameObject);
    }
}
