using Player;
using UnityEngine;
using Weapons;

namespace Supervisor {
    public class SupervisorController : MonoBehaviour {
        [SerializeField] private GameObject pistol;
        
        private PlayerController playerController;

        private void Start() {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        public PlayerController GetPlayerController() {
            return playerController;
        }

        public AbstractWeapon GetWeapon(WeaponType type) {
            if (type.Equals(WeaponType.Pistol))
                return new Pistol(pistol);
            return null;
        }
    }
}