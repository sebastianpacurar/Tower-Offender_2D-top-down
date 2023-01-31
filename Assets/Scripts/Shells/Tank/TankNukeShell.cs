using UnityEngine;

namespace Shells.Tank {
    public class TankNukeShell : MonoBehaviour {
        [SerializeField] private ParticleSystem[] nukeExplosionPs;
        [SerializeField] private ParticleSystem trailPs;
        [SerializeField] private GameObject aoeHitAreaObj;

        private CircleCollider2D _circleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        private NukeAreaOfEffect _nukeAoeScript;

        private ParticleSystem.EmissionModule _trailEmMod;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            _trailEmMod = trailPs.emission;
        }

        private void Start() {
            _nukeAoeScript = transform.Find("AreaOfEffect").GetComponent<NukeAreaOfEffect>();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("WorldBorder")) {
                DestroyShell();
            }
        }

        // using Stay for the hit area sibling, so the shell can be as close to its position as possible
        private void OnTriggerStay2D(Collider2D col) {
            if (col.gameObject.CompareTag("NukeAoeHitArea")) {
                transform.position = col.gameObject.transform.position;
                DestroyShell();
            }
        }

        private void DestroyShell() {
            Destroy(aoeHitAreaObj);
            _trailEmMod.enabled = false;
            _nukeAoeScript.EnableCircleCollider();
            _sr.enabled = false;
            _circleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            StartPsEmModsAndPlay();
            Invoke(nameof(DestroyObj), 3.0f);
        }

        private void DestroyObj() {
            Destroy(transform.parent.gameObject);
        }

        private void StartPsEmModsAndPlay() {
            foreach (var ps in nukeExplosionPs) {
                var emMod = ps.emission;
                emMod.enabled = true;
                ps.Play();
            }
        }
    }
}