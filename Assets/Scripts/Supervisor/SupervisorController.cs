using Player;
using UnityEngine;
using Weapons;

namespace Supervisor {
    public class SupervisorController : MonoBehaviour {
        [SerializeField] private GameObject pistol;
        [SerializeField] private GameObject shotgun;
        
        private PlayerController playerController;

        private void Start() {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        public PlayerController GetPlayerController() {
            return playerController;
        }

        public AbstractWeapon GetWeapon(WeaponType type) {
            switch (type) {
                case WeaponType.Pistol:
                    return new Pistol(pistol);
                case WeaponType.Shotgun:
                    return new Shotgun(shotgun);
                default:
                    return null;
            }
        }
    }
}