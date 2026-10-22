using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>Startup menu that floats in front of the player until one of the Raspberry Pis found on
/// the LAN is chosen, by pressing its button with a fingertip or by pointing at it and pinching;
/// this headset's combat output then goes to that Pi. CLOSE dismisses it with no Pi, leaving the
/// output off. Each Pi's receiver broadcasts "THERMAL-PI-1:&lt;name&gt;" to UDP <see cref="DiscoveryPort"/>
/// about once a second, so no Pi IPs live in Unity. The expected names always have a button, greyed
/// until that Pi is heard; any other Pi that announces itself is added after them. It appears once
/// per launch, so restart the app to choose again. Keys 1-9 select and Escape closes, for editor
/// testing.</summary>
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
    private static readonly Vector2 CloseSize = new Vector2(70f, 34f);
    // Metres; a pointer ray farther from the board than this is ignored.
    private const float MaxRayLength = 3f;
    private static readonly Color Idle = new Color(0.045f, 0.075f, 0.11f, 0.95f);
    private static readonly Color Disabled = new Color(0.05f, 0.05f, 0.06f, 0.8f);

    private sealed class PiButton
    {
        public string name, host;
        public float lastSeen = float.NegativeInfinity;
        public Vector2 center, size = ButtonSize;
        public Image panel;
        public TextMeshProUGUI title, address;
        public bool shown, close;
        public bool Valid => close || (host != null && Time.unscaledTime - lastSeen < StaleAfter);
        public bool Pressable => close || shown;
    }
    private sealed class Finger
    {
        public OVRPlugin.Hand side;
        public OVRHand hand;
        public OVRSkeleton skeleton;
        public bool armed, pinching;
        public LineRenderer ray;
    }

    private RectTransform board;
    private Vector2 boardSize;
    private readonly List<PiButton> buttons = new List<PiButton>();
    private PiButton closeButton;
    private Material rayMaterial;
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
        rayMaterial = CombatVfxStyle.CreateMaterial("Pi menu pointer", Color.white);
        foreach (var finger in fingers)
        {
            finger.ray = CombatVfxStyle.CreateLine(transform, "Pi menu pointer", rayMaterial, true, 0.004f);
            finger.ray.startColor = HudKit.A(FusionRoundHud.Friendly, 0.15f);
            finger.ray.endColor = HudKit.A(FusionRoundHud.Friendly, 0.85f);
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

    private IEnumerable<PiButton> Targets()
    {
        foreach (var button in buttons) yield return button;
        if (closeButton != null) yield return closeButton;
    }

    /// <summary>The pressable button under a point in board units, if any.</summary>
    private PiButton ButtonAt(Vector3 local)
    {
        foreach (var button in Targets())
            if (button.Pressable && Mathf.Abs(local.x - button.center.x) < button.size.x * 0.5f
                && Mathf.Abs(local.y - button.center.y) < button.size.y * 0.5f) return button;
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
        Label(board, "PRESS WITH A FINGERTIP, OR POINT AND PINCH", 11f, FusionRoundHud.Friendly, new Vector2(0f, 70f), boardSize.x - 20f);
        status = Label(board, "", 10f, FusionRoundHud.Soft, new Vector2(0f, -105f), boardSize.x - 20f);

        closeButton = new PiButton
        {
            name = "CLOSE", close = true, size = CloseSize,
            center = new Vector2(boardSize.x * 0.5f - CloseSize.x * 0.5f - 12f, boardSize.y * 0.5f - CloseSize.y * 0.5f - 12f),
        };
        var closeRoot = HudKit.Rect(board, "Close button", closeButton.center, CloseSize);
        closeButton.panel = HudKit.Image(closeRoot, "Panel", HudSprites.Panel(10), Idle, Vector2.zero, CloseSize, true, 1f);
        Label(closeRoot, "CLOSE", 11f, Color.white, Vector2.zero, CloseSize.x - 8f);

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
        if (keys == null) return;
        if (keys.escapeKey.wasPressedThisFrame) { Select(closeButton); return; }
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
        foreach (var button in Targets())
            if (button.Pressable) button.panel.color = Idle;
        for (int f = 0; f < fingers.Length; f++)
        {
            var finger = fingers[f];
            bool pinching = finger.hand != null && finger.hand.IsTracked
                && finger.hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            bool pinchStarted = pinching && !finger.pinching;
            finger.pinching = pinching;
            // A fingertip at the board presses directly; otherwise the hand points from a distance.
            if (tips[f] != null && Poke(finger, tips[f].Value)) finger.ray.enabled = false;
            else Point(finger, pinching, pinchStarted);
            if (selected) return;
        }
    }

    /// <summary>Handles a fingertip near a button; false when it isn't near one.</summary>
    private bool Poke(Finger finger, Vector3 tip)
    {
        // The board faces away from the player, so the near side is negative local z.
        Vector3 local = board.InverseTransformPoint(tip);
        float depth = -local.z;
        PiButton over = ButtonAt(local);
        if (over == null || depth > 120f || depth < -40f) { finger.armed = false; return false; }
        // A press needs an approach from the front; sliding in sideways behind the panel does nothing.
        if (depth > 12f) finger.armed = true;
        over.panel.color = Color.Lerp(Idle, FusionRoundHud.Friendly, depth <= 12f ? 0.7f : 0.3f);
        if (finger.armed && depth <= 0f) Select(over);
        return true;
    }

    /// <summary>Casts the hand's system pointer ray at the board; a pinch presses the button it is on.</summary>
    private void Point(Finger finger, bool pinching, bool pinchStarted)
    {
        var hand = finger.hand;
        finger.ray.enabled = false;
        if (hand == null || !hand.IsTracked || !hand.IsPointerPoseValid || hand.PointerPose == null) return;
        var ray = new Ray(hand.PointerPose.position, hand.PointerPose.forward);
        if (!new Plane(board.forward, board.position).Raycast(ray, out float distance) || distance > MaxRayLength) return;
        Vector3 hit = ray.GetPoint(distance);
        Vector3 local = board.InverseTransformPoint(hit);
        if (Mathf.Abs(local.x) > boardSize.x * 0.5f || Mathf.Abs(local.y) > boardSize.y * 0.5f) return;
        finger.ray.positionCount = 2;
        finger.ray.SetPosition(0, ray.origin);
        finger.ray.SetPosition(1, hit);
        finger.ray.enabled = true;
        PiButton over = ButtonAt(local);
        if (over == null) return;
        over.panel.color = Color.Lerp(Idle, FusionRoundHud.Friendly, pinching ? 0.7f : 0.3f);
        if (pinchStarted) Select(over);
    }

    private void Select(PiButton button)
    {
        if (selected || button == null || !button.Valid) return;
        selected = true;
        discovery?.Dispose();
        discovery = null;
        foreach (var finger in fingers) finger.ray.enabled = false;
        hideAt = Time.unscaledTime + 1.2f;
        if (button.close)
        {
            button.panel.color = FusionRoundHud.Friendly;
            status.text = "NO PI: COMBAT OUTPUT OFF UNTIL RESTART";
            status.color = FusionRoundHud.Warn;
            Debug.Log("[PiSelectionMenu] Closed without a Pi; combat output stays off this run.");
            return;
        }
        bool sent = CombatEventOutput.SelectHost(button.host);
        button.panel.color = sent ? FusionRoundHud.Friendly : FusionRoundHud.Warn;
        status.text = sent ? $"SENDING TO {button.name} ({button.host})" : "PI OUTPUT DISABLED IN COMBAT-OUTPUT.JSON";
        status.color = sent ? FusionRoundHud.Friendly : FusionRoundHud.Warn;
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
        if (rayMaterial != null) Destroy(rayMaterial);
    }
}
