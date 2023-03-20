using UnityEngine.Tilemaps;

namespace TileMap {
    public class RoadTileData {
        public RoadTileData(TileBase tile, float accFactor, float speed) {
            Tile = tile;
            AccFactor = accFactor;
            Speed = speed;
        }

        public TileBase Tile { get; set; }
        public float AccFactor { get; set; }
        public float Speed { get; set; }
    }
}