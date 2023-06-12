using Altom.AltDriver;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay {
    public class ServiceBeaconShop {
        private AltDriver _altDriver;
        private pages.GamePlay _gamePlayPage;
        private bool _hasTurned;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("DemoLevel");
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        // [Test]
        // public void BuyEmpShell() { }
        //
        // [Test]
        // public void BuySniperShell() { }
        //
        // [Test]
        // public void BuyNukeShell() { }
    }
}