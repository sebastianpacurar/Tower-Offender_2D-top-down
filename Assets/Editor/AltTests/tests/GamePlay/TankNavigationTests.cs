using System.Collections.Generic;
using System.Linq;
using Altom.AltDriver;
using Editor.AltTests.gameObjects;
using Editor.AltTests.Helpers;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay {
    public class TankNavigationTests {
        private AltDriver _altDriver;
        private pages.GamePlay _gamePlayPage;

        private List<AltObject> _circlePoints;
        private List<AltObject> _squarePoints;
        private List<AltObject> _trianglePoints;

        private List<AltObject> _allPoints;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("NavigationScene");

            _circlePoints = _altDriver.FindObjects(By.TAG, "NavPointCircle").OrderBy(i => int.Parse(i.name[^1].ToString())).ToList();
            _squarePoints = _altDriver.FindObjects(By.TAG, "NavPointSquare").OrderBy(i => int.Parse(i.name[^1].ToString())).ToList();
            _trianglePoints = _altDriver.FindObjects(By.TAG, "NavPointTriangle").OrderBy(i => int.Parse(i.name[^1].ToString())).ToList();
            // _allPoints = _circlePoints.Concat(_squarePoints).Concat(_trianglePoints).ToList();
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        [SetUp]
        public void TestSetup() {
            _gamePlayPage = new pages.GamePlay(_altDriver);

            // set zoom out for every test. 
            _gamePlayPage.SetFinalOrthoZoom(-2.5f);
        }


        [Test, Order(1)]
        public void NavigateToSquarePoints() {
            // set only the square points active in the scene
            Globals.SetObjectsActive(_circlePoints, false);
            Globals.SetObjectsActive(_trianglePoints, false);
            Globals.SetObjectsActive(_squarePoints, true);

            PerformAssertions(_squarePoints);
        }

        [Test, Order(2)]
        public void NavigateToTrianglePoints() {
            // set only the triangle points active in the scene
            Globals.SetObjectsActive(_circlePoints, false);
            Globals.SetObjectsActive(_trianglePoints, true);
            Globals.SetObjectsActive(_squarePoints, false);

            PerformAssertions(_trianglePoints);
        }

        [Test, Order(3)]
        public void NavigateToCirclePoints() {
            // set only the circle points active in the scene
            Globals.SetObjectsActive(_circlePoints, true);
            Globals.SetObjectsActive(_trianglePoints, false);
            Globals.SetObjectsActive(_squarePoints, false);

            PerformAssertions(_circlePoints);
        }

        // verify if navigation point turns to green upon contact
        private void PerformAssertions(List<AltObject> targetPoints) {
            foreach (var altObj in targetPoints) {
                var gameObj = new NavPointObj(_altDriver, altObj.name);
                var shouldStop = altObj.name.Equals(targetPoints[^1].name);

                Assert.AreEqual(gameObj.IsTriggered(), false);
                _gamePlayPage.NavigateToLocation(gameObj.Pos(), gameObj.GetTriggerVal(), shouldStop);
                Assert.AreEqual(gameObj.IsTriggered(), true);
            }
        }
    }
}