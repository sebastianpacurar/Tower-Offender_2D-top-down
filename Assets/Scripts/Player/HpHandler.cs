using UnityEngine;

namespace Player {
    public class HpHandler : MonoBehaviour {
        public float HealthPoints { get; private set; } = 10;

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TowerBullet")) {
                HealthPoints -= 1;
                Destroy(col.gameObject);
            }
        }
    }
}