using TileMap;
using UnityEngine;

namespace Shells.Tank {
    public class TankLightShell : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionPs, trailPs;
        private WallTileManager mapManager;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private ParticleSystem.EmissionModule _explosionEmMod;

        private void Awake() {
            mapManager = FindObjectOfType<WallTileManager>();

            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Start() {
            _explosionEmMod = explosionPs.emission;
        }

        private void OnCollisionEnter2D(Collision2D col) {
            if (col.gameObject.CompareTag("TurretObj") || col.gameObject.CompareTag("BodyObj") || col.gameObject.CompareTag("WorldBorder") || col.gameObject.CompareTag(tag)) {
                DestroyShell();
            }

            if (col.gameObject.CompareTag("Wall")) {
                var isChecked = false;

                for (var i = 0; i < col.contacts.Length; i++) {
                    var hit = col.GetContact(i);

                    // if the hit area is the one related to this specific shell
                    if (hit.otherRigidbody.Equals(_rb)) {
                        // TODO: investigate this issue in more detail - why are there more than 1 iterations although the length is 1
                        // avoid reiteration in case there are more contacts related to the same shell
                        if (!isChecked) {
                            isChecked = true;
                            DisablePhysics();

                            // calculate location of hit tile
                            Vector2 hitPos;
                            hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                            hitPos.y = hit.point.y - 0.01f * hit.normal.y;

                            mapManager.HandleWallTileLife(hitPos);
                            DestroyShell();
                        }
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("LightShellGhost")) {
                DisablePhysics();
                DestroyShell();
            }
        }

        // destroy Trail Particle System and halt all active physics 
        private void DisablePhysics() {
            trailPs.gameObject.SetActive(false);
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void DestroyShell() {
            _explosionEmMod.enabled = true;
            _capsuleCollider2D.enabled = false;
            _sr.enabled = false;

            Invoke(nameof(DestroyObj), 0.5f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}