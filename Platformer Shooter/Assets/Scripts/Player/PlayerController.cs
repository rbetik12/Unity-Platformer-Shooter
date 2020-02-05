using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

    [SerializeField] private float enemyBulletDamage = 100f;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private Image healthbar;
    [SerializeField] private ParticlesController particlesController;

    private float hp = 100f;

    private bool isAlive = true;

    private void Update() {
        CheckHealth();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Enemy Bullet")) {
            hp -= enemyBulletDamage;
            Destroy(other.gameObject);
        }
    }

    private void CheckHealth() {
        if (!isAlive) return;
        if (hp <= 0) {
            OnDeath();
        }
        ScaleHealthBar();
    }

    private void ScaleHealthBar() {
        healthbar.transform.localScale = new Vector3(hp / 100, 1, 1);
    }

    private void OnDeath() {
        Destroy(this.gameObject);
        particlesController.PlayerDeathParticles(transform.position);
        isAlive = false;
    }
}