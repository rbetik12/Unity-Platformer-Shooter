using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbstractPlaceable {
    protected GameObject placeable;
    protected Vector3 position;
    public abstract void Place();
    public abstract void Destroy(Tilemap map);

    public virtual void Destroy() {
        Object.Destroy(placeable);
    }
}