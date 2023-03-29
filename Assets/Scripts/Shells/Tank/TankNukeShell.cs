using Player;
using UnityEngine;

namespace Shells.Tank {
    public class TankNukeShell : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionCenterPs, explosionPs, explosionWavePs, trailPs;
        [SerializeField] private GameObject aoeHitAreaObj;

        private CircleCollider2D _circleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        private NukeAreaOfEffect _nukeAoeScript;
        private WeaponStatsManager _weaponStats;

        private ParticleSystem.EmissionModule _explosionEmMod, _explosionCenterEmMod, _explosionWaveEmMod, _trailEmMod;
        private ParticleSystem.ShapeModule _explosionShapeMod, _explosionWaveShapeMod;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _nukeAoeScript = transform.Find("AreaOfEffect").GetComponent<NukeAreaOfEffect>();
            _weaponStats = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStatsManager>();

            _explosionEmMod = explosionPs.emission;
            _explosionCenterEmMod = explosionCenterPs.emission;
            _explosionWaveEmMod = explosionWavePs.emission;
            _trailEmMod = trailPs.emission;

            _explosionShapeMod = explosionPs.shape;
            _explosionWaveShapeMod = explosionWavePs.shape;
        }

        // NOTE: to contain the explosion and explosion wave radii inside the default aoe radius, from the SO:
        // set explosion to default radius / 2.5f
        // set explosionWave to default radius -1f
        private void Update() {
            _explosionShapeMod.radius = _weaponStats.nukeAoeRadius / 2.5f;
            _explosionWaveShapeMod.radius = _weaponStats.nukeAoeRadius - 2f;
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

            _explosionEmMod.enabled = true;
            _explosionWaveEmMod.enabled = true;
            _explosionCenterEmMod.enabled = true;
            explosionCenterPs.Play();
            explosionPs.Play();
            explosionWavePs.Play();
            Invoke(nameof(DestroyObj), 2f);
        }

        private void DestroyObj() {
            Destroy(transform.parent.gameObject);
        }
    }
}