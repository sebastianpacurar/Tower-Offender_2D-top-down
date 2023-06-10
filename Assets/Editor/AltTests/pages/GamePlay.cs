using Altom.AltDriver;
using Editor.AltTests.props;

namespace Editor.AltTests.pages {
    public class GamePlay : BasePage {
        public GamePlay(AltDriver driver) : base(driver) { }
        private bool _hasTurned;

        public void NavigateToLocation(AltVector2 location) {
            while (Props.TankDistFrom(Driver, location) > 0.5f) {
                PerformNavigationTo(location);
            }
        }

        private void PerformNavigationTo(AltVector2 target) {
            AltKeyCode activeKey = AltKeyCode.NoKey;
            AltKeyCode inactiveKey = AltKeyCode.NoKey;

            if (!_hasTurned) {
                while (true) {
                    var angX = Props.TankTargetAngles(Driver, target).x;
                    var angY = Props.TankTargetAngles(Driver, target).y;

                    if (angX >= 0 && angY >= -1) {
                        activeKey = AltKeyCode.D;
                        inactiveKey = AltKeyCode.A;
                    } else if (angX < 0 && angY >= -1) {
                        activeKey = AltKeyCode.A;
                        inactiveKey = AltKeyCode.D;
                    }

                    Driver.KeyDown(activeKey);
                    Driver.KeyUp(inactiveKey);


                    if (angY > 0.8f) {
                        Driver.KeysUp(new[] { AltKeyCode.A, AltKeyCode.D });

                        if (angX > 0f) {
                            Driver.PressKey(AltKeyCode.D);
                        } else if (angX > 0f) {
                            Driver.PressKey(AltKeyCode.A);
                        }

                        _hasTurned = true;
                        break;
                    }
                }

                if (_hasTurned) {
                    while (true) {
                        var dist = Props.TankDistFrom(Driver, target);
                        var angX = Props.TankTargetAngles(Driver, target).x;
                        var angY = Props.TankTargetAngles(Driver, target).y;

                        if (angX < 0.8f && angY > 0f && !MovingUpTowards(target) || (dist < 2f)) {
                            Driver.KeyUp(AltKeyCode.W);
                            StopTank();
                            break;
                        }

                        Driver.KeyDown(AltKeyCode.W);
                    }


                    if (Props.TankDistFrom(Driver, target) > 2f) {
                        _hasTurned = false;
                        PerformNavigationTo(target);
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

        // TODO: experimental
        private bool MovingUpTowards(AltVector2 target) {
            var upAngle = Props.TankTargetAngles(Driver, target).y;
            var dist = Props.TankDistFrom(Driver, target);
            var val = upAngle * dist;
            return val > 0f;
        }
    }
}