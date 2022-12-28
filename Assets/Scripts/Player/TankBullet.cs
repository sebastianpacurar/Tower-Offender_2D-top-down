using UnityEngine;

namespace Player {
    public class TankBullet : MonoBehaviour {
        private AimController _ac;
        private Vector3 _mousePos;
        private Camera _mainCam;
        private Rigidbody2D _rb;
        [SerializeField] private float speed;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Start() {
            _ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _mousePos = _mainCam.ScreenToWorldPoint(_ac.AimVal);
            var direction = _mousePos - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        }
    }
}