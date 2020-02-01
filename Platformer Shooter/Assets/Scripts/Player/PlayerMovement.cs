using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private float gravityScale;
    private bool moveRight = false;
    private bool moveLeft = false;
    private bool jump = false;
    private int jumpsAmount = 0;

    private const float MOVEMENT_SPEED = 1000f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
        Debug.Log(gravityScale);
    }


    void Update() {
        Move();
    }

    private void Move() {
        if (Input.GetKey(KeyCode.D)) {
            moveRight = true;
            // Debug.Log("Right pressed");
        }
        if (Input.GetKey(KeyCode.A)) {
            moveLeft = true;
            // Debug.Log("Left pressed");
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpsAmount < 2) {
            jump = true;
            jumpsAmount++;
        }
    }

    private void FixedUpdate() {
        if (moveRight) {
            rb.AddForce(Vector3.right * gravityScale * Time.deltaTime * MOVEMENT_SPEED);
            moveRight = false;
            // Debug.Log("Applied force to the right");
        }
        if (moveLeft) {
            rb.AddForce(Vector3.left * gravityScale * Time.deltaTime * MOVEMENT_SPEED);
            moveLeft = false;
            // Debug.Log("Applied force to the left");
        }
        if (jump) {
            rb.AddForce(Vector3.up * gravityScale * Time.deltaTime * MOVEMENT_SPEED * 20f);
            jump = false;
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -3.5f, 3.5f), rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag.Equals("Floor")) {
            jumpsAmount = 0;
        }
    }
}
