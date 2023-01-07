using ScriptableObjects;
using UnityEngine;

namespace Enemy.Hp {
    public class TowerHpHandler : MonoBehaviour {
        public float towerHealthPoints;
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private GameObject towerUI;
        [SerializeField] private SpriteRenderer towerTrigger;
        [SerializeField] private SpriteRenderer towerTurret;
        private Tower _towerScript;

        private ParticleSystem _ps;
        private ParticleSystem.EmissionModule _emissionModule;

        private void Start() {
            towerHealthPoints = towerStatsSo.Hp;
            _towerScript = GetComponent<Tower>();
            _ps = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _emissionModule = _ps.emission;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankShell")) {
                towerHealthPoints -= 1;
            }
        }

        private void Update() {
            if (towerHealthPoints == 0) {
                towerUI.SetActive(false);
                _towerScript.enabled = false;
                towerTrigger.enabled = false;
                towerTurret.enabled = false;
                _emissionModule.enabled = true;
            }
        }
    }
}