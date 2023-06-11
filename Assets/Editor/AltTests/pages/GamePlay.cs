using System;
using Altom.AltDriver;
using Editor.AltTests.props;

namespace Editor.AltTests.pages {
    public class GamePlay : BasePage {
        public GamePlay(AltDriver driver) : base(driver) { }
        private bool _hasTurned;

        public void NavigateToLocation(AltVector2 location, Func<bool> conditionFunc) {
            while (true) {
                AltKeyCode activeKey = AltKeyCode.NoKey;
                AltKeyCode inactiveKey = AltKeyCode.NoKey;

                if (!_hasTurned) {
                    // Rotation Logic
                    while (true) {
                        var angX = Props.TankTargetAngles(Driver, location).x;
                        var angY = Props.TankTargetAngles(Driver, location).y;

                        if (angY > 0.8f) {
                            Driver.KeysUp(new[] { AltKeyCode.A, AltKeyCode.D });

                            if (angX < 0f) {
                                Driver.PressKey(AltKeyCode.D);
                            } else if (angX > 0f) {
                                Driver.PressKey(AltKeyCode.A);
                            }

                            _hasTurned = true;
                            break;
                        }

                        // calculate which way to rotate
                        if (angX >= 0 && angY >= -1) {
                            activeKey = AltKeyCode.D;
                            inactiveKey = AltKeyCode.A;
                        } else if (angX < 0 && angY >= -1) {
                            activeKey = AltKeyCode.A;
                            inactiveKey = AltKeyCode.D;
                        }

                        Driver.KeyDown(activeKey);
                        Driver.KeyUp(inactiveKey);
                    }
                } else {
                    // Move Forward Logic
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
                    }

                    // if stopped but condition not met, re-execute logic
                    if (!conditionFunc()) {
                        _hasTurned = false;
                        NavigateToLocation(location, conditionFunc);
                    }
                }
            }
        }

        private void StopTank() {
            while (Props.TankLocalVelocity(Driver).y > 0.1f) {
                Driver.KeyDown(AltKeyCode.S);
            }

            Driver.KeyUp(AltKeyCode.S);
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