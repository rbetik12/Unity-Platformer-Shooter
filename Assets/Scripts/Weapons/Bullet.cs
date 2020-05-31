using Managers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Weapons {
    public class Bullet : MonoBehaviour {
        private GameManager gameManager;
        private Tilemap tilemap;
        
        private void Start() {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            tilemap = gameManager.map.GetTileMap();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.name.Contains("Tilemap")) {
                Vector3 hitPosition = Vector3.zero;
                foreach (ContactPoint2D hit in other.contacts) {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                    gameManager.particles.OnBlockDestroy(hitPosition,
                        (Tile) (tilemap.GetTile(tilemap.WorldToCell(hitPosition))));
                    tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                }

                Destroy(gameObject);
            }
        }
    }
}