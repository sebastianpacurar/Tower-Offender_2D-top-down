using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Beacons {
    public class HandleHpBeacon : MonoBehaviour {
        [SerializeField] private Light2D greenLight;
        [SerializeField] private float sineFreq;
        [SerializeField] private float sineAmp;

        [SerializeField] private float initialIntensityVal;
        [SerializeField] private bool isOnBeacon;
        [SerializeField] private bool isSineOn;

        private void Awake() {
            initialIntensityVal = 1f;
        }

        private void Update() {
            HandleLightIntensity();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                isOnBeacon = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                isOnBeacon = false;
            }
        }

        private float GetSineVal() {
            return Mathf.Sin(Time.time * sineFreq) * sineAmp + initialIntensityVal;
        }

        // fadeIn (on enter) - Sine (on stay) - fadeOut (on exit)
        private void HandleLightIntensity() {
            if (isOnBeacon) {
                if (!isSineOn) {
                    if (greenLight.intensity < GetSineVal()) {
                        greenLight.intensity += Time.deltaTime;
                    } else {
                        isSineOn = true;
                    }
                }

                if (isSineOn) {
                    greenLight.intensity = GetSineVal();
                }
            } else {
                if (greenLight.intensity > 0) {
                    greenLight.intensity -= Time.deltaTime;
                } else {
                    isSineOn = true;
                }

                isSineOn = false;
            }

            greenLight.intensity = Mathf.Clamp(greenLight.intensity, 0, float.MaxValue);
        }
    }
}