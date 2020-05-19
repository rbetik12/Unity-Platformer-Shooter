using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float enemyBulletDamage = 100f;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private Image healthbar;
    [SerializeField] private ParticlesController particlesController;
    [SerializeField] private GameUIContoller gameUIContoller;

    private Tilemap tilemap;
    private float hp = 100f;
    private Rigidbody2D rb;
    private Vector3 speed;
    private bool isAlive = true;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        tilemap = GameObject.Find("Map").GetComponentInChildren<Tilemap>();
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
        if (other.gameObject.name.Contains("Tilemap")) {
            if (other.contactCount > 0) {
                particlesController.PlayerCollidePatricles(other.GetContact(0).point, new Color(74 / 256f, 74 / 256f, 74 / 256f, 1f));
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

    public void GetBombDamage() {
        hp -= 50f;
    }
}