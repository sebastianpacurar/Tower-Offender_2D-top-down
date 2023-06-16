using Altom.AltDriver;
using UnityEngine;

namespace Editor.AltTests.gameObjects {
    public class TankObj : GameObj {
        public TankObj(AltDriver driver) : base(driver) { }
        private readonly string _playerTag = "Player";

        #region Assembly Names (modules)
        private readonly string _cSharp = "Assembly-CSharp";
        #endregion

        #region Components
        private readonly string _controller = "Player.Controllers.TankController";
        #endregion

        #region Properties
        private readonly string _steerFactor = "steerFactor";
        #endregion

        public AltVector2 VectorAlignment(AltVector2 target) {
            var dirVector = ToV(DirectionTo(target));
            var upVector = ToV(Up());
            var rightVector = ToV(Right());
            var alignX = Vector2.Dot(rightVector, dirVector);
            var alignY = Vector2.Dot(upVector, dirVector);

            return ToAltV(alignX, alignY);
        }

        public AltVector2 LocalVel() => LocalVelocity(By.TAG, _playerTag, Driver);

        // get the total amount of time in seconds, needed for the tank to turn towards a target
        public float RotationDuration(AltVector2 target) {
            var tankUp = ToV(Up());
            var dir = ToV(DirectionTo(target));
            var rads = Vector2.Angle(tankUp, dir) * Mathf.Deg2Rad;
            return rads / SteerFactor();
        }

        public float DistanceFrom(AltVector2 target) => Vector3.Distance(ToV(Pos()), ToV(target));

        private AltVector2 DirectionTo(AltVector2 target) {
            var tankPosVector = ToV(Pos());
            var targetPosVector = ToV(target);
            return ToAltV((targetPosVector - tankPosVector).normalized);
        }

        private float SteerFactor() {
            return Driver.FindObject(By.TAG, "Player").GetComponentProperty<float>(_controller, "steerFactor", _cSharp, 1);
        }

        private AltVector2 Pos() => GetPos(By.TAG, _playerTag, Driver);
        private AltVector2 Up() => UpVector(By.TAG, _playerTag, Driver);
        private AltVector2 Right() => RightVector(By.TAG, _playerTag, Driver);
    }
}