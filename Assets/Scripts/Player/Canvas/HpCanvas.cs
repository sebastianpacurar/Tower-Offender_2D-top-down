using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class HpCanvas : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private UnityEngine.Canvas canvas;
        [SerializeField] private GameObject tankObj;
        [SerializeField] private Image greenBar;

        private Camera _mainCam;
        private TankHpManager _tankHpManager;

        private void Start() {
            _tankHpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<TankHpManager>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            transform.position = tankObj.transform.position;
            greenBar.fillAmount = (_tankHpManager.TankHealthPoints / tankStatsSo.MaxHp) * 0.5f;
        }
    }
}