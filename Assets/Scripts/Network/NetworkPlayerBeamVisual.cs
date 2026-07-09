using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkObject))]
public sealed class NetworkPlayerBeamVisual : NetworkBehaviour
{
    [SerializeField] private bool showLocalNetworkBeam = false;
    [SerializeField, Min(0.001f)] private float lineWidth = 0.025f;
    [SerializeField] private bool useThermalEffects = true;

    [Networked] private NetworkBool BeamVisible { get; set; }
    [Networked] private Vector3 BeamStart { get; set; }
    [Networked] private Vector3 BeamEnd { get; set; }
    [Networked] private Vector4 BeamColor { get; set; }
    [Networked] private NetworkBool ChargeVisible { get; set; }
    [Networked] private Vector3 ChargePosition { get; set; }
    [Networked] private Vector4 ChargeColor { get; set; }

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
        beamLine.enabled = visible;
        if (!visible)
        {
            bool showRemoteCharge = ChargeVisible && !IsLocalPlayer;
            if (thermalEffects != null && showRemoteCharge)
            {
                Color chargeColor = new Color(ChargeColor.x, ChargeColor.y, ChargeColor.z, ChargeColor.w);
                thermalEffects.ShowCharge(ChargePosition, chargeColor);
            }
            else if (thermalEffects != null)
            {
                thermalEffects.HideBeam();
            }

            return;
        }

        Color color = new Color(BeamColor.x, BeamColor.y, BeamColor.z, BeamColor.w);
        beamLine.startColor = color;
        beamLine.endColor = color;
        beamLine.SetPosition(0, BeamStart);
        beamLine.SetPosition(1, BeamEnd);

        if (thermalEffects != null)
        {
            thermalEffects.ShowBeam(BeamStart, BeamEnd, color, false);
        }
    }

    public void SubmitBeam(Vector3 start, Vector3 end, Color color)
    {
        Vector4 colorVector = new Vector4(color.r, color.g, color.b, color.a);

        if (Object != null && Object.HasStateAuthority)
        {
            SetBeam(start, end, colorVector, true);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitBeam(start, end, colorVector, true);
        }
    }

    public void SubmitCharge(Vector3 position, Color color)
    {
        Vector4 colorVector = new Vector4(color.r, color.g, color.b, color.a);

        if (Object != null && Object.HasStateAuthority)
        {
            SetCharge(position, colorVector, true);
            return;
        }

        if (Object != null)
        {
            RPC_SubmitCharge(position, colorVector, true);
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
            RPC_SubmitBeam(BeamStart, BeamEnd, BeamColor, false);
            RPC_SubmitCharge(ChargePosition, ChargeColor, false);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitBeam(Vector3 start, Vector3 end, Vector4 color, NetworkBool visible)
    {
        SetBeam(start, end, color, visible);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SubmitCharge(Vector3 position, Vector4 color, NetworkBool visible)
    {
        SetCharge(position, color, visible);
    }

    private void SetBeam(Vector3 start, Vector3 end, Vector4 color, NetworkBool visible)
    {
        BeamStart = start;
        BeamEnd = end;
        BeamColor = color;
        BeamVisible = visible;
        if (visible)
        {
            ChargeVisible = false;
        }
    }

    private void SetCharge(Vector3 position, Vector4 color, NetworkBool visible)
    {
        ChargePosition = position;
        ChargeColor = color;
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
