using System;
using Player;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Int32;

namespace Menus.ServiceMenu {
    public class HandleShopSection : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;

        [SerializeField] private TextMeshProUGUI empCount;
        [SerializeField] private TextMeshProUGUI empValue;
        [SerializeField] private Image empPlus;
        [SerializeField] private Image empMinus;

        [SerializeField] private TextMeshProUGUI sniperCount;
        [SerializeField] private TextMeshProUGUI sniperValue;
        [SerializeField] private Image sniperPlus;
        [SerializeField] private Image sniperMinus;

        [SerializeField] private TextMeshProUGUI nukeCount;
        [SerializeField] private TextMeshProUGUI nukeValue;
        [SerializeField] private Image nukePlus;
        [SerializeField] private Image nukeMinus;

        [SerializeField] private TextMeshProUGUI availableCash;
        [SerializeField] private TextMeshProUGUI totalPriceValue;
        [SerializeField] private TextMeshProUGUI finalCashValue;
        [SerializeField] private Image buyButton;

        [SerializeField] private float empFinalPrice;
        [SerializeField] private float sniperFinalPrice;
        [SerializeField] private float nukeFinalPrice;

        private float _empCountVal;
        private float _sniperCountVal;
        private float _nukeCountVal;

        private Button _empMinusBtn;
        private Button _sniperMinusBtn;
        private Button _nukeMinusBtn;
        private Button _empPlusBtn;
        private Button _sniperPlusBtn;
        private Button _nukePlusBtn;
        private Button _buyBtn;

        private AmmoManager _ammoManager;
        private CashManager _cashManager;

        private void Awake() {
            _empPlusBtn = empPlus.GetComponent<Button>();
            _empMinusBtn = empMinus.GetComponent<Button>();
            _sniperPlusBtn = sniperPlus.GetComponent<Button>();
            _sniperMinusBtn = sniperMinus.GetComponent<Button>();
            _nukePlusBtn = nukePlus.GetComponent<Button>();
            _nukeMinusBtn = nukeMinus.GetComponent<Button>();
            _buyBtn = buyButton.GetComponent<Button>();
        }


        private void Start() {
            var tank = GameObject.FindGameObjectWithTag("Player");
            _ammoManager = tank.GetComponent<AmmoManager>();
            _cashManager = tank.GetComponent<CashManager>();
        }


        private void Update() {
            SetFinalPrices();
            ValidateIncrementalButtons();
            ValidateDecrementalButtons();
            ValidateBuyBtn();
            UpdateCountAndValueFields();
        }

        private void SetFinalPrices() {
            empFinalPrice = _empCountVal * tankStatsSo.EmpShellStatsSo.Cost;
            sniperFinalPrice = _sniperCountVal * tankStatsSo.SniperShellStatsSo.Cost;
            nukeFinalPrice = _nukeCountVal * tankStatsSo.NukeShellStatsSo.Cost;
        }

        // prevent counters from going under 0
        private void ValidateDecrementalButtons() {
            _empMinusBtn.interactable = _empCountVal > 0;
            _sniperMinusBtn.interactable = _sniperCountVal > 0;
            _nukeMinusBtn.interactable = _nukeCountVal > 0;
        }

        // formula is ->  availableCash - (currTotalPrice + (relevantShellCost * 1))
        private void ValidateIncrementalButtons() {
            var currTotalPrice = empFinalPrice + sniperFinalPrice + nukeFinalPrice;
            _empPlusBtn.interactable = _cashManager.finalCash - (currTotalPrice + tankStatsSo.EmpShellStatsSo.Cost) >= 0;
            _sniperPlusBtn.interactable = _cashManager.finalCash - (currTotalPrice + tankStatsSo.SniperShellStatsSo.Cost) >= 0;
            _nukePlusBtn.interactable = _cashManager.finalCash - (currTotalPrice + tankStatsSo.NukeShellStatsSo.Cost) >= 0;
        }

        // buy button is interactable only when there is at least one item in the cart AND the total price is lower or equal to the total cash
        private void ValidateBuyBtn() {
            _buyBtn.interactable = _empCountVal + _sniperCountVal + _nukeCountVal > 0 && _cashManager.finalCash >= nukeFinalPrice + sniperFinalPrice + empFinalPrice;
        }

        private void UpdateCountAndValueFields() {
            var currTotalPrice = empFinalPrice + sniperFinalPrice + nukeFinalPrice;
            var currCash = Math.Round(_cashManager.finalCash, 2);

            empCount.text = $"{_empCountVal}";
            empValue.text = $"${empFinalPrice}";

            sniperCount.text = $"{_sniperCountVal}";
            sniperValue.text = $"${sniperFinalPrice}";

            nukeCount.text = $"{_nukeCountVal}";
            nukeValue.text = $"${nukeFinalPrice}";

            availableCash.text = $"{currCash}";
            totalPriceValue.text = $"{currTotalPrice}";
            finalCashValue.text = $"{currCash - currTotalPrice}";
        }

        // called in HandleServiceMenu.cs  
        public void ResetValues() {
            _empCountVal = 0;
            _sniperCountVal = 0;
            _nukeCountVal = 0;
        }


        // the below are used as unity events for the Plus, Minus and Buy buttons
        public void IncreaseEmpCount() => _empCountVal++;
        public void DecreaseEmpCount() => _empCountVal--;
        public void IncreaseSniperCount() => _sniperCountVal++;
        public void DecreaseSniperCount() => _sniperCountVal--;
        public void IncreaseNukeCount() => _nukeCountVal++;
        public void DecreaseNukeCount() => _nukeCountVal--;


        public void BuyItems() {
            _ammoManager.EmpShellAmmo += Parse(empCount.text);
            _ammoManager.SniperShellAmmo += Parse(sniperCount.text);
            _ammoManager.NukeShellAmmo += Parse(nukeCount.text);

            _cashManager.finalCash -= empFinalPrice + sniperFinalPrice + nukeFinalPrice;
            ResetValues();
        }
    }
}