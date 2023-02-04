using Player.Controllers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class CooldownCanvas : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Image tankShellImgBar, empShellImgBar, sniperShellImgBar, nukeShellImgBar, speedBoostImgBar;
        [SerializeField] private TextMeshProUGUI empReloadPercentage, sniperReloadPercentage, nukeReloadPercentage, speedBoostPercentage;
        private ShootController _shootController;
        private TankController _tankController;

        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _shootController = tank.GetComponent<ShootController>();
            _tankController = tank.GetComponent<TankController>();
        }

        private void Update() {
            UpdateTankShellCd();
            UpdateEmpShellCd();
            UpdateSniperShellCd();
            UpdateNukeShellCd();
            UpdateSpeedBoostCd();
        }

        private void UpdateTankShellCd() {
            tankShellImgBar.fillAmount = _shootController.CanFireLightShell ? 0.125f : (_shootController.LightShellCdTimer / tankStatsSo.LightShellReloadTime) * 0.125f;
        }

        private void UpdateEmpShellCd() {
            var currentProgress = _shootController.EmpShellCdTimer / tankStatsSo.EmpShellReloadTime;
            empShellImgBar.fillAmount = _shootController.CanFireEmpShell ? 0.125f : currentProgress * 0.125f;
            empReloadPercentage.text = _shootController.CanFireEmpShell ? "100%" : $"{(int)(currentProgress * 100)}%";
        }

        private void UpdateSniperShellCd() {
            var currentProgress = _shootController.SniperShellCdTimer / tankStatsSo.SniperShellReloadTime;
            sniperShellImgBar.fillAmount = _shootController.CanFireSniperShell ? 0.125f : currentProgress * 0.125f;
            sniperReloadPercentage.text = _shootController.CanFireSniperShell ? "100%" : $"{(int)(currentProgress * 100)}%";
        }

        private void UpdateNukeShellCd() {
            var currentProgress = _shootController.NukeShellCdTimer / tankStatsSo.NukeShellReloadTime;
            nukeShellImgBar.fillAmount = _shootController.CanFireNukeShell ? 0.125f : currentProgress * 0.125f;
            nukeReloadPercentage.text = _shootController.CanFireNukeShell ? "100%" : $"{(int)(currentProgress * 100)}%";
        }

        private void UpdateSpeedBoostCd() {
            var currentProgress = _tankController.SpeedBoostVal / tankStatsSo.SpeedBoostCapacity;
            speedBoostImgBar.fillAmount = currentProgress;
            speedBoostPercentage.text = $"{(int)(currentProgress * 100)}%";
        }
    }
}