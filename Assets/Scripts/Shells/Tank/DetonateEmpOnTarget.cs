using UnityEngine;

namespace Shells.Tank {
    public class DetonateEmpOnTarget : MonoBehaviour {
        private TankEmpShell _tankEmpShell;

        private void Start() {
            _tankEmpShell = transform.parent.GetComponent<TankEmpShell>();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("AoeHitArea")) {
                _tankEmpShell.DestroyShell();
            }
        }
    }
}