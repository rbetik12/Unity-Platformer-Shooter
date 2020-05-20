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

        private const float MovementSpeed = 1000f;
        private const float MaxVelocity = 5f;
        private const float MinVelocity = -MaxVelocity;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            gravityScale = rb.gravityScale;
        }

        private void FixedUpdate() {
            speed = rb.velocity;
            if (IsFloorColliding() && !moveRight && !moveLeft && !jump) {
                rb.velocity = new Vector2(0, 0);
            }

            if (moveRight) {
                rb.AddForce(Vector3.right * (gravityScale * Time.deltaTime * MovementSpeed));
                moveRight = false;
            }

            if (moveLeft) {
                rb.AddForce(Vector3.left * (gravityScale * Time.deltaTime * MovementSpeed));
                moveLeft = false;
            }

            if (jump) {
                rb.AddForce(Vector3.up * (gravityScale * Time.deltaTime * MovementSpeed * 20f));
                jump = false;
            }

            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, MinVelocity, MaxVelocity), rb.velocity.y);
        }

        private void Update() {
            CheckHealth();
            Move();
        }

        private void Move() {
            if (Input.GetKey(KeyCode.D)) {
                moveRight = true;
            }

            if (Input.GetKey(KeyCode.A)) {
                moveLeft = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && jumpsAmount < 2) {
                jump = true;
                jumpsAmount++;
            }
        }

        private bool IsFloorColliding() {
            float dist = 0.01f;
            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f),
                Vector2.down, dist);
            if (raycastHit.collider != null) {
                bool isNotTrigger = !raycastHit.collider.isTrigger;
                if (isNotTrigger)
                    jumpsAmount = 0;
                return isNotTrigger;
            }

            return false;
        }

        private void CheckHealth() {
            if (!isAlive) return;
            if (hp <= 0) {
                OnDeath();
            }

            ScaleHealthBar();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.tag.Equals("Enemy Bullet")) {
                hp -= enemyBulletDamage;
                Destroy(other.gameObject);
                rb.velocity = speed;
            }
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
    }
}