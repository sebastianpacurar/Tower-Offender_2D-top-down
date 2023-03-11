using Enemy.Tower.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class TurretDamageParticles : MonoBehaviour {
        [SerializeField] private TurretStatsSo turretStatsSo;
        [SerializeField] private ParticleSystem smokeLight, smokeHeavy, smokeCritical;
        private ParticleSystem.EmissionModule _lightEm, _heavyEm, _criticalEm;
        private TurretHpManager _turretHpManager;
        private Transform _tankPos;

        private void Awake() {
            _lightEm = smokeLight.emission;
            _heavyEm = smokeHeavy.emission;
            _criticalEm = smokeCritical.emission;
        }

        private void Start() {
            _tankPos = GameObject.FindGameObjectWithTag("Player").transform;
            _turretHpManager = transform.parent.GetComponent<TurretHpManager>();
        }

        void Update() {
            var direction = _tankPos.position - transform.position;
            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            smokeLight.transform.rotation = Quaternion.Euler(0, 0, -rotZ);
            smokeHeavy.transform.rotation = Quaternion.Euler(0, 0, -rotZ);
            smokeCritical.transform.rotation = Quaternion.Euler(0, 0, -rotZ);

            switch (_turretHpManager.TurretHealthPoints / turretStatsSo.MaxHp) {
                case > 0.7f and < 0.9f:
                    _lightEm.enabled = true;
                    break;
                case > 0.4f and < 0.7f:
                    _heavyEm.enabled = true;
                    break;
                case < 0.4f and > 0f:
                    _criticalEm.enabled = true;
                    break;
                case <= 0f:
                    _lightEm.enabled = false;
                    _heavyEm.enabled = false;
                    _criticalEm.enabled = false;
                    break;
            }
        }
    }
}