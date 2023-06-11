using Altom.AltDriver;
using Editor.AltTests.props;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay {
    public class TankMoveTests {
        private AltDriver _altDriver;
        private bool _hasTurned;

        private pages.GamePlay _gamePlayPage;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("DemoLevel");
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        [SetUp]
        public void TestSetup() {
            _gamePlayPage = new pages.GamePlay(_altDriver);
        }

        [Test]
        public void NavigateToServiceBeacon() {
            Assert.AreEqual(Props.IsServiceMenuInteractable(_altDriver), false);
            
            _gamePlayPage.NavigateToLocation(Props.ServicePos(_altDriver), Props.ServiceMenuInteractableDelegate(_altDriver));
            
            Assert.AreEqual(Props.IsServiceMenuInteractable(_altDriver), true);
        }
    }
}