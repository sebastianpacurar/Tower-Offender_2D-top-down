using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class AimController : MonoBehaviour {
        public Vector3 AimVal { get; private set; }

        [SerializeField] private GameObject hull;
        [SerializeField] private GameObject aoeGhost;

        private PlayerControls _controls;
        private Camera _mainCam;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Aim(InputAction.CallbackContext ctx) {
            AimVal = _mainCam.ScreenToWorldPoint(_controls.Player.Aim.ReadValue<Vector2>());
        }

        private void Update() {
            MoveHull();
        }

        private void MoveHull() {
            var direction = AimVal - hull.transform.position;
            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            hull.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            aoeGhost.transform.position = new Vector3(AimVal.x, AimVal.y, 0f);
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