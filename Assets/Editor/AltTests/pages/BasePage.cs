using Altom.AltDriver;

namespace Editor.AltTests.pages {
    public class BasePage {
        public AltDriver Driver { get; set; }

        public BasePage(AltDriver driver) {
            Driver = driver;
        }
    }
}