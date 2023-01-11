using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class CooldownCanvas : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Image tankShellImgBar;
        [SerializeField] private Image empShellImgBar;
        private Shoot _shootScript;

        private void Start() {
            _shootScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        }

        private void Update() {
            UpdateTankShellCd();
            UpdateEmpShellCd();
        }

        private void UpdateTankShellCd() {
            tankShellImgBar.fillAmount = _shootScript.CanFireLightShell ? 0.25f : (_shootScript.LightShellCdTimer / tankStatsSo.LightShellReloadTime) * 0.25f;
        }

        private void UpdateEmpShellCd() {
            empShellImgBar.fillAmount = _shootScript.CanFireEmpShell ? 0.25f : (_shootScript.EmpShellCdTimer / tankStatsSo.EmpShellReloadTime) * 0.25f;
        }
    }
}