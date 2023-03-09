using UnityEngine.Tilemaps;

namespace TileMap {
    public class MyTileData {
        public MyTileData(TileBase tile, int lives) {
            Tile = tile;
            Lives = lives;
        }

        public TileBase Tile { get; set; }
        public int Lives { get; set; }
    }
}