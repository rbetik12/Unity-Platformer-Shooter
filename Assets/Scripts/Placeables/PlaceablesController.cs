using System.Collections.Generic;
using UnityEngine;
using System;
using Managers;

namespace Placeables {
    public class PlaceablesController : MonoBehaviour {
        [SerializeField] private GameObject bombPrefab;
        
        private GameManager gameManager;
        private Queue<Bomb> bombs;
        private float bombTimer = 1;
        
        private const int bombExplosionTime = 5;

        private void Start() {
            bombs = new Queue<Bomb>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void Update() {
            CheckForBomb();
            bombTimer += Time.deltaTime;
        }

        private void CheckForBomb() {
            if (bombs.Count == 0) return;
            if (Convert.ToInt32(bombTimer) % bombExplosionTime != 0) return;
            Bomb queueBomb = bombs.Dequeue();
            queueBomb.Destroy(gameManager.map.GetTileMap());
        }

        public void SpawnBomb(Vector3 position) {
            Bomb spawnBomb = new Bomb(bombPrefab, position, gameManager);
            bombs.Enqueue(spawnBomb);
            spawnBomb.Place();
        }
    }
}