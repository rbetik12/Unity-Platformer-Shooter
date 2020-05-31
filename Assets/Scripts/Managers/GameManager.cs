using Player;
using UIControllers;
using UnityEngine;
using UnityEngine.Tilemaps;
using Weapons;

namespace Managers {
    public class GameManager : MonoBehaviour {
        [SerializeField] private GameObject pistolPrefab;
        [SerializeField] private GameObject shotgunPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject remotePlayerPrefab;
        [SerializeField] private GameUIController uiController;
        [SerializeField] private ParticlesController particlesController;
        [SerializeField] private Tilemap tilemap;

        private PlayerController playerController;
        private GameObject localPlayer;

        public static float bulletDamage = 50f;

        public Player player;
        public UI ui;
        public Particles particles;
        public Map map;
        public Weapon weapon;

        private void Start() {
            localPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            playerController = localPlayer.GetComponent<PlayerController>();
            player = new Player(this);
            
            ui = new UI(this);
            
            particles = new Particles(this);
            
            map = new Map(this);
            map.SetTilemap(tilemap);
            
            weapon = new Weapon(this);
        }

        public void OnPlayerJump() {
            player.OnJumpClicked();
        }

        public class Player {
            private GameManager instance;
            
            public Player(GameManager instance) {
                this.instance = instance;
            }
            public void OnDamage() {
                instance.ui.OnPlayerDamage();
            }

            public PlayerController GetPlayerController() {
                return instance.playerController;
            }

            public void OnJumpClicked() {
                instance.playerController.Jump();
            }
        }

        public class UI {
            private GameManager instance;

            public UI(GameManager instance) {
                this.instance = instance;
            }
            
            public void OnPlayerDeath() {
                instance.uiController.OnPlayerDeath();
            }

            public void OnPlayerDamage() {
                instance.uiController.ScaleHealthBar();
            }
        }

        public class Particles {
            private GameManager instance;

            public Particles(GameManager instance) {
                this.instance = instance;
            }
            
            public void OnPlayerDeath(Vector3 deathPosition) {
                instance.particlesController.PlayerDeathParticles(deathPosition);
            }

            public void OnBlockDestroy(Vector3 position, Tile tile) {
                instance.particlesController.OnBlockDestroyParticles(position, tile);
            }
        }

        public class Map {
            private GameManager instance;
            private Tilemap tilemap;

            public Map(GameManager instance) {
                this.instance = instance;
            }

            public void SetTilemap(Tilemap tilemap) {
                this.tilemap = tilemap;
            }

            public Tilemap GetTileMap() {
                return tilemap;
            }
        }

        public class Weapon {
            private GameManager instance;

            public Weapon(GameManager instance) {
                this.instance = instance;
            }

            public AbstractWeapon GetWeapon(WeaponType type) {
                switch (type) {
                    case WeaponType.Pistol:
                        return new Pistol(instance.pistolPrefab);
                    case WeaponType.Shotgun:
                        return new Shotgun(instance.shotgunPrefab);
                    default:
                        return null;
                }
            }
        }
    }
}