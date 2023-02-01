using Player.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Menus {
    public class PauseMenu : MonoBehaviour {
        private PlayerControls _controls;
        private AimController _aimController;
        private Shoot _shootController;
        [SerializeField] private GameObject panel;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _aimController = tank.GetComponent<AimController>();
            _shootController = tank.GetComponent<Shoot>();
        }

        private void PauseMenuActivate() {
            Time.timeScale = 0;
            _aimController.enabled = false;
            _shootController.enabled = false;
            panel.SetActive(true);
        }

        public void PauseMenuDeactivate() {
            Time.timeScale = 1;
            _aimController.enabled = true;
            _shootController.enabled = true;
            panel.SetActive(false);
        }

        public void GoToMainMenu() {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        private void ToggleMenu(InputAction.CallbackContext ctx) {
            if (panel.activeSelf) {
                PauseMenuDeactivate();
            } else {
                PauseMenuActivate();
            }
        }

        private void OnEnable() {
            _controls.UI.Pause.Enable();
            _controls.UI.Pause.performed += ToggleMenu;
        }

        private void OnDisable() {
            _controls.UI.Pause.performed -= ToggleMenu;
            _controls.UI.Pause.Disable();
        }
    }
}