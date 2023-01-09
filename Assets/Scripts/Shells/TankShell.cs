using Player;
using UnityEngine;

namespace Shells {
    public class TankShell : MonoBehaviour {
        [SerializeField] private float speed;
        private CapsuleCollider2D _capsuleCollider2D;
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
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Start() {
            _ps = transform.Find("Explode Particle System").GetComponent<ParticleSystem>();
            _emissionModule = _ps.emission;

            _ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _mousePos = _mainCam.ScreenToWorldPoint(_ac.AimVal);

            var direction = _mousePos - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ - 90f);
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Tower") || col.gameObject.CompareTag("WorldBorder")) {
                DestroyShell();
            }

            if (col.gameObject.CompareTag("Wall")) {
                DestroyShell();
                Destroy(col.gameObject);
            }
        }

        private void DestroyShell() {
            _emissionModule.enabled = true;
            _rb.velocity = new Vector2(0, 0);
            Destroy(_capsuleCollider2D);
            Destroy(_sr);
            Invoke(nameof(DestroyObj), 0.5f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}