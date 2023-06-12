using System;
using Altom.AltDriver;
using Editor.AltTests.props;

namespace Editor.AltTests.pages {
    public class GamePlay : BasePage {
        public GamePlay(AltDriver driver) : base(driver) { }
        private bool _hasTurned;

        public void NavigateToLocation(AltVector2 location, Func<bool> conditionFunc) {
            while (true) {
                var activeKey = AltKeyCode.NoKey;
                var inactiveKey = AltKeyCode.NoKey;

                if (!_hasTurned) {
                    // Rotation Logic
                    while (true) {
                        var angX = Props.TankTargetAngles(Driver, location).x;
                        var angY = Props.TankTargetAngles(Driver, location).y;

                        // exit condition if facing target
                        // angY == 1.0f means facing target in a straight line
                        if (angY > 0.8f) {
                            Driver.KeysUp(new[] { AltKeyCode.A, AltKeyCode.D });

                            // if angY offset is smaller than 0.95f
                            // if angX < 0f turn left to reach 0f else if angX > 0f turn right to reach 0f
                            switch (angX) {
                                case < 0f when angY < 0.925f:
                                    Driver.PressKey(AltKeyCode.A, DurBasedOnDist(location));
                                    break;
                                case > 0f when angY < 0.925f:
                                    Driver.PressKey(AltKeyCode.D, DurBasedOnDist(location));
                                    break;
                            }

                            _hasTurned = true;
                            break;
                        }

                        // calculate which way to rotate
                        switch (angX) {
                            // left
                            case < 0 when angY >= -1:
                                activeKey = AltKeyCode.A;
                                inactiveKey = AltKeyCode.D;
                                break;
                            // right
                            case >= 0 when angY >= -1:
                                activeKey = AltKeyCode.D;
                                inactiveKey = AltKeyCode.A;
                                break;
                        }

                        // perform rotation
                        Driver.KeyDown(activeKey);
                        Driver.KeyUp(inactiveKey);
                    }
                } else {
                    // if _hasTurned == true => Move Forward Logic
                    while (true) {
                        var dist = Props.TankDistFrom(Driver, location);
                        var angX = Props.TankTargetAngles(Driver, location).x;
                        var angY = Props.TankTargetAngles(Driver, location).y;

                        // exit when condition met
                        if (conditionFunc()) {
                            Driver.KeyUp(AltKeyCode.W);
                            StopTank();
                            return;
                        }

                        if ((angX < 0.8f && angY < 0f && !MovingUpTowards(location)) || dist < 2f) {
                            Driver.KeyUp(AltKeyCode.W);
                            StopTank();
                            break;
                        }

                        Driver.KeyDown(AltKeyCode.W);
                        SpeedBoostTank(location);
                    }

                    _hasTurned = false;
                    NavigateToLocation(location, conditionFunc);
                }
            }
        }

        private void StopTank() {
            while (Props.TankLocalVelocity(Driver).y > -0.2f) {
                Driver.KeyDown(AltKeyCode.S);
            }

            Driver.KeyUp(AltKeyCode.S);
        }

        private void SpeedBoostTank(AltVector2 target) {
            var isMovingForward = Props.TankLocalVelocity(Driver).y > 0.1f;
            var dist = Props.TankDistFrom(Driver, target);

            if (isMovingForward && dist > 12.5f) {
                Driver.KeyDown(AltKeyCode.LeftShift);
            } else {
                Driver.KeyUp(AltKeyCode.LeftShift);
            }
        }

        // TODO: needs more tweaking
        private float DurBasedOnDist(AltVector2 target) {
            return Props.TankDistFrom(Driver, target) switch {
                > 15f => 0.01f,
                > 10f and < 15f => 0.025f,
                > 5f and < 10f => 0.05f,
                > 0f and < 5f => 1f,
                _ => 0f
            };
        }

        // TODO: seems faulty
        private bool MovingUpTowards(AltVector2 target) {
            var upAngle = Props.TankTargetAngles(Driver, target).y;
            var dist = Props.TankDistFrom(Driver, target);
            var val = upAngle * dist;
            return val > 0f;
        }
    }
}