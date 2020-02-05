using UnityEngine;

public class ParticlesController : MonoBehaviour {
    [SerializeField] private ParticleSystem playerDeathParticles;

    public void PlayerDeathParticles(Vector3 deathPosition) {
        playerDeathParticles.transform.position = deathPosition;
        playerDeathParticles.Play();
    }
}