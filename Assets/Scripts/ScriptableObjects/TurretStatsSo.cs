using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TurretStatsSO", menuName = "SOs/TurretStats")]
    public class TurretStatsSo : ScriptableObject {
        [SerializeField] private float range;
        [SerializeField] private GameObject shellPrefab;
        [SerializeField] private float secondsBetweenShooting;
        [SerializeField] private int maxHp;
        [SerializeField] private float cashValue;
        [SerializeField] private Color circleRangeAreaColor;
        [SerializeField] private Gradient detectionLineActiveColor = new() {
            alphaKeys = new[] {
                new GradientAlphaKey(0, 0f),
                new GradientAlphaKey(1, 1f)
            },
            colorKeys = new[] {
                new GradientColorKey(Color.red, 1f),
                new GradientColorKey(Color.green, 1f),
                new GradientColorKey(Color.blue, 1f)
            },
            mode = GradientMode.Blend
        };
        [SerializeField] private Gradient detectionLineInactiveColor = new() {
            alphaKeys = new[] {
                new GradientAlphaKey(0, 0f),
                new GradientAlphaKey(1, 1f)
            },
            colorKeys = new[] {
                new GradientColorKey(Color.red, 1f),
                new GradientColorKey(Color.green, 1f),
                new GradientColorKey(Color.blue, 1f)
            },
            mode = GradientMode.Blend
        };

        public float Range => range;
        public GameObject ShellPrefab => shellPrefab;
        public float SecondsBetweenShooting => secondsBetweenShooting;
        public int MaxHp => maxHp;
        public float CashValue => cashValue;
        public Color CircleRangeAreaColor => circleRangeAreaColor;
        public Gradient DetectionLineActiveColor => detectionLineActiveColor;
        public Gradient DetectionLineInactiveColor => detectionLineInactiveColor;
    }
}