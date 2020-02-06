using UnityEngine;

public class ShootingEnemyController : MonoBehaviour {

    [SerializeField] private ParticlesController particlesController;

    private SpriteRenderer spriteRenderer;
    private float hp = 100f;
    private float playerDamage = 50f;
    private bool isAlive = true;
    private Color damagedColor;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        damagedColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Player Bullet")) {
            Destroy(other.gameObject);
            GetDamage();
        }
    }

    private void GetDamage() {
        hp -= playerDamage;
        float healthRatio = Mathf.Clamp(hp / 100f, 0.1f, 1f);
        damagedColor = spriteRenderer.color * healthRatio;
        damagedColor.a = 1f;
    }

    private void Update() {
        if (isAlive) {
            CheckHealth();
        } else
            return;
    }

    private void CheckHealth() {
        if (hp <= 0) {
            OnDeath();
        }
        if (spriteRenderer.color != damagedColor)
            ChangeColor();
    }

    private void ChangeColor() {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, damagedColor, 0.2f);
    }

    private void OnDeath() {
        isAlive = false;
        Destroy(this.gameObject);
        particlesController.EnemyDeathParticles(transform.position, spriteRenderer.color);
    }
}