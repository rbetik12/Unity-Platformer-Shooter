﻿using UIControllers;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private float enemyBulletDamage = 100f;
        [SerializeField] private Image healthbar;
        [SerializeField] private ParticlesController particlesController;
        [SerializeField] private GameUIContoller gameUIContoller;

        private float hp = 100f;
        private Rigidbody2D rb;
        private Vector3 speed;
        private bool isAlive = true;
        private float gravityScale;
        private bool moveRight;
        private bool moveLeft;
        private bool jump;
        private int jumpsAmount;
        private bool isFloorColliding;

        private const float MovementSpeed = 1000f;
        private const float MaxVelocity = 4.5f;
        private const float MinVelocity = -MaxVelocity;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            gravityScale = rb.gravityScale;
        }

        private void FixedUpdate() {
            speed = rb.velocity;
            if (isFloorColliding && !moveRight && !moveLeft && !jump) {
                rb.velocity = new Vector2(0, 0);
            }
            else {
                if (moveRight) {
                    rb.AddForce(Vector3.right * (gravityScale * Time.deltaTime * MovementSpeed));
                }

                if (moveLeft) {
                    rb.AddForce(Vector3.left * (gravityScale * Time.deltaTime * MovementSpeed));
                }

                if (jump) {
                    rb.AddForce(Vector3.up * (gravityScale * Time.deltaTime * MovementSpeed * 20f));
                    jump = false;
                }
            }

            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, MinVelocity, MaxVelocity), rb.velocity.y);
        }

        private void Update() {
            Move();
            isFloorColliding = IsFloorColliding();
            CheckHealth();
        }

        private void Move() {
            if (Input.GetKeyDown(KeyCode.D)) {
                moveRight = true;
            }

            if (Input.GetKeyUp(KeyCode.D)) {
                moveRight = false;
            }

            if (Input.GetKeyDown(KeyCode.A)) {
                moveLeft = true;
            }
            
            if (Input.GetKeyUp(KeyCode.A)) {
                moveLeft = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && jumpsAmount < 2) {
                jump = true;
                jumpsAmount++;
            }
        }

        private bool IsFloorColliding() {
            const float dist = 0.1f;
            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f),
                Vector2.down, dist);
            if (raycastHit.collider == null) return false;
            bool isNotTrigger = !raycastHit.collider.isTrigger;
            if (isNotTrigger)
                jumpsAmount = 0;
            return isNotTrigger;

        }

        private void CheckHealth() {
            if (!isAlive) return;
            if (hp <= 0) {
                OnDeath();
            }

            ScaleHealthBar();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.tag.Equals("Enemy Bullet")) return;
            hp -= enemyBulletDamage;
            Destroy(other.gameObject);
            rb.velocity = speed;
        }

        private void ScaleHealthBar() {
            healthbar.transform.localScale = new Vector3(hp / 100, 1, 1);
        }

        private void OnDeath() {
            Destroy(gameObject);
            particlesController.PlayerDeathParticles(transform.position);
            isAlive = false;
            gameUIContoller.OnPlayerDeath();
        }

        public void GetBombDamage() {
            hp -= 50f;
        }

        public void OutOfBoundsCollision() {
            OnDeath();
        }
    }
}