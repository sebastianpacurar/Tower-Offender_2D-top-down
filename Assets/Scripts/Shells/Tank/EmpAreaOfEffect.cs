using Enemy.Tower;
using Player;
using UnityEngine;

namespace Shells.Tank {
    public class EmpAreaOfEffect : MonoBehaviour {
        private CircleCollider2D _circleCollider2D;
        private WeaponStatsManager _weaponStats;

        private void Awake() {
            _weaponStats = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStatsManager>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _circleCollider2D.radius = _weaponStats.empAoeRadius;
        }

        public void EnableCircleCollider() {
            _circleCollider2D.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TurretObj")) {
                if (!col.gameObject.transform.Find("Turret")) return;
                col.gameObject.transform.Find("Turret").GetComponent<TurretController>().IsPowerOff = true;
            }
        }
    }
}