using UnityEngine;

namespace Shells {
    public class HandleMultiShells : MonoBehaviour {
        private Transform _tankPos, _startPos;

        private void Awake() {
            _tankPos = GameObject.FindGameObjectWithTag("Player").transform;
            _startPos = transform;
        }

        private void Start() {
            // rotate towards the tank
            var rotation = _tankPos.position - _startPos.position;
            var rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        // destroy container when there are no shells as children
        private void Update() {
            if (transform.childCount > 0) return;
            Destroy(gameObject);
        }
    }
}