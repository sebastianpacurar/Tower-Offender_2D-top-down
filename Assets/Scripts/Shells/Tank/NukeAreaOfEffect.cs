using Enemy.Tower.Hp;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank {
    public class NukeAreaOfEffect : MonoBehaviour {
        private TurretHpManager _turretHpManager;
        private CircleCollider2D _circleCollider2D;
        private WeaponStatsManager _weaponStats;

        private void Awake() {
            _weaponStats = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStatsManager>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _circleCollider2D.radius = _weaponStats.nukeAoeRadius;
        }

        public void EnableCircleCollider() {
            _circleCollider2D.enabled = true;
        }

        // insta-kill the turret when in range
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TurretObj")) {
                var turret = col.gameObject.GetComponent<TurretHpManager>();

                if (!turret.IsDead) {
                    turret.TurretHealthPoints = 0;
                }
            }
        }
    }
}