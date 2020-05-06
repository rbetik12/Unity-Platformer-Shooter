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
    }

    void Update() {
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

    private void FixedUpdate() {
        if (IsFloorColliding() && !moveRight && !moveLeft && !jump) {
            rb.velocity = new Vector2(0, 0);
        }
        if (moveRight) {
            rb.AddForce(Vector3.right * gravityScale * Time.deltaTime * MOVEMENT_SPEED);
            moveRight = false;
        }
        if (moveLeft) {
            rb.AddForce(Vector3.left * gravityScale * Time.deltaTime * MOVEMENT_SPEED);
            moveLeft = false;
        }
        if (jump) {
            rb.AddForce(Vector3.up * gravityScale * Time.deltaTime * MOVEMENT_SPEED * 20f);
            jump = false;
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -3.5f, 3.5f), rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name.Contains("Tilemap")) {
            jumpsAmount = 0;
        }
    }

    private bool IsFloorColliding() {
        float dist = 0.1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.down, dist);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down));
        if (raycastHit.collider != null) {
            Debug.Log(raycastHit.collider.gameObject.transform.position);
            Debug.Log(raycastHit.collider.gameObject.name);
            return true;
        }
        return false;
    }
}