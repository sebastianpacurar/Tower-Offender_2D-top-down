using Player;
using ScriptableObjects;
using Unity.VisualScripting;
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

        private void Update() {
            if (IsDead || TurretHealthPoints > 0) return;
            _cashManager.currCash += turretStatsSo.CashValue;
            Destroy(towerUI);
            Destroy(turretObj);
            gameObject.layer = 0;
            IsDead = true;
        }
    }
}