using UnityEngine;

namespace Enemy {
    public class DestroyWall : MonoBehaviour {
        private int _lives;

        private void Awake() {
            if (name.StartsWith("Light"))
                _lives = 1;
            else if (name.StartsWith("Mid"))
                _lives = 3;
            else if (name.StartsWith("Heavy")) _lives = 5;
        }

        private void Update() {
            if (_lives == 0) {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TankLightShell")) {
                _lives--;
            }
        }
    }
}