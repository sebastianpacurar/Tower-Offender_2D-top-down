using Enemy.Tower.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank {
    public class NukeAreaOfEffect : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        private TowerHpManager _towerHpManager;
        private CircleCollider2D _circleCollider2D;

        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _circleCollider2D.radius = tankStatsSo.NukeShellStatsSo.AoeRadius;
        }

        public void EnableCircleCollider() {
            _circleCollider2D.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TowerObj")) {
                var tower = col.gameObject.GetComponent<TowerHpManager>();
                if (!tower.IsDead) {
                    tower.TowerHealthPoints -= tankStatsSo.NukeShellStatsSo.Damage;
                }
            }
        }
    }
}