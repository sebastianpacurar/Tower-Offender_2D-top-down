using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class AmmoManager : MonoBehaviour {
        public int LightShellAmmo { get; set; }
        public int EmpShellAmmo { get; set; }

        [SerializeField] private TankShellStatsSo lightShellStatsSo;
        [SerializeField] private TankShellStatsSo empShellStatsSo;


        private void Awake() {
            LightShellAmmo = lightShellStatsSo.Ammo;
            EmpShellAmmo = empShellStatsSo.Ammo;
        }
    }
}