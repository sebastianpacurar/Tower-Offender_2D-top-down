using System;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class TurretController : MonoBehaviour {
        public bool IsPowerOff { get; set; }
        public float ShootTimer { get; private set; }
        public float PowerOffTimer { get; private set; }

        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Transform turretEdge;
        [SerializeField] private GameObject shellsContainer;
        [SerializeField] private Transform circleRangeTransform;
        [SerializeField] private SpriteRenderer circleRangeSr;
        [SerializeField] private LineRenderer detectionLine;

        private Animator _shootAnimation;
        private Transform _tankPos;
        private TankHpManager _tankHpHandler;
        private bool _detected;
        private bool _canFire = true;
        private bool _towerDetected;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private void Awake() {
            _shootAnimation = GetComponent<Animator>();
        }

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _tankPos = tank.transform;
            _tankHpHandler = tank.GetComponent<TankHpManager>();

            circleRangeSr.color = towerStatsSo.CircleRangeAreaColor;
            circleRangeTransform.localScale = new Vector3(towerStatsSo.Range * 2, towerStatsSo.Range * 2, 1f);

            detectionLine.positionCount = 2;
            detectionLine.SetPosition(0, transform.position);
            detectionLine.SetPosition(1, _tankPos.position);
        }

        private void HandlePowerOffCd() {
            if (!IsPowerOff) return;
            PowerOffTimer += Time.deltaTime;
            if (PowerOffTimer > tankStatsSo.EmpShellStatsSo.AoeEffectDuration) {
                IsPowerOff = false;
                PowerOffTimer = 0f;
            }
        }

        private void Update() {
            HandlePowerOffCd();
            if (IsPowerOff) {
                circleRangeSr.enabled = false;
                detectionLine.enabled = false;
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
            var rayMask = 1 << LayerMask.NameToLayer("Military Unit");
            var rayInfo = Physics2D.RaycastAll(transform.position, direction, towerStatsSo.Range, rayMask);

            var playerIndex = Array.FindIndex(rayInfo[1..], obj => obj.collider.CompareTag("Player"));
            _towerDetected = Array.FindLastIndex(rayInfo[1..], obj => obj.collider.CompareTag("TowerObj")) > -1;

            if (playerIndex >= 0) {
                SetDetectionOn(direction, _towerDetected);
            } else {
                SetDetectionOff();
            }
        }

        private void SetDetectionOn(Vector3 direction, bool isTowerDetected) {
            _detected = true;
            detectionLine.SetPosition(1, _tankPos.position);
            detectionLine.enabled = true;
            circleRangeSr.enabled = false;
            detectionLine.colorGradient = isTowerDetected ? towerStatsSo.DetectionLineInactiveColor : towerStatsSo.DetectionLineActiveColor;
            transform.transform.up = direction;
        }

        private void SetDetectionOff() {
            _detected = false;
            detectionLine.SetPosition(1, transform.position);
            detectionLine.enabled = false;
            circleRangeSr.enabled = true;
        }

        private void Shoot() {
            if (!_detected || _towerDetected) return;

            if (!_canFire) {
                ShootTimer += Time.deltaTime;
                if (ShootTimer > towerStatsSo.SecondsBetweenShooting) {
                    _canFire = true;
                    ShootTimer = 0f;
                }
            } else {
                _canFire = false;
                _shootAnimation.SetBool(IsShooting, true);
                Instantiate(towerStatsSo.ShellPrefab, turretEdge.position, Quaternion.identity, shellsContainer.transform);
            }
        }
    }
}