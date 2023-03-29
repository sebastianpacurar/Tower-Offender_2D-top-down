using Player;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower.Hp {
    public class TurretHpManager : MonoBehaviour {
        public float TurretHealthPoints { get; set; }
        public bool IsDead { get; private set; }
        [SerializeField] private TurretStatsSo turretStatsSo;
        [SerializeField] private GameObject towerUI;
        [SerializeField] private GameObject turretObj;

        private WeaponStatsManager _weaponStats;
        private CashManager _cashManager;

        private void Awake() {
            TurretHealthPoints = turretStatsSo.MaxHp;
        }

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _weaponStats = tank.GetComponent<WeaponStatsManager>();
            _cashManager = tank.GetComponent<CashManager>();
        }

        // handle damage from TankLightShell
        private void OnCollisionEnter2D(Collision2D col) {
            if (col.gameObject.CompareTag("TankLightShell")) {
                TurretHealthPoints -= _weaponStats.lightShellDamage;
            }
        }

        // handle damage from TankSniperShell
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankSniperShell")) {
                TurretHealthPoints -= _weaponStats.sniperDamage;
            }
        }

        // execute only once when TurretHealthPoints equals 0
        private void Update() {
            if (IsDead || TurretHealthPoints > 0) return;

            // update the Cash Value accumulated (check BodyController.cs for the rest of the logic)
            _cashManager.finalCash += turretStatsSo.CashValue;
            Destroy(towerUI);
            Destroy(turretObj);
            gameObject.layer = 0;
            IsDead = true;
        }
    }
}