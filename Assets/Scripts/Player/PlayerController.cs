using Managers;
using UnityEngine;

namespace Player {
    public class PlayerController : MonoBehaviour { 
        private Joystick joystick;
        private float hp = 100f;
        private Rigidbody2D rb;
        private Vector3 speed;
        private bool isAlive = true;
        private bool moveRight;
        private bool moveLeft;
        private bool jump;
        private bool isFloorColliding;
        private GameManager gameManager;
        private NetworkManager networkManager;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
        }

        private void FixedUpdate() {
            Move();
        }

        public void Jump() {
            jump = true;
        }

        private void Move() {
            speed = rb.velocity;
            float[] inputNet = new float[2];

            inputNet[0] = joystick.Horizontal;
            
            if (jump) {
                inputNet[1] = 1;
                jump = false;
            }
            else {
                inputNet[1] = 0;
            }

            networkManager.GetClientSend().PlayerMovement(inputNet);
        }

        private void Update() {
            CheckHealth();
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