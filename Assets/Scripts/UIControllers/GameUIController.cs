using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIControllers {
    public class GameUIController : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI restartLabel;
        [SerializeField] private TextMeshProUGUI youDiedLabel;
        [SerializeField] private Image healthbar;

        private GameManager gameManager;

        private void Start() {
            restartLabel.gameObject.SetActive(false);
            youDiedLabel.gameObject.SetActive(false);
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        
        public void OnRestartEnter() {
            restartLabel.color = new Color(66 / 256f, 135 / 256f, 245 / 256f, 1f);
        }

        public void OnRestartExit() {
            restartLabel.color = new Color(1f, 1f, 1f, 1f);
        }

        public void OnRestartClick() {
            restartLabel.color = new Color(66 / 256f, 135 / 256f, 245 / 256f, 1f);
        }

        public void OnPlayerDeath() {
            restartLabel.gameObject.SetActive(true);
            youDiedLabel.gameObject.SetActive(true);
        }
        
        public void ScaleHealthBar() {
            healthbar.transform.localScale = new Vector3(gameManager.player.GetPlayerController().GetHp() / 100, 1, 1);
        }
    }
}