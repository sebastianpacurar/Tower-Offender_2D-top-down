using Altom.AltDriver;
using Editor.AltTests.props;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay.Purchasables {
    public class ServiceBeaconShop {
        private AltDriver _altDriver;
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

        [Test]
        public void NavigateToServiceBeacon() {
            Assert.AreEqual(Props.GetServiceMenuInteractableStatus(_altDriver), false);

            while (Props.TankDistFrom(_altDriver, Props.ServicePos(_altDriver)) > 4f) {
                // Props.LogProps(_altDriver);
                MoveHorizontally();
                MoveVertically();
            }

            while (Props.TankUpSpeed(_altDriver) >= 1f) {
                _altDriver.KeyDown(AltKeyCode.S);
            }

            _altDriver.KeyUp(AltKeyCode.S);

            Assert.AreEqual(Props.GetServiceMenuInteractableStatus(_altDriver), true);
        }

        private void MoveHorizontally() {
            if (!_hasTurned) {
                if (Props.TankFacingTargetRatio(_altDriver, Props.ServicePos(_altDriver)) < 0.8f) {
                    _altDriver.KeyDown(AltKeyCode.A);
                } else if (Props.TankFacingTargetRatio(_altDriver, Props.ServicePos(_altDriver)) > 0.8f) {
                    _altDriver.KeyUp(AltKeyCode.A);
                    _hasTurned = true;
                }
            }
        }

        private void MoveVertically() {
            if (_hasTurned) {
                var isInFront = Props.TankFacingTargetRatio(_altDriver, Props.ServicePos(_altDriver)) > 0f;

                if (Props.TankDistFrom(_altDriver, Props.ServicePos(_altDriver)) > 4.25f) {
                    _altDriver.KeyDown(isInFront ? AltKeyCode.W : AltKeyCode.S);
                } else {
                    _altDriver.KeyUp(isInFront ? AltKeyCode.W : AltKeyCode.S);
                }
            }
        }
    }
}