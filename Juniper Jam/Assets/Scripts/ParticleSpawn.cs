using UnityEngine;

public class ParticleSpawn : MonoBehaviour
{
    [SerializeField] private ParticleSystem cookedParticle;
    [SerializeField] private ParticleSystem smokeParticle;

    private void Start()
    {
        transform.parent.GetComponent<FoodItem>().OnCooked += PlayCooked;
        transform.parent.GetComponent<FoodItem>().OnBurning += PlaySmoke;
    }

    private void OnDestroy()
    {
        transform.parent.GetComponent<FoodItem>().OnCooked -= PlayCooked;
        transform.parent.GetComponent<FoodItem>().OnBurning -= PlaySmoke;
    }

    public void PlayCooked(object sender, System.EventArgs e)
    {
        if (cookedParticle == null) return;
        cookedParticle.Play();
    }

    public void PlaySmoke(object sender, System.EventArgs e)
    {
        if (smokeParticle == null) return;
        smokeParticle.Play();
    }
}
