using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.ServiceMenu.UpgradesSubSections {
    public class EmpShellSection : MonoBehaviour {
        [SerializeField] private Image fireRateImg;
        [SerializeField] private Image fireRateUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI fireRatePrice;

        [SerializeField] private Image aoeRadiusImg;
        [SerializeField] private Image aoeRadiusUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI aoeRadiusPrice;

        [SerializeField] private Image aoeTimeImg;
        [SerializeField] private Image aoeTimeUpgradeBtnImg;
        [SerializeField] private TextMeshProUGUI aoeTimePrice;

        [SerializeField] private float startPrice = 10f;
        [SerializeField] private float currFireRatePrice;
        [SerializeField] private float currAoeRadiusPrice;
        [SerializeField] private float currAoeTimePrice;

        private Button _fireRateBtn;
        private Button _aoeRadiusBtn;
        private Button _aoeTimeBtn;

        private CashManager _cashManager;
        private WeaponStatsManager _weaponStats;


        private void Awake() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _cashManager = tank.GetComponent<CashManager>();
            _weaponStats = tank.GetComponent<WeaponStatsManager>();

            _aoeRadiusBtn = aoeRadiusUpgradeBtnImg.GetComponent<Button>();
            _aoeTimeBtn = aoeTimeUpgradeBtnImg.GetComponent<Button>();
            _fireRateBtn = fireRateUpgradeBtnImg.GetComponent<Button>();

            currFireRatePrice = startPrice;
            currAoeRadiusPrice = startPrice;
            currAoeTimePrice = startPrice;
        }


        private void Update() {
            ValidateButtons();
        }

        private void ValidateButtons() {
            fireRatePrice.text = !fireRateImg.fillAmount.Equals(1f) ? $"${currFireRatePrice}" : "MAX";
            aoeRadiusPrice.text = !aoeRadiusImg.fillAmount.Equals(1f) ? $"${currAoeRadiusPrice}" : "MAX";
            aoeTimePrice.text = !aoeTimeImg.fillAmount.Equals(1f) ? $"${currAoeTimePrice}" : "MAX";

            _fireRateBtn.interactable = !fireRateImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currFireRatePrice;
            _aoeRadiusBtn.interactable = !aoeRadiusImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currAoeRadiusPrice;
            _aoeTimeBtn.interactable = !aoeTimeImg.fillAmount.Equals(1f) && _cashManager.finalCash >= currAoeTimePrice;
        }

        // decrease current reload time with 0.25f
        public void IncreaseFireRate() {
            _cashManager.finalCash -= currFireRatePrice;
            fireRateImg.fillAmount += 0.1f;
            currFireRatePrice += 15;
            _weaponStats.empFireRate -= 0.25f;
        }

        // increase shell AoE Radius with 1f
        public void IncreaseAoeRadius() {
            _cashManager.finalCash -= currAoeRadiusPrice;
            aoeRadiusImg.fillAmount += 0.1f;
            currAoeRadiusPrice += 15;
            _weaponStats.empAoeRadius += 1;
        }

        // increase shell AoE Duration with 0.5f (half a second)
        public void IncreaseAoeTime() {
            _cashManager.finalCash -= currAoeTimePrice;
            aoeTimeImg.fillAmount += 0.1f;
            currAoeTimePrice += 20;
            _weaponStats.empAoeDuration += 0.5f;
        }
    }
}