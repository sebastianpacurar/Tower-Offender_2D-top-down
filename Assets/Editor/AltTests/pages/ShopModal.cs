using Altom.AltDriver;

namespace Editor.AltTests.pages {
    public class ShopModal : BasePage {
        public ShopModal(AltDriver driver) : base(driver) { }

        public AltObject PlusBtnEmp => Driver.WaitForObject(By.NAME, "PlusBtnEmp");
        public AltObject MinusBtnEmp => Driver.WaitForObject(By.NAME, "MinusBtnEmp");
        public AltObject SniperShellPlusBtn => Driver.WaitForObject(By.NAME, "PlusBtnSniper");
        public AltObject SniperShellMinusBtn => Driver.WaitForObject(By.NAME, "MinusBtnSniper");
        
    }
}