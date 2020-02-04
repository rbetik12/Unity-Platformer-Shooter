using UnityEngine;

public class PlayerController : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Enemy Bullet")) {
            Destroy(other.gameObject);
        }
    }
}
