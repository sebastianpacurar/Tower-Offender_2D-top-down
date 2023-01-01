using UnityEngine;

namespace Enemy {
    public class MoveTower : MonoBehaviour {
        [SerializeField] private GameObject[] locations;
        [SerializeField] private float speed;
        private int _index;
        private TowerHpHandler _hpHandlerScript;

        private void Start() {
            _hpHandlerScript = transform.parent.Find("TowerObj").GetComponent<TowerHpHandler>();
        }

        private void Update() {
            if (!(_hpHandlerScript.TowerHealthPoints > 0)) return;
            if (Vector2.Distance(locations[_index].transform.position, transform.position) < 0.1f) {
                _index++;
                if (_index >= locations.Length) {
                    _index = 0;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, locations[_index].transform.position, speed * Time.deltaTime);
        }
    }
}