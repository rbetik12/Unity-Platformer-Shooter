using System;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Bullet : MonoBehaviour {

    private Tilemap tilemap;
    private ParticlesController particlesController;

    private void Start() {
        particlesController = GameObject.Find("Particles Controller").GetComponent<ParticlesController>();
        tilemap = GameObject.Find("Tilemap").GetComponentInChildren<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name.Contains("Tilemap")) {
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in other.contacts) {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                particlesController.OnBlockDestroyParticles(hitPosition, (Tile) (tilemap.GetTile(tilemap.WorldToCell(hitPosition))));
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
            Destroy(this.gameObject);
        }
    }
}