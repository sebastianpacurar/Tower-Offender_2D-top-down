using Player.Controllers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class CooldownCanvas : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Image tankShellImgBar, empShellImgBar, sniperShellImgBar, nukeShellImgBar;
        [SerializeField] private TextMeshProUGUI empReloadPercentage, sniperReloadPercentage, nukeReloadPercentage;
        private ShootController _shootController;

        private void Start() {
            _shootController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootController>();
        }

        private void Update() {
            UpdateTankShellCd();
            UpdateEmpShellCd();
            UpdateSniperShellCd();
            UpdateNukeShellCd();
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
    }
}