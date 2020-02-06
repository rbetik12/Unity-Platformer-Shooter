using UnityEngine;

public class ParticlesController : MonoBehaviour {
    [SerializeField] private ParticleSystem playerDeathParticles;
    [SerializeField] private ParticleSystem enemyDeathParticles;

    public void PlayerDeathParticles(Vector3 deathPosition) {
        playerDeathParticles.transform.position = deathPosition;
        playerDeathParticles.Play();
    }

    public void EnemyDeathParticles(Vector3 deathPosition, Color particlesColor) {
        enemyDeathParticles.transform.position = deathPosition;
        ParticleSystem.MainModule ma = enemyDeathParticles.main;
        ma.startColor = particlesColor;
        enemyDeathParticles.Play();
    }
}