using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour {
    public void GoToMainScene() {
        SceneManager.LoadScene("MainScene");
    }
}
