using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Tower.Hp {
    public class TowerCanvas : MonoBehaviour {
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private TankShellStatsSo empShellStats;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject towerObj;
        [SerializeField] private GameObject powerOffCdObj;
        [SerializeField] private Image hpGreenBar, powerOffCdBar;

        private TurretController _turretController;
        private Camera _mainCam;
        private TowerHpManager _towerHpManager;

        private void Start() {
            var towerObjTransform = transform.parent.Find("TowerObj").transform;
            _towerHpManager = towerObjTransform.GetComponent<TowerHpManager>();
            _turretController = towerObjTransform.Find("Turret").GetComponent<TurretController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            transform.position = towerObj.transform.position;
            hpGreenBar.fillAmount = _towerHpManager.TowerHealthPoints / towerStatsSo.MaxHp;

            if (!_turretController.IsPowerOff) {
                powerOffCdBar.fillAmount = 0f;

                if (powerOffCdObj.activeSelf) {
                    powerOffCdObj.SetActive(false);
                }
            } else {
                powerOffCdBar.fillAmount = _turretController.PowerOffTimer / empShellStats.AoeEffectDuration;

                if (!powerOffCdObj.activeSelf) {
                    powerOffCdObj.SetActive(true);
                }
            }
        }
    }
}