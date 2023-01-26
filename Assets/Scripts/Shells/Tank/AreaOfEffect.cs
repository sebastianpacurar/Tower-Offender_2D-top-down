using Enemy.Tower;
using UnityEngine;

namespace Shells.Tank {
    public class AreaOfEffect : MonoBehaviour {
        private CircleCollider2D _circleCollider2D;

        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        public void EnableCircleCollider() {
            _circleCollider2D.enabled = true;
        }

        //TODO: fix the issue with aoe game object exception for EMP Shell, in case it hits a tower before reaching the aoe target
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TowerObj")) {
                col.gameObject.transform.Find("Turret").GetComponent<TurretController>().IsPowerOff = true;
            }
        }
    }
}