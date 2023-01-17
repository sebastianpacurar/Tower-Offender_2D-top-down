using Player.Controllers;
using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank {
    public class TankEmpShell : MonoBehaviour {
        [SerializeField] private TankShellStatsSo empShellStats;
        [SerializeField] private ParticleSystem explosionPs, trailPs;
        [SerializeField] private GameObject aoeHitAreaObj;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        private AreaOfEffect _aoeScript;

        private ParticleSystem.EmissionModule _explosionEmMod, _trailEmMod;
        private AimController _ac;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

            _explosionEmMod = explosionPs.emission;
            _trailEmMod = trailPs.emission;
        }

        private void Start() {
            _aoeScript = transform.Find("AreaOfEffect").GetComponent<AreaOfEffect>();
            _ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();

            var direction = _ac.AimVal - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * empShellStats.Speed;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TowerObj") || col.gameObject.CompareTag("WorldBorder")) {
                DestroyShell();
            }
        }

        public void DestroyShell() {
            Destroy(aoeHitAreaObj);
            _aoeScript.EnableCircleCollider();
            _sr.enabled = false;
            _capsuleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            _explosionEmMod.enabled = true;
            _trailEmMod.enabled = false;
            explosionPs.Play();

            Invoke(nameof(DestroyObj), 2.0f);
        }

        private void DestroyObj() {
            Destroy(transform.parent.gameObject);
        }
    }
}