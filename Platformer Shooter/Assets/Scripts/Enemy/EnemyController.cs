using Pathfinding;
using UnityEngine;
public class EnemyController : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2000f;
    [SerializeField] private float nextWaypointDistance;

    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private bool grounded = true;

    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void Update() {
        CheckIsGrounded();
        PathFinding();
    }

    private void OnPathComplete(Path path) {
        if (!path.error) {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    private void CheckIsGrounded() {
        RaycastHit2D hit = Physics2D.Raycast((Vector2) transform.position, Vector2.down, 2f);
        if (hit.collider != null) {
            if (Mathf.Abs(hit.point.y - transform.position.y) < 1e-3) {
                grounded = true;
            }
        }
    }

    private void PathFinding() {
        if (path == null) {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;

        CheckMoveDirection(direction);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    private void CheckMoveDirection(Vector2 direction) {
        Vector2 force = direction * speed * Time.deltaTime;
        Debug.Log(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Debug.Log(grounded);
        if (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg > 40 && grounded) {
            grounded = false;
            rb.AddForce(Vector2.up * 100f);
            Debug.Log("Jumping");
        } else {
            rb.AddForce(new Vector2(force.x, 0));
            Debug.Log("Moving");
        }
        // rb.AddForce(force);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Debug.Log("Collided with " + other.gameObject.tag);
        if (other.gameObject.tag.Equals("Floor")) {
            grounded = true;
        }
    }
}