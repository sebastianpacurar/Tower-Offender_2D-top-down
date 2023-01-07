using System.Collections;
using UnityEngine;

namespace Shells {
    public class KillShellsOverTime : MonoBehaviour {
        [SerializeField] private float timeToLiveInSecs;
    
        private void Start() {
            StartCoroutine(StartCountdown());
        }

        // private IEnumerator private Destroy
        private IEnumerator StartCountdown() {
            while (true) {
                yield return new WaitForSeconds(timeToLiveInSecs);
                Destroy(gameObject);
            }
        }
    }
}