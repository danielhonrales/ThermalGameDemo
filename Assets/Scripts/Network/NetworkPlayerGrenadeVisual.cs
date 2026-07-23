using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkObject))]
public sealed class NetworkPlayerGrenadeVisual : NetworkBehaviour
{
    [SerializeField] private bool useIceEffects = true;

    [Networked] private NetworkBool ChargeVisible { get; set; }
    [Networked] private Vector3 ChargePosition { get; set; }
    [Networked] private Quaternion ChargeRotation { get; set; }
    [Networked] private float ChargeProgress { get; set; }
    [Networked] private NetworkBool GrenadeVisible { get; set; }
    [Networked] private Vector3 GrenadeOrigin { get; set; }
    [Networked] private Vector3 GrenadeVelocity { get; set; }
    [Networked] private float GrenadeFloorY { get; set; }
    [Networked] private NetworkBool ExplosionVisible { get; set; }
    [Networked] private Vector3 ExplosionPosition { get; set; }
    [Networked] private int ExplosionSequence { get; set; }

    private IceGrenadeEffects iceEffects;
    private IceGrenadeProjectile remoteProjectile;
    private int lastRenderedExplosionSequence;
    private bool remoteLaunchConsumed;

    public bool IsLocalPlayer => Object != null && Object.HasInputAuthority;

    public override void Spawned()
    {
        EnsureIceEffects();
        EnsureRemoteProjectile();
    }

    private void LateUpdate()
    {
        if (IsLocalPlayer)
        {
            return;
        }

        EnsureIceEffects();
        EnsureRemoteProjectile();

        if (ChargeVisible)
        {
            iceEffects?.ShowCharge(DecodeRemotePoint(ChargePosition), DecodeRemoteRotation(ChargeRotation), ChargeProgress);
        }
        else
        {
            iceEffects?.HideCharge();
        }

        if (GrenadeVisible && !remoteLaunchConsumed && remoteProjectile != null && !remoteProjectile.IsAlive)
        {
            remoteLaunchConsumed = true;
            Transform visual = iceEffects != null ? iceEffects.CreateGrenadeVisual(remoteProjectile.transform) : null;
            Vector3 correctedOrigin = DecodeRemotePoint(GrenadeOrigin);
            Vector3 correctedVelocity = DecodeRemoteDirection(GrenadeVelocity);
            float correctedFloorY = DecodeRemoteFloorY(GrenadeFloorY) + RemotePlayerCorrection.WorldOffset.y;
            remoteProjectile.Launch(correctedOrigin, correctedVelocity, CombatLayers.CombatHitMask, visual, OnRemoteExploded, 1f, correctedFloorY);
            iceEffects?.StartFlightTrail(remoteProjectile.transform);
        }
        else if (!GrenadeVisible)
        {
            remoteLaunchConsumed = false;
            if (remoteProjectile != null && remoteProjectile.IsAlive)
            {
                remoteProjectile.enabled = false;
                remoteProjectile.gameObject.SetActive(false);
                iceEffects?.StopFlightTrail();
            }
        }

        if (ExplosionSequence != lastRenderedExplosionSequence)
        {
            lastRenderedExplosionSequence = ExplosionSequence;
            iceEffects?.PlayExplosion(DecodeRemotePoint(ExplosionPosition));
        }
    }

    public void SubmitCharge(Vector3 position, Quaternion rotation, float progress)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            position = NetworkPlayerAlignment.InverseTransformPoint(position);
            rotation = NetworkPlayerAlignment.InverseTransformRotation(rotation);
        }

        if (Object != null && Object.HasStateAuthority)
        {
            SetCharge(position, rotation, progress, true);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitCharge(position, rotation, progress, true);
        }
    }

    public void SubmitThrow(Vector3 origin, Vector3 launchVelocity, float floorWorldY)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            origin = NetworkPlayerAlignment.InverseTransformPoint(origin);
            launchVelocity = NetworkPlayerAlignment.InverseTransformDirection(launchVelocity);
            floorWorldY = NetworkPlayerAlignment.InverseTransformPoint(new Vector3(0f, floorWorldY, 0f)).y;
        }

        if (Object != null && Object.HasStateAuthority)
        {
            SetThrow(origin, launchVelocity, floorWorldY, true);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitThrow(origin, launchVelocity, floorWorldY, true);
        }
    }

    public void SubmitExplosion(Vector3 position)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            position = NetworkPlayerAlignment.InverseTransformPoint(position);
        }

        if (Object != null && Object.HasStateAuthority)
        {
            SetExplosion(position, true);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitExplosion(position, true);
        }
    }

    public void HideGrenade()
    {
        if (Object != null && Object.HasStateAuthority)
        {
            ChargeVisible = false;
            GrenadeVisible = false;
            return;
        }

        if (Object != null)
        {
            RPC_SubmitCharge(ChargePosition, ChargeRotation, ChargeProgress, false);
            RPC_SubmitThrow(GrenadeOrigin, GrenadeVelocity, GrenadeFloorY, false);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitCharge(Vector3 position, Quaternion rotation, float progress, NetworkBool visible)
    {
        SetCharge(position, rotation, progress, visible);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitThrow(Vector3 origin, Vector3 launchVelocity, float floorWorldY, NetworkBool visible)
    {
        SetThrow(origin, launchVelocity, floorWorldY, visible);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitExplosion(Vector3 position, NetworkBool visible)
    {
        SetExplosion(position, visible);
    }

    private void SetCharge(Vector3 position, Quaternion rotation, float progress, NetworkBool visible)
    {
        ChargePosition = position;
        ChargeRotation = rotation;
        ChargeProgress = progress;
        ChargeVisible = visible;
        if (visible)
        {
            GrenadeVisible = false;
            ExplosionVisible = false;
        }
    }

    private void SetThrow(Vector3 origin, Vector3 launchVelocity, float floorWorldY, NetworkBool visible)
    {
        GrenadeOrigin = origin;
        GrenadeVelocity = launchVelocity;
        GrenadeFloorY = floorWorldY;
        GrenadeVisible = visible;
        if (visible)
        {
            ChargeVisible = false;
            ExplosionVisible = false;
        }
    }

    private void SetExplosion(Vector3 position, NetworkBool visible)
    {
        ExplosionPosition = position;
        ExplosionVisible = visible;
        if (visible)
        {
            ChargeVisible = false;
            GrenadeVisible = false;
            ExplosionSequence++;
        }
    }

    private void OnRemoteExploded(Vector3 position, Collider hitCollider)
    {
        iceEffects?.StopFlightTrail();
    }

    private void EnsureIceEffects()
    {
        if (!useIceEffects || iceEffects != null)
        {
            return;
        }

        iceEffects = gameObject.GetComponent<IceGrenadeEffects>();
        if (iceEffects == null)
        {
            iceEffects = gameObject.AddComponent<IceGrenadeEffects>();
        }
    }

    private void EnsureRemoteProjectile()
    {
        if (remoteProjectile != null)
        {
            return;
        }

        GameObject projectileObject = new GameObject("RemoteIceGrenadeProjectile");
        projectileObject.transform.SetParent(transform, false);
        remoteProjectile = projectileObject.AddComponent<IceGrenadeProjectile>();
        projectileObject.SetActive(false);
    }

    private static Vector3 DecodeRemotePoint(Vector3 point)
    {
        Vector3 worldPoint = NetworkPlayerAlignment.HasCalibration
            ? NetworkPlayerAlignment.TransformPoint(point)
            : point;
        return RemotePlayerCorrection.Apply(worldPoint);
    }

    private static Quaternion DecodeRemoteRotation(Quaternion rotation)
    {
        return NetworkPlayerAlignment.HasCalibration
            ? NetworkPlayerAlignment.TransformRotation(rotation)
            : rotation;
    }

    private static Vector3 DecodeRemoteDirection(Vector3 direction)
    {
        return NetworkPlayerAlignment.HasCalibration
            ? NetworkPlayerAlignment.TransformDirection(direction)
            : direction;
    }

    private static float DecodeRemoteFloorY(float floorY)
    {
        return NetworkPlayerAlignment.HasCalibration
            ? NetworkPlayerAlignment.TransformPoint(new Vector3(0f, floorY, 0f)).y
            : floorY;
    }
}
