using Player;
using Player.Controllers;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Menus {
    public class SpeedAndHpBarHandler : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;

        [SerializeField] private Material speedBoostMat;
        [SerializeField] private Material hpMat;
        [SerializeField] private Material lightShellMat;
        [SerializeField] private Material empShellMat;
        [SerializeField] private Material sniperShellMat;
        [SerializeField] private Material nukeShellMat;

        [SerializeField] private TextMeshProUGUI speedBoostTxt;
        [SerializeField] private TextMeshProUGUI hpTxt;
        [SerializeField] private TextMeshProUGUI empAmmoTxt;
        [SerializeField] private TextMeshProUGUI sniperAmmoTxt;
        [SerializeField] private TextMeshProUGUI nukeAmmoTxt;

        private TankController _tankController;
        private ShootController _shootController;
        private TankHpManager _tankHpManager;
        private GameObject _tankObj;
        private AmmoManager _ammoManager;
        private static readonly int FillAmountProgress = Shader.PropertyToID("_FillAmountProgress");

        private void Awake() {
            _tankObj = GameObject.FindGameObjectWithTag("Player");
            _shootController = _tankObj.GetComponent<ShootController>();
            _tankController = _tankObj.GetComponent<TankController>();
            _tankHpManager = _tankObj.GetComponent<TankHpManager>();
            _ammoManager = _tankController.GetComponent<AmmoManager>();
        }

        private void Update() {
            UpdateHpBar();
            UpdateSpeedBoostCd();

            UpdateLightShellReloadCd();
            UpdateEmpShellReloadCd();
            UpdateSniperShellReloadCd();
            UpdateNukeShellReloadCd();
        }

        private void UpdateHpBar() {
            var currentHpValue = _tankHpManager.TankHealthPoints / tankStatsSo.MaxHp;
            hpMat.SetFloat(FillAmountProgress, currentHpValue);
            hpTxt.text = $"{(int)(currentHpValue * 100)}%";
        }

        private void UpdateSpeedBoostCd() {
            var currentProgress = _tankController.SpeedBoostVal / tankStatsSo.SpeedBoostCapacity;
            speedBoostMat.SetFloat(FillAmountProgress, currentProgress);
            speedBoostTxt.text = $"{(int)(currentProgress * 100)}%";
        }

        private void UpdateLightShellReloadCd() {
            var currentProgress = _shootController.LightShellCdTimer / tankStatsSo.LightShellReloadTime;
            lightShellMat.SetFloat(FillAmountProgress, _shootController.CanFireLightShell ? 1 : currentProgress);
        }

        private void UpdateEmpShellReloadCd() {
            var currentProgress = _shootController.EmpShellCdTimer / tankStatsSo.EmpShellReloadTime;
            empShellMat.SetFloat(FillAmountProgress, _shootController.CanFireEmpShell ? 1 : currentProgress);
            empAmmoTxt.text = $"x{_ammoManager.EmpShellAmmo}";
        }

        private void UpdateSniperShellReloadCd() {
            var currentProgress = _shootController.SniperShellCdTimer / tankStatsSo.SniperShellReloadTime;
            sniperShellMat.SetFloat(FillAmountProgress, _shootController.CanFireSniperShell ? 1 : currentProgress);
            sniperAmmoTxt.text = $"x{_ammoManager.SniperShellAmmo}";
        }

        private void UpdateNukeShellReloadCd() {
            var currentProgress = _shootController.NukeShellCdTimer / tankStatsSo.NukeShellReloadTime;
            nukeShellMat.SetFloat(FillAmountProgress, _shootController.CanFireNukeShell ? 1 : currentProgress);
            nukeAmmoTxt.text = $"x{_ammoManager.NukeShellAmmo}";
        }
    }
}