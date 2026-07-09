using UnityEngine;

public sealed class OrbController : MonoBehaviour
{
    public GameObject hand;
    public Vector3 followOffset;
    public float followSpeed = 12f;
    public AudioSource chargeAudio;
    public AudioSource dischargeAudio;
    public AudioSource hotAudio;
    public AudioSource coldAudio;
    public AudioSource hotBeamAudio;
    public AudioSource coldBeamAudio;
    public AudioSource beamChargeAudio;
    public GameObject visuals;

    public void PlayChargeAudio()
    {
        if (chargeAudio != null)
        {
            chargeAudio.Play();
        }
    }

    public void PlayDischargeAudio(bool hot)
    {
        if (beamChargeAudio != null)
        {
            beamChargeAudio.Play();
        }

        AudioSource elementAudio = hot ? hotAudio : coldAudio;
        if (elementAudio != null)
        {
            elementAudio.Play();
        }

        AudioSource beamAudio = hot ? hotBeamAudio : coldBeamAudio;
        if (beamAudio != null)
        {
            beamAudio.Play();
        }
    }
}
