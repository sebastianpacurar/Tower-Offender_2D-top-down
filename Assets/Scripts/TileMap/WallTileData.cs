using UnityEngine.Tilemaps;

namespace TileMap {
    public class WallTileData {
        public WallTileData(TileBase tile, float hp) {
            Tile = tile;
            Hp = hp;
        }

        public TileBase Tile { get; set; }
        public float Hp { get; set; }
    }
}