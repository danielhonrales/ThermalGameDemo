using UnityEngine;

[DisallowMultipleComponent]
public sealed class RightHandWeaponModeToggle : MonoBehaviour
{
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private CombatWeaponMode weaponMode;
    [SerializeField, Range(0f, 1f)] private float pinchThreshold = 0.75f;
    [SerializeField, Range(0f, 1f)] private float indexPinchMax = 0.35f;
    [SerializeField, Min(0f)] private float holdSeconds = 1f;
    [SerializeField, Min(0f)] private float repeatCooldownSeconds = 1.5f;
    [SerializeField] private bool requireHandTracked = true;
    [SerializeField] private bool logModeSwitch = true;

    private float heldTime;
    private float nextAllowedToggleTime;
    private bool loggedPoseDetected;

    private void Awake()
    {
        FindSceneReferences();
    }

    private void Update()
    {
        FindSceneReferences();

        bool poseActive = IsMiddleFingerPinchActive();
        if (!poseActive)
        {
            heldTime = 0f;
            loggedPoseDetected = false;
            return;
        }

        if (Time.time < nextAllowedToggleTime)
        {
            return;
        }

        if (logModeSwitch && !loggedPoseDetected)
        {
            loggedPoseDetected = true;
            Debug.Log($"Right-hand weapon toggle pose detected. Holding for {holdSeconds:0.00}s.", this);
        }

        heldTime += Time.deltaTime;
        if (heldTime < holdSeconds)
        {
            return;
        }

        heldTime = 0f;
        loggedPoseDetected = false;
        nextAllowedToggleTime = Time.time + repeatCooldownSeconds;
        ToggleWeaponMode();
    }

    [ContextMenu("Toggle Weapon Mode")]
    public void ToggleWeaponMode()
    {
        if (weaponMode == null)
        {
            weaponMode = GetComponent<CombatWeaponMode>();
        }

        if (weaponMode == null)
        {
            Debug.LogWarning("Cannot toggle weapon mode because no CombatWeaponMode was found.", this);
            return;
        }

        CombatWeaponMode.WeaponMode nextMode = weaponMode.ActiveMode == CombatWeaponMode.WeaponMode.ThermalBeam
            ? CombatWeaponMode.WeaponMode.IceGrenade
            : CombatWeaponMode.WeaponMode.ThermalBeam;

        weaponMode.SetMode(nextMode);

        if (logModeSwitch)
        {
            Debug.Log($"Weapon mode switched to {nextMode}.", this);
        }
    }

    [ContextMenu("Find Scene References")]
    public void FindSceneReferences()
    {
        if (weaponMode == null)
        {
            weaponMode = GetComponent<CombatWeaponMode>();
        }

        if (rightHand == null)
        {
            rightHand = FindRightHand();
        }
    }

    private bool IsMiddleFingerPinchActive()
    {
        if (rightHand == null)
        {
            return false;
        }

        if (requireHandTracked && !rightHand.IsTracked)
        {
            return false;
        }

        return rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) >= pinchThreshold
            && rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) <= indexPinchMax;
    }

    private static OVRHand FindRightHand()
    {
        OVRHand[] hands = FindObjectsByType<OVRHand>(FindObjectsSortMode.None);
        foreach (OVRHand hand in hands)
        {
            if (hand == null)
            {
                continue;
            }

            if (hand.gameObject.name.IndexOf("Right", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return hand;
            }
        }

        foreach (OVRHand hand in hands)
        {
            Transform rightAnchor = GameObject.Find("RightHandAnchor")?.transform;
            if (rightAnchor != null && hand.transform.IsChildOf(rightAnchor))
            {
                return hand;
            }
        }

        return hands.Length > 0 ? hands[^1] : null;
    }
}
