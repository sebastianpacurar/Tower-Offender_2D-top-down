using Altom.AltDriver;
using UnityEngine;

namespace Editor.AltTests.props {
    public static class Props {
        #region Tank Data
        public static AltVector2 TankUpVector(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("UnityEngine.Transform", "up", "UnityEngine.CoreModule", 1).normalized;
            return ToAltV(v);
        }

        public static AltVector2 TankRightVector(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("UnityEngine.Transform", "right", "UnityEngine.CoreModule", 1).normalized;
            return ToAltV(v);
        }

        //NOTE: returns the current speed as => new AltVector2(x, y) Where
        //  X == left (negative) or right (positive) rotation
        //  Y => == up (positive) or down (negative) movement
        public static AltVector2 TankLocalVelocity(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("Player.Controllers.TankController", "localVelocity", "Assembly-CSharp", 1);
            return ToAltV(v);
        }

        // get the angle between (rightVector and direction), and (upVector and direction)
        // NOTE: result => new AltVector2(right vs dir, up vs dir)
        public static AltVector2 TankTargetAngles(AltDriver driver, AltVector2 target) {
            var dirVector = ToV(TankDirTo(driver, target));
            var upVector = ToV(TankUpVector(driver));
            var rightVector = ToV(TankRightVector(driver));
            var upAngle = Vector2.Dot(upVector, dirVector);
            var rightAngle = Vector2.Dot(rightVector, dirVector);

            return ToAltV(rightAngle, upAngle);
        }

        public static AltVector2 TankPos(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetWorldPosition();
            return ToAltV(v.x, v.y);
        }

        public static AltVector2 TankDirTo(AltDriver d, AltVector2 target) {
            var tank = ToV(TankPos(d));
            var t = ToV(target);
            return ToAltV((t - tank).normalized);
        }

        public static float TankDistFrom(AltDriver driver, AltVector2 target) {
            return Vector3.Distance(ToV(TankPos(driver)), ToV(target));
        }
        #endregion


        #region Service Beacon Data
        public static AltVector2 ServicePos(AltDriver driver) {
            var p = driver.FindObject(By.NAME, "ServiceBeacon").GetWorldPosition();
            return ToAltV(p.x, p.y);
        }

        public static bool GetServiceMenuInteractableStatus(AltDriver driver) {
            var canvas = driver.FindObject(By.PATH, "//BeaconsGrid/Beacons/ServiceBeacon/Canvas");
            return canvas.GetComponentProperty<bool>("UnityEngine.CanvasGroup", "interactable", "UnityEngine.UIModule");
        }
        #endregion


        // TODO: switch back to private after experiment over
        #region private methods
        public static AltVector2 ToAltV(Vector2 v) => new(v.x, v.y);
        public static AltVector2 ToAltV(float x, float y) => new(x, y);

        public static Vector2 ToV(AltVector2 v) => new(v.x, v.y);
        public static Vector2 ToV(float x, float y) => new(x, y);
        #endregion
    }
}