using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank.AoeScalers {
    public class LightShellAoeScaler : MonoBehaviour {
        [SerializeField] private TankShellStatsSo tankShellStatsSo;
        [SerializeField] private Transform circleRadiusArea;
        [SerializeField] private float multiplyFactor;
        private CircleCollider2D _circleCollider2D;

        public float radiusVal; // called in AimController.cs to draw the line render with its proper length
        public float finalRadiusVal; // called in TankLightShell.cs, when shell hits turret or destroyable wall  


        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            radiusVal = tankShellStatsSo.AoeRadius;
        }

        private void Update() {
            HandleCircleRangeSizeOverTime();

            _circleCollider2D.radius = radiusVal;
            circleRadiusArea.localScale = new Vector3(radiusVal * multiplyFactor, radiusVal * multiplyFactor, 1f);
        }

        // increase radius if shells hit a valid target; decrease overtime
        private void HandleCircleRangeSizeOverTime() {
            if (radiusVal > tankShellStatsSo.MinAoeRadius && radiusVal > finalRadiusVal) {
                radiusVal -= Time.deltaTime * 0.1f;
            }

            if (radiusVal < finalRadiusVal && finalRadiusVal < tankShellStatsSo.MaxAoeRadius) {
                radiusVal += Time.deltaTime * 0.2f;
            } else {
                finalRadiusVal = 0f;
            }

            radiusVal = Mathf.Clamp(radiusVal, tankShellStatsSo.MinAoeRadius, tankShellStatsSo.MaxAoeRadius);
        }
    }
}