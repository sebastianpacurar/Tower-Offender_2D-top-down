using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "RoadTileDataSo", menuName = "SOs/RoadTileData")]
    public class RoadTileDataSo : ScriptableObject {
        [SerializeField] private TileBase[] tiles;
        public IEnumerable<TileBase> Tiles => tiles;

        [SerializeField] private float accFactor;
        public float AccFactor => accFactor;

        [SerializeField] private float boostAccFactor;
        public float BoostAccFactor => boostAccFactor;

        [SerializeField] private float maxSpeed;
        public float MaxSpeed => maxSpeed;

        [SerializeField] private float boostMaxSpeed;
        public float BoostMaxSpeed => boostMaxSpeed;
    }
}