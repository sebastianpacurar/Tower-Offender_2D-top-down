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

        private void Awake() {
            TurretHealthPoints = turretStatsSo.MaxHp;
            _weaponStats = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStatsManager>();
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
            Destroy(towerUI);
            Destroy(turretObj);
            gameObject.layer = 0;
            IsDead = true;
        }
    }
}