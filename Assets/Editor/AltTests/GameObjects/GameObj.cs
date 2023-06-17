using System.Collections.Generic;
using Altom.AltDriver;
using UnityEngine;

namespace Editor.AltTests.gameObjects {
    public abstract class GameObj {
        protected AltDriver Driver { get; }

        protected GameObj(AltDriver driver) {
            Driver = driver;
        }

        # region Assembly Names (modules)
        private readonly string _uEngineCoreModule = "UnityEngine.CoreModule";
        private readonly string _uEnginePhysicsModule = "UnityEngine.Physics2DModule";
        #endregion

        # region Components
        private readonly string _uEngineTransform = "UnityEngine.Transform";
        private readonly string _uEngineGameObject = "UnityEngine.GameObject";
        private readonly string _uEngineRigidbody = "UnityEngine.Rigidbody2D";
        #endregion

        #region Properties
        private readonly string _up = "up";
        private readonly string _right = "right";
        private readonly string _velocity = "velocity";
        #endregion

        #region Methods
        private readonly string _setActive = "SetActive";
        private readonly string _inverseTransformDir = "InverseTransformDirection";
        private readonly string _compareTag = "CompareTag";
        #endregion


        #region Primitives shortcuts
        private readonly string _boolType = typeof(bool).FullName;
        private readonly string _stringType = typeof(string).FullName;
        private readonly string _floatType = typeof(float).FullName;
        #endregion

        protected AltVector2 ToAltV(Vector2 v) => new(v.x, v.y);
        protected AltVector2 ToAltV(float x, float y) => new(x, y);

        protected Vector2 ToV(AltVector2 v) => new(v.x, v.y);
        protected Vector2 ToV(float x, float y) => new(x, y);


        protected AltVector2 GetPos(By by, string value, AltDriver driver) {
            var v = driver.FindObject(by, value).GetWorldPosition();
            return ToAltV(v.x, v.y);
        }

        //TODO: currently unused anywhere
        protected bool CompareTag(By by, string value, string targetTag, AltDriver driver) {
            var altObj = driver.FindObject(by, value);
            return altObj.CallComponentMethod<bool>(_uEngineGameObject, _compareTag, _uEngineCoreModule, new object[] { targetTag }, new[] { _stringType });
        }


        //NOTE: returns the current speed as => new AltVector2(x, y) Where
        //  X == left (negative) or right (positive) rotation
        //  Y => == up (positive) or down (negative) movement
        protected AltVector2 LocalVelocity(By by, string value, AltDriver driver) {
            var altObj = driver.FindObject(by, value);
            var rbVel = ToV(RbVelocityVector(altObj));

            var param = new object[] { rbVel.x, rbVel.y, 0f };
            var paramType = new[] { _floatType, _floatType, _floatType };

            var localVel = altObj.CallComponentMethod<Vector3>(_uEngineTransform, _inverseTransformDir, _uEngineCoreModule, param, paramType);
            return ToAltV(localVel.x, localVel.y);
        }


        protected AltVector2 UpVector(By by, string value, AltDriver driver) {
            var altObj = driver.FindObject(by, value);
            var vector = altObj.GetComponentProperty<Vector2>(_uEngineTransform, _up, _uEngineCoreModule, 1).normalized;
            return ToAltV(vector);
        }


        protected AltVector2 RightVector(By by, string value, AltDriver driver) {
            var altObj = driver.FindObject(by, value);
            var vector = altObj.GetComponentProperty<Vector2>(_uEngineTransform, _right, _uEngineCoreModule, 1).normalized;
            return ToAltV(vector);
        }


        private AltVector2 RbVelocityVector(AltObject altObj) {
            var vector = altObj.GetComponentProperty<Vector2>(_uEngineRigidbody, _velocity, _uEnginePhysicsModule, 2);
            return ToAltV(vector);
        }
    }
}