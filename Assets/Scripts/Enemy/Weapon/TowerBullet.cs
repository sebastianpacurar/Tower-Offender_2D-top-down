using UnityEngine;

namespace Enemy.Weapon {
    public class TowerBullet : MonoBehaviour {
        [SerializeField] private float bulletSpeed;
        private Transform _towerPosition;
        private CircleCollider2D _circleCollider2D;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Transform _tankPos;

        private ParticleSystem _ps;
        private ParticleSystem.EmissionModule _emissionModule;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void Start() {
            _tankPos = GameObject.FindGameObjectWithTag("Player").transform;
            _ps = transform.Find("Shell").Find("Particle System").GetComponent<ParticleSystem>();
            _emissionModule = _ps.emission;

            // in case the passing value is a multiShell game object
            if (!transform.parent.name.Equals("BulletsContainer")) {
                _towerPosition = transform.parent.transform.parent.transform.parent.Find("TowerObj").gameObject.transform;
            } else {
                _towerPosition = transform.parent.transform.parent.Find("TowerObj").gameObject.transform;
            }

            var direction = _tankPos.position - _towerPosition.position;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Tower")) {
                DestroyBullet();
            }
        }

        private void DestroyBullet() {
            _emissionModule.enabled = true;
            _rb.velocity = new Vector2(0, 0);
            Destroy(_circleCollider2D);
            Destroy(_sr);
            Invoke(nameof(DestroyObj), 0.25f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}