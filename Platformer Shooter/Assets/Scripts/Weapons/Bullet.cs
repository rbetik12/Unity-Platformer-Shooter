using UnityEngine;

public class Bullet : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Floor")) {
            Destroy(this.gameObject);
        }
    }
}
