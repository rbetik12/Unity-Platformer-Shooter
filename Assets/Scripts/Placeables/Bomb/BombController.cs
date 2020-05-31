using UnityEngine;

public class BombController : MonoBehaviour {
    private bool playerWithinRadius;
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name.Contains("Player"))
            playerWithinRadius = true;
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name.Contains("Player"))
            playerWithinRadius = false;
    }

    public bool IsPlayerWithinRadius() {
        return playerWithinRadius;
    }
}