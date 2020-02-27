using UnityEngine;

public class Bullet : MonoBehaviour {
    
    private ParticlesController particlesController;

    private void Start() {
        particlesController = GameObject.Find("Particles Controller").GetComponent<ParticlesController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Floor")) {
            Debug.Log("Trigger");
            Destroy(this.gameObject);
        }
    }
}
