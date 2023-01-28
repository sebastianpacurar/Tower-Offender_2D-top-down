using Player.Controllers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class CooldownCanvas : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Image tankShellImgBar;
        [SerializeField] private Image empShellImgBar;
        [SerializeField] private Image sniperShellImgBar;
        private Shoot _shootScript;

        private void Start() {
            _shootScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        }

        private void Update() {
            UpdateTankShellCd();
            UpdateEmpShellCd();
            UpdateSniperShellCd();
        }

        private void UpdateTankShellCd() {
            tankShellImgBar.fillAmount = _shootScript.CanFireLightShell ? 0.125f : (_shootScript.LightShellCdTimer / tankStatsSo.LightShellReloadTime) * 0.125f;
        }

        private void UpdateEmpShellCd() {
            empShellImgBar.fillAmount = _shootScript.CanFireEmpShell ? 0.125f : (_shootScript.EmpShellCdTimer / tankStatsSo.EmpShellReloadTime) * 0.125f;
        }

        private void UpdateSniperShellCd() {
            sniperShellImgBar.fillAmount = _shootScript.CanFireSniperShell ? 0.125f : (_shootScript.SniperShellCdTimer / tankStatsSo.SniperShellReloadTime) * 0.125f;
        }
    }
}