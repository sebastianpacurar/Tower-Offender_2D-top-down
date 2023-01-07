using UnityEngine;

namespace Shells {
    public class HandleMultiShells : MonoBehaviour {
        private Transform _tankPos, _towerPos;

        private void Start() {
            _tankPos = GameObject.FindGameObjectWithTag("Player").transform;
            _towerPos = transform.parent.transform.parent.Find("TowerObj").gameObject.transform;

            // rotate towards the tank
            var rotation = _tankPos.position - _towerPos.position;
            var rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ - 90f);
        }

        // destroy container when there are no shells as children
        private void Update() {
            if (transform.childCount > 0) return;
            Destroy(gameObject);
        }
    }
}