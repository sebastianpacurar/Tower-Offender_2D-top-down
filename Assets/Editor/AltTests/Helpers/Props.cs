using System;
using System.Collections.Generic;
using Altom.AltDriver;
using UnityEngine;

namespace Editor.AltTests.Helpers {
    public static class Globals {
        
        // call SetActive of GameObject. //NOTE: set game object to active or inactive between tests
        public static void SetObjectsActive(IEnumerable<AltObject> altObjects, bool value) {
            foreach (var altObj in altObjects) {
                altObj.CallComponentMethod<bool>("UnityEngine.GameObject", "SetActive", "UnityEngine.CoreModule", new object[] { value }, new[] { "System.Boolean" });
            }
        }

        
        //TODO: move everything below to their relevant GameObj type
        #region Progress Bars
        public static float HpShaderFillVal(AltDriver driver) {
            return driver.FindObject(By.TAG, "GameUI").GetComponentProperty<float>("Menus.ProgressBars", "currentHpValue", "Assembly-CSharp", 1);
        }

        public static Func<float> SpeedShaderFillVal(AltDriver driver) {
            return () => driver.FindObject(By.TAG, "GameUI").GetComponentProperty<float>("Menus.ProgressBars", "currentSpeedValue", "Assembly-CSharp", 1);
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
        

        #region vector conversion methods
        private static AltVector2 ToAltV(Vector2 v) => new(v.x, v.y);
        private static AltVector2 ToAltV(float x, float y) => new(x, y);

        private static Vector2 ToV(AltVector2 v) => new(v.x, v.y);
        private static Vector2 ToV(float x, float y) => new(x, y);
        #endregion
    }
}