using UnityEngine;

namespace Shells.Tank {
    public class TankEmpShell : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionPs, explosionWavePs, trailPs;
        [SerializeField] private GameObject aoeHitAreaObj;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        private AreaOfEffect _aoeScript;

        private ParticleSystem.EmissionModule _explosionEmMod, _explosionWaveEmMod, _trailEmMod;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

            _explosionEmMod = explosionPs.emission;
            _explosionWaveEmMod = explosionWavePs.emission;
            _trailEmMod = trailPs.emission;
        }

        private void Start() {
            _aoeScript = transform.Find("AreaOfEffect").GetComponent<AreaOfEffect>();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TowerObj") || col.gameObject.CompareTag("WorldBorder") || col.gameObject.CompareTag("HomingShell")) {
                DestroyShell();
            }
        }

        // using Stay for the hit area sibling, so the shell can be as close to its position as possible
        private void OnTriggerStay2D(Collider2D col) {
            if (col.gameObject.CompareTag("AoeHitArea")) {
                DestroyShell();
            }
        }

        private void DestroyShell() {
            Destroy(aoeHitAreaObj);
            _trailEmMod.enabled = false;
            _aoeScript.EnableCircleCollider();
            _sr.enabled = false;
            _capsuleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            _explosionEmMod.enabled = true;
            _explosionWaveEmMod.enabled = true;
            explosionPs.Play();
            explosionWavePs.Play();

            Invoke(nameof(DestroyObj), 2.0f);
        }

        private void DestroyObj() {
            Destroy(transform.parent.gameObject);
        }
    }
}