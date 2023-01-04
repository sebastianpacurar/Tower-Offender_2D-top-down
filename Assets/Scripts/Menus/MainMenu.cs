using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void StartLevelOne() {
        SceneManager.LoadScene("LevelOne");
    }

    public void StartLevelTwo() {
        SceneManager.LoadScene("LevelTwo");
    }

    public void StartLevelThree() {
        SceneManager.LoadScene("LevelThree");
    }

    public void StartLevelFour() {
        SceneManager.LoadScene("LevelFour");
    }

    public void StartLevelFive() {
        SceneManager.LoadScene("LevelFive");
    }
}