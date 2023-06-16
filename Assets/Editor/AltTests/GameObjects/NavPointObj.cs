using System;
using Altom.AltDriver;

namespace Editor.AltTests.gameObjects {
    public class NavPointObj : GameObj {
        public NavPointObj(AltDriver driver) : base(driver) { }

        #region Assembly Names (modules)
        private readonly string _cSharp = "Assembly-CSharp";
        #endregion

        # region Components
        private readonly string _navPointHandler = "AltTestRelated.NavPointHandler";
        #endregion

        #region Properties
        private readonly string _isTriggered = "isTriggered";
        #endregion

        public AltVector2 NavPointPos(AltDriver driver, string objName) {
            var v = driver.FindObject(By.NAME, objName).GetWorldPosition();
            return ToAltV(v.x, v.y);
        }

        public AltVector2 Pos(string name) => GetPos(By.NAME, name, Driver);

        public Func<bool> GetTriggerVal(string objName) {
            return () => Driver.FindObject(By.NAME, objName).GetComponentProperty<bool>(_navPointHandler, _isTriggered, _cSharp, 1);
        }

        public bool IsTriggered(string objName) {
            return Driver.FindObject(By.NAME, objName).GetComponentProperty<bool>(_navPointHandler, _isTriggered, _cSharp, 1);
        }
    }
}
// }        #region Navigation Point Data
// public static AltVector2 NavPointPos(AltDriver driver, string objName) {
//     var v = driver.FindObject(By.NAME, objName).GetWorldPosition();
//     return ToAltV(v.x, v.y);
// }
//

// #endregion