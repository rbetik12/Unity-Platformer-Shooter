using System.Collections.Generic;
using Placeables;
using Managers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Weapons;

namespace Player {
    public class PlayerShootingController : MonoBehaviour {
        [SerializeField] private GameObject bulletPrefab;

        private AbstractWeapon weapon;
        private Vector3 rotation;
        private PlaceablesController placeables;
        private GameManager gameManager;


        private void Start() {
            placeables = GameObject.Find("PlaceablesController").GetComponent<PlaceablesController>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            weapon = gameManager.weapon.GetWeapon(WeaponType.Shotgun);
            weapon.Create(transform);
            Assert.IsTrue(placeables != null);
            Assert.IsTrue(gameManager != null);
        }

        private void Update() {
            Aim();
            Shoot();
            if (Input.GetKeyDown(KeyCode.F)) {
                placeables.SpawnBomb(transform.position);
            }
        }

        private void Aim() {
            if (!IsPointerOverUIObject()) {
                rotation = Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
            }
        }

        private bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void Shoot() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                if (!IsPointerOverUIObject()) {
                    weapon.Shoot(bulletPrefab, rotation);
                }
            }
        }
    }
}