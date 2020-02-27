using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

    [SerializeField] private float enemyBulletDamage = 100f;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private Image healthbar;
    [SerializeField] private ParticlesController particlesController;
    [SerializeField] private GameUIContoller gameUIContoller;

    private float hp = 100f;
    private Rigidbody2D rb;
    private Vector3 speed;
    private bool isAlive = true;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        speed = rb.velocity;
    }

    private void Update() {
        CheckHealth();
    }

    private void CheckHealth() {
        if (!isAlive) return;
        if (hp <= 0) {
            OnDeath();
        }
        ScaleHealthBar();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag.Equals("Floor")) {
            if (other.contactCount > 0) {
                particlesController.PlayerCollidePatricles(other.GetContact(0).point);
            }
        }

        if (other.gameObject.tag.Equals("Enemy Bullet")) {
            hp -= enemyBulletDamage;
            Destroy(other.gameObject);
            rb.velocity = speed;
        }
    }

    

    private void ScaleHealthBar() {
        healthbar.transform.localScale = new Vector3(hp / 100, 1, 1);
    }

    private void OnDeath() {
        Destroy(this.gameObject);
        particlesController.PlayerDeathParticles(transform.position);
        isAlive = false;
        gameUIContoller.OnPlayerDeath();
    }
}