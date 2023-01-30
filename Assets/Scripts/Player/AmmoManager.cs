using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class AmmoManager : MonoBehaviour {
        public int LightShellAmmo { get; set; }
        public int EmpShellAmmo { get; set; }
        public int SniperShellAmmo { get; set; }
        public int NukeShellAmmo { get; set; }
        [SerializeField] private TankStatsSo tankStatsSo;

        private void Awake() {
            LightShellAmmo = tankStatsSo.LightShellStatsSo.Ammo;
            EmpShellAmmo = tankStatsSo.EmpShellStatsSo.Ammo;
            SniperShellAmmo = tankStatsSo.SniperShellStatsSo.Ammo;
            NukeShellAmmo = tankStatsSo.NukeShellStatsSo.Ammo;
        }
    }
}