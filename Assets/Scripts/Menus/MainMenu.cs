using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus {
    public class MainMenu : MonoBehaviour {
        public void StartLevelOne() {
            SceneManager.LoadScene("LevelOne");
        }
    }
}