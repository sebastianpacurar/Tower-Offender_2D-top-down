using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Tower.Hp {
    public class TowerCanvas : MonoBehaviour {
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private TankShellStatsSo empShellStats;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject towerObj;
        [SerializeField] private Image hpGreenBar, fireCdBar, powerOffFilledBar, lightningIcon;

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
            powerOffFilledBar.fillAmount = 0f;
        }

        private void Update() {
            transform.position = towerObj.transform.position;
            hpGreenBar.fillAmount = _towerHpManager.TowerHealthPoints / towerStatsSo.MaxHp;
            fireCdBar.fillAmount = _turretController.ShootTimer / towerStatsSo.SecondsBetweenShooting;

            if (!_turretController.IsPowerOff) {
                powerOffFilledBar.fillAmount = 1f;
                lightningIcon.color = Color.yellow;
            } else {
                powerOffFilledBar.fillAmount = _turretController.PowerOffTimer / empShellStats.AoeEffectDuration;
                lightningIcon.color = Color.red;
            }
        }
    }
}