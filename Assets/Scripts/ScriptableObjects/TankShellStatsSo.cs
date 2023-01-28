using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TankShellStatsSO", menuName = "SOs/TankShellStats")]
    public class TankShellStatsSo : ScriptableObject {
        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private int ammo;
        [SerializeField] private float aoeRadius;
        [SerializeField] private float aoeEffectDuration;

        public float Speed => speed;
        public float Damage => damage;
        public int Ammo => ammo;
        public float AoeRadius => aoeRadius;
        public float AoeEffectDuration => aoeEffectDuration;
    }
}