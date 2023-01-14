using ScriptableObjects;
using UnityEngine;

namespace Enemy.Hp {
    public class TowerHpManager : MonoBehaviour {
        public float TowerHealthPoints { get; private set; }
        public bool IsDead { get; private set; }
        [SerializeField] private TankShellStatsSo tankLightShellStats;
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private GameObject towerUI;
        [SerializeField] private SpriteRenderer towerTrigger;
        [SerializeField] private SpriteRenderer towerTurret;
        private TurretController _turretController;
        
        private void Awake() {
            TowerHealthPoints = towerStatsSo.MaxHp;
        }

        private void Start() {
            _turretController = transform.Find("Turret").GetComponent<TurretController>();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankLightShell")) {
                TowerHealthPoints -= tankLightShellStats.Damage;
            }
        }

        private void Update() {
            if (IsDead) return;
            if (TowerHealthPoints == 0) {
                towerUI.SetActive(false);
                _turretController.enabled = false;
                towerTrigger.enabled = false;
                towerTurret.enabled = false;
                IsDead = true;
            }
        }
    }
}