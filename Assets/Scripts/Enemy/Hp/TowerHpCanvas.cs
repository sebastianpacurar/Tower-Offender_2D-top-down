using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Hp {
    public class TowerHpCanvas : MonoBehaviour {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject towerObj;
        [SerializeField] private Image greenBar;
        private float _maxHp;

        private Camera _mainCam;
        private TowerHpHandler _hpHandlerScript;

        private void Start() {
            _hpHandlerScript = transform.parent.Find("TowerObj").GetComponent<TowerHpHandler>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;

            _maxHp = _hpHandlerScript.TowerHealthPoints;
        }

        private void Update() {
            transform.position = towerObj.transform.position;
            greenBar.fillAmount = _hpHandlerScript.TowerHealthPoints / _maxHp;
        }
    }
}