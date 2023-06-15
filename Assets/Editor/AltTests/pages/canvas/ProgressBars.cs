using Altom.AltDriver;
using System;

namespace Editor.AltTests.pages.canvas {
    public class ProgressBars : BasePage {
        public ProgressBars(AltDriver driver) : base(driver) { }

        #region AltObjects
        private AltObject HpRadialBarTxt => Driver.WaitForObject(By.NAME, "HpTxtValue");
        private AltObject SpeedRadialBarTxt => Driver.WaitForObject(By.NAME, "SpeedTxtValue");
        #endregion

        #region getters
        public int GetHpBarTxt() => int.Parse(HpRadialBarTxt.GetText()[..^1]);
        public Func<int> GetSpeedBarTxt() => () => int.Parse(SpeedRadialBarTxt.GetText()[..^1]);
        #endregion
    }
}