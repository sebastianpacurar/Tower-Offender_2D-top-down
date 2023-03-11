using Enemy.Tower.Hp;
using UnityEngine;

namespace Enemy.Tower {
    public class TurretExplosionParticles : MonoBehaviour {
        [SerializeField] private ParticleSystem center, fireWave, shockWave;
        private ParticleSystem.EmissionModule _centerEm, _fireWaveEm, _shockWaveEm;
        private TurretHpManager _turretHpManager;

        private void Awake() {
            _turretHpManager = GetComponent<TurretHpManager>();

            _centerEm = center.emission;
            _fireWaveEm = fireWave.emission;
            _shockWaveEm = shockWave.emission;
        }

        private void Update() {
            if (_turretHpManager.IsDead) {
                _centerEm.enabled = true;
                center.Play();

                _fireWaveEm.enabled = true;
                fireWave.Play();

                _shockWaveEm.enabled = true;
                shockWave.Play();
            }
        }
    }
}