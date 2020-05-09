using UnityEngine;
public class Bomb : AbstractPlaceable {

    public Bomb(GameObject gameObject, Vector3 position) {
        this.placeable = gameObject;
        this.position = position;
    }

    override public void Place() {
        this.placeable = Object.Instantiate(placeable, position, Quaternion.Euler(0, 0, 0));
    }
}