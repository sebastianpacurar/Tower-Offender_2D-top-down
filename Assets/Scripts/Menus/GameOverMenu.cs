using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus {
    public class GameOverMenu : MonoBehaviour {
        [SerializeField] private GameObject panel;

        public void Retry() {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // called from DestroyTank.cs
        public void ToggleGameOver() {
            Time.timeScale = 0;
            panel.SetActive(true);
        }
    }
}