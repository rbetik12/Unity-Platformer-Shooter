using UnityEngine;

namespace Weapons {
    public class Pistol : AbstractWeapon {

        private readonly Vector3 firePointOffset = new Vector3(-6f, -0.05f, 0);
        
        public Pistol(GameObject weaponObj) {
            weapon = weaponObj;
            type = WeaponType.Pistol;
        }
        public override WeaponType GetType() {
            return type;
        }

        public override Transform GetFirePoint() {
            return firePoint.transform;
        }

        public override void Create(Transform transform) {
            weapon = Object.Instantiate(weapon, transform.position + new Vector3(0.5f, 0, 0), Quaternion.Euler(0, 0, 0));
            weapon.transform.SetParent(transform);
            firePoint = weapon.transform.GetChild(0).gameObject;
            firePoint.transform.position = firePointOffset;
        }
    }
}