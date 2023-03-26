using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TankShellStatsSO", menuName = "SOs/TankShellStats")]
    public class TankShellStatsSo : ScriptableObject {
        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private int ammo;
        [SerializeField] private float cost;
        [SerializeField] private float minAoeRadius;
        [SerializeField] private float aoeRadius;
        [SerializeField] private float maxAoeRadius;
        [SerializeField] private float aoeEffectDuration;

        [Header("Used for turret HoverIcons obj")]
        [SerializeField] private Color hoverArrowColor;

        public float Speed => speed;
        public float Damage => damage;
        public int Ammo => ammo;
        public float Cost => cost;
        public float MinAoeRadius => minAoeRadius;
        public float AoeRadius => aoeRadius;
        public float MaxAoeRadius => maxAoeRadius;
        public float AoeEffectDuration => aoeEffectDuration;
        public Color HoverArrowColor => hoverArrowColor;
    }
}