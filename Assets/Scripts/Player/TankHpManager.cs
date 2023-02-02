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

        private Rigidbody2D _tankRb;
        private AimController _aimController;
        private TankController _tankController;
        private ShootController _shootController;

        private ParticleSystem.MainModule _damageMainMod;
        private ParticleSystem.EmissionModule _damageEmMod;

        private void Awake() {
            _damageMainMod = damagePs.main;
            _damageEmMod = damagePs.emission;
        }

        private void Start() {
            TankHealthPoints = tankStatsSo.MaxHp;
            _tankRb = GetComponent<Rigidbody2D>();
            _aimController = GetComponent<AimController>();
            _tankController = GetComponent<TankController>();
            _shootController = GetComponent<ShootController>();

            damagePs.Play();
        }

        private void Update() {
            damagePs.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("BasicShell")) {
                if (TankHealthPoints > 0) {
                    TankHealthPoints -= col.gameObject.GetComponent<TowerShell>().ShellDamage;
                }

                switch (TankHealthPoints / tankStatsSo.MaxHp) {
                    case > 0.6f and < 0.8f:
                        _damageEmMod.enabled = true;
                        break;
                    case > 0.4f and < 0.6f:
                        _damageMainMod.startSize = 0.2f;
                        break;
                    case > 0.2f and < 0.4f:
                        _damageMainMod.startSize = 0.3f;
                        break;
                    case <= 0:
                        _tankRb.velocity = new Vector2(0, 0);
                        DestroyTankComponents();
                        DisableTankComponents();
                        IsDead = true;
                        break;
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