using Player.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Menus {
    public class PauseMenu : MonoBehaviour {
        [SerializeField] private GameObject panel;
        private PlayerControls _controls;
        private AimController _aimController;
        private ShootController _shootController;

        private InGameMenu _inGameMenu;


        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _aimController = tank.GetComponent<AimController>();
            _shootController = tank.GetComponent<ShootController>();
            _inGameMenu = GameObject.FindGameObjectWithTag("GameUI").GetComponent<InGameMenu>();
        }

        private void PauseMenuActivate() {
            Cursor.visible = true;
            Time.timeScale = 0;
            _aimController.enabled = false;
            _shootController.enabled = false;
            panel.SetActive(true);
        }

        public void PauseMenuDeactivate() {
            Cursor.visible = _inGameMenu.SelectedShell.CompareTag("TankLightShell");
            Time.timeScale = 1;
            _aimController.enabled = true;
            _shootController.enabled = true;
            panel.SetActive(false);
        }

        public void GoToMainMenu() {
            Cursor.visible = true;
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