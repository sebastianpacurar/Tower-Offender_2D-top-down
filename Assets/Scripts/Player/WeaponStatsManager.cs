using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class WeaponStatsManager : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;

        public float lightShellDamage;
        public float lightShellFireRate;
        public float lightShellSpeed;

        public float empAoeRadius;
        public float empFireRate;

        public float sniperDamage;
        public float sniperFireRate;

        public float nukeAoeRadius;
        public float nukeFireRate;

        // initialize to default values
        private void Awake() {
            lightShellDamage = tankStatsSo.LightShellStatsSo.Damage;
            lightShellFireRate = tankStatsSo.LightShellReloadTime;
            lightShellSpeed = tankStatsSo.LightShellStatsSo.Speed;

            empAoeRadius = tankStatsSo.EmpShellStatsSo.AoeRadius;
            empFireRate = tankStatsSo.EmpShellReloadTime;

            sniperDamage = tankStatsSo.SniperShellStatsSo.Damage;
            sniperFireRate = tankStatsSo.SniperShellReloadTime;

            nukeAoeRadius = tankStatsSo.NukeShellStatsSo.AoeRadius;
            nukeFireRate = tankStatsSo.NukeShellReloadTime;
        }
    }
}