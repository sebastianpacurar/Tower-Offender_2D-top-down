using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menus {
    public class InGameMenu : MonoBehaviour {
        public GameObject SelectedShell { get; private set; }

        [SerializeField] private Image[] weaponImages;
        [SerializeField] private GameObject[] shellPrefabs;
        [SerializeField] private TextMeshProUGUI lightShellAmmo;
        [SerializeField] private TextMeshProUGUI empShellAmmo;

        private PlayerControls _controls;
        private AmmoManager _ammoManager;
        private GameObject _aoeGhost;
        private Color _unavailableColor = new(0.75f, 0f, 0f, 1f);
        private Color _unselectedColor = new(0.75f, 0.75f, 0f, 1f);
        private Color _selectedColor = new(0f, 0.75f, 0f, 1f);

        private void Awake() {
            _controls = new PlayerControls();
            SelectedShell = shellPrefabs[0];
        }

        private void Start() {
            _ammoManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoManager>();
            _aoeGhost = GameObject.FindGameObjectWithTag("AoeGhost");
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
            _aoeGhost.GetComponent<CircleCollider2D>().enabled = false;
            _aoeGhost.transform.Find("CircleArea").gameObject.SetActive(false);

            if (_ammoManager.EmpShellAmmo > 0) {
                weaponImages[1].color = _unselectedColor;
            } else {
                weaponImages[1].color = _unavailableColor;
            }
        }

        private void SelectSecondShell(InputAction.CallbackContext ctx) {
            SelectedShell = shellPrefabs[1];
            weaponImages[1].color = _selectedColor;
            _aoeGhost.GetComponent<CircleCollider2D>().enabled = true;
            _aoeGhost.transform.Find("CircleArea").gameObject.SetActive(true);

            if (_ammoManager.LightShellAmmo > 0) {
                weaponImages[0].color = _unselectedColor;
            } else {
                weaponImages[0].color = _unavailableColor;
            }
        }
    }
}