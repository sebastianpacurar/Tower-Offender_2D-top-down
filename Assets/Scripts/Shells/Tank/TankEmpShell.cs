using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Shells.Tank {
    public class TankEmpShell : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private ParticleSystem explosionPs, explosionWavePs, trailPs;
        [SerializeField] private GameObject aoeHitAreaObj;
        [SerializeField] private Light2D topLight;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        private EmpAreaOfEffect _empAoeScript;

        private ParticleSystem.EmissionModule _explosionEmMod, _explosionWaveEmMod, _trailEmMod;
        private ParticleSystem.ShapeModule _explosionShapeMod, _explosionWaveShapeMod;
        private ParticleSystem.NoiseModule _explosionNoiseModule;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

            _empAoeScript = transform.Find("AreaOfEffect").GetComponent<EmpAreaOfEffect>();

            _trailEmMod = trailPs.emission;

            _explosionEmMod = explosionPs.emission;
            _explosionShapeMod = explosionPs.shape;
            _explosionNoiseModule = explosionPs.noise;

            _explosionWaveShapeMod = explosionWavePs.shape;
            _explosionWaveEmMod = explosionWavePs.emission;
        }

        // NOTE: to contain the explosion and explosion wave radii inside the default aoe radius, from the SO:
        // set explosion shape radius to default radius
        // set explosion noise strength to default / 3.5f
        // set explosionWave to default radius
        private void Start() {
            _explosionShapeMod.radius = tankStatsSo.EmpShellStatsSo.AoeRadius;
            _explosionNoiseModule.strength = tankStatsSo.EmpShellStatsSo.AoeRadius / 3.5f;
            _explosionWaveShapeMod.radius = tankStatsSo.EmpShellStatsSo.AoeRadius;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("WorldBorder")) {
                DestroyShell();
            }
        }

        // using Stay for the hit area sibling, so the shell can be as close to its position as possible
        private void OnTriggerStay2D(Collider2D col) {
            if (col.gameObject.CompareTag("EmpAoeHitArea")) {
                transform.position = col.gameObject.transform.position;
                DestroyShell();
            }
        }

        private void DestroyShell() {
            Destroy(aoeHitAreaObj);
            _empAoeScript.EnableCircleCollider();
            _trailEmMod.enabled = false;
            topLight.enabled = false;
            _sr.enabled = false;
            _capsuleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            _explosionEmMod.enabled = true;
            _explosionWaveEmMod.enabled = true;
            explosionPs.Play();
            explosionWavePs.Play();

            Invoke(nameof(DestroyObj), 2f);
        }

        private void DestroyObj() {
            Destroy(transform.parent.gameObject);
        }
    }
}