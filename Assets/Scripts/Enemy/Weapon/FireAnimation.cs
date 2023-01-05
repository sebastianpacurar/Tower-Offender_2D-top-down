using UnityEngine;

namespace Enemy.Weapon {
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