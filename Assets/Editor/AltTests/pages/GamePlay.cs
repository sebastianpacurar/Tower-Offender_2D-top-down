using System;
using Altom.AltDriver;
using Editor.AltTests.props;

namespace Editor.AltTests.pages {
    public class GamePlay : BasePage {
        public GamePlay(AltDriver driver) : base(driver) { }

        public void NavigateToLocation(AltVector2 location, Func<bool> conditionFunc, bool stopOnTarget = true) {
            var alignX = Props.TankTargetAlignment(Driver, location).x;
            var pressDur = Props.TankRotDur(Driver, location);

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

            // Move Forward Logic
            Driver.KeyDown(AltKeyCode.W);
            SpeedBoostTank(location);

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

            while (Props.TankLocalVelocity(Driver).y > -0.05f) {
                Driver.KeyDown(AltKeyCode.S);
            }

            Driver.KeyUp(AltKeyCode.S);
        }

        private void SpeedBoostTank(AltVector2 target) {
            var isMovingForward = Props.TankLocalVelocity(Driver).y > 0.1f;
            var dist = Props.TankDistFrom(Driver, target);

            if (isMovingForward && dist > 5f) {
                Driver.KeyDown(AltKeyCode.LeftShift);
            } else {
                Driver.KeyUp(AltKeyCode.LeftShift);
            }
        }
    }
}