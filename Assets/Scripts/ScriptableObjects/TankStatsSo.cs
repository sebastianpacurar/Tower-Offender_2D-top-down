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
        [SerializeField] private TankShellStatsSo lightShellStatsSo;

        [SerializeField] private TankShellStatsSo empShellStatsSo;
        [SerializeField] private TankShellStatsSo sniperShellStatsSo;
        [SerializeField] private TankShellStatsSo nukeShellStatsSo;
        [SerializeField] private float lightShellReloadTime;
        [SerializeField] private float empShellReloadTime;
        [SerializeField] private float sniperShellReloadTime;
        [SerializeField] private float nukeShellReloadTime;

        public TankShellStatsSo LightShellStatsSo => lightShellStatsSo;
        public TankShellStatsSo EmpShellStatsSo => empShellStatsSo;
        public TankShellStatsSo SniperShellStatsSo => sniperShellStatsSo;
        public TankShellStatsSo NukeShellStatsSo => nukeShellStatsSo;
        public float LightShellReloadTime => lightShellReloadTime;
        public float EmpShellReloadTime => empShellReloadTime;
        public float SniperShellReloadTime => sniperShellReloadTime;
        public float NukeShellReloadTime => nukeShellReloadTime;
    }
}