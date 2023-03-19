using ScriptableObjects;
using UnityEngine;

namespace Shells {
    public class TowerShell : MonoBehaviour {
        public float ShellDamage { get; private set; }
        [SerializeField] private ShellStatsSo shellStatsSo;
        [SerializeField] private ParticleSystem explosionPs, trailPs;

        private Transform startPos;
        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Transform _tankPos;
        private float _isMultiShell;
        private float _finalSpeed;

        private ParticleSystem.EmissionModule _explosionEmMod, _trailEmMod;

        private void Awake() {
            _explosionEmMod = explosionPs.emission;
            _trailEmMod = trailPs.emission;

            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _tankPos = GameObject.FindGameObjectWithTag("Player").transform;

            ShellDamage = shellStatsSo.Damage;
        }


        private void Start() {
            // in case the passing value is a multiShell game object
            if (!transform.parent.name.Equals("ShellsContainer")) {
                _finalSpeed = name.Equals("MiddleShell") ? shellStatsSo.MiddleShellSpeed : shellStatsSo.SideShellsSpeed;
                startPos = transform.parent;
            } else {
                _finalSpeed = shellStatsSo.SideShellsSpeed;
                startPos = transform;
            }

            if (CompareTag("BasicShell")) {
                var direction = _tankPos.position - startPos.position;
                _rb.velocity = new Vector2(direction.x, direction.y).normalized * _finalSpeed;

                var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0, 0, rotZ);
            }
        }

        private void Update() {
            // if Homing Shell then also apply rotation towards the player
            if (!CompareTag("HomingShell")) return;

            var direction = _tankPos.position - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * _finalSpeed;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("WorldBorder") || col.gameObject.CompareTag("SafeWalls")) {
                DestroyShell();
            }

            // destroy homing shell when is in the radius of the emp shell
            if (CompareTag("HomingShell") && col.gameObject.CompareTag("EmpAoeRadius")) {
                DestroyShell();
            }
        }

        private void DestroyShell() {
            _sr.enabled = false;
            _capsuleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            _explosionEmMod.enabled = true;
            _trailEmMod.enabled = false;

            explosionPs.Play();
            Invoke(nameof(DestroyObj), 0.5f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}