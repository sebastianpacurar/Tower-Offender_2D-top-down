using System.Collections;
using Player.Controllers;
using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank {
    public class TankEmpShell : MonoBehaviour {
        [SerializeField] private TankShellStatsSo empShellStats;
        [SerializeField] private ParticleSystem explosionPs, trailPs;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        private AreaOfEffect _aoe;

        private ParticleSystem.EmissionModule _explosionEmMod, _trailEmMod;

        private AimController _ac;
        private Vector3 _mousePos;
        private Camera _mainCam;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Start() {
            _aoe = transform.Find("AreaOfEffect").GetComponent<AreaOfEffect>();

            _explosionEmMod = explosionPs.emission;
            _trailEmMod = trailPs.emission;

            _ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _mousePos = _mainCam.ScreenToWorldPoint(_ac.AimVal);

            var direction = _mousePos - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * empShellStats.Speed;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown() {
            while (true) {
                yield return new WaitForSeconds(empShellStats.TimeToLive);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Tower") || col.gameObject.CompareTag("WorldBorder")) {
                DestroyShell();
            }
        }

        private void DestroyShell() {
            _aoe.EnableCircleCollider();
            _sr.enabled = false;
            _capsuleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            _explosionEmMod.enabled = true;
            _trailEmMod.enabled = false;

            explosionPs.Play();
            Invoke(nameof(DestroyObj), 2.0f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}