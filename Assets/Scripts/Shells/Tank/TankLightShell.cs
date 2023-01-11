using System.Collections;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank {
    public class TankLightShell : MonoBehaviour {
        [SerializeField] private TankShellStatsSo lightShellStatsSo;
        [SerializeField] private ParticleSysSo shellExplosion, shellTrail;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private ParticleSystem _explosionPs, _trailPs;
        private ParticleSystem.EmissionModule _explodeEmissionMod, _trailEmissionMod;

        private AimController _ac;
        private Vector3 _mousePos;
        private Camera _mainCam;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Start() {
            _explosionPs = transform.Find("Explode Particle System").GetComponent<ParticleSystem>();
            _trailPs = transform.Find("Trail Particle System").GetComponent<ParticleSystem>();

            _explodeEmissionMod = _explosionPs.emission;
            _trailEmissionMod = _trailPs.emission;

            Global.InitParticleSystem(_explosionPs, shellExplosion);
            Global.InitParticleSystem(_trailPs, shellTrail);

            _ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _mousePos = _mainCam.ScreenToWorldPoint(_ac.AimVal);

            var direction = _mousePos - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * lightShellStatsSo.Speed;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ - 90f);

            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown() {
            while (true) {
                yield return new WaitForSeconds(lightShellStatsSo.TimeToLive);
                Destroy(gameObject);
            }
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
            _sr.enabled = false;
            _capsuleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            _explodeEmissionMod.enabled = true;
            _trailEmissionMod.enabled = false;

            Invoke(nameof(DestroyObj), 0.5f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}