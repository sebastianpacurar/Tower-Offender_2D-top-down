using Menus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class AimController : MonoBehaviour {
        public Vector3 AimVal { get; private set; }

        [SerializeField] private GameObject hull;
        [SerializeField] private GameObject aoeGhost, shellGhost;
        [SerializeField] private LineRenderer directionLine;

        private Camera _mainCam;
        private InGameMenu _inGameMenu;

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _inGameMenu = GameObject.FindGameObjectWithTag("GameUI").GetComponent<InGameMenu>();
        }

        private void Update() {
            AimVal = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            AimVal = new Vector3(AimVal.x, AimVal.y, 0f);
            MoveHull();
            HandleLightShellDirectionLine();
        }

        private void MoveHull() {
            var hullPosition = hull.transform.position;
            var direction = AimVal - hullPosition;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            hull.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            aoeGhost.transform.position = AimVal;

            // TODO: fix hardcoded 12.5f offset
            var furthestPoint = transform.position + direction.normalized * 12.5f;
            var mouseDistanceFromHull = Vector2.Distance(AimVal, hullPosition);

            // set the shellGhost on top of the collider circle if mouse is outside of the surrounding circle
            //  if mouse is inside the collider circle, then render shellGhost on the Mouse's position
            if (mouseDistanceFromHull > 12.5f) {
                shellGhost.transform.position = furthestPoint;
            } else {
                shellGhost.transform.position = AimVal;
            }
        }

        private void HandleLightShellDirectionLine() {
            if (_inGameMenu.SelectedShell.CompareTag("TankLightShell")) {
                directionLine.enabled = true;
                directionLine.SetPosition(0, transform.position);
                directionLine.SetPosition(1, shellGhost.transform.position);
            } else {
                if (!directionLine.enabled) return;
                directionLine.enabled = false;
            }
        }
    }
}