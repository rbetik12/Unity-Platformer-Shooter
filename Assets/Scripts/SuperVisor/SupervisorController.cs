using UnityEngine;

public class SupervisorController : MonoBehaviour {
    private GameObject player;
    private PlayerController playerController;

    private void Start() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    public PlayerController GetPlayerController() {
        return playerController;
    }
}