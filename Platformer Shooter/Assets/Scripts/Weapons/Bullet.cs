using UnityEngine;

public class Bullet : MonoBehaviour {
    
    private ParticlesController particlesController;

    private void Start() {
        particlesController = GameObject.Find("Particles Controller").GetComponent<ParticlesController>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag.Equals("Floor")) {
            Debug.Log("Bullet collision with floor");
            Destroy(this.gameObject);
            particlesController.BulletDestroyPatricles(other.GetContact(0).point);
        }
    }
}
