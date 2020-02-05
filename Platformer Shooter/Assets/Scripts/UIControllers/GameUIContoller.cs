using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class GameUIContoller : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI restartLabel;
    [SerializeField] private TextMeshProUGUI youDiedLabel;
    [SerializeField] private LevelLoader levelLoader;

    private void Start() {
        restartLabel.gameObject.SetActive(false);
        youDiedLabel.gameObject.SetActive(false);
    }

    public void OnRestartEnter() {
        restartLabel.color = new Color(66 / 256f, 135 / 256f, 245 / 256f, 1f);
    }

    public void OnRestartExit() {
        restartLabel.color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnRestartClick() {
        restartLabel.color = new Color(66 / 256f, 135 / 256f, 245 / 256f, 1f);
        levelLoader.LoadLevel1();
    }

    public void OnPlayerDeath() {
        restartLabel.gameObject.SetActive(true);
        youDiedLabel.gameObject.SetActive(true);
    }
}