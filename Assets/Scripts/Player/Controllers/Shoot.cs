using Menus;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class Shoot : MonoBehaviour {
        private PlayerControls _controls;
        public float LightShellCdTimer { get; private set; }
        public bool CanFireEmpShell { get; private set; } = true;

        public float EmpShellCdTimer { get; private set; }
        public bool CanFireLightShell { get; private set; } = true;

        private AmmoManager _ammoManager;

        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private GameObject turretEdge;
        [SerializeField] private GameObject shellsContainer;
        [SerializeField] private Animator shootAnimationPoint;

        private InGameMenu _inGameMenu;

        private void Awake() {
            _controls = new PlayerControls();
            _ammoManager = GetComponent<AmmoManager>();
        }

        private void Start() {
            _inGameMenu = GameObject.FindGameObjectWithTag("GameUI").GetComponent<InGameMenu>();
        }

        private void Update() {
            HandleShellReload();
        }

        private void HandleShellReload() {
            if (!CanFireLightShell) {
                LightShellCdTimer += Time.deltaTime;
                if (LightShellCdTimer > tankStatsSo.LightShellReloadTime) {
                    CanFireLightShell = true;
                    LightShellCdTimer = 0f;
                }
            }
            if (!CanFireEmpShell) {
                EmpShellCdTimer += Time.deltaTime;
                if (EmpShellCdTimer > tankStatsSo.EmpShellReloadTime) {
                    CanFireEmpShell = true;
                    EmpShellCdTimer = 0f;
                }
            }
        }

        private void ShootShell(InputAction.CallbackContext ctx) {
            var selectedShell = _inGameMenu.SelectedShell;
            if (CanFireLightShell && selectedShell.CompareTag("TankLightShell")) {
                _ammoManager.LightShellAmmo -= 1;
                CanFireLightShell = false;
                shootAnimationPoint.SetBool("IsShooting", true);
                Instantiate(selectedShell, turretEdge.transform.position, Quaternion.identity, shellsContainer.transform);
            } // TODO: find a better way to handle this separation
            else if (CanFireEmpShell && selectedShell.CompareTag("TankEmpShell")) {
                _ammoManager.EmpShellAmmo -= 1;
                CanFireEmpShell = false;
                shootAnimationPoint.SetBool("IsShooting", true);
                Instantiate(selectedShell, turretEdge.transform.position, Quaternion.identity, shellsContainer.transform);
            }
        }


        private void OnEnable() {
            _controls.Player.Shoot.Enable();
            _controls.Player.Shoot.performed += ShootShell;
        }

        private void OnDisable() {
            _controls.Player.Shoot.performed -= ShootShell;
            _controls.Player.Shoot.Disable();
        }
    }
}