using UnityEngine;

public class Bullet : MonoBehaviour {
    
    private ParticlesController particlesController;

    private void Start() {
        particlesController = GameObject.Find("Particles Controller").GetComponent<ParticlesController>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name.Contains("Tilemap")) {
            Destroy(this.gameObject);
            particlesController.BulletDestroyPatricles(other.GetContact(0).point);
        }
    }
}
