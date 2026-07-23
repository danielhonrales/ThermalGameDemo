using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkObject))]
public sealed class NetworkPlayerShieldVisual : NetworkBehaviour
{
    [SerializeField] private bool useShieldEffects = true;

    [Networked] private NetworkBool ShieldVisible { get; set; }
    [Networked] private Vector3 ShieldPosition { get; set; }
    [Networked] private Quaternion ShieldRotation { get; set; }

    private ForearmShieldEffects shieldEffects;
    private NetworkPlayerHealth playerHealth;

    public bool IsLocalPlayer => Object != null && Object.HasInputAuthority;

    public override void Spawned()
    {
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

        if (Object != null && Object.HasStateAuthority)
        {
            SetShield(active, position, rotation);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitShield(active, position, rotation);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitShield(NetworkBool active, Vector3 position, Quaternion rotation)
    {
        SetShield(active, position, rotation);
    }

    private void SetShield(NetworkBool active, Vector3 position, Quaternion rotation)
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
