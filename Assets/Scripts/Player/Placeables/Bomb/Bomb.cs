using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : AbstractPlaceable {
    private const int BOMB_RADIUS = 2;
    private BombController bombController;
    private SupervisorController supervisorController;

    public Bomb(GameObject gameObject, Vector3 position, SupervisorController supervisorController) {
        placeable = gameObject;
        this.position = position;
        this.supervisorController = supervisorController;
    }

    override public void Place() {
        placeable = Object.Instantiate(placeable, position, Quaternion.Euler(0, 0, 0));
        bombController = placeable.GetComponent<BombController>();
    }

    override public void Destroy(Tilemap tilemap) {
        Vector3Int bombPositionOnTilemap = tilemap.WorldToCell(placeable.transform.position);
        Vector3Int tilepos = new Vector3Int();
        for (int y = bombPositionOnTilemap.y + BOMB_RADIUS; y >= bombPositionOnTilemap.y - BOMB_RADIUS; y--) {
            for (int x = bombPositionOnTilemap.x - BOMB_RADIUS; x <= bombPositionOnTilemap.x + BOMB_RADIUS; x++) {
                tilepos.x = x;
                tilepos.y = y;
                tilemap.SetTile(tilepos, null);
            }
        }
        if (bombController.IsPlayerWithinRadius())
            supervisorController.GetPlayerController().GetBombDamage();
        Object.Destroy(placeable);
    }
}