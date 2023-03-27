using Menus.ServiceMenu;
using Player.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Beacons {
    public class HandleServiceMenu : MonoBehaviour {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private Image shopBtnImage;
        [SerializeField] private Image upgradesBtnImage;
        [SerializeField] private GameObject shopContent;
        [SerializeField] private GameObject upgradesContent;

        [SerializeField] private bool menuFadeIn;
        [SerializeField] private bool menuFadeOut;
        [SerializeField] private bool sectionFadeIn;
        [SerializeField] private bool sectionFadeOut;
        [SerializeField] private bool isShopDisplayed;
        [SerializeField] private bool isUpgradesDisplayed;

        private Button _shopBtn;
        private Button _upgradesBtn;
        private ShootController _shootController;
        private HandleShopSection _shopSection;

        private CanvasGroup _canvasGroupShopContent;
        private CanvasGroup _canvasGroupUpgradesContent;


        private void Awake() {
            _shopSection = GetComponent<HandleShopSection>();
            _shopBtn = shopBtnImage.GetComponent<Button>();
            _upgradesBtn = upgradesBtnImage.GetComponent<Button>();
            _canvasGroupShopContent = shopContent.GetComponent<CanvasGroup>();
            _canvasGroupUpgradesContent = upgradesContent.GetComponent<CanvasGroup>();
        }

        private void Start() {
            isShopDisplayed = true;
            _shootController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootController>();
        }

        private void Update() {
            TriggerMenuVisibility();
            ValidateButtons();
            FadeBetweenSections();
        }

        private void ValidateButtons() {
            _shopBtn.interactable = !isShopDisplayed;
            _upgradesBtn.interactable = !isUpgradesDisplayed;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                menuFadeIn = true;
                menuFadeOut = false;

                _shootController.enabled = false;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                menuFadeIn = false;
                menuFadeOut = true;

                _shootController.enabled = true;
            }
        }

        // 1) fade out current content, then disable it
        // 2) enable target content, then fade it in
        private void FadeBetweenSections() {
            var targetObj = isShopDisplayed ? shopContent : upgradesContent;
            var currObj = isShopDisplayed ? upgradesContent : shopContent;
            var targetCanvas = isShopDisplayed ? _canvasGroupShopContent : _canvasGroupUpgradesContent;
            var currCanvas = isShopDisplayed ? _canvasGroupUpgradesContent : _canvasGroupShopContent;

            // fade out current content 
            if (sectionFadeOut) {
                currCanvas.interactable = false;

                if (currCanvas.alpha >= 0) {
                    currCanvas.alpha -= Time.deltaTime * 10;

                    if (currCanvas.alpha <= 0) {
                        sectionFadeIn = true;
                        sectionFadeOut = false;
                        currObj.SetActive(false); // set current to false!!
                    }
                }
            }

            // fade in target content
            if (sectionFadeIn) {
                targetObj.SetActive(true); // set target to true!!
                targetCanvas.interactable = true;

                if (targetCanvas.alpha <= 1) {
                    targetCanvas.alpha += Time.deltaTime * 10;

                    if (targetCanvas.alpha >= 1) {
                        sectionFadeIn = false;
                    }
                }
            }
        }


        private void TriggerMenuVisibility() {
            if (menuFadeIn) {
                canvasGroup.interactable = true;

                if (canvasGroup.alpha < 1) {
                    canvasGroup.alpha += Time.deltaTime;

                    if (canvasGroup.alpha >= 1) {
                        menuFadeIn = false;
                    }
                }
            }

            if (menuFadeOut) {
                canvasGroup.interactable = false;

                if (canvasGroup.alpha > 0) {
                    canvasGroup.alpha -= Time.deltaTime;

                    if (canvasGroup.alpha <= 0) {
                        menuFadeOut = false;

                        // reset all values to 0 when fade reaches 0
                        _shopSection.ResetValues();
                    }
                }
            }
        }


        // the below are used as unity events for the Shop, Upgrades buttons
        public void SwitchToUpgrades() {
            if (sectionFadeOut) return;
            sectionFadeOut = true;
            isUpgradesDisplayed = true;
            isShopDisplayed = false;
        }

        public void SwitchToShop() {
            if (sectionFadeOut) return;
            sectionFadeOut = true;
            isShopDisplayed = true;
            isUpgradesDisplayed = false;
        }
    }
}