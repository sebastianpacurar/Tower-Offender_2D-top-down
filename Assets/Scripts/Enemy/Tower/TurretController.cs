using System;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class TurretController : MonoBehaviour {
        public bool IsPowerOff { get; set; }
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private TankShellStatsSo empShellStatsSo;
        [SerializeField] private SpriteRenderer triggerLight;
        [SerializeField] private Transform turretEdge;
        [SerializeField] private GameObject shellsContainer;
        [SerializeField] private Transform circleRangeTransform;
        [SerializeField] private SpriteRenderer circleRangeSr;
        [SerializeField] private LineRenderer detectionLine;

        private Animator _shootAnimation;
        private Transform _tankPos;
        private TankHpManager _tankHpHandler;
        private bool _detected;
        public float ShootTimer { get; private set; }
        public float PowerOffTimer { get; private set; }
        private bool _canFire = true;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private void Awake() {
            _shootAnimation = GetComponent<Animator>();
        }

        private void Start() {
            var grandParentName = transform.parent.parent.name;
            var tank = GameObject.FindGameObjectWithTag("Player");
            _tankPos = tank.transform;
            _tankHpHandler = tank.GetComponent<TankHpManager>();

            if (grandParentName.Contains("Light")) {
                circleRangeSr.color = new Color(1f, 1f, 0f, 0.05f);
                detectionLine.startColor = new Color(1f, 1f, 0f, 0.25f);
                detectionLine.endColor = new Color(1f, 1f, 0f, 0.25f);
            } else if (grandParentName.Contains("Mid")) {
                circleRangeSr.color = new Color(1f, 0f, 0f, 0.05f);
                detectionLine.startColor = new Color(1f, 0f, 0f, 0.25f);
                detectionLine.endColor = new Color(1f, 0f, 0f, 0.25f);
            } else if (grandParentName.Contains("Heavy")) {
                circleRangeSr.color = new Color(0.5411765f, 0.1686275f, 0.8862745f, 0.05f);
                detectionLine.startColor = new Color(0.5411765f, 0.1686275f, 0.8862745f, 0.25f);
                detectionLine.endColor = new Color(0.5411765f, 0.1686275f, 0.8862745f, 0.025f);
            }

            circleRangeTransform.localScale = new Vector3(towerStatsSo.Range * 2, towerStatsSo.Range * 2, 1f);
            detectionLine.positionCount = 2;
            detectionLine.SetPosition(0, transform.position);
            detectionLine.SetPosition(1, _tankPos.position);
        }

        private void HandlePowerOffCd() {
            if (!IsPowerOff) return;
            PowerOffTimer += Time.deltaTime;
            if (PowerOffTimer > empShellStatsSo.AoeEffectDuration) {
                IsPowerOff = false;
                PowerOffTimer = 0f;
            }
        }

        private void Update() {
            HandlePowerOffCd();
            if (IsPowerOff) {
                circleRangeSr.enabled = false;
                detectionLine.enabled = false;
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
            detectionLine.SetPosition(1, _tankPos.position);
            detectionLine.enabled = true;
            circleRangeSr.enabled = false;

            triggerLight.color = Color.yellow;
            transform.transform.up = direction;
        }

        private void SetDetectionOff() {
            _detected = false;
            detectionLine.SetPosition(1, transform.position);
            detectionLine.enabled = false;
            circleRangeSr.enabled = true;
            
            triggerLight.color = new Color(0f, 0.75f, 0f, 1f);
        }

        private void Shoot() {
            if (!_detected) return;

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