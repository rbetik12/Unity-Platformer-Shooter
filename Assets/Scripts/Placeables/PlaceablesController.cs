using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

namespace Placeables {
    public class PlaceablesController : MonoBehaviour {
        [SerializeField] private GameObject bomb;
        [SerializeField] private int bombExplosionTime;
        [SerializeField] private Tilemap tilemap;
        private SupervisorController supervisorController;

        private Queue<Bomb> bombs;
        private float bombTimer = 1;

        private void Start() {
            bombs = new Queue<Bomb>();
            supervisorController = GameObject.Find("ObjectsSupervisor").GetComponent<SupervisorController>();
        }

        private void Update() {
            CheckForBomb();
            bombTimer += Time.deltaTime;
        }

        private void CheckForBomb() {
            if (bombs.Count == 0) return;
            if (Convert.ToInt32(bombTimer) % bombExplosionTime != 0) return;
            Bomb queueBomb = bombs.Dequeue();
            queueBomb.Destroy(tilemap);
        }

        public void SpawnBomb(Vector3 position) {
            Bomb spawnBomb = new Bomb(bomb, position, supervisorController);
            bombs.Enqueue(spawnBomb);
            spawnBomb.Place();
        }
    }
}