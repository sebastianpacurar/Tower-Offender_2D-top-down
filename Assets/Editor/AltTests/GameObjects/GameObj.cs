using System.Collections.Generic;
using Altom.AltDriver;
using UnityEngine;

namespace Editor.AltTests.gameObjects {
    public abstract class GameObj {
        protected AltDriver Driver { get; set; }

        protected GameObj(AltDriver driver) {
            Driver = driver;
        }

        # region Assembly Names (modules)
        private readonly string _uEngineCore = "UnityEngine.CoreModule";
        private readonly string _uEnginePhysicsModule = "UnityEngine.Physics2DModule";
        #endregion

        # region Components
        private readonly string _uEngineTransform = "UnityEngine.Transform";
        private readonly string _uEngineGameObject = "UnityEngine.GameObject";
        private readonly string _uEngineRigidbody = "UnityEngine.Rigidbody2D";
        private readonly string _uEngineVector3 = "UnityEngine.Vector3";
        #endregion

        #region Properties
        private readonly string _up = "up";
        private readonly string _right = "right";
        private readonly string _velocity = "velocity";
        #endregion

        #region Methods
        private readonly string _SetActive = "SetActive";
        private readonly string _InverseTransformDir = "InverseTransformDirection";
        #endregion

        protected AltVector2 ToAltV(Vector2 v) => new(v.x, v.y);
        protected AltVector2 ToAltV(float x, float y) => new(x, y);

        protected Vector2 ToV(AltVector2 v) => new(v.x, v.y);
        protected Vector2 ToV(float x, float y) => new(x, y);

        protected AltVector2 GetPos(By by, string value, AltDriver driver) {
            var v = driver.FindObject(by, value).GetWorldPosition();
            return ToAltV(v.x, v.y);
        }

        protected void SetObjectActive(AltObject altObj, bool value) {
            altObj.CallComponentMethod<bool>(_uEngineGameObject, _SetActive, _uEngineCore, new object[] { value }, new[] { "System.Boolean" });
        }

        protected void SetObjectsActive(IEnumerable<AltObject> altObjects, bool value) {
            foreach (var altObj in altObjects) {
                altObj.CallComponentMethod<bool>(_uEngineGameObject, _SetActive, _uEngineCore, new object[] { value }, new[] { "System.Boolean" });
            }
        }

        // not working???
        //NOTE: returns the current speed as => new AltVector2(x, y) Where
        //  X == left (negative) or right (positive) rotation
        //  Y => == up (positive) or down (negative) movement
        protected AltVector2 LocalVelocity(By by, string value, AltDriver driver) {
            var altObj = driver.FindObject(by, value);
            var rbVel = ToV(RbVelocityVector(altObj));
            var rbVel3V = new Vector3(rbVel.x, rbVel.y, 0f);
            var localVel = altObj.CallComponentMethod<Vector3>(_uEngineTransform, _InverseTransformDir, _uEngineCore, new object[] { rbVel3V }, new[] { "UnityEngine.Vector3" });
            return ToAltV(localVel.x, localVel.y);
        }

        protected AltVector2 UpVector(By by, string value, AltDriver driver) {
            var altObj = driver.FindObject(by, value);
            var vector = altObj.GetComponentProperty<Vector2>(_uEngineTransform, _up, _uEngineCore, 1).normalized;
            return ToAltV(vector);
        }

        protected AltVector2 RightVector(By by, string value, AltDriver driver) {
            var altObj = driver.FindObject(by, value);
            var vector = altObj.GetComponentProperty<Vector2>(_uEngineTransform, _right, _uEngineCore, 1).normalized;
            return ToAltV(vector);
        }

        private AltVector2 RbVelocityVector(AltObject altObj) {
            var vector = altObj.GetComponentProperty<Vector2>(_uEngineRigidbody, _velocity, _uEnginePhysicsModule, 2);
            return ToAltV(vector);
        }
    }
}