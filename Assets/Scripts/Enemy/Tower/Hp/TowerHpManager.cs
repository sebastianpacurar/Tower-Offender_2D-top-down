using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower.Hp {
    public class TowerHpManager : MonoBehaviour {
        public float TowerHealthPoints { get; private set; }
        public bool IsDead { get; private set; }
        [SerializeField] private TankShellStatsSo tankLightShellStats;
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private GameObject towerUI;
        [SerializeField] private GameObject turretObj;

        private void Awake() {
            TowerHealthPoints = towerStatsSo.MaxHp;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankLightShell")) {
                TowerHealthPoints -= tankLightShellStats.Damage;
            }
        }

        private void Update() {
            if (IsDead || TowerHealthPoints > 0) return;
            Destroy(towerUI);
            Destroy(turretObj);
            gameObject.layer = 0;
            IsDead = true;
        }
    }
}