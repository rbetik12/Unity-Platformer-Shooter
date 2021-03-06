﻿using Managers;
using UnityEngine;

namespace Utils {
    public class OutOfBoundsController : MonoBehaviour {
        private GameManager gameManager;
        private void Start() {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name.Contains("Bullet")) {
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("Player")) {
                gameManager.player.GetPlayerController().OutOfBoundsCollision();
            }
        }
    }
}