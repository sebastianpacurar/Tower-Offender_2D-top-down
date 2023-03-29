using Player;
using UnityEngine;

namespace Shells.Tank.AoeScalers {
    public class NukeShellAoeScaler : MonoBehaviour {
        [SerializeField] private Transform circleRadiusArea;
        [SerializeField] private float multiplyFactor;
        [SerializeField] private float radiusVal;
        private CircleCollider2D _circleCollider2D;
        private WeaponStatsManager _weaponStats;

        private void Awake() {
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }


        private void Start() {
            _weaponStats = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStatsManager>();
            
            // initiate to false since the game starts with LightShell selected
            circleRadiusArea.gameObject.SetActive(false);
            circleRadiusArea.transform.parent.Find("AoeHitArea").gameObject.SetActive(false);
        }

        private void Update() {
            radiusVal = _weaponStats.nukeAoeRadius;
            _circleCollider2D.radius = radiusVal;
            circleRadiusArea.localScale = new Vector3(radiusVal * multiplyFactor, radiusVal * multiplyFactor, 1f);
        }
    }
}