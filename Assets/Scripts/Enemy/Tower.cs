using System;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Enemy {
    public class Tower : MonoBehaviour {
        public bool IsPowerOff { get; set; } = false;
        [SerializeField] private TowerStatsSo towerStatsSo;

        [SerializeField] private SpriteRenderer triggerLight;
        [SerializeField] private Transform turretPos;
        [SerializeField] private Transform turretEdge;
        [SerializeField] private GameObject shellsContainer;
        [SerializeField] private Animator turretFireAnimation;

        private Transform _tankPos;
        private HpHandler _tankHpHandler;
        private bool _detected;
        private float _fireTimer;
        private float _powerOFfTimer;
        private bool _canFire = true;

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _tankPos = tank.transform;
            _tankHpHandler = tank.GetComponent<HpHandler>();
        }

        private void HandlePowerOffCd() {
            if (!IsPowerOff) return;
            _powerOFfTimer += Time.deltaTime;
            if (_powerOFfTimer > 10f) {
                IsPowerOff = false;
                _powerOFfTimer = 0f;
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

            var direction = _tankPos.position - turretPos.position;
            var rayInfo = Physics2D.RaycastAll(turretPos.position, direction, towerStatsSo.Range);
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
            turretPos.transform.up = direction;
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
                turretFireAnimation.SetBool("IsShooting", true);
                Instantiate(towerStatsSo.ShellPrefab, turretEdge.position, Quaternion.identity, shellsContainer.transform);
            }
        }
    }
}