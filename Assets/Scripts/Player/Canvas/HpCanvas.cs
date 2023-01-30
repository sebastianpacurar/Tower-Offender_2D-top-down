using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class HpCanvas : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private UnityEngine.Canvas canvas;
        [SerializeField] private GameObject tankObj;
        [SerializeField] private Image greenBar;
        [SerializeField] private TextMeshProUGUI hpPercentage;

        private Camera _mainCam;
        private TankHpManager _tankHpManager;

        private void Start() {
            _tankHpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<TankHpManager>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            var currentHpValue = _tankHpManager.TankHealthPoints / tankStatsSo.MaxHp;
            transform.position = tankObj.transform.position;
            greenBar.fillAmount = currentHpValue * 0.5f;
            hpPercentage.text = $"{(int)(currentHpValue * 100)}%";
        }
    }
}