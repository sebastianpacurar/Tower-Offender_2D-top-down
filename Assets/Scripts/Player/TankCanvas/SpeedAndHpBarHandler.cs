using Player.Controllers;
using ScriptableObjects;
using UnityEngine;

namespace Player.TankCanvas {
    public class SpeedAndHpBarHandler : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Material speedBoostMat;
        [SerializeField] private Material hpMat;
        private TankController _tankController;

        // private Canvas _canvas;
        // private Camera _mainCam;
        private TankHpManager _tankHpManager;
        private GameObject _tankObj;
        private static readonly int FillAmountProgress = Shader.PropertyToID("_FillAmountProgress");

        private void Awake() {
            // _canvas = GetComponent<Canvas>();
            // _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            _tankObj = GameObject.FindGameObjectWithTag("Player");
            _tankController = _tankObj.GetComponent<TankController>();
            _tankHpManager = _tankObj.GetComponent<TankHpManager>();
        }

        private void Start() {
            // _canvas.renderMode = RenderMode.WorldSpace;
            // _canvas.worldCamera = _mainCam;
        }

        private void Update() {
            var currentHpValue = _tankHpManager.TankHealthPoints / tankStatsSo.MaxHp;
            hpMat.SetFloat(FillAmountProgress, currentHpValue);
            // greenBar.fillAmount = currentHpValue;

            UpdateSpeedBoostCd();
        }

        private void UpdateSpeedBoostCd() {
            var currentProgress = _tankController.SpeedBoostVal / tankStatsSo.SpeedBoostCapacity;
            speedBoostMat.SetFloat(FillAmountProgress, currentProgress);
            // speedBoostImgBar.fillAmount = currentProgress;
        }
    }
}