using UnityEngine;

namespace Weapons {
    public abstract class AbstractWeapon {
        protected GameObject weapon;
        protected GameObject firePoint;
        protected WeaponType type;
        public abstract WeaponType GetType();
        public abstract Transform GetFirePoint();
        public abstract void Create(Transform transform);
    }
}