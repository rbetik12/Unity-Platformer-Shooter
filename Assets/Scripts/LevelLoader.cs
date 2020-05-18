using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    private void LoadLevel(int index) {
        SceneManager.LoadScene(index);
    }

    public void LoadLevel1() {
        LoadLevel(0);
    }
}
