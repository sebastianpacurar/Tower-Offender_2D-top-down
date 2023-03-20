using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap {
    public class RoadTileManager : MonoBehaviour {
        [SerializeField] private Tilemap roadMap;
        [SerializeField] private List<RoadTileDataSo> tileDataSos;
        private Dictionary<TileBase, RoadTileDataSo> _tilesStatsSo;

        private void Awake() {
            _tilesStatsSo = new Dictionary<TileBase, RoadTileDataSo>();

            // fill the tileStats with default life value
            foreach (var tileData in tileDataSos) {
                foreach (var tile in tileData.Tiles) {
                    _tilesStatsSo.Add(tile, tileData);
                }
            }
        }

        // NOTE: used in TankController.cs, to manipulate the tank acceleration factor and speed
        // return scriptable obj with the corresponding data values 
        public RoadTileDataSo GetTileData(Vector2 worldPos) {
            var gridPos = roadMap.WorldToCell(worldPos);
            var tile = roadMap.GetTile(gridPos);
            return _tilesStatsSo[tile];
        }

        // used in TankController to trigger GetTileData() if returns true
        public bool IsRoadTile(Vector2 worldPos) {
            return roadMap.GetTile(roadMap.WorldToCell(worldPos));
        }
    }
}