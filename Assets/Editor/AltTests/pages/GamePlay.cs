using System;
using Altom.AltDriver;
using Editor.AltTests.gameObjects;

namespace Editor.AltTests.pages {
    public class GamePlay : BasePage {
        public TankObj Tank { get; }

        public GamePlay(AltDriver driver) : base(driver) {
            Tank = new TankObj(driver);
        }

        public void NavigateToLocation(AltVector2 location, Func<bool> conditionFunc, bool stopOnTarget = true) {
            var alignX = Tank.VectorAlignment(location).x;
            var pressDur = Tank.RotationDuration(location);

            // Rotation Logic
            switch (alignX) {
                // left
                case < 0:
                    Driver.PressKey(AltKeyCode.A, duration: pressDur, wait: true);
                    break;
                // right
                case >= 0:
                    Driver.PressKey(AltKeyCode.D, duration: pressDur, wait: true);
                    break;
            }

            Accelerate(location, stopOnTarget);
            SpeedBoostTank(location, stopOnTarget);

            // exit conditions
            if (!conditionFunc()) {
                // execute Rotation again to maintain a straight line 
                NavigateToLocation(location, conditionFunc, stopOnTarget);
            } else if (conditionFunc() && stopOnTarget) {
                StopTank(); // stop when target reached and stop=true
            }
        }

        private void StopTank() {
            Driver.KeyUp(AltKeyCode.W);

            while (Tank.LocalVel().y > -0.05f) {
                Driver.KeyDown(AltKeyCode.S);
            }

            Driver.KeyUp(AltKeyCode.S);
        }

        private void Accelerate(AltVector2 location, bool stopOnTarget) {
            if (stopOnTarget && Tank.DistanceFrom(location) < 2.5f && Tank.LocalVel().y > 2f) {
                Driver.KeyUp(AltKeyCode.W);
            } else {
                Driver.KeyDown(AltKeyCode.W);
            }
        }

        private void SpeedBoostTank(AltVector2 location, bool stopOnTarget) {
            var isMovingForward = Tank.LocalVel().y > 0.1f;
            var dist = Tank.DistanceFrom(location);

            if (isMovingForward && !stopOnTarget) {
                Driver.KeyDown(AltKeyCode.LeftShift);
            } else if (dist > 6.5f) {
                Driver.KeyUp(AltKeyCode.LeftShift);
            }
        }
    }
}