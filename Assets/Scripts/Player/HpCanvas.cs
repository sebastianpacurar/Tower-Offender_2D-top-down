using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class HpCanvas : MonoBehaviour {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject tankObj;
        [SerializeField] private Image greenBar;
        private float _maxHp;

        private Camera _mainCam;
        private HpHandler _hpHandlerScript;

        private void Start() {
            _hpHandlerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HpHandler>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
            _maxHp = _hpHandlerScript.healthPoints;
        }

        private void Update() {
            transform.position = tankObj.transform.position;
            greenBar.fillAmount = _hpHandlerScript.healthPoints / _maxHp;
        }
    }
}