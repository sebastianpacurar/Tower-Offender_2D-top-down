using System;
using System.Collections;
using Player.Controllers;
using ScriptableObjects;
using Shells;
using UnityEngine;

namespace Player {
    public class TankHpManager : MonoBehaviour {
        public float TankHealthPoints { get; private set; }
        public bool IsDead { get; private set; }
        [SerializeField] private ParticleSystem damagePs;
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private GameObject[] destroyableObjects;
        [SerializeField] private bool isBeingFixed;

        private Rigidbody2D _tankRb;
        private AimController _aimController;
        private TankController _tankController;
        private ShootController _shootController;

        private ParticleSystem.MainModule _damageMainMod;
        private ParticleSystem.EmissionModule _damageEmMod;

        private void Awake() {
            _tankRb = GetComponent<Rigidbody2D>();
            _aimController = GetComponent<AimController>();
            _tankController = GetComponent<TankController>();
            _shootController = GetComponent<ShootController>();

            _damageMainMod = damagePs.main;
            _damageEmMod = damagePs.emission;
            TankHealthPoints = tankStatsSo.MaxHp;
        }

        private void Start() {
            damagePs.Play();
            StartCoroutine(FixTankOvertime());
        }

        private void Update() {
            damagePs.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            HandleDamageParticles();
        }

        private void HandleDamageParticles() {
            switch (TankHealthPoints / tankStatsSo.MaxHp) {
                case > 0.8f:
                    _damageEmMod.enabled = false;
                    break;
                case > 0.6f and < 0.8f:
                    _damageEmMod.enabled = true;
                    break;
                case > 0.4f and < 0.6f:
                    _damageMainMod.startSize = 0.2f;
                    break;
                case > 0.2f and < 0.4f:
                    _damageMainMod.startSize = 0.3f;
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("BasicShell") || col.gameObject.CompareTag("HomingShell")) {
                if (TankHealthPoints > 0) {
                    TankHealthPoints -= col.gameObject.GetComponent<TowerShell>().ShellDamage;
                } else {
                    _tankRb.velocity = new Vector2(0, 0);
                    DestroyTankComponents();
                    DisableTankComponents();
                    IsDead = true;
                }
            }

            if (col.gameObject.CompareTag("HealingBeacon")) {
                if (TankHealthPoints < tankStatsSo.MaxHp) {
                    isBeingFixed = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("HealingBeacon")) {
                isBeingFixed = false;
            }
        }

        private IEnumerator FixTankOvertime() {
            while (true) {
                yield return new WaitForSeconds(0.1f);

                if (isBeingFixed) {
                    TankHealthPoints += 2.5f;
                    TankHealthPoints = Math.Clamp(TankHealthPoints, 0, tankStatsSo.MaxHp);
                }
            }
        }

        private void DestroyTankComponents() {
            foreach (var obj in destroyableObjects) {
                Destroy(obj);
            }
        }

        private void DisableTankComponents() {
            _tankController.SetTrackAnimationTo(false);
            _tankController.enabled = false;
            _aimController.enabled = false;
            _shootController.enabled = false;
            _damageEmMod.enabled = false;
        }
    }
}