using Altom.AltDriver;
using UnityEngine;

namespace Editor.AltTests.props {
    public static class Props {
        #region Tank Data
        public static AltVector2 TankUpVelocity(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("Player.Controllers.TankController", "forwardVelocity", "Assembly-CSharp", 1);
            return ToAltV(v);
        }

        public static float TankUpSpeed(AltDriver driver) {
            return driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("Player.Controllers.TankController", "forwardVelocity", "Assembly-CSharp", 1).magnitude;
        }

        public static AltVector2 TankUp(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("UnityEngine.Transform", "up", "UnityEngine.CoreModule", 1).normalized;
            return ToAltV(v);
        }

        public static AltVector2 TankRightVelocity(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("Player.Controllers.TankController", "rightVelocity", "Assembly-CSharp", 1);
            return ToAltV(v);
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

        // if 1 then facing target, if -1 then target is behind tank
        public static float TankFacingTargetRatio(AltDriver driver, AltVector2 target) {
            var dir = ToV(TankDirTo(driver, target));
            var velocity = ToV(TankUp(driver));
            return Vector2.Dot(dir, velocity);
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

        #region Props Debug
        public static void LogProps(AltDriver driver) {
            var tankPos = TankPos(driver);
            var tankUp = TankUp(driver);
            var tankUpV = TankUpVelocity(driver);
            var tankUpSpeed = TankUpSpeed(driver);
            var tankRightV = TankRightVelocity(driver);
            var servicePos = ServicePos(driver);
            var interactableMenuStatus = GetServiceMenuInteractableStatus(driver);
            Debug.Log($"Tank Pos: ({tankPos.x}, {tankPos.y}");
            Debug.Log($"Tank Up:  ({tankUp.x},{tankUp.y}) ");
            Debug.Log($"Service Pos: ({servicePos.x}, {servicePos.y})");
            Debug.Log($"Distance From Service: {TankDistFrom(driver, ServicePos(driver))}");
            Debug.Log($"Facing Direction Ratio: {TankFacingTargetRatio(driver, servicePos)}");
            Debug.Log($"Tank Up Velocity: ({tankUpV.x}, {tankUpV.y}) | Tank Right Velocity: ({tankRightV.x}, {tankRightV.y})");
            Debug.Log($"Tank Up Speed: {tankUpSpeed}");
            Debug.Log($"Is Menu Active: {interactableMenuStatus}");
        }
        #endregion


        #region private methods
        private static AltVector2 ToAltV(Vector2 v) => new(v.x, v.y);
        private static AltVector2 ToAltV(float x, float y) => new(x, y);

        private static Vector2 ToV(AltVector2 v) => new(v.x, v.y);
        private static Vector2 ToV(float x, float y) => new(x, y);
        #endregion
    }
}