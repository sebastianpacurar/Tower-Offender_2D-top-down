using UnityEngine.Tilemaps;

namespace TileMap {
    public class TileData {
        public TileData(TileBase tile, int lives) {
            Tile = tile;
            Lives = lives;
        }

        public TileBase Tile { get; set; }
        public int Lives { get; set; }
    }
}