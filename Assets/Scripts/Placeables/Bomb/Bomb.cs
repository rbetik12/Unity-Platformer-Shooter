using Managers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Placeables {
    public class Bomb : AbstractPlaceable {
        private const int BombRadius = 2;
        private BombController bombController;
        private readonly GameManager gameManager;

        public Bomb(GameObject gameObject, Vector3 position, GameManager gameManager) {
            placeable = gameObject;
            this.position = position;
            this.gameManager = gameManager;
        }

        public override void Place() {
            placeable = Object.Instantiate(placeable, position, Quaternion.Euler(0, 0, 0));
            bombController = placeable.GetComponent<BombController>();
        }

        public override void Destroy(Tilemap tilemap) {
            Vector3Int bombPositionOnTilemap = tilemap.WorldToCell(placeable.transform.position);
            Vector3Int tilepos = new Vector3Int();
            for (int y = bombPositionOnTilemap.y + BombRadius; y >= bombPositionOnTilemap.y - BombRadius; y--) {
                for (int x = bombPositionOnTilemap.x - BombRadius; x <= bombPositionOnTilemap.x + BombRadius; x++) {
                    tilepos.x = x;
                    tilepos.y = y;
                    tilemap.SetTile(tilepos, null);
                }
            }

            if (bombController.IsPlayerWithinRadius()) {
                gameManager.player.GetPlayerController().GetBombDamage();
            }

            Object.Destroy(placeable);
        }
    }
}