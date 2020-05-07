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
            // particlesController.BulletDestroyPatricles(other.GetContact(0).point);
            // Debug.Log($"{Convert.ToInt32(Mathf.Ceil(other.GetContact(0).point.x))} {Convert.ToInt32(Mathf.Ceil(other.GetContact(0).point.y))}");
            // Vector3Int collisionPoint = new Vector3Int(Convert.ToInt32(Mathf.Ceil(other.GetContact(0).point.x)), Convert.ToInt32(Mathf.Ceil(other.GetContact(0).point.y)), 0);
            // Tile tile = (Tile) tilemap.GetTile(collisionPoint);
            // Debug.Log(tile);
            // Debug.Log(tile.color);
            // Tile tile1 = (Tile) tilemap.GetTile(new Vector3Int(0, 0, 0));
            // Debug.Log(tile1);
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in other.contacts) {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
            Destroy(this.gameObject);
        }
    }
}