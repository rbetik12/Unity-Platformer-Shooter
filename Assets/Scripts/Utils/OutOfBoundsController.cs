using Supervisor;
using UnityEngine;

namespace Utils {
    public class OutOfBoundsController : MonoBehaviour {
        private SupervisorController supervisorController;
        private void Start() {
            supervisorController = GameObject.Find("ObjectsSupervisor").GetComponent<SupervisorController>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name.Contains("Bullet")) {
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("Player")) {
                supervisorController.GetPlayerController().OutOfBoundsCollision();
            }
        }
    }
}