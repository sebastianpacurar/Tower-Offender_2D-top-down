using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class AimController : MonoBehaviour {
        public Vector2 AimVal { get; set; }

        private PlayerControls _controls;
        private Camera _mainCam;

        [SerializeField] private GameObject hull;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Aim(InputAction.CallbackContext ctx) {
            AimVal = _controls.Player.Aim.ReadValue<Vector2>();
        }

        private void Update() {
            MoveHull();
        }

        private void MoveHull() {
            var mousePos = _mainCam.ScreenToWorldPoint(AimVal);

            var direction = mousePos - hull.transform.position;
            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            hull.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        private void OnEnable() {
            _controls.Player.Aim.Enable();
            _controls.Player.Aim.performed += Aim;
        }

        private void OnDisable() {
            _controls.Player.Aim.performed -= Aim;
            _controls.Player.Aim.Disable();
        }
    }
}