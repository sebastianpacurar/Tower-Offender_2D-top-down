using Enemy.Hp;
using UnityEngine;

namespace Enemy {
    public class AoeHover : MonoBehaviour {
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private SpriteRenderer turret;
        private TowerHpManager _towerHpManager;

        private void Awake() {
            _towerHpManager = GetComponent<TowerHpManager>();
        }

        private void OnTriggerStay2D(Collider2D col) {
            if (_towerHpManager.IsDead) return;
            if (col.gameObject.CompareTag("AoeGhost")) {
                body.color = new Color(1f, 1f, 1f, 0.5f);
                turret.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("AoeGhost") || col.gameObject.CompareTag("ShellGhost")) {
                body.color = new Color(1f, 1f, 1f, 1f);
                turret.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}