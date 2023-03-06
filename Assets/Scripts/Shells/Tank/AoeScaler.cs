using ScriptableObjects;
using UnityEngine;

namespace Shells.Tank {
    public class AoeScaler : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private Transform circleRadiusArea;
        private CircleCollider2D _circleCollider2D;

        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void Start() {
            var offset = 0.05f;
            if (name.StartsWith("Emp")) {
                // applies for hollow circle sprite (hollow circle of scale 1 equals to a radius of 0.81f)
                _circleCollider2D.radius = tankStatsSo.EmpShellStatsSo.AoeRadius * 0.81f;
                circleRadiusArea.localScale = new Vector3(tankStatsSo.EmpShellStatsSo.AoeRadius + offset, tankStatsSo.EmpShellStatsSo.AoeRadius + offset, 1f);
                circleRadiusArea.gameObject.SetActive(false);
                circleRadiusArea.transform.parent.Find("AoeHitArea").gameObject.SetActive(false);
            } else if (name.StartsWith("Nuke")) {
                // applies for regular filled circle sprite
                _circleCollider2D.radius = tankStatsSo.NukeShellStatsSo.AoeRadius;
                circleRadiusArea.localScale = new Vector3(tankStatsSo.NukeShellStatsSo.AoeRadius * 2f + offset, tankStatsSo.NukeShellStatsSo.AoeRadius * 2f + offset, 1f);
                circleRadiusArea.gameObject.SetActive(false);
            }
        }
    }
}