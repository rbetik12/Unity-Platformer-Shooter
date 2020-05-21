using UnityEngine;

public class BombController : MonoBehaviour {
    private bool playerWithinRadius;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name.Equals("Player"))
            playerWithinRadius = true;
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name.Equals("Player"))
            playerWithinRadius = false;
    }

    public bool IsPlayerWithinRadius() {
        return playerWithinRadius;
    }
}