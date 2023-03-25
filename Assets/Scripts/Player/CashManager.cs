using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class CashManager : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        public float currCash;

        private void Awake() {
            currCash = tankStatsSo.Cash;
        }
    }
}