using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap {
    public class WallTileManager : MonoBehaviour {
        [SerializeField] private Tilemap wallMap;
        [SerializeField] private List<WallTileDataSo> tileDataSos;

        private Dictionary<TileBase, WallTileDataSo> _tilesStatsSo;
        private Dictionary<Vector3Int, WallTileData> _cachedTiles;

        // 0,1 are yellow L and T shapes
        // 2,3 are red L and T shapes
        // 4,5 are purple L and T shapes
        [SerializeField] private Tile[] spawnableYrpWalls;

        private void Awake() {
            _tilesStatsSo = new Dictionary<TileBase, WallTileDataSo>();
            _cachedTiles = new Dictionary<Vector3Int, WallTileData>();

            // fill the tileStats with default life value
            foreach (var tileData in tileDataSos) {
                foreach (var tile in tileData.Tiles) {
                    _tilesStatsSo.Add(tile, tileData);
                }
            }
        }


        private void Start() {
            StartCoroutine(GetAllWallTiles());
        }


        // create surrounding walls around the tower
        // NOTE: called in BodyController.cs upon instantiation of new turret
        public void GenerateFort(GameObject turretObj) {
            var bodyPos = wallMap.WorldToCell(turretObj.transform.position);
            var target = new int[] { };

            // NOTE: based on the order of the tiles in list ( LT = yellow{0,1}, MT = red{2,3}, HT = purple{4,5} )
            if (turretObj.name.Contains("LT")) target = new[] { 0, 1 };
            else if (turretObj.name.Contains("MT")) target = new[] { 2, 3 };
            else if (turretObj.name.Contains("HT")) target = new[] { 4, 5 };

            // Going Clockwise from North to NorthWest
            //
            // North;    - rotZ = 0
            GenerateFortWallTile(spawnableYrpWalls[target[1]], bodyPos + Vector3Int.up, 0f);
            // NorthEast - rotZ = 0
            GenerateFortWallTile(spawnableYrpWalls[target[0]], bodyPos + Vector3Int.up + Vector3Int.right, 0f);
            // East      - rotZ = 270
            GenerateFortWallTile(spawnableYrpWalls[target[1]], bodyPos + Vector3Int.right, 270f);
            // SouthEast - rotZ = 270
            GenerateFortWallTile(spawnableYrpWalls[target[0]], bodyPos + Vector3Int.down + Vector3Int.right, 270f);
            // South     - rotZ = 180
            GenerateFortWallTile(spawnableYrpWalls[target[1]], bodyPos + Vector3Int.down, 180f);
            // SouthWest - rotZ = 180
            GenerateFortWallTile(spawnableYrpWalls[target[0]], bodyPos + Vector3Int.down + Vector3Int.left, 180f);
            // West      - rotZ = 90
            GenerateFortWallTile(spawnableYrpWalls[target[1]], bodyPos + Vector3Int.left, 90f);
            // NorthWest - rotZ = 90
            GenerateFortWallTile(spawnableYrpWalls[target[0]], bodyPos + Vector3Int.up + Vector3Int.left, 90f);
        }


        // decrease life of tile, and destroy it when it reaches 0
        // NOTE: called in TankLightShell.cs since it's the only one with an isTrigger=false as collider
        public void HandleWallTileLife(Vector2 worldPos) {
            Vector3Int gridPos = wallMap.WorldToCell(worldPos);
            WallTileData cachedEntry;

            // HACK: avoid getting null pointer exception
            try {
                cachedEntry = _cachedTiles[gridPos];
            } catch (Exception) {
                return;
            }

            if (cachedEntry.Lives >= 0) {
                cachedEntry.Lives -= 1;

                // set tile to null in both tilemap and _cachedTiles
                if (cachedEntry.Lives == 0) {
                    wallMap.SetTile(gridPos, null);
                    _cachedTiles.Remove(gridPos);
                }
            }
        }


        // set the tile
        // transform its rotation to match the correct way of positioning on the Z axis
        // cache the tile with its relevant life value
        private void GenerateFortWallTile(Tile tile, Vector3Int tilePos, float rotZ) {
            wallMap.SetTile(tilePos, tile);
            wallMap.SetTransformMatrix(tilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotZ), Vector3.one));
            _cachedTiles[tilePos] = new WallTileData(tile, _tilesStatsSo[tile].Life);
        }


        // NOTE: initiates only once at start, afterwards the cached tiles are populated manually
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
                    var life = _tilesStatsSo[tile].Life;
                    var tileData = new WallTileData(tile, life);
                    _cachedTiles[cellPosition] = tileData;

                    yield return tileData;
                }
            }
        }
    }
}