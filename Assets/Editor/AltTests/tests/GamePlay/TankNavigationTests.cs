using System.Linq;
using Altom.AltDriver;
using Editor.AltTests.props;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay {
    public class TankNavigationTests {
        private AltDriver _altDriver;
        private pages.GamePlay _gamePlayPage;

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
        }

        [Test]
        public void NavigateToServiceBeacon() {
            Assert.AreEqual(Props.IsServiceMenuInteractable(_altDriver), false);
            _gamePlayPage.NavigateToLocation(Props.ServicePos(_altDriver), Props.ServiceMenuInteractableDelegate(_altDriver));
            Assert.AreEqual(Props.IsServiceMenuInteractable(_altDriver), true);
        }

        [Test]
        public void NavigateToHealingBeacon() {
            Assert.AreEqual(Props.IsHealingBeaconActive(_altDriver), false);
            _gamePlayPage.NavigateToLocation(Props.HealingPos(_altDriver), Props.HealingBeaconActiveDelegate(_altDriver));
            Assert.AreEqual(Props.IsHealingBeaconActive(_altDriver), true);
        }

        [Test]
        public void NavigateToPoints() {
            // get the target points, sorted based on the final char of their name (TODO doesn't seem to work...)
            var navPoints = _altDriver.FindObjectsWhichContain(By.TAG, "NavigationPoint").OrderBy(i => int.Parse(i.name[^1].ToString())).ToList();

            foreach (var obj in navPoints) {
                var name = obj.name;
                var pos = Props.NavPointPos(_altDriver, name);
                var isLast = obj.name.Equals(navPoints[^1].name);

                Assert.AreEqual(Props.IsNavPointTriggered(_altDriver, name), false);
                _gamePlayPage.NavigateToLocation(pos, Props.NavPointTriggerDelegate(_altDriver, name), isLast);
                Assert.AreEqual(Props.IsNavPointTriggered(_altDriver, name), true);
            }
        }
    }
}