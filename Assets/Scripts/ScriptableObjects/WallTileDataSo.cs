using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "WallTileDataSO", menuName = "SOs/WallTileData")]
    public class WallTileDataSo : ScriptableObject {
        [SerializeField] private TileBase[] tiles;
        public IEnumerable<TileBase> Tiles => tiles;

        [SerializeField] private int life;
        public int Life => life;
    }
}