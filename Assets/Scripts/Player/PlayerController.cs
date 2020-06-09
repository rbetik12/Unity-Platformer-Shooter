using Managers;
using UnityEngine;

namespace Player {
    public class PlayerController : MonoBehaviour { 
        private Joystick joystick;
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
        private GameManager gameManager;
        private NetworkManager networkManager;

        private const float MovementSpeed = 1000f;
        private const float MaxVelocity = 4.5f;
        private const float MinVelocity = -MaxVelocity;
        private const float MobileMovementSpeedMultiplier = 1.5f;

        //GC Optimization
        private Vector2 horizontalJoystickVelocity = Vector2.zero;
        private Vector2 clampedVelocity = Vector2.zero;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            gravityScale = rb.gravityScale;
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
        }

        private void FixedUpdate() {
            Move();
        }

        public void Jump() {
            if (jumpsAmount < 1) {
                jumpsAmount += 1;
                jump = true;
            }
        }

        private void Move() {
            speed = rb.velocity;
            // Debug.Log("Is colliding floor: " + isFloorColliding);
            float[] inputNet = new float[3];
            if (isFloorColliding && joystick.Horizontal.Equals(0) && !jump) {
                // rb.velocity = Vector2.zero;
                // movementForceNet = Vector2.down;
                inputNet[0] = 0;
            }
            else {
                inputNet[0] = 1;
                if (!joystick.Horizontal.Equals(0)) {
                    horizontalJoystickVelocity.x = joystick.Horizontal;
                    horizontalJoystickVelocity.y = 0;
                    // rb.AddForce(horizontalJoystickVelocity * (gravityScale * Time.deltaTime * MovementSpeed * MobileMovementSpeedMultiplier));
                    inputNet[1] = joystick.Horizontal;
                }

                if (jump) {
                    // rb.AddForce(Vector3.up * (gravityScale * Time.deltaTime * MovementSpeed * 20f));
                    inputNet[2] = 1;
                    jump = false;
                }
                else {
                    inputNet[2] = 0;
                }
            }

            networkManager.GetClientSend().PlayerMovement(inputNet);
            // clampedVelocity.x = Mathf.Clamp(rb.velocity.x, MinVelocity, MaxVelocity);
            // clampedVelocity.y = rb.velocity.y;
            // rb.velocity = clampedVelocity;
        }

        private void Update() {
            isFloorColliding = IsFloorColliding();
            CheckHealth();
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
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.tag.Equals("Bullet")) return;
            OnDamage(GameManager.bulletDamage);
            Destroy(other.gameObject);
            rb.velocity = speed;
        }

        private void OnDeath() {
            Destroy(gameObject);
            gameManager.particles.OnPlayerDeath(transform.position);
            isAlive = false;
            gameManager.ui.OnPlayerDeath();
        }

        public void GetBombDamage() {
            OnDamage(50);
        }

        public void OutOfBoundsCollision() {
            OnDeath();
        }

        public float GetHp() {
            return hp;
        }

        private void OnDamage(float damage) {
            hp -= damage;
            gameManager.player.OnDamage();
        }
    }
}