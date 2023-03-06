using Menus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class AimController : MonoBehaviour {
        public Vector3 AimVal { get; private set; }

        [SerializeField] private GameObject hull, turretEdge, empAoeGhost, nukeAoeGhost, lightShellGhost, sniperShellGhost;
        [SerializeField] private LineRenderer lightAimLine, sniperAimLine;

        private Camera _mainCam;
        private InGameMenu _inGameMenu;
        private Vector2 _lightShellFurthestPoint;
        private Vector2 _sniperShellFurthestPoint;

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _inGameMenu = GameObject.FindGameObjectWithTag("GameUI").GetComponent<InGameMenu>();
        }

        private void Update() {
            AimVal = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            AimVal = new Vector3(AimVal.x, AimVal.y, 0f);
            MoveHull();
            HandleLightShellAimLine();
        }

        private void MoveHull() {
            var pos = transform.position;
            var hullPosition = hull.transform.position;
            var direction = AimVal - hullPosition;

            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            hull.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            _sniperShellFurthestPoint = pos + direction.normalized * 200f;
            sniperShellGhost.transform.position = AimVal;

            _lightShellFurthestPoint = pos + direction.normalized * 6.25f;
            empAoeGhost.transform.position = AimVal;
            nukeAoeGhost.transform.position = AimVal;

            // set the shellGhost on top of the collider circle if mouse is outside of the surrounding circle
            //  if mouse is inside the collider circle, then render shellGhost on the Mouse's position
            var mouseDistanceFromHull = Vector2.Distance(AimVal, hullPosition);

            if (mouseDistanceFromHull > 6.25f) {
                lightShellGhost.transform.position = _lightShellFurthestPoint;
            } else {
                lightShellGhost.transform.position = AimVal;
            }
        }


        // TODO: fix bad handling here and in InGameMenu.cs (maybe consolidate)
        private void HandleLightShellAimLine() {
            if (_inGameMenu.SelectedShell.CompareTag("TankLightShell")) {
                sniperAimLine.enabled = false;
                lightAimLine.enabled = true;
                lightAimLine.SetPosition(0, turretEdge.transform.position);
                lightAimLine.SetPosition(1, lightShellGhost.transform.position);
            } else if (_inGameMenu.SelectedShell.CompareTag("TankSniperShell")) {
                lightAimLine.enabled = false;
                sniperAimLine.enabled = true;
                sniperAimLine.SetPosition(0, turretEdge.transform.position);
                sniperAimLine.SetPosition(1, _sniperShellFurthestPoint);
            } else {
                lightAimLine.enabled = false;
                sniperAimLine.enabled = false;
            }
        }
    }
}