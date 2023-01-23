using UnityEngine;

namespace Enemy.Tower {
    public class TurretShootAnimation : MonoBehaviour {
        private Animator _animator;
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void StopShootAnimation() {
            _animator.SetBool(IsShooting, false);
        }
    }
}