using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menus {
    public class InGameMenu : MonoBehaviour {
        public GameObject SelectedShell { get; private set; }

        private PlayerControls _controls;
        private AmmoManager _ammoManager;

        [SerializeField] private Image[] weaponImages;
        [SerializeField] private GameObject[] shellPrefabs;
        [SerializeField] private TextMeshProUGUI lightShellAmmo;
        [SerializeField] private TextMeshProUGUI empShellAmmo;
        private Color _unavailableColor = new(0.75f, 0f, 0f, 1f);
        private Color _unselectedColor = new(0.75f, 0.75f, 0f, 1f);
        private Color _selectedColor = new(0f, 0.75f, 0f, 1f);

        private void Awake() {
            _controls = new PlayerControls();
            SelectedShell = shellPrefabs[0];
        }

        private void Start() {
            _ammoManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoManager>();
        }

        private void Update() {
            lightShellAmmo.text = $"x{_ammoManager.LightShellAmmo}";
            empShellAmmo.text = $"x{_ammoManager.EmpShellAmmo}";
        }

        private void OnEnable() {
            _controls.UI.FirstWeapon.Enable();
            _controls.UI.SecondWeapon.Enable();

            _controls.UI.FirstWeapon.performed += SelectFirstShell;
            _controls.UI.SecondWeapon.performed += SelectSecondShell;
        }

        private void OnDisable() {
            _controls.UI.FirstWeapon.performed -= SelectFirstShell;
            _controls.UI.SecondWeapon.performed -= SelectSecondShell;

            _controls.UI.FirstWeapon.Disable();
            _controls.UI.SecondWeapon.Disable();
        }

        private void SelectFirstShell(InputAction.CallbackContext ctx) {
            SelectedShell = shellPrefabs[0];
            weaponImages[0].color = _selectedColor;
            if (_ammoManager.EmpShellAmmo > 0) {
                weaponImages[1].color = _unselectedColor;
            } else {
                weaponImages[1].color = _unavailableColor;
            }
        }

        private void SelectSecondShell(InputAction.CallbackContext ctx) {
            SelectedShell = shellPrefabs[1];
            weaponImages[1].color = _selectedColor;
            if (_ammoManager.LightShellAmmo > 0) {
                weaponImages[0].color = _unselectedColor;
            } else {
                weaponImages[0].color = _unavailableColor;
            }
        }
    }
}