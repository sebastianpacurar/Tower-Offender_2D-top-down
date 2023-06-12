using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AltTestRelated {
    public class NavPointHandler : MonoBehaviour {
        [SerializeField] private bool isTriggered;
        private Light2D _triggerLight;
        private float _maxIntensity;
        private float _maxFalloff;

        private void Awake() {
            _triggerLight = transform.GetComponentInChildren<Light2D>();
            isTriggered = false;
            _maxIntensity = 1.5f;
            _maxFalloff = 3f;
        }

        private void Start() {
            _triggerLight.color = Color.red;
            _triggerLight.intensity = 0.5f;
            _triggerLight.shapeLightFalloffSize = 1f;
            _triggerLight.falloffIntensity = 0f;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player") && !isTriggered) {
                isTriggered = true;
                _triggerLight.color = Color.green;
                _triggerLight.intensity = 0f;
                _triggerLight.shapeLightFalloffSize = 0f;

                StartCoroutine(nameof(SwapEmpLightningIcon));
            }
        }

        private IEnumerator SwapEmpLightningIcon() {
            while (true) {
                yield return new WaitForSeconds(0.001f);

                _triggerLight.intensity += 0.025f;
                _triggerLight.shapeLightFalloffSize += 0.05f;
                _triggerLight.falloffIntensity += 0.025f;

                _triggerLight.intensity = Mathf.Clamp(_triggerLight.intensity, 0.5f, _maxIntensity);
                _triggerLight.shapeLightFalloffSize = Mathf.Clamp(_triggerLight.shapeLightFalloffSize, 1f, _maxFalloff);
                _triggerLight.falloffIntensity = Mathf.Clamp(_triggerLight.falloffIntensity, 0f, 1f);

                if (_triggerLight.intensity.Equals(_maxIntensity) && _triggerLight.shapeLightFalloffSize.Equals(_maxFalloff) && _triggerLight.falloffIntensity.Equals(1f)) {
                    StopCoroutine(nameof(SwapEmpLightningIcon));
                }
            }
        }
    }
}