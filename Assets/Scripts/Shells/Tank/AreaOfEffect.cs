using Enemy;
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

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Tower")) {
                col.gameObject.GetComponent<Tower>().IsPowerOff = true;
            }
        }
    }
}