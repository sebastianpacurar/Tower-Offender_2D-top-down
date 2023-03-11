using Enemy.Tower.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class Body : MonoBehaviour {
        [SerializeField] private TurretStatsSo turretStatsSo;
        private TurretHpManager _turretHpManager;
        private SpriteRenderer _sr;

        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            _turretHpManager = transform.parent.GetComponent<TurretHpManager>();
        }

        private void Update() {
            if (!_turretHpManager.IsDead) return;
        }
    }
}