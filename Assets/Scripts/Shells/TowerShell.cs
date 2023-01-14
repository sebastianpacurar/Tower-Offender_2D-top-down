using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Shells {
    public class TowerShell : MonoBehaviour {
        public float ShellDamage { get; private set; }
        [SerializeField] private ShellStatsSo shellStatsSo;
        [SerializeField] private ParticleSystem explosionPs, trailPs;

        private Transform _towerPosition;
        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Transform _tankPos;
        private float _isMultiShell;

        private ParticleSystem.EmissionModule _explosionEmMod, _trailEmMod;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            ShellDamage = shellStatsSo.Damage;
        }

        // TODO: could be improved
        private IEnumerator StartCountdown() {
            while (true) {
                yield return new WaitForSeconds(shellStatsSo.TimeToLive);
                Destroy(gameObject);
            }
        }

        private void Start() {
            float shellSpeed;
            _tankPos = GameObject.FindGameObjectWithTag("Player").transform;

            _explosionEmMod = explosionPs.emission;
            _trailEmMod = trailPs.emission;

            // in case the passing value is a multiShell game object
            if (!transform.parent.name.Equals("ShellsContainer")) {
                _towerPosition = transform.parent.transform.parent.transform.parent.Find("TowerObj").gameObject.transform;
                shellSpeed = name.Equals("MiddleShell") ? shellStatsSo.MiddleShellSpeed : shellStatsSo.SideShellsSpeed;
            } else {
                _towerPosition = transform.parent.transform.parent.Find("TowerObj").gameObject.transform;
                shellSpeed = shellStatsSo.SideShellsSpeed;
            }

            var direction = _tankPos.position - _towerPosition.position;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * shellSpeed;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            StartCoroutine(StartCountdown());
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("WorldBorder")) {
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