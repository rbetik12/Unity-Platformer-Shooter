using Placeables;
using Supervisor;
using UnityEngine;
using UnityEngine.Assertions;
using Weapons;

namespace Player {
    public class PlayerShootingController : MonoBehaviour {
        [SerializeField] private GameObject bulletObj;

        private AbstractWeapon weapon;
        private Vector3 rotation;
        private PlaceablesController placeables;
        private SupervisorController supervisor;
        

        private void Start() {
            placeables = GameObject.Find("PlaceablesController").GetComponent<PlaceablesController>(); 
            supervisor =  GameObject.Find("ObjectsSupervisor").GetComponent<SupervisorController>();
            weapon = supervisor.GetWeapon(WeaponType.Pistol);
            weapon.Create(transform);
            Assert.IsTrue(placeables != null);
            Assert.IsTrue(supervisor != null);
        }

        private void Update() {  
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
                GameObject bullet = Instantiate(bulletObj, weapon.GetFirePoint().position, weapon.GetFirePoint().rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y) * 20f;
            }
        }
    }
}