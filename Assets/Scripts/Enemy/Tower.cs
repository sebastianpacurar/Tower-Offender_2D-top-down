using System;
using System.Linq;
using UnityEngine;

namespace Enemy {
    public class Tower : MonoBehaviour {
        [SerializeField] private float range;
        [SerializeField] private SpriteRenderer triggerLight;
        [SerializeField] private Transform turret;
        [SerializeField] private Transform turretEdge;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject bulletsContainer;
        [SerializeField] private float timeBetweenFiring;
        private Transform _tankPos;
        private bool _detected;
        private float _timer;
        private bool _canFire = true;

        private void Start() {
            _tankPos = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update() {
            HandleDetection();
            Shoot();
        }

        private void HandleDetection() {
            var turretPos = transform.position;
            var direction = _tankPos.position - turretPos;
            var rayInfo = Physics2D.RaycastAll(turretPos, direction, range);
            var playerIndex = Array.FindIndex(rayInfo, obj => obj.collider.CompareTag("Player"));
            var wallIndex = Array.FindIndex(rayInfo, obj => obj.collider.CompareTag("Wall"));
            
            if (playerIndex < wallIndex) {
                SetDetectionOn(direction);
            } else {
                SetDetectionOff();
            }
        }

        private void SetDetectionOn(Vector3 direction) {
            _detected = true;
            triggerLight.color = Color.green;
            turret.transform.up = direction;
        }

        private void SetDetectionOff() {
            _detected = false;
            triggerLight.color = Color.red;
        }

        private void Shoot() {
            if (!_detected) return;

            if (!_canFire) {
                _timer += Time.deltaTime;
                if (_timer > timeBetweenFiring) {
                    _canFire = true;
                    _timer = 0f;
                }
            } else {
                _canFire = false;
                Instantiate(bulletPrefab, turretEdge.position, Quaternion.identity, bulletsContainer.transform);
            }
        }
    }
}