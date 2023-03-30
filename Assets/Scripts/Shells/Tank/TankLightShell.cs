using Shells.Tank.AoeScalers;
using TileMap;
using UnityEngine;

namespace Shells.Tank {
    public class TankLightShell : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionPs, trailPs;
        private WallTileManager _wallMapManager;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private ParticleSystem.EmissionModule _explosionEmMod;
        private LightShellAoeScaler _lightShellAoeScaler;

        private void Awake() {
            _wallMapManager = FindObjectOfType<WallTileManager>();

            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _lightShellAoeScaler = GameObject.FindGameObjectWithTag("LightShellGhost").GetComponent<LightShellAoeScaler>();
        }

        private void Start() {
            _explosionEmMod = explosionPs.emission;
        }

        private void OnCollisionEnter2D(Collision2D col) {
            if (col.gameObject.CompareTag("BodyObj") || col.gameObject.CompareTag("WorldBorder") || col.gameObject.CompareTag("SafeWalls") || col.gameObject.CompareTag(tag)) {
                DestroyShell();
            }

            // increase LightShellGhost radius with 0.1f when turret is hit
            if (col.gameObject.CompareTag("TurretObj")) {
                //TODO: think of a way to handle this dynamical
                _lightShellAoeScaler.finalRadiusVal = _lightShellAoeScaler.radiusVal + 0.5f;
                DestroyShell();
            }

            if (col.gameObject.CompareTag("Wall")) {
                _lightShellAoeScaler.finalRadiusVal = _lightShellAoeScaler.radiusVal + 0.25f;
                var hit = col.GetContact(0);

                DisablePhysics();

                // NOTE: this destroys the exact wall which was hit, even though the collision point is on the edge of the WallTile 
                // calculate location of hit tile
                Vector2 hitPos;
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.y - 0.01f * hit.normal.y;
                _wallMapManager.HandleWallTile(hitPos);

                DestroyShell();
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