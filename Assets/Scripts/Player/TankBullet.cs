using UnityEngine;

namespace Player {
    public class TankBullet : MonoBehaviour {
        [SerializeField] private float speed;
        private CircleCollider2D _circleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private ParticleSystem _ps;
        private ParticleSystem.EmissionModule _emissionModule;

        private AimController _ac;
        private Vector3 _mousePos;
        private Camera _mainCam;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void Start() {
            _ps = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _emissionModule = _ps.emission;

            _ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _mousePos = _mainCam.ScreenToWorldPoint(_ac.AimVal);
            var direction = _mousePos - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Tower")) {
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