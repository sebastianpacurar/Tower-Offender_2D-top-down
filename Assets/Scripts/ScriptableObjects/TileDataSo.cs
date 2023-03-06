using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TileDataSO", menuName = "SOs/TileData")]
    public class TileDataSo : ScriptableObject {
        public TileBase[] tiles;

        [SerializeField] public int life;
        public int Life => life;
    }
}