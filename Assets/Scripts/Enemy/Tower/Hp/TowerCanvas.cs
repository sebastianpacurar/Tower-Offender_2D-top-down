using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Tower.Hp {
    public class TowerCanvas : MonoBehaviour {
        [SerializeField] private TurretStatsSo turretStatsSo;
        [SerializeField] private TankShellStatsSo empShellStats;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject turretObj;
        [SerializeField] private GameObject powerOffCdObj;
        [SerializeField] private Image hpGreenBar, powerOffCdBar;

        private TurretController _turretController;
        private TurretHpManager _turretHpManager;
        private Camera _mainCam;

        private void Awake() {
            var towerObjTransform = transform.parent.Find("TurretObj").transform;
            _turretHpManager = towerObjTransform.GetComponent<TurretHpManager>();
            _turretController = towerObjTransform.Find("Turret").GetComponent<TurretController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Start() {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            transform.position = turretObj.transform.position;
            hpGreenBar.fillAmount = _turretHpManager.TurretHealthPoints / turretStatsSo.MaxHp;

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