using Menus.ServiceMenu;
using Player.Controllers;
using UnityEngine;

namespace Beacons {
    public class HandleServiceMenu : MonoBehaviour {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool fadeIn;
        [SerializeField] private bool fadeOut;

        private ShootController _shootController;
        private HandleShopSection _shopSection;

        private void Start() {
            _shootController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootController>();
            _shopSection = GetComponent<HandleShopSection>();
        }

        public void Update() {
            FadeMenu();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                fadeIn = true;
                fadeOut = false;

                _shootController.enabled = false;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                fadeIn = false;
                fadeOut = true;

                _shootController.enabled = true;
            }
        }

        private void FadeMenu() {
            if (fadeIn) {
                canvasGroup.interactable = true;

                if (canvasGroup.alpha < 1) {
                    canvasGroup.alpha += Time.deltaTime;

                    if (canvasGroup.alpha >= 1) {
                        fadeIn = false;
                    }
                }
            }

            if (fadeOut) {
                canvasGroup.interactable = false;

                if (canvasGroup.alpha > 0) {
                    canvasGroup.alpha -= Time.deltaTime;

                    if (canvasGroup.alpha <= 0) {
                        fadeOut = false;

                        // reset all values to 0 when fade reaches 0
                        _shopSection.ResetValues();
                    }
                }
            }
        }
    }
}