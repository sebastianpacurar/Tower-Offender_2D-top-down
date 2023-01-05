using System;
using Player;
using UnityEngine;

namespace Enemy {
    public class Tower : MonoBehaviour {
        [SerializeField] private float range;
        [SerializeField] private SpriteRenderer triggerLight;
        [SerializeField] private Transform turretPos;
        [SerializeField] private Transform turretEdge;
        [SerializeField] private GameObject targetBullet;
        [SerializeField] private GameObject bulletsContainer;
        [SerializeField] private float timeBetweenFiring;
        [SerializeField] private Animator turretFireAnimation;

        private GameObject _resultBullet;
        private Transform _tankPos;
        private HpHandler _tankHpHandler;
        private bool _detected;
        private float _timer;
        private bool _canFire = true;

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _tankPos = tank.transform;
            _tankHpHandler = tank.GetComponent<HpHandler>();
        }

        private void Update() {
            HandleDetection();
            Shoot();
        }

        private void HandleDetection() {
            if (_tankHpHandler.HealthPoints == 0) {
                SetDetectionOff();
                return;
            }

            var direction = _tankPos.position - turretPos.position;
            var rayInfo = Physics2D.RaycastAll(turretPos.position, direction, range);
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
                _timer += Time.deltaTime;
                if (_timer > timeBetweenFiring) {
                    _canFire = true;
                    _timer = 0f;
                }
            } else {
                _canFire = false;
                turretFireAnimation.SetBool("IsShooting", true);
                Instantiate(targetBullet, turretEdge.position, Quaternion.identity, bulletsContainer.transform);
            }
        }
    }
}