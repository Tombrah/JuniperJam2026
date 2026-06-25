using UnityEngine;

public class ParticleSpawn : MonoBehaviour
{
    [SerializeField] private ParticleSystem cookedParticle;
    [SerializeField] private ParticleSystem smokeParticle;

    public void PlayCooked()
    {
        cookedParticle.Play();
    }

    public void PlaySmoke()
    {
        smokeParticle.Play();
    }
}
