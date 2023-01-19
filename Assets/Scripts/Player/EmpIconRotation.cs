using UnityEngine;

namespace Player {
    // since the parent, TankObj is being rotated, then the icon also gets rotated when steering is applied;
    // set the lightning icon to its parent's z-axis rotation multiplied with -1
    public class EmpIconRotation : MonoBehaviour {
        [SerializeField] private Transform tankObj;
        private float _tankObjRotationZ;

        private void Update() {
            _tankObjRotationZ = transform.rotation.z;
            transform.rotation = Quaternion.Euler(0f, 0f, -_tankObjRotationZ);
        }
    }
}