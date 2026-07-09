using UnityEngine;

public sealed class EnemyController : MonoBehaviour
{
    public Rigidbody rb;
    public ParticleSystem deathParticles;
    public AudioSource deathAudio;
    public AudioSource boopAudio;
    public AudioSource hoverAudio;
    public GameObject target;

    public void PlayDeathEffects()
    {
        if (target != null)
        {
            target.SetActive(false);
        }

        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        if (deathAudio != null)
        {
            deathAudio.Play();
        }

        if (hoverAudio != null)
        {
            hoverAudio.Stop();
        }
    }
}
