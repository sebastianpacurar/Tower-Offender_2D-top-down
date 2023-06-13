using Altom.AltDriver;

namespace Editor.AltTests.pages {
    public class ModalPage : BasePage {
        public ModalPage(AltDriver driver) : base(driver) { }

        #region AltObjects
        public AltObject ShopBtn => Driver.WaitForObject(By.NAME, "ShopSectionBtn");
        public AltObject UpgradesBtn => Driver.WaitForObject(By.NAME, "UpgradeSectionBtn");
        #endregion

        #region Actions
        public void PressShopBtn() => ShopBtn.Tap();
        public void PressUpgradesBtn() => UpgradesBtn.Tap();
        #endregion
    }
}