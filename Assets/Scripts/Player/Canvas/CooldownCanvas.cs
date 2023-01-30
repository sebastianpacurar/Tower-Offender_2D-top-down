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
        private Shoot _shootScript;

        private void Start() {
            _shootScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        }

        private void Update() {
            UpdateTankShellCd();
            UpdateEmpShellCd();
            UpdateSniperShellCd();
            UpdateNukeShellCd();
        }

        private void UpdateTankShellCd() {
            tankShellImgBar.fillAmount = _shootScript.CanFireLightShell ? 0.125f : (_shootScript.LightShellCdTimer / tankStatsSo.LightShellReloadTime) * 0.125f;
        }

        private void UpdateEmpShellCd() {
            var currentProgress = _shootScript.EmpShellCdTimer / tankStatsSo.EmpShellReloadTime;
            empShellImgBar.fillAmount = _shootScript.CanFireEmpShell ? 0.125f : currentProgress * 0.125f;
            empReloadPercentage.text = _shootScript.CanFireEmpShell ? "100%" : $"{(int)(currentProgress * 100)}%";
        }

        private void UpdateSniperShellCd() {
            var currentProgress = _shootScript.SniperShellCdTimer / tankStatsSo.SniperShellReloadTime;
            sniperShellImgBar.fillAmount = _shootScript.CanFireSniperShell ? 0.125f : currentProgress * 0.125f;
            sniperReloadPercentage.text = _shootScript.CanFireSniperShell ? "100%" : $"{(int)(currentProgress * 100)}%";
        }

        private void UpdateNukeShellCd() {
            var currentProgress = _shootScript.NukeShellCdTimer / tankStatsSo.NukeShellReloadTime;
            nukeShellImgBar.fillAmount = _shootScript.CanFireNukeShell ? 0.125f : currentProgress * 0.125f;
            nukeReloadPercentage.text = _shootScript.CanFireNukeShell ? "100%" : $"{(int)(currentProgress * 100)}%";
        }
    }
}