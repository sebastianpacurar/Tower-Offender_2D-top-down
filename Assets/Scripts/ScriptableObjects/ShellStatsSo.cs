using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "ShellStatsSO", menuName = "SOs/ShellStats")]
    public class ShellStatsSo : ScriptableObject {
        [SerializeField] private float sideShellsSpeed;
        [SerializeField] private float middleShellSpeed;
        [SerializeField] private float timeToLive;
        [SerializeField] private float damage;

        public float SideShellsSpeed => sideShellsSpeed;
        public float MiddleShellSpeed => middleShellSpeed;
        public float TimeToLive => timeToLive;
        public float Damage => damage;
    }
}