using UnityEngine;

namespace Player {
    public class PlayerShooting : MonoBehaviour {
        [SerializeField] private GameObject bulletObj;
        [SerializeField] private GameObject firePoint;

        private Vector3 rotation;
        private PlaceablesController placeables;

        void Start() {
            placeables = GameObject.Find("PlaceablesController").GetComponent<PlaceablesController>();
        } 

        void Update() {
            Aim();
            Shoot();
            if (Input.GetKeyDown(KeyCode.F)) {
                placeables.SpawnBomb(transform.position);
            }
        } 

        private void Aim() {
            rotation = Vector3.Normalize(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
        }

        private void Shoot() {
            if (Input.GetMouseButtonDown(0)) {
                GameObject bullet = Instantiate(bulletObj, firePoint.transform.position, firePoint.transform.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y) * 20f;
            }
        }
    }
}