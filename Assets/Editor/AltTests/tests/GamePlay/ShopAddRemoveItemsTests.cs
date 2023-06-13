using Altom.AltDriver;
using Editor.AltTests.pages;
using Editor.AltTests.props;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay {
    public class ShopAddRemoveItemsTests {
        private AltDriver _altDriver;
        private pages.GamePlay _gamePlayPage;
        private ShopModal _shopModal;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("NavigationScene");
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        [SetUp]
        public void TestSetup() {
            _gamePlayPage = new pages.GamePlay(_altDriver);
            _shopModal = new ShopModal(_altDriver);

            if (!Props.IsServiceMenuInteractable(_altDriver)) {
                NavigateToServicePos();
            }
        }

        [Test]
        public void AddRemoveEmpShell() {
            for (var i = 1; i <= 10; i++) AddEmpShell(_shopModal.GetCurrentCashTxt());
            for (var i = 1; i <= 10; i++) RemoveEmpShell(_shopModal.GetCurrentCashTxt());
        }

        [Test]
        public void AddRemoveSniperShell() {
            for (var i = 1; i <= 10; i++) AddSniperShell(_shopModal.GetCurrentCashTxt());
            for (var i = 1; i <= 10; i++) RemoveSniperShell(_shopModal.GetCurrentCashTxt());
        }

        [Test]
        public void AddRemoveNukeShell() {
            for (var i = 1; i <= 10; i++) AddNukeShell(_shopModal.GetCurrentCashTxt());
            for (var i = 1; i <= 10; i++) RemoveNukeShell(_shopModal.GetCurrentCashTxt());
        }


        private void NavigateToServicePos() {
            _gamePlayPage.NavigateToLocation(Props.ServicePos(_altDriver), Props.ServiceMenuInteractableDelegate(_altDriver));
            Assert.AreEqual(Props.IsServiceMenuInteractable(_altDriver), true);
        }


        #region Emp asserts
        private void AddEmpShell(float initialPlayerCash) {
            var initialEmpCount = _shopModal.GetCountTxtEmp();
            var empUnitPrice = _shopModal.GetLabelPriceEmp();

            _shopModal.PressPlusBtnEmp();

            var finalTotalPrice = _shopModal.GetFinalPriceTxt();
            var afterEmpCount = _shopModal.GetCountTxtEmp();
            var afterEmpPrice = _shopModal.GetPriceTxtEmp();

            Assert.AreEqual(finalTotalPrice, initialPlayerCash - afterEmpPrice, "final price vs total cost mismatch");
            Assert.AreEqual(afterEmpCount, initialEmpCount + 1, "Count mismatch");
            Assert.AreEqual(afterEmpPrice, empUnitPrice * afterEmpCount, "Price mismatch");
        }

        private void RemoveEmpShell(float initialPlayerCash) {
            var initialEmpCount = _shopModal.GetCountTxtEmp();
            var empUnitPrice = _shopModal.GetLabelPriceEmp();

            _shopModal.PressMinusBtnEmp();

            var finalTotalPrice = _shopModal.GetFinalPriceTxt();
            var afterEmpCount = _shopModal.GetCountTxtEmp();
            var afterEmpPrice = _shopModal.GetPriceTxtEmp();

            Assert.AreEqual(finalTotalPrice, initialPlayerCash - afterEmpPrice, "final price vs total cost mismatch");
            Assert.AreEqual(afterEmpCount, initialEmpCount - 1, "Count mismatch");
            Assert.AreEqual(afterEmpPrice, empUnitPrice * afterEmpCount, "Price mismatch");
        }
        #endregion

        # region Sniper asserts
        private void AddSniperShell(float initialPlayerCash) {
            var initialSniperCount = _shopModal.GetCountTxtSniper();
            var sniperUnitPrice = _shopModal.GetLabelPriceSniper();

            _shopModal.PressPlusBtnSniper();

            var finalTotalPrice = _shopModal.GetFinalPriceTxt();
            var afterSniperCount = _shopModal.GetCountTxtSniper();
            var afterSniperPrice = _shopModal.GetPriceTxtSniper();

            Assert.AreEqual(finalTotalPrice, initialPlayerCash - afterSniperPrice, "final price vs total cost mismatch");
            Assert.AreEqual(afterSniperCount, initialSniperCount + 1, "Count mismatch");
            Assert.AreEqual(afterSniperPrice, sniperUnitPrice * afterSniperCount, "Price mismatch");
        }

        private void RemoveSniperShell(float initialPlayerCash) {
            var initialSniperCount = _shopModal.GetCountTxtSniper();
            var sniperUnitPrice = _shopModal.GetLabelPriceSniper();

            _shopModal.PressMinusBtnSniper();

            var finalTotalPrice = _shopModal.GetFinalPriceTxt();
            var afterSniperCount = _shopModal.GetCountTxtSniper();
            var afterSniperPrice = _shopModal.GetPriceTxtSniper();

            Assert.AreEqual(finalTotalPrice, initialPlayerCash - afterSniperPrice, "final price vs total cost mismatch");
            Assert.AreEqual(afterSniperCount, initialSniperCount - 1, "Count mismatch");
            Assert.AreEqual(afterSniperPrice, sniperUnitPrice * afterSniperCount, "Price mismatch");
        }
        #endregion

        #region Nuke asserts
        private void AddNukeShell(float initialPlayerCash) {
            var initialNukeCount = _shopModal.GetCountTxtNuke();
            var nukeUnitPrice = _shopModal.GetLabelPriceNuke();

            _shopModal.PressPlusBtnNuke();

            var finalTotalPrice = _shopModal.GetFinalPriceTxt();
            var afterNukeCount = _shopModal.GetCountTxtNuke();
            var afterNukePrice = _shopModal.GetPriceTxtNuke();

            Assert.AreEqual(finalTotalPrice, initialPlayerCash - afterNukePrice, "final price vs total cost mismatch");
            Assert.AreEqual(afterNukeCount, initialNukeCount + 1, "Count mismatch");
            Assert.AreEqual(afterNukePrice, nukeUnitPrice * afterNukeCount, "Price mismatch");
        }

        private void RemoveNukeShell(float initialPlayerCash) {
            var initialNukeCount = _shopModal.GetCountTxtNuke();
            var nukeUnitPrice = _shopModal.GetLabelPriceNuke();

            _shopModal.PressMinusBtnNuke();

            var finalTotalPrice = _shopModal.GetFinalPriceTxt();
            var afterNukeCount = _shopModal.GetCountTxtNuke();
            var afterNukePrice = _shopModal.GetPriceTxtNuke();

            Assert.AreEqual(finalTotalPrice, initialPlayerCash - afterNukePrice, "final price vs total cost mismatch");
            Assert.AreEqual(afterNukeCount, initialNukeCount - 1, "Count mismatch");
            Assert.AreEqual(afterNukePrice, nukeUnitPrice * afterNukeCount, "Price mismatch");
        }
        #endregion
    }
}