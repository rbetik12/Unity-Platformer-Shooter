using System;
using Managers;
using UnityEngine;
using TMPro;

namespace Net {
    public class UIManager : MonoBehaviour {
        [SerializeField] private GameObject startMenu;
        [SerializeField] private TMP_InputField usernameField;

        private GameManager gameManager;

        private void Start() {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public void ConnectToServer() {
            startMenu.SetActive(false);
            usernameField.interactable = false;
            gameManager.network.GetNetworkManager().GetClient().ConnectToServer();
            gameManager.ui.getGameUIController().gameObject.SetActive(true);
        }

        public TMP_InputField GetUsernameField() {
            return usernameField;
        }
    }
}