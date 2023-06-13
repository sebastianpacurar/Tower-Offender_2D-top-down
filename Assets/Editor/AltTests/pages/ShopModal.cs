using System.Text.RegularExpressions;
using Altom.AltDriver;

namespace Editor.AltTests.pages {
    public class ShopModal : ModalPage {
        public ShopModal(AltDriver driver) : base(driver) { }

        #region AltObjects
        private AltObject ItemLabelEmp => Driver.WaitForObject(By.NAME, "EmpLabelAndPriceTxt");
        private AltObject PlusBtnEmp => Driver.WaitForObject(By.NAME, "PlusBtnEmp");
        private AltObject CountTxtEmp => Driver.WaitForObject(By.NAME, "EmpCountTxt");
        private AltObject MinusBtnEmp => Driver.WaitForObject(By.NAME, "MinusBtnEmp");
        private AltObject PriceTxtEmp => Driver.WaitForObject(By.NAME, "EmpPrice");

        private AltObject ItemLabelSniper => Driver.WaitForObject(By.NAME, "SniperLabelAndPriceTxt");
        private AltObject PlusBtnSniper => Driver.WaitForObject(By.NAME, "PlusBtnSniper");
        private AltObject CountTxtSniper => Driver.WaitForObject(By.NAME, "SniperCountTxt");
        private AltObject MinusBtnSniper => Driver.WaitForObject(By.NAME, "MinusBtnSniper");
        private AltObject PriceTxtSniper => Driver.WaitForObject(By.NAME, "SniperPrice");

        private AltObject ItemLabelNuke => Driver.WaitForObject(By.NAME, "NukeLabelAndPriceTxt");
        private AltObject PlusBtnNuke => Driver.WaitForObject(By.NAME, "PlusBtnNuke");
        private AltObject CountTxtNuke => Driver.WaitForObject(By.NAME, "NukeCountTxt");
        private AltObject MinusBtnNuke => Driver.WaitForObject(By.NAME, "MinusBtnNuke");
        private AltObject PriceTxtNuke => Driver.WaitForObject(By.NAME, "NukePrice");

        private AltObject CurrentCashTxt => Driver.WaitForObject(By.NAME, "CurrentCash");
        private AltObject TotalPriceTxt => Driver.WaitForObject(By.NAME, "TotalCash");
        private AltObject FinalPriceTxt => Driver.WaitForObject(By.NAME, "FinalCash");

        private AltObject BuyBtn => Driver.WaitForObject(By.NAME, "ShopBuyBtn");
        #endregion

        #region Actions
        public int GetLabelPriceEmp() => ExtractInt(ItemLabelEmp.GetText());
        public void PressPlusBtnEmp() => PlusBtnEmp.Tap();
        public int GetCountTxtEmp() => int.Parse(CountTxtEmp.GetText());
        public void PressMinusBtnEmp() => MinusBtnEmp.Tap();
        public int GetPriceTxtEmp() => int.Parse(PriceTxtEmp.GetText()[1..]);

        public int GetLabelPriceSniper() => ExtractInt(ItemLabelSniper.GetText());
        public void PressPlusBtnSniper() => PlusBtnSniper.Tap();
        public int GetCountTxtSniper() => int.Parse(CountTxtSniper.GetText());
        public void PressMinusBtnSniper() => MinusBtnSniper.Tap();
        public int GetPriceTxtSniper() => int.Parse(PriceTxtSniper.GetText()[1..]);

        public int GetLabelPriceNuke() => ExtractInt(ItemLabelNuke.GetText());
        public void PressPlusBtnNuke() => PlusBtnNuke.Tap();
        public int GetCountTxtNuke() => int.Parse(CountTxtNuke.GetText());
        public void PressMinusBtnNuke() => MinusBtnNuke.Tap();
        public int GetPriceTxtNuke() => int.Parse(PriceTxtNuke.GetText()[1..]);

        public float GetCurrentCashTxt() => float.Parse(CurrentCashTxt.GetText());
        public float GetTotalPriceTxt() => float.Parse(TotalPriceTxt.GetText());
        public float GetFinalPriceTxt() => float.Parse(FinalPriceTxt.GetText());

        public void PressBuyBtn() => BuyBtn.Tap();
        #endregion

        private int ExtractInt(string text) {
            var match = Regex.Match(text, @"\d+");
            var res = 0;

            if (Regex.Match(text, @"\d+").Success) {
                res = int.Parse(match.Value);
            }

            return res;
        }
    }
}