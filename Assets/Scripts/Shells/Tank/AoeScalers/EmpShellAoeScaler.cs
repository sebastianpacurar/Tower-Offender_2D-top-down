using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank.AoeScalers {
    public class EmpShellAoeScaler : MonoBehaviour {
        [SerializeField] private TankShellStatsSo shellStatsSo;
        [SerializeField] private Transform circleRadiusArea;
        [SerializeField] private float multiplyFactor;
        [SerializeField] private float radiusVal;
        private CircleCollider2D _circleCollider2D;

        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            radiusVal = shellStatsSo.AoeRadius;
        }

        private void Start() {
            _circleCollider2D.radius = radiusVal;
            circleRadiusArea.localScale = new Vector3(radiusVal * multiplyFactor, radiusVal * multiplyFactor, 1f);

            // initiate to false since the game starts with LightShell selected
            circleRadiusArea.gameObject.SetActive(false);
            circleRadiusArea.transform.parent.Find("AoeHitArea").gameObject.SetActive(false);
        }
    }
}