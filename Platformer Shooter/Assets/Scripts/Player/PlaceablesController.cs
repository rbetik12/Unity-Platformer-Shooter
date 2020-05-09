using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaceablesController : MonoBehaviour {
    [SerializeField] private GameObject bomb;
    [SerializeField] private int bombExplosionTime;

    private Queue<Bomb> bombs;
    private float bombTimer = 1;
    void Start() {
        bombs = new Queue<Bomb>();
    }

    void Update() {
        CheckForBomb();
        bombTimer += Time.deltaTime;
    }

    private void CheckForBomb() {
        if (bombs.Count == 0) return;
        if (Convert.ToInt32(bombTimer) % bombExplosionTime == 0) {
            Bomb queueBomb = bombs.Dequeue();
            queueBomb.Destroy();
        }
    }

    public void SpawnBomb(Vector3 position) {
        Bomb spawnBomb = new Bomb(bomb, position);
        bombs.Enqueue(spawnBomb);
        spawnBomb.Place();
    }
}