using System.Collections.Generic;
using System.Linq;
using Altom.AltDriver;
using Editor.AltTests.props;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay {
    public class TankNavigationTests {
        private AltDriver _altDriver;
        private pages.GamePlay _gamePlayPage;

        private List<AltObject> _circlePoints;
        private List<AltObject> _squarePoints;
        private List<AltObject> _trianglePoints;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("NavigationScene");

            _circlePoints = _altDriver.FindObjects(By.TAG, "NavPointCircle").OrderBy(i => int.Parse(i.name[^1].ToString())).ToList();
            _squarePoints = _altDriver.FindObjects(By.TAG, "NavPointSquare").OrderBy(i => int.Parse(i.name[^1].ToString())).ToList();
            _trianglePoints = _altDriver.FindObjects(By.TAG, "NavPointTriangle").OrderBy(i => int.Parse(i.name[^1].ToString())).ToList();


            Props.SetCamFinalOrthoZoom(_altDriver, -5);
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        [SetUp]
        public void TestSetup() {
            _gamePlayPage = new pages.GamePlay(_altDriver);
        }


        [Test, Order(1)]
        public void NavigateToSquarePoints() {
            // set only the square points active in the scene
            Props.SetObjectsActive(_circlePoints, false);
            Props.SetObjectsActive(_trianglePoints, false);
            Props.SetObjectsActive(_squarePoints, true);

            PerformAssertions(_squarePoints);
        }

        [Test, Order(2)]
        public void NavigateToTrianglePoints() {
            // set only the triangle points active in the scene
            Props.SetObjectsActive(_circlePoints, false);
            Props.SetObjectsActive(_trianglePoints, true);
            Props.SetObjectsActive(_squarePoints, false);

            PerformAssertions(_trianglePoints);
        }

        [Test, Order(3)]
        public void NavigateToCirclePoints() {
            // set only the circle points active in the scene
            Props.SetObjectsActive(_circlePoints, true);
            Props.SetObjectsActive(_trianglePoints, false);
            Props.SetObjectsActive(_squarePoints, false);

            PerformAssertions(_circlePoints);
        }

        // verify if navigation point turns to green upon contact
        private void PerformAssertions(List<AltObject> targetPoints) {
            foreach (var obj in targetPoints) {
                var name = obj.name;
                var pos = Props.NavPointPos(_altDriver, name);
                var isLast = obj.name.Equals(targetPoints[^1].name);

                Assert.AreEqual(Props.IsNavPointTriggered(_altDriver, name), false);
                _gamePlayPage.NavigateToLocation(pos, Props.NavPointTriggerDelegate(_altDriver, name), isLast);
                Assert.AreEqual(Props.IsNavPointTriggered(_altDriver, name), true);
            }
        }
    }
}