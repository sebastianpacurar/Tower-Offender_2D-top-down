using UnityEngine;

namespace Enemy {
    public class TowerHpHandler : MonoBehaviour {
        public float TowerHealthPoints { get; private set; } = 5;

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankBullet")) {
                TowerHealthPoints -= 1;
                Destroy(col.gameObject);
            }
        }
    }
}