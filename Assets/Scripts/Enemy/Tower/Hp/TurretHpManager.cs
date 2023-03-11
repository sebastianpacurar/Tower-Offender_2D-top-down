using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower.Hp {
    public class TurretHpManager : MonoBehaviour {
        public float TurretHealthPoints { get; set; }
        public bool IsDead { get; private set; }
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private TurretStatsSo turretStatsSo;
        [SerializeField] private GameObject towerUI;
        [SerializeField] private GameObject turretObj;

        private void Awake() {
            TurretHealthPoints = turretStatsSo.MaxHp;
        }

        // handle damage from TankLightShell
        private void OnCollisionEnter2D(Collision2D col) {
            if (col.gameObject.CompareTag("TankLightShell")) {
                TurretHealthPoints -= tankStatsSo.LightShellStatsSo.Damage;
            }
        }

        // handle damage from TankSniperShell
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankSniperShell")) {
                TurretHealthPoints -= tankStatsSo.SniperShellStatsSo.Damage;
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