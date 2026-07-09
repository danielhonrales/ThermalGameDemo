using UnityEngine;

public sealed class BeamController : MonoBehaviour
{
    [Header("Original ThermalInMotion References")]
    public GameObject hand;
    public Vector3 followOffset;
    public Vector3 directionOffset;
    public float followSpeed = 12f;
    public GameObject hotBeam;
    public GameObject coldBeam;
    public ParticleSystem chargeParticles;
    public GameObject beamCollider;

    [Header("Runtime State")]
    [SerializeField] private bool hotActive;
    [SerializeField] private bool coldActive;
    [SerializeField] private bool chargeActive;

    private void Awake()
    {
        ApplyState();
    }

    private void LateUpdate()
    {
        if (hand == null)
        {
            return;
        }

        Vector3 targetPosition = hand.transform.position
            + followOffset.x * hand.transform.right
            + followOffset.y * hand.transform.up
            + followOffset.z * hand.transform.forward;

        transform.SetPositionAndRotation(
            Vector3.MoveTowards(transform.position, targetPosition, Vector3.Distance(transform.position, targetPosition) * followSpeed * Time.deltaTime),
            Quaternion.LookRotation(hand.transform.up + directionOffset, Vector3.up));
    }

    public void SetHotActive(bool active)
    {
        hotActive = active;
        if (active)
        {
            coldActive = false;
        }

        ApplyState();
    }

    public void SetColdActive(bool active)
    {
        coldActive = active;
        if (active)
        {
            hotActive = false;
        }

        ApplyState();
    }

    public void SetChargeActive(bool active)
    {
        chargeActive = active;
        ApplyState();
    }

    public void SetAllInactive()
    {
        hotActive = false;
        coldActive = false;
        chargeActive = false;
        ApplyState();
    }

    public void SetBeamMode(bool useHotBeam, bool active)
    {
        hotActive = active && useHotBeam;
        coldActive = active && !useHotBeam;
        ApplyState();
    }

    private void ApplyState()
    {
        if (hotBeam != null)
        {
            hotBeam.SetActive(hotActive);
        }

        if (coldBeam != null)
        {
            coldBeam.SetActive(coldActive);
        }

        if (beamCollider != null)
        {
            beamCollider.SetActive(hotActive || coldActive);
        }

        if (chargeParticles == null)
        {
            return;
        }

        if (chargeActive && !chargeParticles.isPlaying)
        {
            chargeParticles.Play();
        }
        else if (!chargeActive && chargeParticles.isPlaying)
        {
            chargeParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
