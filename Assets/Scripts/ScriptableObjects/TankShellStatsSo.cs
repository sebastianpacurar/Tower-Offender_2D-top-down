using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TankShellStatsSO", menuName = "SOs/TankShellStats")]
    public class TankShellStatsSo : ScriptableObject {
        [SerializeField] private float speed;
        [SerializeField] private float timeToLive;
        [SerializeField] private float damage;
        [SerializeField] private int ammo;
        [SerializeField] private Color shellColor;
        [SerializeField] private float aoeRadius;

        public float Speed => speed;
        public float TimeToLive => timeToLive;
        public float Damage => damage;
        public int Ammo => ammo;
        public Color ShellColor => shellColor;
        public float AoeRadius => aoeRadius;
    }
}