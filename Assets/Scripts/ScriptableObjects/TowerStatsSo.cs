using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TowerStatsSO", menuName = "SOs/TowerStats")]
    public class TowerStatsSo : ScriptableObject {
        [SerializeField] private float range;
        [SerializeField] private GameObject shellPrefab;
        [SerializeField] private float secondsBetweenShooting;
        [SerializeField] private int hp;

        public float Range => range;
        public GameObject ShellPrefab => shellPrefab;
        public float SecondsBetweenShooting => secondsBetweenShooting;
        public int Hp => hp;
    }
}