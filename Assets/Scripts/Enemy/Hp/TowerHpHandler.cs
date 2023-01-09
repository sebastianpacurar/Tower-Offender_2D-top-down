using ScriptableObjects;
using UnityEngine;

namespace Enemy.Hp {
    public class TowerHpHandler : MonoBehaviour {
        [SerializeField] private TowerStatsSo towerStatsSo;
        [SerializeField] private GameObject towerUI;
        [SerializeField] private SpriteRenderer towerTrigger;
        [SerializeField] private SpriteRenderer towerTurret;
        public float TowerHealthPoints { get; private set; }
        private Tower _towerScript;

        private ParticleSystem _ps;
        private ParticleSystem.EmissionModule _emissionModule;

        private void Start() {
            TowerHealthPoints = towerStatsSo.Hp;
            _towerScript = GetComponent<Tower>();
            _ps = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _emissionModule = _ps.emission;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankShell")) {
                TowerHealthPoints -= 1;
            }
        }

        private void Update() {
            if (TowerHealthPoints == 0) {
                towerUI.SetActive(false);
                _towerScript.enabled = false;
                towerTrigger.enabled = false;
                towerTurret.enabled = false;
                _emissionModule.enabled = true;
            }
        }
    }
}