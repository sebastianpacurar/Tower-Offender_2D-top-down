using Shells;
using UnityEngine;

namespace Player {
    public class HpHandler : MonoBehaviour {
        public float healthPoints;
        [SerializeField] private GameObject tankHull;

        private Rigidbody2D _tankRb;
        private AimController _aimController;
        private TankController _tankController;
        private Shoot _shoot;

        private ParticleSystem _ps;
        private ParticleSystem.EmissionModule _emissionModule;
        private ParticleSystem.MainModule _mainModule;

        private void Start() {
            _tankRb = GetComponent<Rigidbody2D>();
            _aimController = GetComponent<AimController>();
            _tankController = GetComponent<TankController>();
            _shoot = GetComponent<Shoot>();

            _ps = transform.Find("Particle System").GetComponent<ParticleSystem>();
            _emissionModule = _ps.emission;
            _mainModule = _ps.main;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("BasicShell") || col.gameObject.CompareTag("HomingShell")) {
                if (healthPoints > 0) {
                    healthPoints -= col.gameObject.GetComponent<TowerShell>().shellDamage;
                }

                switch (healthPoints) {
                    case > 5 and < 8:
                        _emissionModule.enabled = true;
                        _mainModule.startColor = new Color(0.4622642f, 0.4295568f, 0.4311143f);
                        _mainModule.startSize = 0.05f;
                        break;
                    case < 5 and > 3:
                        _mainModule.startColor = new Color(0.3207547f, 0.1982022f, 0.1982022f);
                        _mainModule.startSize = 0.1f;
                        break;
                    case < 3 and > 0:
                        _mainModule.startColor = Color.red;
                        _mainModule.startSize = 0.15f;
                        break;
                    case <= 0:
                        _mainModule.startSize = 0.2f;
                        tankHull.SetActive(false);
                        _aimController.enabled = false;
                        _tankController.enabled = false;
                        _shoot.enabled = false;
                        _tankRb.velocity = new Vector2(0, 0);
                        break;
                }
            }
        }
    }
}