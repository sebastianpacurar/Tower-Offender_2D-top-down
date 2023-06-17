using System;
using Altom.AltDriver;

namespace Editor.AltTests.gameObjects {
    public class NavPointObj : GameObj {
        private string Name { get; }

        public NavPointObj(AltDriver driver, string name) : base(driver) {
            Name = name;
        }

        #region Assembly Names (modules)
        private readonly string _cSharp = "Assembly-CSharp";
        #endregion

        # region Components
        private readonly string _navPointHandler = "AltTestRelated.NavPointHandler";
        #endregion

        #region Properties
        private readonly string _isTriggered = "isTriggered";
        #endregion

        public AltVector2 Pos() => GetPos(By.NAME, Name, Driver);
        public bool IsTag(string tagValue) => CompareTag(By.NAME, Name, tagValue, Driver);
        

        public Func<bool> GetTriggerVal() {
            return () => Driver.FindObject(By.NAME, Name).GetComponentProperty<bool>(_navPointHandler, _isTriggered, _cSharp, 1);
        }

        public bool IsTriggered() {
            return Driver.FindObject(By.NAME, Name).GetComponentProperty<bool>(_navPointHandler, _isTriggered, _cSharp, 1);
        }
    }
}