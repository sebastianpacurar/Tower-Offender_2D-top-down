using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class HpCanvas : MonoBehaviour {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject tankObj;
        [SerializeField] private Image greenBar;
        private Camera _mainCam;
        private HpHandler _hpHandlerScript;

        private void Start() {
            _hpHandlerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HpHandler>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            transform.position = tankObj.transform.position;
            greenBar.fillAmount = _hpHandlerScript.HealthPoints / 10;
        }
    }
}