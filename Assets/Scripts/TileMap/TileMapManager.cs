using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap {
    public class TileMapManager : MonoBehaviour {
        [SerializeField] private Tilemap map;
        [SerializeField] private List<TileDataSo> tileDataSos;

        private Dictionary<TileBase, TileDataSo> tilesStatsSo;
        private Dictionary<Vector3Int, TileData> cachedTiles;


        private void Awake() {
            tilesStatsSo = new Dictionary<TileBase, TileDataSo>();
            cachedTiles = new Dictionary<Vector3Int, TileData>();

            // fill the tileStats with default life value
            foreach (var tileData in tileDataSos) {
                foreach (var tile in tileData.tiles) {
                    tilesStatsSo.Add(tile, tileData);
                }
            }
        }


        private void Start() {
            StartCoroutine(GetAllWallTiles());
        }


        // decrease life of tile, and destroy it when it reaches 0
        public void HandleWallTileLife(Vector2 worldPosition) {
            Vector3Int gridPosition = map.WorldToCell(worldPosition);
            var cachedEntry = cachedTiles[gridPosition];

            if (cachedEntry.Lives >= 0) {
                cachedEntry.Lives -= 1;

                // set tile to null in both tilemap and cachedTiles
                if (cachedEntry.Lives == 0) {
                    map.SetTile(gridPosition, null);
                    cachedTiles[gridPosition].Tile = null;
                }
            }
        }


        // TODO: add loading screen until all tiles are cached
        // populate the cachedTiles
        private IEnumerator GetAllWallTiles() {
            var bounds = map.cellBounds;

            // iterate over every tile based on its cellBounds min X and min Y values
            for (var x = bounds.min.x; x < bounds.max.x; x++) {
                for (var y = bounds.min.y; y < bounds.max.y; y++) {
                    var cellPosition = new Vector3Int(x, y, 0);
                    var tile = map.GetTile(cellPosition);

                    // skip if tile is null
                    if (!tile) {
                        continue;
                    }

                    // grab the life stats from the tile SO 
                    var life = tilesStatsSo[tile].life;
                    var tileData = new TileData(tile, life);
                    cachedTiles.Add(cellPosition, tileData);

                    yield return tileData;
                }
            }
        }
    }
}