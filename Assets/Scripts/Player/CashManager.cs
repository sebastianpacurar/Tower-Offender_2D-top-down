using System;
using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class CashManager : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        public float currCash;
        public float finalCash;

        private void Awake() {
            finalCash = tankStatsSo.Cash;
        }

        // increase the change time of the values based on the difference between the 2 cash values
        private float ShuffleSpeed() {
            return Math.Abs(currCash - finalCash) switch {
                < 100 => 100f,
                > 100 and < 200 => 150f,
                > 200 and < 500 => 300f,
                > 500 and < 1000 => 500f,
                > 1000 => 1000f,
                _ => 1000f
            };
        }

        private void Update() {
            if (currCash.Equals(finalCash)) return;

            if (currCash < finalCash) {
                currCash += Time.deltaTime * ShuffleSpeed();
                currCash = Mathf.Clamp(currCash, currCash, finalCash);
            } else if (currCash > finalCash) {
                currCash -= Time.deltaTime * ShuffleSpeed();
                currCash = Mathf.Clamp(currCash, finalCash, currCash);
            }
        }
    }
}