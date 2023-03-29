using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.ServiceMenu.UpgradesSubSections {
    public class SniperShellSection : MonoBehaviour {
        [SerializeField] private Image fireRateImg;
        [SerializeField] private Image fireRateUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI fireRatePrice;

        [SerializeField] private Image damageImg;
        [SerializeField] private Image damageUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI damagePrice;

        [SerializeField] private float startPrice = 20f;
        [SerializeField] private float currFireRatePrice;
        [SerializeField] private float currDamagePrice;

        private Button _fireRateBtn;
        private Button _damageBtn;

        private CashManager _cashManager;
        private WeaponStatsManager _weaponStats;


        private void Awake() {
            _fireRateBtn = fireRateUpgradeBtnImg.GetComponent<Button>();
            _damageBtn = damageUpgradeBtnImg.GetComponent<Button>();

            currFireRatePrice = startPrice;
            currDamagePrice = startPrice;
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
            damagePrice.text = !damageImg.fillAmount.Equals(1f) ? $"${currDamagePrice}" : "MAX";

            _fireRateBtn.interactable = !fireRateImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currFireRatePrice;
            _damageBtn.interactable = !damageImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currDamagePrice;
        }

        // decrease current reload time with 0.25f
        public void IncreaseFireRate() {
            _cashManager.finalCash -= currFireRatePrice;
            fireRateImg.fillAmount += 0.1f;
            currFireRatePrice += 20;
            _weaponStats.sniperFireRate -= 0.25f;
        }

        // increase shell damage with 1f
        public void IncreaseDamage() {
            _cashManager.finalCash -= currDamagePrice;
            damageImg.fillAmount += 0.1f;
            currDamagePrice += 20;
            _weaponStats.sniperDamage += 1f;
        }
    }
}