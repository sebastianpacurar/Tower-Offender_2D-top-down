using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class WeaponStatsManager : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;

        public float lightShellDamage;
        public float lightShellFireRate;
        public float lightShellSpeed;

        public float empFireRate;
        public float empAoeRadius;
        public float empAoeDuration;

        public float sniperFireRate;
        public float sniperDamage;
        public float sniperSpeed;

        public float nukeAoeRadius;
        public float nukeFireRate;
        public float nukeSpeed;

        // initialize to default values
        private void Awake() {
            lightShellDamage = tankStatsSo.LightShellStatsSo.Damage;
            lightShellFireRate = tankStatsSo.LightShellReloadTime;
            lightShellSpeed = tankStatsSo.LightShellStatsSo.Speed;

            empFireRate = tankStatsSo.EmpShellReloadTime;
            empAoeRadius = tankStatsSo.EmpShellStatsSo.AoeRadius;
            empAoeDuration = tankStatsSo.EmpShellStatsSo.AoeEffectDuration;

            sniperFireRate = tankStatsSo.SniperShellReloadTime;
            sniperSpeed = tankStatsSo.SniperShellStatsSo.Speed;
            sniperDamage = tankStatsSo.SniperShellStatsSo.Damage;

            nukeFireRate = tankStatsSo.NukeShellReloadTime;
            nukeAoeRadius = tankStatsSo.NukeShellStatsSo.AoeRadius;
            nukeSpeed = tankStatsSo.NukeShellStatsSo.Speed;
        }
    }
}