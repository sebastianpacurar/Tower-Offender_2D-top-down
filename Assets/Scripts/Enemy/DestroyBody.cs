using Enemy.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Enemy {
    public class DestroyBody : MonoBehaviour {
        [SerializeField] private ParticleSystem explosionCenterPs, explosionFireWavePs, explosionShockWavePs, burnPhasePs, smokePhasePs;
        [SerializeField] private TowerStatsSo towerStatsSo;
        private float _explosionPhaseTimer, _burnPhaseTimer;
        private bool _explosionStarted, _explosionEnded, _burnStarted, _burnEnded, _smokeStarted;
        private TowerHpManager _towerHpManager;
        private ParticleSystem.EmissionModule _burnEmMod, _smokeEmMod;
        private SpriteRenderer _sr;


        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
            _burnEmMod = burnPhasePs.emission;
            _smokeEmMod = smokePhasePs.emission;
        }

        private void Start() {
            _towerHpManager = transform.parent.GetComponent<TowerHpManager>();

            explosionCenterPs.Stop();
            explosionFireWavePs.Stop();
            explosionShockWavePs.Stop();
        }

        private void Update() {
            if (!_towerHpManager.IsDead) return;
            if (_smokeStarted) return;

            if (!_explosionEnded) {
                _explosionPhaseTimer += Time.deltaTime;
                if (!_explosionStarted) {
                    explosionCenterPs.Play();
                    explosionFireWavePs.Play();
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