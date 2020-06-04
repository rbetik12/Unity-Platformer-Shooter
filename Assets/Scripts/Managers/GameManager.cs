using System.Collections.Generic;
using Net;
using Placeables;
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
        [SerializeField] private PlaceablesController placeablesController;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private UIManager uiManager;

        private PlayerController playerController;
        private GameObject localPlayer;

        public static float bulletDamage = 50f;

        public Player player;
        public UI ui;
        public Particles particles;
        public Map map;
        public Weapon weapon;
        public Placeable placeable;
        public Network network;

        private void Start() {
            player = new Player(this);
            ui = new UI(this);
            particles = new Particles(this);
            weapon = new Weapon(this);
            placeable = new Placeable(this);
            network = new Network(this);

            map = new Map(this);
            map.SetTilemap(tilemap);
        }

        public void UIOnPlayerJump() {
            player.OnJumpClicked();
        }

        public void UIOnSpawnBomb() {
            placeable.SpawnBomb();
        }

        public void UIConnectToServer() {
            ui.GetNetUIManager().ConnectToServer();
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

            public UIManager GetNetUIManager() {
                return instance.uiManager;
            }

            public GameUIController getGameUIController() {
                return instance.uiController;
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

        public class Placeable {
            private GameManager instance;

            public Placeable(GameManager instance) {
                this.instance = instance;
            }

            public void SpawnBomb(Vector3 position) {
                instance.placeablesController.SpawnBomb(position);
            }

            public void SpawnBomb() {
                instance.placeablesController.SpawnBomb(instance.localPlayer.transform.position);
            }
        }

        public class Network {
            private GameManager instance;
            private Dictionary<int, PlayerNetworkManager> players = new Dictionary<int, PlayerNetworkManager>();

            public Network(GameManager instance) {
                this.instance = instance;
            }

            public Dictionary<int, PlayerNetworkManager> GetPlayers() {
                return players;
            }

            public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation) {
                GameObject player;
                if (id == instance.networkManager.GetClient().myId) {
                    player = Instantiate(instance.playerPrefab, position, rotation);
                    instance.playerController = player.GetComponent<PlayerController>();
                }
                else {
                    // player = Insta
                    player = null;
                }
                // localPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                // playerController = localPlayer.GetComponent<PlayerController>();

                PlayerNetworkManager pnm = player.GetComponent<PlayerNetworkManager>();
                pnm.Initialize(id, username);
                players.Add(id, pnm);
            }

            public NetworkManager GetNetworkManager() {
                return instance.networkManager;
            }
        }
    }
}