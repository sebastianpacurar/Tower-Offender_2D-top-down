using Enemy.Tower.Hp;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Enemy.Tower {
    public class MapTile : MonoBehaviour {
        [SerializeField] private MapTileColorSo mapTileColorSo;
        [SerializeField] private TextMeshProUGUI turretCounter;
        private TurretHpManager _turretHpManager;
        private SpriteRenderer _sr;

        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
            _turretHpManager = transform.parent.Find("TurretObj").GetComponent<TurretHpManager>();
        }

        private void Start() {
            _sr.enabled = true;
            _sr.color = mapTileColorSo.ActiveColor;
            var turretEntityName = transform.parent.name;

            turretCounter.text = turretEntityName[0].ToString(); // set the number to be equal to the number of cannons the turret has
            turretCounter.color = turretEntityName.Contains("LT") ? new Color(0f, 0f, 0f, 0.7f) : new Color(1f, 1f, 1f, 0.7f); // Light is on black and Red/Purple on white
        }

        private void Update() {
            if (!_turretHpManager.IsDead) return;
            _sr.color = mapTileColorSo.InactiveColor;
        }
    }
}