using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus {
    public class MainMenu : MonoBehaviour {
        public void StartLevelOne() {
            Cursor.visible = true;
            SceneManager.LoadScene("LevelOne");
        }
    }
}