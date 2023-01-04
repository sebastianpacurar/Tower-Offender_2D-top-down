using UnityEngine;

namespace Enemy {
    public class FireAnimation : MonoBehaviour {
        private Animator _animator;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void StopFireAnimation() {
            _animator.SetBool("IsShooting", false);
        }
    }
}