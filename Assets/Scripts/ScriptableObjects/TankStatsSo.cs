using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "TankStatsSO", menuName = "SOs/TankStats")]
    public class TankStatsSo : ScriptableObject {
        [SerializeField] private float maxHp;
        public float MaxHp => maxHp;

        [Space(20)]
        [Header("Physics Related")]
        [SerializeField] private float accFactor;

        [SerializeField] private float steerFactor;
        [SerializeField] private float driftFactor;
        [SerializeField] private float maxSpeed;

        public float AccFactor => accFactor;
        public float SteerFactor => steerFactor;
        public float DriftFactor => driftFactor;
        public float MaxSpeed => maxSpeed;


        [Space(20)]
        [Header("Shell Related")]
        [SerializeField] private float lightShellReloadTime;

        [SerializeField] private float empShellReloadTime;

        public float LightShellReloadTime => lightShellReloadTime;
        public float EmpShellReloadTime => empShellReloadTime;
    }
}