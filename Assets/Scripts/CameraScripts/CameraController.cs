using UnityEngine;

namespace CameraScripts {
    public class CameraController : MonoBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float cameraBasicSize = 10f;
        [SerializeField] private float cameraScaleSpeed = 2f;

        private Rigidbody2D playerRb;

        void Start() {
            playerRb = player.GetComponent<Rigidbody2D>();
        }


        void Update() {
            if (player == null) return;
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            mainCamera.orthographicSize =
                Mathf.Clamp(
                    Mathf.MoveTowards(mainCamera.orthographicSize, cameraBasicSize + playerRb.velocity.magnitude,
                        cameraScaleSpeed * Time.deltaTime), 10, 13);
        }
    }
}