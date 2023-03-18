using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank {
    public class AoeScaler : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Transform circleRadiusArea;
        [SerializeField] private float multiplyFactor;
        private CircleCollider2D _circleCollider2D;

        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void Start() {
            if (name.StartsWith("Emp")) {
                _circleCollider2D.radius = tankStatsSo.EmpShellStatsSo.AoeRadius;
                circleRadiusArea.localScale = new Vector3(tankStatsSo.EmpShellStatsSo.AoeRadius * multiplyFactor, tankStatsSo.EmpShellStatsSo.AoeRadius * multiplyFactor, 1f);

                // initiate to false since the game starts with LightShell selected
                circleRadiusArea.gameObject.SetActive(false);
                circleRadiusArea.transform.parent.Find("AoeHitArea").gameObject.SetActive(false);
            } else if (name.StartsWith("Nuke")) {
                _circleCollider2D.radius = tankStatsSo.NukeShellStatsSo.AoeRadius;
                circleRadiusArea.localScale = new Vector3(tankStatsSo.NukeShellStatsSo.AoeRadius * multiplyFactor, tankStatsSo.NukeShellStatsSo.AoeRadius * multiplyFactor, 1f);

                // initiate to false since the game starts with LightShell selected
                circleRadiusArea.gameObject.SetActive(false);
                circleRadiusArea.transform.parent.Find("AoeHitArea").gameObject.SetActive(false);
            }
        }
    }
}