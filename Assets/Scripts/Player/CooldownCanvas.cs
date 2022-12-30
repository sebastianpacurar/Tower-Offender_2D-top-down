using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class CooldownCanvas : MonoBehaviour {
        [SerializeField] private GameObject tankObj;
        [SerializeField] private Image imgBar;
        private Shoot _shootScript;

        private void Start() {
            _shootScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        }

        private void Update() {
            transform.position = tankObj.transform.position;
            imgBar.color = _shootScript.CanFire ? Color.green : Color.red;
        }
    }
}