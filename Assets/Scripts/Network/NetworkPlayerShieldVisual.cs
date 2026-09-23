using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public sealed class NetworkPlayerShieldVisual : NetworkBehaviour
{
    [SerializeField] private bool useShieldEffects = true;

    [SyncVar] private bool ShieldVisible;
    [SyncVar] private Vector3 ShieldPosition;
    [SyncVar] private Quaternion ShieldRotation;

    private ForearmShieldEffects shieldEffects;
    private NetworkPlayerHealth playerHealth;

    public bool IsLocalPlayer => isOwned;

    public override void OnStartClient()
    {
        base.OnStartClient();
        EnsureShieldEffects();
        playerHealth = GetComponent<NetworkPlayerHealth>();
    }

    private void LateUpdate()
    {
        if (IsLocalPlayer)
        {
            return;
        }

        EnsureShieldEffects();
        if (ShieldVisible)
        {
            Vector3 position = NetworkPlayerAlignment.HasCalibration
                ? NetworkPlayerAlignment.TransformPoint(ShieldPosition)
                : ShieldPosition;
            Quaternion rotation = NetworkPlayerAlignment.HasCalibration
                ? NetworkPlayerAlignment.TransformRotation(ShieldRotation)
                : ShieldRotation;
            shieldEffects?.ShowShield(RemotePlayerCorrection.Apply(position), rotation);
        }
        else
        {
            shieldEffects?.HideShield();
        }
    }

    public void SubmitShield(bool active, Vector3 position, Quaternion rotation)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            position = NetworkPlayerAlignment.InverseTransformPoint(position);
            rotation = NetworkPlayerAlignment.InverseTransformRotation(rotation);
        }

        if (isServer)
        {
            SetShield(active, position, rotation);
            return;
        }

        if (isOwned || isServer)
        {
            CmdSubmitShield(active, position, rotation);
        }
    }

    [Command]
    private void CmdSubmitShield(bool active, Vector3 position, Quaternion rotation)
    {
        SetShield(active, position, rotation);
    }

    private void SetShield(bool active, Vector3 position, Quaternion rotation)
    {
        ShieldVisible = active;
        ShieldPosition = position;
        ShieldRotation = rotation;

        if (playerHealth != null)
        {
            playerHealth.SetShieldActive(active);
        }
    }

    private void EnsureShieldEffects()
    {
        if (!useShieldEffects || shieldEffects != null)
        {
            return;
        }

        shieldEffects = gameObject.GetComponent<ForearmShieldEffects>();
        if (shieldEffects == null)
        {
            shieldEffects = gameObject.AddComponent<ForearmShieldEffects>();
        }
    }
}
