using UnityEngine;

namespace Player {
    public class TankShootAnimation : MonoBehaviour {
        private Animator _animator;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void StopShootAnimation() {
            _animator.SetBool("IsShooting", false);
        }
    }
}