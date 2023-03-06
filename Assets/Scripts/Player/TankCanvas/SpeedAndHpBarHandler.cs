using Player.Controllers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Player.TankCanvas {
    public class SpeedAndHpBarHandler : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private GameObject tankObj;
        [SerializeField] private Image speedBoostImgBar;
        [SerializeField] private Image greenBar;
        private TankController _tankController;

        private Canvas _canvas;
        private Camera _mainCam;
        private TankHpManager _tankHpManager;

        private void Awake() {
            _canvas = GetComponent<Canvas>();
        }

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            var tank = GameObject.FindGameObjectWithTag("Player");
            _tankController = tank.GetComponent<TankController>();
            _tankHpManager = tank.GetComponent<TankHpManager>();

            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.worldCamera = _mainCam;
        }

        private void Update() {
            var currentHpValue = _tankHpManager.TankHealthPoints / tankStatsSo.MaxHp;
            transform.position = tankObj.transform.position;
            greenBar.fillAmount = currentHpValue;

            UpdateSpeedBoostCd();
        }

        private void UpdateSpeedBoostCd() {
            var currentProgress = _tankController.SpeedBoostVal / tankStatsSo.SpeedBoostCapacity;
            speedBoostImgBar.fillAmount = currentProgress;
        }
    }
}