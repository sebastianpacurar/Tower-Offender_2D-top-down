using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class AimController : MonoBehaviour {
        public Vector3 AimVal { get; private set; }

        [SerializeField] private GameObject hull;
        [SerializeField] private GameObject aoeGhost, shellGhost;
        private Camera _mainCam;

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Update() {
            AimVal = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            MoveHull();
        }

        private void MoveHull() {
            var direction = AimVal - hull.transform.position;
            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            hull.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            aoeGhost.transform.position = new Vector3(AimVal.x, AimVal.y, 0f);
            shellGhost.transform.position = new Vector3(AimVal.x, AimVal.y, 0f);
        }
    }
}