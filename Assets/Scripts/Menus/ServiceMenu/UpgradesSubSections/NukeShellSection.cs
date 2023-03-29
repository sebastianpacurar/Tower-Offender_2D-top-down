using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.ServiceMenu.UpgradesSubSections {
    public class NukeShellSection : MonoBehaviour {
        [SerializeField] private Image fireRateImg;
        [SerializeField] private Image fireRateUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI fireRatePrice;

        [SerializeField] private Image aoeRadiusImg;
        [SerializeField] private Image aoeRadiusUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI aoeRadiusPrice;

        [SerializeField] private float startPrice = 25f;
        [SerializeField] private float currFireRatePrice;
        [SerializeField] private float currAoeRadiusPrice;

        private Button _fireRateBtn;
        private Button _aoeRadiusBtn;

        private CashManager _cashManager;
        private WeaponStatsManager _weaponStats;


        private void Awake() {
            _aoeRadiusBtn = aoeRadiusUpgradeBtnImg.GetComponent<Button>();
            _fireRateBtn = fireRateUpgradeBtnImg.GetComponent<Button>();

            currFireRatePrice = startPrice;
            currAoeRadiusPrice = startPrice;
        }

        
        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _cashManager = tank.GetComponent<CashManager>();
            _weaponStats = tank.GetComponent<WeaponStatsManager>();
        }


        private void Update() {
            ValidateButtons();
        }

        private void ValidateButtons() {
            fireRatePrice.text = !fireRateImg.fillAmount.Equals(1f) ? $"${currFireRatePrice}" : "MAX";
            aoeRadiusPrice.text = !aoeRadiusImg.fillAmount.Equals(1f) ? $"${currAoeRadiusPrice}" : "MAX";

            _fireRateBtn.interactable = !fireRateImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currFireRatePrice;
            _aoeRadiusBtn.interactable = !aoeRadiusImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currAoeRadiusPrice;
        }

        // decrease current reload time with 0.25f
        public void IncreaseFireRate() {
            _cashManager.finalCash -= currFireRatePrice;
            fireRateImg.fillAmount += 0.1f;
            currFireRatePrice += 25;
            _weaponStats.nukeFireRate -= 0.25f;
        }

        // increase shell AoE Radius with 1f
        public void IncreaseAoeRadius() {
            _cashManager.finalCash -= currAoeRadiusPrice;
            aoeRadiusImg.fillAmount += 0.1f;
            currAoeRadiusPrice += 25;
            _weaponStats.nukeAoeRadius += 1;
        }
    }
}