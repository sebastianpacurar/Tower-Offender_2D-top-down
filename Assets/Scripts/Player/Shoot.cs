using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Shoot : MonoBehaviour {
        private float _timer;
        private PlayerControls _controls;
        [SerializeField] private bool canFire;
        [SerializeField] private float timeBetweenFiring;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject turretEdge;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Update() {
            if (!canFire) {
                _timer += Time.deltaTime;
                if (_timer > timeBetweenFiring) {
                    canFire = true;
                }
            }
        }

        private void ShootBullet(InputAction.CallbackContext ctx) {
            if (canFire) {
                Instantiate(bulletPrefab, turretEdge.transform.position, Quaternion.identity);
                canFire = false;
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