using UnityEngine;

namespace Player {
    public class TankShootAnimation : MonoBehaviour {
        private Animator _animator;
        private SpriteRenderer _sr;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private void Awake() {
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
        }

        public void StopShootAnimation() {
            _sr.enabled = false;
            _animator.SetBool(IsShooting, false);
        }
    }
}