using UnityEngine;

namespace Shells.Tank {
    public class TankSniperShell : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionPs, trailPs;
        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private ParticleSystem.EmissionModule _explosionEmMod, _trailEmMod;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

            _explosionEmMod = explosionPs.emission;
            _trailEmMod = trailPs.emission;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("WorldBorder")) {
                DestroyShell();
            }

            if (col.gameObject.CompareTag("TowerObj")) {
                _explosionEmMod.enabled = true;
            }
        }

        private void OnTriggerStay2D(Collider2D col) {
            if (col.gameObject.CompareTag("TowerObj")) {
                explosionPs.transform.position = col.gameObject.transform.position;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            _explosionEmMod.enabled = false;
        }

        private void DestroyShell() {
            _rb.velocity = new Vector2(0, 0);
            _capsuleCollider2D.enabled = false;
            _trailEmMod.enabled = false;

            _sr.enabled = false;
            _explosionEmMod.enabled = true;
            Invoke(nameof(DestroyObj), 2.0f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}