using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public sealed class NetworkPlayerGrenadeVisual : NetworkBehaviour
{
    [SerializeField] private bool useIceEffects = true;

    [SyncVar] private bool ChargeVisible;
    [SyncVar] private Vector3 ChargePosition;
    [SyncVar] private Quaternion ChargeRotation;
    [SyncVar] private float ChargeProgress;
    [SyncVar] private bool GrenadeVisible;
    [SyncVar] private Vector3 GrenadeOrigin;
    [SyncVar] private Vector3 GrenadeVelocity;
    [SyncVar] private float GrenadeFloorY;
    [SyncVar] private bool ExplosionVisible;
    [SyncVar] private Vector3 ExplosionPosition;
    [SyncVar] private int ExplosionContactKind;
    [SyncVar] private Vector3 ExplosionContactPoint;
    [SyncVar] private int ExplosionSequence;

    private IceGrenadeEffects iceEffects;
    private IceGrenadeProjectile remoteProjectile;
    private int lastRenderedExplosionSequence;
    private bool remoteLaunchConsumed;

    public bool IsLocalPlayer => isOwned;

    public override void OnStartClient()
    {
        base.OnStartClient();
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
            iceEffects?.PlayExplosion(DecodeRemotePoint(ExplosionPosition),
                ExplosionContactKind, DecodeRemotePoint(ExplosionContactPoint));
        }
    }

    public void SubmitCharge(Vector3 position, Quaternion rotation, float progress)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            position = NetworkPlayerAlignment.InverseTransformPoint(position);
            rotation = NetworkPlayerAlignment.InverseTransformRotation(rotation);
        }

        if (isServer)
        {
            SetCharge(position, rotation, progress, true);
            return;
        }

        if (isOwned || isServer)
        {
            CmdSubmitCharge(position, rotation, progress, true);
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

        if (isServer)
        {
            SetThrow(origin, launchVelocity, floorWorldY, true);
            return;
        }

        if (isOwned || isServer)
        {
            CmdSubmitThrow(origin, launchVelocity, floorWorldY, true);
        }
    }

    public void SubmitExplosion(Vector3 position, int contactKind, Vector3 contactPoint)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            position = NetworkPlayerAlignment.InverseTransformPoint(position);
            contactPoint = NetworkPlayerAlignment.InverseTransformPoint(contactPoint);
        }

        if (isServer)
        {
            SetExplosion(position, contactKind, contactPoint, true);
            return;
        }

        if (isOwned || isServer)
        {
            CmdSubmitExplosion(position, contactKind, contactPoint, true);
        }
    }

    public void HideGrenade()
    {
        if (isServer)
        {
            ChargeVisible = false;
            GrenadeVisible = false;
            return;
        }

        if (isOwned || isServer)
        {
            CmdSubmitCharge(ChargePosition, ChargeRotation, ChargeProgress, false);
            CmdSubmitThrow(GrenadeOrigin, GrenadeVelocity, GrenadeFloorY, false);
        }
    }

    [Command]
    private void CmdSubmitCharge(Vector3 position, Quaternion rotation, float progress, bool visible)
    {
        SetCharge(position, rotation, progress, visible);
    }

    [Command]
    private void CmdSubmitThrow(Vector3 origin, Vector3 launchVelocity, float floorWorldY, bool visible)
    {
        SetThrow(origin, launchVelocity, floorWorldY, visible);
    }

    [Command]
    private void CmdSubmitExplosion(Vector3 position, int contactKind,
        Vector3 contactPoint, bool visible)
    {
        SetExplosion(position, contactKind, contactPoint, visible);
    }

    private void SetCharge(Vector3 position, Quaternion rotation, float progress, bool visible)
    {
        ChargePosition = position;
        ChargeRotation = rotation;
        ChargeProgress = progress;
        ChargeVisible = visible;
        if (visible)
        {
            // The next bomb may charge while the previous one is still flying.
            ExplosionVisible = false;
        }
    }

    private void SetThrow(Vector3 origin, Vector3 launchVelocity, float floorWorldY, bool visible)
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

    private void SetExplosion(Vector3 position, int contactKind,
        Vector3 contactPoint, bool visible)
    {
        ExplosionPosition = position;
        ExplosionContactKind = contactKind;
        ExplosionContactPoint = contactPoint;
        ExplosionVisible = visible;
        if (visible)
        {
            // Keep a new charge visible while the previous bomb detonates.
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
