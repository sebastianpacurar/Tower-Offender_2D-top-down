using System;
using System.Collections;
using Player.Controllers;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Shells.Tank {
    public class TankLightShell : MonoBehaviour {
        [SerializeField] private TankShellStatsSo lightShellStatsSo;
        [SerializeField] private ParticleSystem explosionPs, trailPs;

        private CapsuleCollider2D _capsuleCollider2D;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private ParticleSystem.EmissionModule _explosionEmMod, _trailEmMod;
        private AimController _ac;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Start() {
            _explosionEmMod = explosionPs.emission;
            _trailEmMod = trailPs.emission;

            _ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();

            var direction = _ac.AimVal - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * lightShellStatsSo.Speed;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TowerObj") || col.gameObject.CompareTag("WorldBorder")) {
                DestroyShell();
            }

            if (col.gameObject.CompareTag("Wall")) {
                DestroyShell();
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("ShellGhost")) {
                DestroyShell();
            }
        }

        private void DestroyShell() {
            _sr.enabled = false;
            _capsuleCollider2D.enabled = false;
            _rb.velocity = new Vector2(0, 0);

            _explosionEmMod.enabled = true;
            _trailEmMod.enabled = false;

            Invoke(nameof(DestroyObj), 0.5f);
        }

        private void DestroyObj() {
            Destroy(gameObject);
        }
    }
}