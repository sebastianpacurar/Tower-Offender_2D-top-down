using Enemy.Tower.Hp;
using UnityEngine;

namespace Enemy.Tower {
    public class AoeHover : MonoBehaviour {
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private SpriteRenderer turret;
        private TowerHpManager _towerHpManager;
        private bool _isHovered;

        private void Awake() {
            _towerHpManager = GetComponent<TowerHpManager>();
        }

        private void Update() {
            if (_isHovered) {
                body.color = Color.Lerp(new Color(0.75f, 0.75f, 0.75f, 0.75f), new Color(0.25f, 0.25f, 0.25f, 0.25f), Mathf.PingPong(Time.time, 0.5f));
                turret.color = Color.Lerp(new Color(0.75f, 0.75f, 0.75f, 0.75f), new Color(0.25f, 0.25f, 0.25f, 0.25f), Mathf.PingPong(Time.time, 0.5f));
            } else {
                body.color = new Color(1f, 1f, 1f, 1f);
                if (!_towerHpManager.IsDead) {
                    turret.color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D col) {
            if (_towerHpManager.IsDead) return;
            if (col.gameObject.CompareTag("EmpAoeGhost") || col.gameObject.CompareTag("NukeAoeGhost")) {
                _isHovered = true;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("EmpAoeGhost") || col.gameObject.CompareTag("NukeAoeGhost")) {
                _isHovered = false;
            }
        }
    }
}