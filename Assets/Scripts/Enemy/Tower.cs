using UnityEngine;

namespace Enemy {
    public class Tower : MonoBehaviour {
        [SerializeField] private float range;
        [SerializeField] private SpriteRenderer triggerLight;
        [SerializeField] private Transform turret;
        [SerializeField] private Transform turretEdge;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float timeBetweenFiring;
        [SerializeField] private float bulletSpeed;
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

            if (rayInfo.Length > 0) {
                if (rayInfo[1].collider.CompareTag("Player")) {
                    SetDetectionOn(direction);
                } else {
                    SetDetectionOff();
                }
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
                var bullet = Instantiate(bulletPrefab, turretEdge.position, Quaternion.identity);
                var direction = _tankPos.transform.position - transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
            }
        }
    }
}