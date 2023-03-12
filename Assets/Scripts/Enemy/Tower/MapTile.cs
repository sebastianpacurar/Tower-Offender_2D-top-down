using Enemy.Tower.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class MapTile : MonoBehaviour {
        [SerializeField] private MapTileColorSo mapTileColorSo;
        private TurretHpManager _turretHpManager;
        private SpriteRenderer _sr;

        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
            _turretHpManager = transform.parent.Find("TurretObj").GetComponent<TurretHpManager>();
        }

        private void Start() {
            _sr.enabled = true;
            _sr.color = mapTileColorSo.ActiveColor;
        }

        private void Update() {
            if (!_turretHpManager.IsDead) return;
            _sr.color = mapTileColorSo.InactiveColor;
        }
    }
}