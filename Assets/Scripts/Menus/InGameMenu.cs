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
        [SerializeField] private TextMeshProUGUI sniperShellAmmo;
        [SerializeField] private TextMeshProUGUI nukeShellAmmo;

        private PlayerControls _controls;
        private AmmoManager _ammoManager;
        private GameObject _lightShellGhost, _empAoeGhost, _sniperShellGhost, _nukeAoeGhost;
        private Color _unavailableColor = new(0.75f, 0f, 0f, 1f);
        private Color _unselectedColor = new(0.75f, 0.75f, 0f, 1f);
        private Color _selectedColor = new(0f, 0.75f, 0f, 1f);

        private void Awake() {
            _controls = new PlayerControls();
            SelectedShell = shellPrefabs[0]; // defaults to TankLightShell
        }

        private void Start() {
            _ammoManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoManager>();
            _empAoeGhost = GameObject.FindGameObjectWithTag("EmpAoeGhost");
            _nukeAoeGhost = GameObject.FindGameObjectWithTag("NukeAoeGhost");
            _lightShellGhost = GameObject.FindGameObjectWithTag("LightShellGhost");
            _sniperShellGhost = GameObject.FindGameObjectWithTag("SniperShellGhost");
        }

        private void Update() {
            lightShellAmmo.text = $"x{_ammoManager.LightShellAmmo}";
            empShellAmmo.text = $"x{_ammoManager.EmpShellAmmo}";
            sniperShellAmmo.text = $"x{_ammoManager.SniperShellAmmo}";
            nukeShellAmmo.text = $"x{_ammoManager.NukeShellAmmo}";
        }

        private void SelectFirstShell(InputAction.CallbackContext ctx) {
            PerformWeaponSwitch(shellPrefabs[0]);
        }

        private void SelectSecondShell(InputAction.CallbackContext ctx) {
            PerformWeaponSwitch(shellPrefabs[1]);
        }

        private void SelectThirdShell(InputAction.CallbackContext ctx) {
            PerformWeaponSwitch(shellPrefabs[2]);
        }

        private void SelectFourthShell(InputAction.CallbackContext ctx) {
            PerformWeaponSwitch(shellPrefabs[3]);
        }

        private void PerformWeaponSwitch(GameObject selectedShell) {
            if (SelectedShell.CompareTag(selectedShell.tag)) return;
            switch (SelectedShell.tag) {
                case "TankLightShell":
                    weaponImages[0].color = _ammoManager.LightShellAmmo > 0 ? _unselectedColor : _unavailableColor;
                    _lightShellGhost.transform.Find("Cursor").gameObject.SetActive(false);
                    _lightShellGhost.transform.Find("CircleRadiusArea").gameObject.SetActive(false);
                    _lightShellGhost.GetComponent<CircleCollider2D>().enabled = false;
                    break;
                case "TankEmpShellEntity":
                    weaponImages[1].color = _ammoManager.EmpShellAmmo > 0 ? _unselectedColor : _unavailableColor;
                    _empAoeGhost.GetComponent<CircleCollider2D>().enabled = false;
                    _empAoeGhost.transform.Find("CircleRadiusArea").gameObject.SetActive(false);
                    break;
                case "TankSniperShell":
                    weaponImages[2].color = _ammoManager.SniperShellAmmo > 0 ? _unselectedColor : _unavailableColor;
                    _sniperShellGhost.transform.Find("Cursor").gameObject.SetActive(false);
                    break;
                case "TankNukeShellEntity":
                    weaponImages[3].color = _ammoManager.NukeShellAmmo > 0 ? _unselectedColor : _unavailableColor;
                    _nukeAoeGhost.GetComponent<CircleCollider2D>().enabled = false;
                    _nukeAoeGhost.transform.Find("CircleRadiusArea").gameObject.SetActive(false);
                    break;
            }

            switch (selectedShell.tag) {
                case "TankLightShell":
                    if (_ammoManager.LightShellAmmo == 0) return;
                    SelectedShell = shellPrefabs[0];
                    weaponImages[0].color = _selectedColor;
                    _lightShellGhost.transform.Find("Cursor").gameObject.SetActive(true);
                    _lightShellGhost.transform.Find("CircleRadiusArea").gameObject.SetActive(true);
                    _lightShellGhost.GetComponent<CircleCollider2D>().enabled = true;
                    break;
                case "TankEmpShellEntity":
                    if (_ammoManager.EmpShellAmmo == 0) return;
                    SelectedShell = shellPrefabs[1];
                    weaponImages[1].color = _selectedColor;
                    _empAoeGhost.GetComponent<CircleCollider2D>().enabled = true;
                    _empAoeGhost.transform.Find("CircleRadiusArea").gameObject.SetActive(true);
                    break;
                case "TankSniperShell":
                    if (_ammoManager.SniperShellAmmo == 0) return;
                    SelectedShell = shellPrefabs[2];
                    weaponImages[2].color = _selectedColor;
                    _sniperShellGhost.transform.Find("Cursor").gameObject.SetActive(true);
                    break;
                case "TankNukeShellEntity":
                    if (_ammoManager.NukeShellAmmo == 0) return;
                    SelectedShell = shellPrefabs[3];
                    weaponImages[3].color = _selectedColor;
                    _nukeAoeGhost.GetComponent<CircleCollider2D>().enabled = true;
                    _nukeAoeGhost.transform.Find("CircleRadiusArea").gameObject.SetActive(true);
                    break;
            }
        }

        private void OnEnable() {
            _controls.UI.FirstWeapon.Enable();
            _controls.UI.SecondWeapon.Enable();
            _controls.UI.ThirdWeapon.Enable();
            _controls.UI.FourthWeapon.Enable();

            _controls.UI.FirstWeapon.performed += SelectFirstShell;
            _controls.UI.SecondWeapon.performed += SelectSecondShell;
            _controls.UI.ThirdWeapon.performed += SelectThirdShell;
            _controls.UI.FourthWeapon.performed += SelectFourthShell;
        }

        private void OnDisable() {
            _controls.UI.FirstWeapon.performed -= SelectFirstShell;
            _controls.UI.SecondWeapon.performed -= SelectSecondShell;
            _controls.UI.ThirdWeapon.performed -= SelectThirdShell;
            _controls.UI.FourthWeapon.performed -= SelectFourthShell;

            _controls.UI.FirstWeapon.Disable();
            _controls.UI.SecondWeapon.Disable();
            _controls.UI.ThirdWeapon.Disable();
            _controls.UI.FourthWeapon.Disable();
        }
    }
}