using UnityEngine;
using UnityEngine.UI;

namespace Menus.ServiceMenu {
    public class HandleUpgradesSection : MonoBehaviour {
        [SerializeField] private Image lightShellBtnImg;
        [SerializeField] private Image empShellBtnImg;
        [SerializeField] private Image sniperShellBtnImg;
        [SerializeField] private Image nukeShellBtnImg;

        [SerializeField] private CanvasGroup canvasGroupLightShell;
        [SerializeField] private CanvasGroup canvasGroupEmpShell;
        [SerializeField] private CanvasGroup canvasGroupSniperShell;
        [SerializeField] private CanvasGroup canvasGroupNukeShell;

        [SerializeField] private bool subSectionFadeIn;
        [SerializeField] private bool subSectionFadeOut;

        private Button _lightShellBtn;
        private Button _empShellBtn;
        private Button _sniperShellBtn;
        private Button _nukeShellBtn;

        private CanvasGroup _currCanvasGroup;
        private CanvasGroup _targetCanvasGroup;


        private void Awake() {
            _lightShellBtn = lightShellBtnImg.GetComponent<Button>();
            _empShellBtn = empShellBtnImg.GetComponent<Button>();
            _sniperShellBtn = sniperShellBtnImg.GetComponent<Button>();
            _nukeShellBtn = nukeShellBtnImg.GetComponent<Button>();

            _currCanvasGroup = canvasGroupLightShell;
        }

        // set default to LightShell when menu is first opened
        private void Start() {
            _lightShellBtn.interactable = false;
            _empShellBtn.interactable = true;
            _sniperShellBtn.interactable = true;
            _nukeShellBtn.interactable = true;
        }

        private void Update() {
            FadeBetweenSubSections();
        }


        // 1) fade out current content, then fade in target content
        // NOTE: setting objects to true/false, because of overlapping buttons leading to misleading clicks
        private void FadeBetweenSubSections() {
            // fade out current content 
            if (subSectionFadeOut) {
                _currCanvasGroup.interactable = false;

                if (_currCanvasGroup.alpha >= 0) {
                    _currCanvasGroup.alpha -= Time.deltaTime * 10;

                    if (_currCanvasGroup.alpha <= 0) {
                        subSectionFadeIn = true;
                        subSectionFadeOut = false;
                        _currCanvasGroup.gameObject.SetActive(false); // set current to false!!
                    }
                }
            }

            // fade in target content
            if (subSectionFadeIn) {
                _targetCanvasGroup.gameObject.SetActive(true); // set target to true!!
                _targetCanvasGroup.interactable = true;

                if (_targetCanvasGroup.alpha <= 1) {
                    _targetCanvasGroup.alpha += Time.deltaTime * 10;

                    if (_targetCanvasGroup.alpha >= 1) {
                        subSectionFadeIn = false;
                        _currCanvasGroup = _targetCanvasGroup; // set current canvas group to target
                    }
                }
            }
        }
        

        // Handled by the subsection menu buttons (used as unity events)
        public void SwitchToLightShell() {
            if (subSectionFadeOut) return;
            subSectionFadeOut = true;
            _lightShellBtn.interactable = false;
            _empShellBtn.interactable = true;
            _sniperShellBtn.interactable = true;
            _nukeShellBtn.interactable = true;
            _targetCanvasGroup = canvasGroupLightShell;
        }

        public void SwitchToEmpShell() {
            if (subSectionFadeOut) return;
            subSectionFadeOut = true;
            _lightShellBtn.interactable = true;
            _empShellBtn.interactable = false;
            _sniperShellBtn.interactable = true;
            _nukeShellBtn.interactable = true;
            _targetCanvasGroup = canvasGroupEmpShell;
        }

        public void SwitchToSniperShell() {
            if (subSectionFadeOut) return;
            subSectionFadeOut = true;
            _lightShellBtn.interactable = true;
            _empShellBtn.interactable = true;
            _sniperShellBtn.interactable = false;
            _nukeShellBtn.interactable = true;
            _targetCanvasGroup = canvasGroupSniperShell;
        }

        public void SwitchToNukeShell() {
            if (subSectionFadeOut) return;
            subSectionFadeOut = true;
            _lightShellBtn.interactable = true;
            _empShellBtn.interactable = true;
            _sniperShellBtn.interactable = true;
            _nukeShellBtn.interactable = false;
            _targetCanvasGroup = canvasGroupNukeShell;
        }
    }
}