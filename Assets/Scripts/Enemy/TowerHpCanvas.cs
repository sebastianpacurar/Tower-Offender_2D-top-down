using UnityEngine;
using UnityEngine.UI;

namespace Enemy {
    public class TowerHpCanvas : MonoBehaviour {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image greenBar;
        [SerializeField] private int maxHp;

        private Camera _mainCam;
        private TowerHpHandler _hpHandlerScript;

        private void Start() {
            _hpHandlerScript = transform.parent.Find("TowerObj").GetComponent<TowerHpHandler>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            greenBar.fillAmount = _hpHandlerScript.TowerHealthPoints / maxHp;
        }
    }
}