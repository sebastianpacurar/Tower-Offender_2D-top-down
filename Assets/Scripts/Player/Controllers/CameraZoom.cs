using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controllers {
    public class CameraZoom : MonoBehaviour {
        private PlayerControls _controls;
        private CinemachineVirtualCamera _cam;
        [SerializeField] private float currentZoomVal;
        [SerializeField] private float finalZoomVal;

        [SerializeField] private float minOrthoSize = 7;
        [SerializeField] private float maxOrthoSize = 25;

        private void Awake() {
            currentZoomVal = minOrthoSize;
            finalZoomVal = minOrthoSize;
            _controls = new PlayerControls();
            _cam = GameObject.FindGameObjectWithTag("CM2D").GetComponent<CinemachineVirtualCamera>();
        }

        private void PerformZoom(InputAction.CallbackContext ctx) {
            finalZoomVal += _controls.Player.CameraZoom.ReadValue<float>() > 1 ? 1 : -1;
            finalZoomVal = Mathf.Clamp(finalZoomVal, minOrthoSize, maxOrthoSize);
        }

        private void Update() {
            PerformSmoothZoom();
        }

        // zoom smoothly on every update-based frame by 0.2f
        private void PerformSmoothZoom() {
            if (!currentZoomVal.Equals(finalZoomVal)) {
                if (currentZoomVal < finalZoomVal) {
                    currentZoomVal += 0.2f;
                    currentZoomVal = Mathf.Clamp(currentZoomVal, currentZoomVal, finalZoomVal);
                } else if (currentZoomVal > finalZoomVal) {
                    currentZoomVal -= 0.2f;
                    currentZoomVal = Mathf.Clamp(currentZoomVal, finalZoomVal, currentZoomVal);
                }
            }

            _cam.m_Lens.OrthographicSize = currentZoomVal;
        }

        private void OnEnable() {
            _controls.Player.CameraZoom.Enable();
            _controls.Player.CameraZoom.performed += PerformZoom;
        }

        private void OnDisable() {
            _controls.Player.CameraZoom.Disable();
            _controls.Player.CameraZoom.performed -= PerformZoom;
        }
    }
}