using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName="MapTileColorSO", menuName="SOs/MapTileColor")]
    public class MapTileColorSo : ScriptableObject
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;
        
        public Color ActiveColor => activeColor;
        public Color InactiveColor => inactiveColor;
    }
}
