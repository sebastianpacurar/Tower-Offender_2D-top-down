using System;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class TurretController : MonoBehaviour {
        public bool IsPowerOff { get; set; }
        public float ShootTimer { get; private set; }
        public float PowerOffTimer { get; private set; }

        public TurretStatsSo turretStatsSo; // set to public since it's called in BodyController, to provide the next cash value of the spawned turret
        [SerializeField] private Transform turretEdge;
        [SerializeField] private Transform circleRangeTransform;
        [SerializeField] private SpriteRenderer circleRangeSr;
        [SerializeField] private LineRenderer detectionLine;

        private Animator _shootAnimation;
        private Transform _tankPos;
        private Transform _shellsContainer;
        private TankHpManager _tankHpHandler;
        private WeaponStatsManager _weaponStats;

        private bool _detected;
        private bool _canFire = true;
        private bool _canShootAtTank;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private void Awake() {
            var container = transform.parent.parent;

            // set the name of the grandparent (container of all) to contain the position so it can match the tower body's position
            container.name = $"{container.name}_{transform.position.ToString()}";
            _shootAnimation = GetComponent<Animator>();

            var tank = GameObject.FindGameObjectWithTag("Player");
            _tankPos = tank.transform;
            _tankHpHandler = tank.GetComponent<TankHpManager>();

            // attach the shells to its matching body position
            _shellsContainer = GameObject.FindGameObjectWithTag("ShellsContainer").transform;
        }

        private void Start() {
            _weaponStats = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStatsManager>();

            //TODO: currently not used since object is disabled in prefab hierarchy
            // circleRangeSr.color = turretStatsSo.CircleRangeAreaColor;
            // circleRangeTransform.localScale = new Vector3(turretStatsSo.Range * 2, turretStatsSo.Range * 2, 1f);

            detectionLine.enabled = false;
            detectionLine.positionCount = 2;
            detectionLine.SetPosition(0, transform.position);
            detectionLine.SetPosition(1, _tankPos.position);
        }

        private void HandlePowerOffCd() {
            if (!IsPowerOff) return;
            PowerOffTimer += Time.deltaTime;

            if (PowerOffTimer > _weaponStats.empAoeDuration) {
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
            var rayMask = 1 << LayerMask.NameToLayer("Military Unit") | 1 << LayerMask.NameToLayer("Safe Wall");
            var rayInfo = Physics2D.RaycastAll(transform.position, direction, turretStatsSo.Range, rayMask);

            var playerIndex = Array.FindIndex(rayInfo[1..], obj => obj.collider.CompareTag("Player"));
            var wallIndex = Array.FindIndex(rayInfo[1..], obj => obj.collider.CompareTag("SafeWalls"));

            // NOTE: return true if there is no other turret between the current turret and tank, and if there is no safeWall between the current turret and the tank
            _canShootAtTank = Array.FindLastIndex(rayInfo[1..], obj => obj.collider.CompareTag("TurretObj")) == -1 && (playerIndex < wallIndex || wallIndex == -1);

            if (playerIndex >= 0) {
                SetDetectionOn(direction, _canShootAtTank);
            } else {
                SetDetectionOff();
            }
        }

        // set detection to true if _canShootAtTank, and the colors accodingly. 
        private void SetDetectionOn(Vector3 direction, bool active) {
            _detected = true;
            detectionLine.SetPosition(1, _tankPos.position);
            detectionLine.enabled = true;
            circleRangeSr.enabled = false;
            detectionLine.colorGradient = active ? turretStatsSo.DetectionLineActiveColor : turretStatsSo.DetectionLineInactiveColor;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        private void SetDetectionOff() {
            _detected = false;
            detectionLine.SetPosition(1, transform.position);
            detectionLine.enabled = false;
            circleRangeSr.enabled = true;
        }

        private void Shoot() {
            if (!_detected || !_canShootAtTank) return;

            if (!_canFire) {
                ShootTimer += Time.deltaTime;

                if (ShootTimer > turretStatsSo.SecondsBetweenShooting) {
                    _canFire = true;
                    ShootTimer = 0f;
                }
            } else {
                _canFire = false;
                _shootAnimation.SetBool(IsShooting, true);
                Instantiate(turretStatsSo.ShellPrefab, turretEdge.position, Quaternion.identity, _shellsContainer);
            }
        }
    }
}