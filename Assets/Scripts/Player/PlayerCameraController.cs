using UnityEngine;

namespace Player {
    public class PlayerCameraController : MonoBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private Camera playerCamera;

        private Rigidbody2D playerRb;
        
        private const float CameraBasicSize = 10f; 
        private const float CameraScaleSpeed = 2f;

        private void Start() {
            playerRb = player.GetComponent<Rigidbody2D>();
        }


        private void Update() {
            if (player == null) return;
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            playerCamera.orthographicSize =
                Mathf.Clamp(
                    Mathf.MoveTowards(playerCamera.orthographicSize, CameraBasicSize + playerRb.velocity.magnitude,
                        CameraScaleSpeed * Time.deltaTime), 10, 13);
        }
    }
}