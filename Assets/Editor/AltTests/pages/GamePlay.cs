using Altom.AltDriver;
using Editor.AltTests.props;


namespace Editor.AltTests.pages {
    public class GamePlay : BasePage {
        public GamePlay(AltDriver driver) : base(driver) { }
        private bool _hasTurned;

        // TODO: add check with angX if it misses the first rotation
        public void NavigateToLocation(AltVector2 location) {
            while (Props.TankDistFrom(Driver, location) > 2f) {
                RotateToFaceTarget(location);
                MoveUpTowardsTarget(location);
                StopTank();
            }
        }

        private void RotateToFaceTarget(AltVector2 target) {
            if (!_hasTurned) {
                while (true) {
                    var angX = Props.TankTargetAngles(Driver, target).x;
                    var angY = Props.TankTargetAngles(Driver, target).y;

                    if (angX >= 0 && angY >= -1) {
                        Driver.KeyUp(AltKeyCode.A);
                        Driver.KeyDown(AltKeyCode.D);
                    } else if (angX < 0 && angY >= -1) {
                        Driver.KeyUp(AltKeyCode.D);
                        Driver.KeyDown(AltKeyCode.A);
                    }

                    if (angY > 0.8f) {
                        break;
                    }
                }

                _hasTurned = true;
                Driver.KeysUp(new[] { AltKeyCode.A, AltKeyCode.D });
            }
        }

        private void MoveUpTowardsTarget(AltVector2 target) {
            if (_hasTurned) {
                while (Props.TankDistFrom(Driver, target) > 2f) {
                    Driver.KeyDown(AltKeyCode.W);
                }

                Driver.KeyUp(AltKeyCode.W);
            }
        }

        private void StopTank() {
            while (Props.TankLocalVelocity(Driver).y > 0.1f) {
                Driver.KeyDown(AltKeyCode.S);
            }

            Driver.KeyUp(AltKeyCode.S);
        }
    }
}