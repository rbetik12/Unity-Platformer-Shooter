using UnityEngine;
public abstract class AbstractPlaceable {
    protected GameObject placeable;
    protected Vector3 position;
    public abstract void Place();
    public void Destroy() {
        Object.Destroy(placeable);
    }
}