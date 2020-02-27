using UnityEngine;

public class ParticlesController : MonoBehaviour {
    [SerializeField] private ParticleSystem playerDeathParticles;
    [SerializeField] private ParticleSystem enemyDeathParticles;
    [SerializeField] private ParticleSystem playerCollisionParticles;
    [SerializeField] private ParticleSystem bulletDestroyPatricles;

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

    public void PlayerCollidePatricles(Vector3 pos) {
        pos = new Vector3(pos.x, pos.y + 0.2f, pos.z);
        playerCollisionParticles.transform.position = pos;
        playerCollisionParticles.Play();
    }
    public void BulletDestroyPatricles(Vector3 pos) {
        // pos = new Vector3(pos.x, pos.y + 0.2f, pos.z);
        bulletDestroyPatricles.transform.position = pos;
        bulletDestroyPatricles.Play();
    }
}