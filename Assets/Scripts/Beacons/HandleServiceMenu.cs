using System;
using Player;
using Player.Controllers;
using TMPro;
using UnityEngine;

namespace Beacons {
    public class HandleServiceMenu : MonoBehaviour {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool fadeIn;
        [SerializeField] private bool fadeOut;

        [SerializeField] private TextMeshProUGUI totalCash;
        private ShootController _shootController;
        private CashManager _cashManager;

        private void Start() {
            var tankObj = GameObject.FindGameObjectWithTag("Player");
            _shootController = tankObj.GetComponent<ShootController>();
            _cashManager = tankObj.GetComponent<CashManager>();
        }

        public void Update() {
            FadeMenu();
            UpdateMoneyDisplay();
        }

        private void UpdateMoneyDisplay() {
            totalCash.text = $"Cash: ${Math.Round(_cashManager.currCash, 2)}";
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
                    }
                }
            }
        }
    }
}