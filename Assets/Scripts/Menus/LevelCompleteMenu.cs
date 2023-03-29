using Player.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus {
    public class LevelCompleteMenu : MonoBehaviour {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject towerBodiesTileMap;
        [SerializeField] private TextMeshProUGUI towersLeftTxt;

        private PlayerControls _controls;
        private AimController _aimController;
        private ShootController _shootController;
        public int towersLeft;

        private void Start() {
            towersLeft = towerBodiesTileMap.transform.childCount;

            var tank = GameObject.FindGameObjectWithTag("Player");
            _aimController = tank.GetComponent<AimController>();
            _shootController = tank.GetComponent<ShootController>();
        }

        private void Update() {
            towersLeftTxt.text = $"Towers: {towersLeft}";
            if (towersLeft == 0) DisplayLevelCompleteMenu();
            
        }

        private void DisplayLevelCompleteMenu() {
            Cursor.visible = true;
            Time.timeScale = 0;
            _aimController.enabled = false;
            _shootController.enabled = false;
            panel.SetActive(true);
        }

        public void Retry() {
            Cursor.visible = true;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoToMainMenu() {
            Cursor.visible = true;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}