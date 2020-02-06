using UnityEngine;

public class ShootingEnemyController : MonoBehaviour {

    private float hp = 100f;

    private float playerDamage = 50f;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Player Bullet")) {
            GetDamage();
        }
    }

    private void GetDamage() {
        hp -= playerDamage;
    }

    private void Update() {
        CheckHealth();
    }

    private void CheckHealth() {
        if (hp < 0) {
            Destroy(this.gameObject);
        }
    }
}
