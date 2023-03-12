using System.Collections;
using Menus;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class ShootController : MonoBehaviour {
        private PlayerControls _controls;
        public bool IsAutoShootOn { get; private set; }

        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private GameObject turretEdge;
        [SerializeField] private GameObject shellsContainer;
        [SerializeField] private Animator shootAnimationPoint;

        [SerializeField] private float lightShellCdTimer;
        [SerializeField] private bool canFireLightShell;

        [SerializeField] private float sniperShellCdTimer;
        [SerializeField] private bool canFireSniperShell;

        [SerializeField] private float distanceBetweenTankAndMouse;

        [SerializeField] private float empShellCdTimer;
        public bool canFireEmpShell; // used in InGameMenu to disable the collider and dim the color of the circle range when reloading

        [SerializeField] private float nukeShellCdTimer;
        public bool canFireNukeShell; // used in InGameMenu to disable the collider and dim the color of the circle range when reloading

        private InGameMenu _inGameMenu;
        private AmmoManager _ammoManager;
        private AimController _aimController;
        private Camera _camera;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        // prevent from shooting when mouse is too close to the tank
        private float ValidateShootPoint() {
            var mouseWorldPos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return Vector2.Distance(mouseWorldPos, transform.position);
        }

        private void Awake() {
            _controls = new PlayerControls();
            _ammoManager = GetComponent<AmmoManager>();
            _aimController = GetComponent<AimController>();
            _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _inGameMenu = GameObject.FindGameObjectWithTag("GameUI").GetComponent<InGameMenu>();
        }
        
        private void Update() {
            HandleShellReload();
            distanceBetweenTankAndMouse = ValidateShootPoint();
        }

        private void HandleShellReload() {
            if (!canFireLightShell) {
                lightShellCdTimer += Time.deltaTime;

                if (lightShellCdTimer > tankStatsSo.LightShellReloadTime) {
                    canFireLightShell = true;
                    lightShellCdTimer = 0f;
                }
            }

            if (!canFireEmpShell) {
                empShellCdTimer += Time.deltaTime;

                if (empShellCdTimer > tankStatsSo.EmpShellReloadTime) {
                    canFireEmpShell = true;
                    empShellCdTimer = 0f;
                }
            }

            if (!canFireSniperShell) {
                sniperShellCdTimer += Time.deltaTime;

                if (sniperShellCdTimer > tankStatsSo.SniperShellReloadTime) {
                    canFireSniperShell = true;
                    sniperShellCdTimer = 0f;
                }
            }

            if (!canFireNukeShell) {
                nukeShellCdTimer += Time.deltaTime;

                if (nukeShellCdTimer > tankStatsSo.NukeShellReloadTime) {
                    canFireNukeShell = true;
                    nukeShellCdTimer = 0f;
                }
            }
        }

        // Shot LightShells automatically when left mouse button is held down
        // used in InGameMenu.cs
        public IEnumerator ShootLightShells() {
            while (true) {
                yield return new WaitUntil(() => canFireLightShell);

                // init only mouse is not hovering the tank collider + offset
                if (!(distanceBetweenTankAndMouse >= 0.8f)) continue;
                canFireLightShell = false;
                shootAnimationPoint.SetBool(IsShooting, true);
                var lightShell = Instantiate(_inGameMenu.SelectedShell, turretEdge.transform.position, Quaternion.identity, shellsContainer.transform);
                InitShell(lightShell, tankStatsSo.LightShellStatsSo.Speed);
            }
        }

        // ShootShell input event
        private void ShootShell(InputAction.CallbackContext ctx) {
            var selectedShell = _inGameMenu.SelectedShell;

            // treat Light Shell separately
            if (selectedShell.CompareTag("TankLightShell")) {
                switch (ctx.phase) {
                    case InputActionPhase.Started:
                    case InputActionPhase.Performed:
                        StartCoroutine(nameof(ShootLightShells));
                        IsAutoShootOn = true;
                        break;
                    case InputActionPhase.Canceled:
                        StopCoroutine(nameof(ShootLightShells));
                        IsAutoShootOn = false;
                        break;
                }
            } else {
                if (canFireEmpShell && selectedShell.CompareTag("TankEmpShellEntity")) {
                    // EMP Shell
                    _ammoManager.EmpShellAmmo -= 1;
                    canFireEmpShell = false;
                    shootAnimationPoint.SetBool(IsShooting, true);
                    var empShell = Instantiate(selectedShell, transform.position, Quaternion.identity, shellsContainer.transform);
                    InitShell(empShell, tankStatsSo.EmpShellStatsSo.Speed);
                } else if (canFireSniperShell && selectedShell.CompareTag("TankSniperShell")) {
                    // Sniper Shell
                    _ammoManager.SniperShellAmmo -= 1;
                    canFireSniperShell = false;
                    shootAnimationPoint.SetBool(IsShooting, true);
                    var sniperShell = Instantiate(selectedShell, transform.position, Quaternion.identity, shellsContainer.transform);
                    InitShell(sniperShell, tankStatsSo.SniperShellStatsSo.Speed);
                } else if (canFireNukeShell && selectedShell.CompareTag("TankNukeShellEntity")) {
                    // Nuke Shell
                    _ammoManager.NukeShellAmmo -= 1;
                    canFireNukeShell = false;
                    shootAnimationPoint.SetBool(IsShooting, true);
                    var nukeShell = Instantiate(selectedShell, transform.position, Quaternion.identity, shellsContainer.transform);
                    InitShell(nukeShell, tankStatsSo.NukeShellStatsSo.Speed);
                }
            }

            _inGameMenu.AutoSwitchToLightShellIfNoAmmo(selectedShell);
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
                case "TankNukeShellEntity":
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
            _controls.Player.Shoot.canceled += ShootShell;
        }

        private void OnDisable() {
            _controls.Player.Shoot.performed -= ShootShell;
            _controls.Player.Shoot.canceled -= ShootShell;
            _controls.Player.Shoot.Disable();
        }
    }
}