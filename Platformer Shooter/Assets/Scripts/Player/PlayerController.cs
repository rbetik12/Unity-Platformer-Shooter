using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float enemyBulletDamage = 20f;
    [SerializeField] private LevelLoader levelLoader;

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
            levelLoader.LoadLevel1();
            isAlive = false;
        }
    }
}
