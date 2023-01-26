using Enemy.Tower.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class MapTile : MonoBehaviour {
        [SerializeField] private MapTileColorSo mapTileColorSo;
        private TowerHpManager _towerHpManager;
        private SpriteRenderer _sr;

        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            _sr.enabled = true;
            _sr.color = mapTileColorSo.ActiveColor;
            _towerHpManager = transform.parent.Find("TowerObj").GetComponent<TowerHpManager>();
        }

        private void Update() {
            if (!_towerHpManager.IsDead) return;
            _sr.color = mapTileColorSo.InactiveColor;
        }
    }
}