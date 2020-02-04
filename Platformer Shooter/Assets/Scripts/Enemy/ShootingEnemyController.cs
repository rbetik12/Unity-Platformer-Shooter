using UnityEngine;

public class ShootingEnemyController : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Player Bullet")) {
            Destroy(other.gameObject);
        }
    }
}
