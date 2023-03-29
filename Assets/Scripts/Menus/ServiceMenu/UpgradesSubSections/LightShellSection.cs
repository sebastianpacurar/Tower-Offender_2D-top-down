using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.ServiceMenu.UpgradesSubSections {
    public class LightShellSection : MonoBehaviour {
        [SerializeField] private Image fireRateImg;
        [SerializeField] private Image fireRateUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI fireRatePrice;

        [SerializeField] private Image speedImg;
        [SerializeField] private Image speedUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI speedPrice;

        [SerializeField] private Image damageImg;
        [SerializeField] private Image damageUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI damagePrice;

        [SerializeField] private float startPrice = 10f;
        [SerializeField] private float currFireRatePrice;
        [SerializeField] private float currSpeedPrice;
        [SerializeField] private float currDamagePrice;

        private Button _fireRateBtn;
        private Button _speedBtn;
        private Button _damageBtn;

        private CashManager _cashManager;
        private WeaponStatsManager _weaponStats;


        private void Awake() {
            _fireRateBtn = fireRateUpgradeBtnImg.GetComponent<Button>();
            _speedBtn = speedUpgradeBtnImg.GetComponent<Button>();
            _damageBtn = damageUpgradeBtnImg.GetComponent<Button>();

            currFireRatePrice = startPrice;
            currSpeedPrice = startPrice;
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
            speedPrice.text = !speedImg.fillAmount.Equals(1f) ? $"${currSpeedPrice}" : "MAX";
            damagePrice.text = !damageImg.fillAmount.Equals(1f) ? $"${currDamagePrice}" : "MAX";

            _fireRateBtn.interactable = !fireRateImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currFireRatePrice;
            _speedBtn.interactable = !speedImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currSpeedPrice;
            _damageBtn.interactable = !damageImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currDamagePrice;
        }

        // decrease current reload time with 0.05f
        public void IncreaseFireRate() {
            _cashManager.finalCash -= currFireRatePrice;
            fireRateImg.fillAmount += 0.1f;
            currFireRatePrice += 10;
            _weaponStats.lightShellFireRate -= 0.05f;
        }

        // increase shell speed with 1f
        public void IncreaseSpeed() {
            _cashManager.finalCash -= currSpeedPrice;
            speedImg.fillAmount += 0.1f;
            currSpeedPrice += 10;
            _weaponStats.lightShellSpeed += 1;
        }

        // increase shell damage with 0.75f
        public void IncreaseDamage() {
            _cashManager.finalCash -= currDamagePrice;
            damageImg.fillAmount += 0.1f;
            currDamagePrice += 10;
            _weaponStats.lightShellDamage += 0.75f;
        }
    }
}