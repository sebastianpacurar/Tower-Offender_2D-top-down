using Menus;
using UnityEngine;

namespace Player {
    public class DestroyTank : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionCenterPs, explosionFireWavePs, explosionShockWavePs, heavySmokePs, criticalSmokePs;
        private float _explosionPhaseTimer, _burnPhaseTimer;
        private bool _explosionStarted, _explosionEnded, _heavySmokeStarted, _heavySmokeEnded, _criticalSmokeStarted;
        private bool _triggerMenu;
        private ParticleSystem.EmissionModule _explosionCenterEmMod, _explosionFireWaveEmMod, _explosionShockwaveEmMod, _heavySmokeEmMod, _criticalSmokeEmMod;

        private TankHpManager _tankHpManager;
        private GameOverMenu _gameOverMenu;

        private void Awake() {
            _explosionCenterEmMod = explosionCenterPs.emission;
            _explosionFireWaveEmMod = explosionFireWavePs.emission;
            _explosionShockwaveEmMod = explosionShockWavePs.emission;
            _heavySmokeEmMod = heavySmokePs.emission;
            _criticalSmokeEmMod = criticalSmokePs.emission;
        }

        private void Start() {
            _tankHpManager = GetComponent<TankHpManager>();
            _gameOverMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<GameOverMenu>();
        }

        private void Update() {
            if (!_tankHpManager.IsDead) return;
            if (_criticalSmokeStarted) return;
            heavySmokePs.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            criticalSmokePs.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            if (!_explosionEnded) {
                _explosionPhaseTimer += Time.deltaTime;
                if (!_explosionStarted) {
                    _explosionCenterEmMod.enabled = true;
                    explosionCenterPs.Play();

                    _explosionFireWaveEmMod.enabled = true;
                    explosionFireWavePs.Play();

                    _explosionShockwaveEmMod.enabled = true;
                    explosionShockWavePs.Play();

                    _explosionStarted = true;
                }
            }

            if (_explosionEnded && !_heavySmokeEnded) {
                _burnPhaseTimer += Time.deltaTime;
                if (!_heavySmokeStarted) {
                    _heavySmokeEmMod.enabled = true;
                    _heavySmokeStarted = true;
                }
            }

            if (_explosionEnded && _heavySmokeEnded) {
                if (!_criticalSmokeStarted) {
                    _criticalSmokeEmMod.enabled = true;
                    _criticalSmokeStarted = true;
                    _triggerMenu = true;
                }
            }

            if (_explosionPhaseTimer >= 1.5f) {
                _explosionEnded = true;
            }

            if (_burnPhaseTimer >= 3f) {
                _heavySmokeEnded = true;
                _heavySmokeEmMod.enabled = false;
            }

            if (_triggerMenu) {
                _gameOverMenu.ToggleGameOver();
            }
        }
    }
}