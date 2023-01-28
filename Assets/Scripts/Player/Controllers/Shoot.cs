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

        public float SniperShellCdTimer { get; private set; }
        public bool CanFireSniperShell { get; private set; } = true;

        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private GameObject turretEdge;
        [SerializeField] private GameObject shellsContainer;
        [SerializeField] private Animator shootAnimationPoint;

        private InGameMenu _inGameMenu;
        private AmmoManager _ammoManager;
        private AimController _aimController;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private void Awake() {
            _controls = new PlayerControls();
            _ammoManager = GetComponent<AmmoManager>();
            _aimController = GetComponent<AimController>();
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
            if (!CanFireSniperShell) {
                SniperShellCdTimer += Time.deltaTime;
                if (SniperShellCdTimer > tankStatsSo.SniperShellReloadTime) {
                    CanFireSniperShell = true;
                    SniperShellCdTimer = 0f;
                }
            }
        }

        private void ShootShell(InputAction.CallbackContext ctx) {
            var selectedShell = _inGameMenu.SelectedShell;
            if (CanFireLightShell && selectedShell.CompareTag("TankLightShell")) {
                _ammoManager.LightShellAmmo -= 1;
                CanFireLightShell = false;
                shootAnimationPoint.SetBool(IsShooting, true);
                var lightShell = Instantiate(selectedShell, turretEdge.transform.position, Quaternion.identity, shellsContainer.transform);
                InitShell(lightShell, tankStatsSo.LightShellStatsSo.Speed);
            } else if (CanFireEmpShell && selectedShell.CompareTag("TankEmpShellEntity")) {
                _ammoManager.EmpShellAmmo -= 1;
                CanFireEmpShell = false;
                shootAnimationPoint.SetBool(IsShooting, true);
                var empShell = Instantiate(selectedShell, transform.position, Quaternion.identity, shellsContainer.transform);
                InitShell(empShell, tankStatsSo.EmpShellStatsSo.Speed);
            } else if (CanFireSniperShell && selectedShell.CompareTag("TankSniperShell")) {
                _ammoManager.SniperShellAmmo -= 1;
                CanFireSniperShell = false;
                shootAnimationPoint.SetBool(IsShooting, true);
                var sniperShell = Instantiate(selectedShell, transform.position, Quaternion.identity, shellsContainer.transform);
                InitShell(sniperShell, tankStatsSo.SniperShellStatsSo.Speed);
            }
        }

        private void InitShell(GameObject obj, float moveSpeed) {
            var mousePos = _aimController.AimVal;
            var direction = mousePos - obj.transform.position;
            var rbVelocity = new Vector2(direction.x, direction.y).normalized * moveSpeed;
            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            switch (_inGameMenu.SelectedShell.tag) {
                case "TankLightShell":
                case "TankSniperShell":
                    obj.GetComponent<Rigidbody2D>().velocity = rbVelocity;
                    obj.transform.rotation = Quaternion.Euler(0, 0, rotZ);
                    break;

                case "TankEmpShellEntity":
                    var shellObj = obj.transform.GetChild(0);
                    var shellTransform = shellObj.transform;
                    var aoeObj = obj.transform.GetChild(1);
                    var startPoint = turretEdge.transform.position;

                    shellObj.GetComponent<Rigidbody2D>().velocity = rbVelocity;
                    shellTransform.rotation = Quaternion.Euler(0, 0, rotZ);
                    shellTransform.position = startPoint;
                    aoeObj.transform.position = mousePos;
                    break;
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