using Enemy.Tower.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class DestroyBody : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionCenterPs, explosionFireWavePs, explosionShockWavePs, burnPhasePs, smokePhasePs;
        [SerializeField] private TowerStatsSo towerStatsSo;
        private float _explosionPhaseTimer, _burnPhaseTimer;
        private bool _explosionStarted, _explosionEnded, _burnStarted, _burnEnded, _smokeStarted;
        private TowerHpManager _towerHpManager;
        private ParticleSystem.EmissionModule _explosionCenterEmMod, _explosionFireWaveEmMod, _explosionShockwaveEmMod, _burnEmMod, _smokeEmMod;
        private SpriteRenderer _sr;


        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
            _explosionCenterEmMod = explosionCenterPs.emission;
            _explosionFireWaveEmMod = explosionFireWavePs.emission;
            _explosionShockwaveEmMod = explosionShockWavePs.emission;
            _burnEmMod = burnPhasePs.emission;
            _smokeEmMod = smokePhasePs.emission;
        }

        private void Start() {
            _towerHpManager = transform.parent.GetComponent<TowerHpManager>();
        }

        private void Update() {
            if (!_towerHpManager.IsDead) return;
            if (_smokeStarted) return;

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

            if (_explosionEnded && !_burnEnded) {
                _burnPhaseTimer += Time.deltaTime;
                if (!_burnStarted) {
                    _burnEmMod.enabled = true;
                    _burnStarted = true;
                }
            }

            if (_explosionEnded && _burnEnded) {
                if (!_smokeStarted) {
                    _smokeEmMod.enabled = true;
                    _smokeStarted = true;
                }
            }

            if (_explosionPhaseTimer >= 1.5f) {
                _explosionEnded = true;
            }

            if (_burnPhaseTimer >= 15f) {
                _burnEnded = true;
                _burnEmMod.enabled = false;
                _sr.sprite = towerStatsSo.DestroyedBody;
            }
        }
    }
}