using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank.AoeScalers {
    public class LightShellAoeScaler : MonoBehaviour {
        [SerializeField] private TankShellStatsSo tankShellStatsSo;
        [SerializeField] private Transform circleRadiusArea;
        [SerializeField] private float multiplyFactor;
        private CircleCollider2D _circleCollider2D;

        public float radiusVal; // called in AimController.cs to draw the line render with its proper length
        public float finalRadiusVal; // called in TankLightShell.cs, when shell hits turret  

        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            radiusVal = tankShellStatsSo.AoeRadius;
        }

        private void Start() {
            StartCoroutine(DecreaseRadiusOverTime());
            StartCoroutine(IncreaseRadiusOverTime());
        }

        private IEnumerator DecreaseRadiusOverTime() {
            while (true) {
                yield return new WaitForSeconds(0.1f);

                if (radiusVal > 4f) {
                    radiusVal -= 0.01f;
                }

                radiusVal = Mathf.Clamp(radiusVal, tankShellStatsSo.MinAoeRadius, tankShellStatsSo.MaxAoeRadius);
            }
        }

        private IEnumerator IncreaseRadiusOverTime() {
            while (true) {
                yield return new WaitForSeconds(0.05f);

                if (radiusVal < finalRadiusVal) {
                    radiusVal += 0.02f;
                } else {
                    finalRadiusVal = 0f;
                }

                radiusVal = Mathf.Clamp(radiusVal, tankShellStatsSo.MinAoeRadius, tankShellStatsSo.MaxAoeRadius);
            }
        }

        private void Update() {
            _circleCollider2D.radius = radiusVal;
            circleRadiusArea.localScale = new Vector3(radiusVal * multiplyFactor, radiusVal * multiplyFactor, 1f);
        }
    }
}