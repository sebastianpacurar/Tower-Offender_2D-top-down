using System;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Enemy {
    public class TurretController : MonoBehaviour {
        public bool IsPowerOff { get; set; } = false;
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private SpriteRenderer triggerLight;
        [SerializeField] private Transform turretEdge;
        [SerializeField] private GameObject shellsContainer;
        private Animator _shootAnimation;
        private Transform _tankPos;
        private TankHpManager _tankHpHandler;
        private bool _detected;
        private float _fireTimer, _powerOffTimer;
        private bool _canFire = true;

        private void Awake() {
            _shootAnimation = GetComponent<Animator>();
        }

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _tankPos = tank.transform;
            _tankHpHandler = tank.GetComponent<TankHpManager>();
        }

        private void HandlePowerOffCd() {
            if (!IsPowerOff) return;
            _powerOffTimer += Time.deltaTime;
            if (_powerOffTimer > 10f) {
                IsPowerOff = false;
                _powerOffTimer = 0f;
            }
        }

        private void Update() {
            HandlePowerOffCd();
            if (IsPowerOff) {
                triggerLight.color = Color.grey;
            } else {
                HandleDetection();
                Shoot();
            }
        }

        private void HandleDetection() {
            if (_tankHpHandler.TankHealthPoints <= 0) {
                SetDetectionOff();
                return;
            }

            var direction = _tankPos.position - transform.position;
            var rayInfo = Physics2D.RaycastAll(transform.position, direction, towerStatsSo.Range);
            var playerIndex = Array.FindIndex(rayInfo, obj => obj.collider.CompareTag("Player"));
            var wallIndex = Array.FindIndex(rayInfo, obj => obj.collider.CompareTag("Wall"));

            if (playerIndex >= 0) {
                SetDetectionOn(direction);
            } else {
                SetDetectionOff();
            }
        }

        private void SetDetectionOn(Vector3 direction) {
            _detected = true;
            triggerLight.color = Color.green;
            transform.transform.up = direction;
        }

        private void SetDetectionOff() {
            _detected = false;
            triggerLight.color = Color.red;
        }

        private void Shoot() {
            if (!_detected) return;

            if (!_canFire) {
                _fireTimer += Time.deltaTime;
                if (_fireTimer > towerStatsSo.SecondsBetweenShooting) {
                    _canFire = true;
                    _fireTimer = 0f;
                }
            } else {
                _canFire = false;
                _shootAnimation.SetBool("IsShooting", true);
                Instantiate(towerStatsSo.ShellPrefab, turretEdge.position, Quaternion.identity, shellsContainer.transform);
            }
        }
    }
}