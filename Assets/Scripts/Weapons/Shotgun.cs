using UnityEngine;

namespace Weapons {
    public class Shotgun : AbstractWeapon {
        private const int FirePointsAmount = 3;
        private readonly Vector3 shotgunOffset = new Vector3(0.5f, 0.2f, 0);
        private readonly GameObject[] firePoints = new GameObject[3];

        public Shotgun(GameObject weaponObj) {
            weapon = weaponObj;
            type = WeaponType.Shotgun;
        }

        public override WeaponType GetType() {
            return type;
        }

        public override Transform GetFirePoint() {
            return firePoint.transform;
        }

        public override void Create(Transform transform) {
            weapon = Object.Instantiate(weapon, transform.position + shotgunOffset, Quaternion.Euler(0, 0, 0));
            weapon.transform.SetParent(transform);
            firePoint = weapon.transform.GetChild(0).gameObject;
            SetFirePoints();
        }

        public override void Shoot(GameObject bulletObj, Vector3 rotation) {
            for (int i = 0; i < FirePointsAmount; i++) {
                GameObject bullet = Object.Instantiate(bulletObj, firePoints[i].transform.position,
                    firePoints[i].transform.rotation);
                int randomOffset = Random.Range(-i - 1, i + 1);
                bullet.GetComponent<Rigidbody2D>().velocity = rotation * 20f + new Vector3(randomOffset , randomOffset ,0);
            }
        }

        private void SetFirePoints() {
            for (int i = 0; i < FirePointsAmount; i++) {
                firePoints[i] = firePoint.transform.GetChild(i).gameObject;
            }
        }
    }
}