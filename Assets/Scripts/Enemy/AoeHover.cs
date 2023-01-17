using UnityEngine;

namespace Enemy {
    public class AoeHover : MonoBehaviour {
        [SerializeField] private SpriteRenderer towerBody;
        [SerializeField] private SpriteRenderer towerTurret;

        private void OnTriggerStay2D(Collider2D col) {
            if (col.gameObject.CompareTag("AoeGhost")) {
                towerBody.color = new Color(1f, 1f, 1f, 0.5f);
                towerTurret.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("AoeGhost")) {
                towerBody.color = new Color(1f, 1f, 1f, 1f);
                towerTurret.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}