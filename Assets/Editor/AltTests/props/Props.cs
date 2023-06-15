using System;
using System.Collections.Generic;
using Altom.AltDriver;
using UnityEngine;

namespace Editor.AltTests.props {
    public static class Props {
        #region Camera
        // NOTE: use negative for Zoom Out and positive for Zoom In
        public static void SetCamFinalOrthoZoom(AltDriver driver, float value) {
            var currZoomVal = CamCurrentOrthoZoom(driver);
            driver.FindObject(By.TAG, "Player").SetComponentProperty("Player.Controllers.CameraZoom", "finalZoomVal", currZoomVal - value, "Assembly-CSharp");
        }

        private static float CamCurrentOrthoZoom(AltDriver driver) {
            return driver.FindObject(By.TAG, "Player").GetComponentProperty<float>("Player.Controllers.CameraZoom", "currentZoomVal", "Assembly-CSharp", 1);
        }
        #endregion

        // TODO: change structure/hierarchy
        #region game objects
        // call SetActive of GameObject. //NOTE: set game object to active or inactive between tests
        public static void SetObjectsActive(IEnumerable<AltObject> altObjects, bool value) {
            foreach (var altObj in altObjects) {
                altObj.CallComponentMethod<bool>("UnityEngine.GameObject", "SetActive", "UnityEngine.CoreModule", new object[] { value }, new[] { "System.Boolean" });
            }
        }
        #endregion

        #region Progress Bars
        public static float HpShaderFillVal(AltDriver driver) {
            return driver.FindObject(By.TAG, "GameUI").GetComponentProperty<float>("Menus.ProgressBars", "currentHpValue", "Assembly-CSharp", 1);
        }

        public static Func<float> SpeedShaderFillVal(AltDriver driver) {
            return () => driver.FindObject(By.TAG, "GameUI").GetComponentProperty<float>("Menus.ProgressBars", "currentSpeedValue", "Assembly-CSharp", 1);
        }
        #endregion

        #region Tank Data
        //NOTE: returns the current speed as => new AltVector2(x, y) Where
        //  X == left (negative) or right (positive) rotation
        //  Y => == up (positive) or down (negative) movement
        public static AltVector2 TankLocalVelocity(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("Player.Controllers.TankController", "localVelocity", "Assembly-CSharp", 1);
            return ToAltV(v);
        }

        // get the alignment between (rightVector and direction), and (upVector and direction)
        // NOTE: result => new AltVector2(right vs dir, up vs dir)
        public static AltVector2 TankTargetAlignment(AltDriver driver, AltVector2 target) {
            var dirVector = ToV(TankDirTo(driver, target));
            var upVector = ToV(TankUpVector(driver));
            var rightVector = ToV(TankRightVector(driver));
            var alignX = Vector2.Dot(rightVector, dirVector);
            var alignY = Vector2.Dot(upVector, dirVector);

            return ToAltV(alignX, alignY);
        }

        public static float TankDistFrom(AltDriver driver, AltVector2 target) {
            return Vector3.Distance(ToV(TankPos(driver)), ToV(target));
        }

        private static AltVector2 TankUpVector(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("UnityEngine.Transform", "up", "UnityEngine.CoreModule", 1).normalized;
            return ToAltV(v);
        }

        private static AltVector2 TankRightVector(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetComponentProperty<Vector2>("UnityEngine.Transform", "right", "UnityEngine.CoreModule", 1).normalized;
            return ToAltV(v);
        }

        private static AltVector2 TankPos(AltDriver driver) {
            var v = driver.FindObject(By.TAG, "Player").GetWorldPosition();
            return ToAltV(v.x, v.y);
        }

        private static AltVector2 TankDirTo(AltDriver d, AltVector2 target) {
            var tank = ToV(TankPos(d));
            var t = ToV(target);
            return ToAltV((t - tank).normalized);
        }

        private static float TankSteerFactor(AltDriver driver) {
            return driver.FindObject(By.TAG, "Player").GetComponentProperty<float>("Player.Controllers.TankController", "steerFactor", "Assembly-CSharp", 1);
        }

        // get the total amount of time in seconds, needed for the tank to turn towards a target
        public static float TankRotDur(AltDriver driver, AltVector2 target) {
            var tankUp = ToV(TankUpVector(driver));
            var dir = ToV(TankDirTo(driver, target));
            var rads = Vector2.Angle(tankUp, dir) * Mathf.Deg2Rad;
            return rads / TankSteerFactor(driver);
        }
        #endregion

        #region Service Beacon Data
        public static AltVector2 ServicePos(AltDriver driver) {
            var p = driver.FindObject(By.NAME, "ServiceBeacon").GetWorldPosition();
            return ToAltV(p.x, p.y);
        }

        public static bool IsServiceMenuInteractable(AltDriver driver) {
            var canvas = driver.FindObject(By.PATH, "//BeaconsGrid/Beacons/ServiceBeacon/Canvas");
            return canvas.GetComponentProperty<bool>("UnityEngine.CanvasGroup", "interactable", "UnityEngine.UIModule");
        }

        public static Func<bool> ServiceMenuInteractableDelegate(AltDriver driver) {
            var canvas = driver.FindObject(By.PATH, "//BeaconsGrid/Beacons/ServiceBeacon/Canvas");
            return () => canvas.GetComponentProperty<bool>("UnityEngine.CanvasGroup", "interactable", "UnityEngine.UIModule");
        }
        #endregion

        #region Healing Beacon Data
        public static AltVector2 HealingPos(AltDriver driver) {
            var p = driver.FindObject(By.TAG, "HealingBeacon").GetWorldPosition();
            return ToAltV(p.x, p.y);
        }

        public static bool IsHealingBeaconActive(AltDriver driver) {
            return driver.FindObject(By.TAG, "HealingBeacon").GetComponentProperty<bool>("Beacons.HandleHpBeacon", "isOnBeacon", "Assembly-CSharp", 1);
        }

        public static Func<bool> HealingBeaconActiveDelegate(AltDriver driver) {
            return () => driver.FindObject(By.TAG, "HealingBeacon").GetComponentProperty<bool>("Beacons.HandleHpBeacon", "isOnBeacon", "Assembly-CSharp", 1);
        }
        #endregion

        #region Navigation Point Data
        public static AltVector2 NavPointPos(AltDriver driver, string objName) {
            var v = driver.FindObject(By.NAME, objName).GetWorldPosition();
            return ToAltV(v.x, v.y);
        }

        public static Func<bool> NavPointTriggerDelegate(AltDriver driver, string objName) {
            return () => driver.FindObject(By.NAME, objName).GetComponentProperty<bool>("AltTestRelated.NavPointHandler", "isTriggered", "Assembly-CSharp", 1);
        }

        public static bool IsNavPointTriggered(AltDriver driver, string objName) {
            return driver.FindObject(By.NAME, objName).GetComponentProperty<bool>("AltTestRelated.NavPointHandler", "isTriggered", "Assembly-CSharp", 1);
        }
        #endregion

        #region vector conversion methods
        private static AltVector2 ToAltV(Vector2 v) => new(v.x, v.y);
        private static AltVector2 ToAltV(float x, float y) => new(x, y);

        private static Vector2 ToV(AltVector2 v) => new(v.x, v.y);
        private static Vector2 ToV(float x, float y) => new(x, y);
        #endregion
    }
}