using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TowerStatsSO", menuName = "SOs/TowerStats")]
    public class TowerStatsSo : ScriptableObject {
        [SerializeField] private float range;
        public float Range => range;

        [SerializeField] private GameObject shellPrefab;
        public GameObject ShellPrefab => shellPrefab;

        [SerializeField] private float secondsBetweenShooting;
        public float SecondsBetweenShooting => secondsBetweenShooting;

        [SerializeField] private int hp;
        public int Hp => hp;
    }
}