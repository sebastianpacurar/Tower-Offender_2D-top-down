using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Shoot : MonoBehaviour {
        private float _timer;
        private PlayerControls _controls;
        public bool CanFire { get; private set; }

        [SerializeField] private float timeBetweenFiring;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject turretEdge;
        [SerializeField] private GameObject bulletsContainer;
        [SerializeField] private Animator shootAnimationPoint;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Update() {
            if (!CanFire) {
                _timer += Time.deltaTime;
                if (_timer > timeBetweenFiring) {
                    CanFire = true;
                    _timer = 0f;
                }
            }
        }

        private void ShootBullet(InputAction.CallbackContext ctx) {
            if (CanFire) {
                Instantiate(bulletPrefab, turretEdge.transform.position, Quaternion.identity, bulletsContainer.transform);
                shootAnimationPoint.SetBool("IsShooting", true);
                CanFire = false;
            }
        }

        private void OnEnable() {
            _controls.Player.Shoot.Enable();
            _controls.Player.Shoot.performed += ShootBullet;
        }

        private void OnDisable() {
            _controls.Player.Shoot.performed -= ShootBullet;
            _controls.Player.Shoot.Disable();
        }
    }
}