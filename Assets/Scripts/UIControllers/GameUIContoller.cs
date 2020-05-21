using TMPro;
using UnityEngine;

namespace UIControllers {
    public class GameUIContoller : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI restartLabel;
        [SerializeField] private TextMeshProUGUI youDiedLabel;

        private void Start() {
            restartLabel.gameObject.SetActive(false);
            youDiedLabel.gameObject.SetActive(false);
        }

        public void OnPlayerDeath() {
            restartLabel.gameObject.SetActive(true);
            youDiedLabel.gameObject.SetActive(true);
        }
    }
}