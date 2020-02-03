using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

    [SerializeField] private GameObject bulletObj;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float shootingPeriod;
    private Vector3 rotation;

    private void Start() {
        StartCoroutine(Aim());
        // StartCoroutine(Shoot());
    }

    void Update() {
    }

    private IEnumerator Aim() {
        while (true) {
            rotation = Vector3.Normalize(player.transform.position - this.transform.position);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator Shoot() {
        while(true) {
            GameObject bullet = Instantiate(bulletObj, firePoint.transform.position, firePoint.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y) * 20f;
            yield return new WaitForSeconds(shootingPeriod);
        }
    }
}
