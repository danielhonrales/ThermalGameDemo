using UnityEngine;

[DisallowMultipleComponent]
public sealed class RightHandWeaponModeToggle : MonoBehaviour
{
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private CombatWeaponMode weaponMode;
    [SerializeField, Range(0f, 1f)] private float pinchThreshold = 0.75f;
    [SerializeField, Range(0f, 1f)] private float indexPinchMax = 0.35f;
    [SerializeField, Min(0f)] private float holdSeconds = 0.5f;
    [SerializeField, Min(0f)] private float repeatCooldownSeconds = 1.5f;
    [SerializeField] private bool requireHandTracked = true;
    [SerializeField] private bool logModeSwitch = true;
    [SerializeField] private WeaponTransitionEffects transitionEffects;

    private float heldTime;
    private float nextAllowedToggleTime;
    private bool loggedPoseDetected;

    private void Awake()
    {
        FindSceneReferences();
        EnsureTransitionEffects();
    }

    private void Update()
    {
        FindSceneReferences();

        bool poseActive = IsMiddleFingerPinchActive();
        if (!poseActive)
        {
            heldTime = 0f;
            loggedPoseDetected = false;
            transitionEffects?.HideTransition();
            return;
        }

        if (Time.time < nextAllowedToggleTime)
        {
            transitionEffects?.HideTransition();
            return;
        }

        if (logModeSwitch && !loggedPoseDetected)
        {
            loggedPoseDetected = true;
            Debug.Log($"Right-hand weapon toggle pose detected. Holding for {holdSeconds:0.00}s.", this);
        }

        heldTime += Time.deltaTime;
        CombatWeaponMode.WeaponMode targetMode = GetNextMode();
        Transform effectMount = GetEffectMount();
        if (transitionEffects != null && effectMount != null)
        {
            float progress = holdSeconds <= 0f ? 1f : Mathf.Clamp01(heldTime / holdSeconds);
            transitionEffects.ShowTransition(effectMount.position, effectMount.rotation, progress, targetMode);
        }

        if (heldTime < holdSeconds)
        {
            return;
        }

        heldTime = 0f;
        loggedPoseDetected = false;
        nextAllowedToggleTime = Time.time + repeatCooldownSeconds;
        ToggleWeaponMode();
        if (transitionEffects != null && effectMount != null)
        {
            transitionEffects.PlayCompletion(effectMount.position, effectMount.rotation, targetMode);
        }
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

        CombatWeaponMode.WeaponMode nextMode = GetNextMode();

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

        EnsureTransitionEffects();
    }

    private CombatWeaponMode.WeaponMode GetNextMode()
    {
        return weaponMode != null && weaponMode.ActiveMode == CombatWeaponMode.WeaponMode.ThermalBeam
            ? CombatWeaponMode.WeaponMode.IceGrenade
            : CombatWeaponMode.WeaponMode.ThermalBeam;
    }

    private Transform GetEffectMount()
    {
        // The OVRHand component can live on a detached tracking object. The anchor is
        // the same transform used by the actual weapons, so prefer it for visible VFX.
        GameObject rightHandAnchor = GameObject.Find("RightHandAnchor");
        if (rightHandAnchor != null)
        {
            return rightHandAnchor.transform;
        }

        return rightHand != null ? rightHand.transform : transform;
    }

    private void EnsureTransitionEffects()
    {
        if (transitionEffects != null)
        {
            return;
        }

        transitionEffects = GetComponent<WeaponTransitionEffects>();
        if (transitionEffects == null)
        {
            transitionEffects = gameObject.AddComponent<WeaponTransitionEffects>();
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
