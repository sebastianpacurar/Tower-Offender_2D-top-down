using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class PlayerMapTile : MonoBehaviour {
        [SerializeField] private SpriteRenderer mapTileSr;
        [SerializeField] private MapTileColorSo mapTileColorSo;
        private TankHpManager _tankHpManager;

        private void Awake() {
            _tankHpManager = GetComponent<TankHpManager>();
        }

        private void Start() {
            mapTileSr.enabled = true;
            mapTileSr.color = mapTileColorSo.ActiveColor;
        }

        private void Update() {
            if (!_tankHpManager.IsDead) return;
            mapTileSr.color = mapTileColorSo.InactiveColor;
        }
    }
}