using System;
using Altom.AltDriver;

namespace Editor.AltTests.gameObjects {
    public class ServiceObj : GameObj {
        public ServiceObj(AltDriver driver) : base(driver) { }

        private readonly string _serviceModalPath = "//BeaconsGrid/Beacons/ServiceBeacon/Canvas";

        #region Assembly Names (modules)
        private readonly string _uEngineUiModule = "UnityEngine.UIModule";
        #endregion

        #region Components
        private readonly string _uEngineCanvasGroup = "UnityEngine.CanvasGroup";
        #endregion

        #region Properties
        private readonly string _interactable = "interactable";
        #endregion

        public bool IsInteractable() {
            var canvas = Driver.FindObject(By.PATH, _serviceModalPath);
            return canvas.GetComponentProperty<bool>(_uEngineCanvasGroup, _interactable, _uEngineUiModule);
        }

        public Func<bool> GetInteractableVal() {
            var canvas = Driver.FindObject(By.PATH, _serviceModalPath);
            return () => canvas.GetComponentProperty<bool>(_uEngineCanvasGroup, _interactable, _uEngineUiModule);
        }

        public AltVector2 Pos() => GetPos(By.PATH, _serviceModalPath, Driver);
    }
}