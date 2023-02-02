using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus {
    public class GameOverMenu : MonoBehaviour {
        [SerializeField] private GameObject panel;

        public void Retry() {
            Cursor.visible = true;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // called from DestroyTank.cs
        public void ToggleGameOver() {
            Cursor.visible = true;
            Time.timeScale = 0;
            panel.SetActive(true);
        }
    }
}