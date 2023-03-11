using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap {
    public class WallTileManager : MonoBehaviour {
        [SerializeField] private Tilemap wallMap;
        [SerializeField] private List<TileDataSo> tileDataSos;

        private Dictionary<TileBase, TileDataSo> _tilesStatsSo;
        private Dictionary<Vector3Int, MyTileData> _cachedTiles;


        private void Awake() {
            _tilesStatsSo = new Dictionary<TileBase, TileDataSo>();
            _cachedTiles = new Dictionary<Vector3Int, MyTileData>();

            // fill the tileStats with default life value
            foreach (var tileData in tileDataSos) {
                foreach (var tile in tileData.tiles) {
                    _tilesStatsSo.Add(tile, tileData);
                }
            }
        }


        private void Start() {
            StartCoroutine(GetAllWallTiles());
        }


        // decrease life of tile, and destroy it when it reaches 0
        public void HandleWallTileLife(Vector2 worldPosition) {
            Vector3Int gridPosition = wallMap.WorldToCell(worldPosition);
            MyTileData cachedEntry;
            Debug.Log(gridPosition);

            // avoid 
            try {
                cachedEntry = _cachedTiles[gridPosition];
            } catch (Exception) {
                return;
            }

            if (cachedEntry.Lives >= 0) {
                cachedEntry.Lives -= 1;

                // set tile to null in both tilemap and _cachedTiles
                if (cachedEntry.Lives == 0) {
                    wallMap.SetTile(gridPosition, null);
                    _cachedTiles.Remove(gridPosition);
                }
            }
        }


        // TODO: add loading screen until all tiles are cached
        // populate the _cachedTiles
        private IEnumerator GetAllWallTiles() {
            var bounds = wallMap.cellBounds;

            // iterate over every tile based on its cellBounds min X and min Y values
            for (var x = bounds.min.x; x < bounds.max.x; x++) {
                for (var y = bounds.min.y; y < bounds.max.y; y++) {
                    var cellPosition = new Vector3Int(x, y, 0);
                    var tile = wallMap.GetTile(cellPosition);

                    // skip if tile is null
                    if (!tile) {
                        continue;
                    }

                    // grab the life stats from the tile SO 
                    var life = _tilesStatsSo[tile].life;
                    var tileData = new MyTileData(tile, life);
                    _cachedTiles.Add(cellPosition, tileData);

                    yield return tileData;
                }
            }
        }
    }
}