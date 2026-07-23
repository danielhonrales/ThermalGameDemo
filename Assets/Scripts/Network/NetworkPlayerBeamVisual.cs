using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkObject))]
public sealed class NetworkPlayerBeamVisual : NetworkBehaviour
{
    [SerializeField] private bool showLocalNetworkBeam = false;
    [SerializeField] private bool showSimpleLineRenderer;
    [SerializeField, Min(0.001f)] private float lineWidth = 0.025f;
    [SerializeField] private bool useThermalEffects = true;

    [Networked] private NetworkBool BeamVisible { get; set; }
    [Networked] private Vector3 BeamStart { get; set; }
    [Networked] private Vector3 BeamEnd { get; set; }
    [Networked] private Vector3 BeamMountPosition { get; set; }
    [Networked] private Quaternion BeamMountRotation { get; set; }
    [Networked] private NetworkBool BeamHitSomething { get; set; }
    [Networked] private Vector3 BeamHitPoint { get; set; }
    [Networked] private Vector4 BeamColor { get; set; }
    [Networked] private NetworkBool ChargeVisible { get; set; }
    [Networked] private Vector3 ChargePosition { get; set; }
    [Networked] private Quaternion ChargeRotation { get; set; }
    [Networked] private Vector4 ChargeColor { get; set; }
    [Networked] private float ChargeProgress { get; set; }

    private LineRenderer beamLine;
    private ThermalBeamEffects thermalEffects;

    public bool IsLocalPlayer => Object != null && Object.HasInputAuthority;

    public override void Spawned()
    {
        EnsureLineRenderer();
        EnsureThermalEffects();
    }

    private void LateUpdate()
    {
        EnsureLineRenderer();

        bool visible = BeamVisible && (!IsLocalPlayer || showLocalNetworkBeam);
        beamLine.enabled = visible && showSimpleLineRenderer;
        if (!visible)
        {
            bool showRemoteCharge = ChargeVisible && !IsLocalPlayer;
            if (thermalEffects != null && showRemoteCharge)
            {
                Color chargeColor = new Color(ChargeColor.x, ChargeColor.y, ChargeColor.z, ChargeColor.w);
                Vector3 correctedChargePosition = DecodeRemotePoint(ChargePosition);
                Quaternion correctedChargeRotation = DecodeRemoteRotation(ChargeRotation);
                thermalEffects.ShowCharge(correctedChargePosition, chargeColor, ChargeProgress, correctedChargePosition, correctedChargeRotation);
            }
            else if (thermalEffects != null)
            {
                thermalEffects.HideBeam();
            }

            return;
        }

        Color color = new Color(BeamColor.x, BeamColor.y, BeamColor.z, BeamColor.w);
        Vector3 beamStart = IsLocalPlayer ? BeamStart : DecodeRemotePoint(BeamStart);
        Vector3 beamEnd = IsLocalPlayer ? BeamEnd : DecodeRemotePoint(BeamEnd);
        Vector3 hitPoint = IsLocalPlayer ? BeamHitPoint : DecodeRemotePoint(BeamHitPoint);
        Vector3 mountPosition = IsLocalPlayer ? BeamMountPosition : DecodeRemotePoint(BeamMountPosition);
        Quaternion mountRotation = IsLocalPlayer ? BeamMountRotation : DecodeRemoteRotation(BeamMountRotation);
        beamLine.startColor = color;
        beamLine.endColor = color;
        beamLine.SetPosition(0, beamStart);
        beamLine.SetPosition(1, beamEnd);

        if (thermalEffects != null)
        {
            thermalEffects.ShowBeam(beamStart, beamEnd, color, BeamHitSomething, hitPoint, mountPosition, mountRotation);
        }
    }

    public void SubmitBeam(Vector3 start, Vector3 end, Color color)
    {
        SubmitBeam(start, end, color, false, end, start, Quaternion.LookRotation((end - start).sqrMagnitude > 0.0001f ? (end - start).normalized : Vector3.forward, Vector3.up));
    }

    public void SubmitBeam(Vector3 start, Vector3 end, Color color, Vector3 mountPosition, Quaternion mountRotation)
    {
        SubmitBeam(start, end, color, false, end, mountPosition, mountRotation);
    }

    public void SubmitBeam(Vector3 start, Vector3 end, Color color, bool hitSomething, Vector3 hitPoint, Vector3 mountPosition, Quaternion mountRotation)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            start = NetworkPlayerAlignment.InverseTransformPoint(start);
            end = NetworkPlayerAlignment.InverseTransformPoint(end);
            hitPoint = NetworkPlayerAlignment.InverseTransformPoint(hitPoint);
            mountPosition = NetworkPlayerAlignment.InverseTransformPoint(mountPosition);
            mountRotation = NetworkPlayerAlignment.InverseTransformRotation(mountRotation);
        }

        Vector4 colorVector = new Vector4(color.r, color.g, color.b, color.a);

        if (Object != null && Object.HasStateAuthority)
        {
            SetBeam(start, end, mountPosition, mountRotation, hitSomething, hitPoint, colorVector, true);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitBeam(start, end, mountPosition, mountRotation, hitSomething, hitPoint, colorVector, true);
        }
    }

    public void SubmitCharge(Vector3 position, Color color, float progress)
    {
        SubmitCharge(position, Quaternion.identity, color, progress);
    }

    public void SubmitCharge(Vector3 position, Quaternion rotation, Color color, float progress)
    {
        if (NetworkPlayerAlignment.HasCalibration)
        {
            position = NetworkPlayerAlignment.InverseTransformPoint(position);
            rotation = NetworkPlayerAlignment.InverseTransformRotation(rotation);
        }

        Vector4 colorVector = new Vector4(color.r, color.g, color.b, color.a);

        if (Object != null && Object.HasStateAuthority)
        {
            SetCharge(position, rotation, colorVector, progress, true);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitCharge(position, rotation, colorVector, progress, true);
        }
    }

    public void HideBeam()
    {
        if (Object != null && Object.HasStateAuthority)
        {
            BeamVisible = false;
            ChargeVisible = false;
            return;
        }

        if (Object != null)
        {
            RPC_SubmitBeam(BeamStart, BeamEnd, BeamMountPosition, BeamMountRotation, BeamHitSomething, BeamHitPoint, BeamColor, false);
            RPC_SubmitCharge(ChargePosition, ChargeRotation, ChargeColor, ChargeProgress, false);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitBeam(Vector3 start, Vector3 end, Vector3 mountPosition, Quaternion mountRotation, NetworkBool hitSomething, Vector3 hitPoint, Vector4 color, NetworkBool visible)
    {
        SetBeam(start, end, mountPosition, mountRotation, hitSomething, hitPoint, color, visible);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitCharge(Vector3 position, Quaternion rotation, Vector4 color, float progress, NetworkBool visible)
    {
        SetCharge(position, rotation, color, progress, visible);
    }

    private void SetBeam(Vector3 start, Vector3 end, Vector3 mountPosition, Quaternion mountRotation, NetworkBool hitSomething, Vector3 hitPoint, Vector4 color, NetworkBool visible)
    {
        BeamStart = start;
        BeamEnd = end;
        BeamMountPosition = mountPosition;
        BeamMountRotation = mountRotation;
        BeamHitSomething = hitSomething;
        BeamHitPoint = hitPoint;
        BeamColor = color;
        BeamVisible = visible;
        if (visible)
        {
            ChargeVisible = false;
        }
    }

    private void SetCharge(Vector3 position, Quaternion rotation, Vector4 color, float progress, NetworkBool visible)
    {
        ChargePosition = position;
        ChargeRotation = rotation;
        ChargeColor = color;
        ChargeProgress = progress;
        ChargeVisible = visible;
        if (visible)
        {
            BeamVisible = false;
        }
    }

    private void EnsureLineRenderer()
    {
        if (beamLine != null)
        {
            return;
        }

        GameObject lineObject = new GameObject("NetworkBeamLine");
        lineObject.transform.SetParent(transform, false);
        beamLine = lineObject.AddComponent<LineRenderer>();
        beamLine.positionCount = 2;
        beamLine.useWorldSpace = true;
        beamLine.startWidth = lineWidth;
        beamLine.endWidth = lineWidth;
        beamLine.material = new Material(Shader.Find("Sprites/Default"));
        beamLine.enabled = false;
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

    private void EnsureThermalEffects()
    {
        if (!useThermalEffects || thermalEffects != null)
        {
            return;
        }

        thermalEffects = gameObject.GetComponent<ThermalBeamEffects>();
        if (thermalEffects == null)
        {
            thermalEffects = gameObject.AddComponent<ThermalBeamEffects>();
        }
    }
}
