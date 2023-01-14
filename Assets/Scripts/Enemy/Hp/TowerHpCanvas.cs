using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Hp {
    public class TowerHpCanvas : MonoBehaviour {
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject towerObj;
        [SerializeField] private Image greenBar;

        private Camera _mainCam;
        private TowerHpManager _towerHpManager;

        private void Start() {
            _towerHpManager = transform.parent.Find("TowerObj").GetComponent<TowerHpManager>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            transform.position = towerObj.transform.position;
            greenBar.fillAmount = _towerHpManager.TowerHealthPoints / towerStatsSo.MaxHp;
        }
    }
}