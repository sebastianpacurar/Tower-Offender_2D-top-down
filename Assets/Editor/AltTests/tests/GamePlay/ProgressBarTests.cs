using System;
using Altom.AltDriver;
using Editor.AltTests.pages.canvas;
using Editor.AltTests.props;
using NUnit.Framework;

namespace Editor.AltTests.tests.GamePlay {
    public class ProgressBarTests {
        private AltDriver _altDriver;
        private ProgressBars _progressBars;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("NavigationScene");
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        [SetUp]
        public void TestSetup() {
            _progressBars = new ProgressBars(_altDriver);
        }

        [Test]
        public void TestSpeedBar() {
            // perform assertion when the progress bar is decreasing and moving forward
            _altDriver.KeysDown(new[] { AltKeyCode.W, AltKeyCode.LeftShift });
            PerformSpeedBarAssertions(Props.SpeedShaderFillVal(_altDriver), _progressBars.GetSpeedBarTxt(), 3.5f);

            // perform assertion when the progress bar is increasing and not moving
            _altDriver.KeysUp(new[] { AltKeyCode.W, AltKeyCode.LeftShift });
            PerformSpeedBarAssertions(Props.SpeedShaderFillVal(_altDriver), _progressBars.GetSpeedBarTxt(), 2.5f);

            // perform assertion when the progress bar is decreasing and moving backward
            _altDriver.KeysDown(new[] { AltKeyCode.S, AltKeyCode.LeftShift });
            PerformSpeedBarAssertions(Props.SpeedShaderFillVal(_altDriver), _progressBars.GetSpeedBarTxt(), 5f);

            // perform assertion when the progress bar is increasing and not moving
            _altDriver.KeysDown(new[] { AltKeyCode.S, AltKeyCode.LeftShift });
            PerformSpeedBarAssertions(Props.SpeedShaderFillVal(_altDriver), _progressBars.GetSpeedBarTxt(), 2.5f);
        }

        // perform assertion using 2 delegates for shader fill Amount (defaults to 1) and gameObject text value (defaults to 100)
        private void PerformSpeedBarAssertions(Func<float> shaderFillVal, Func<int> barTxtVal, float seconds) {
            var start = DateTime.Now;
            var end = start.AddSeconds(seconds);

            while (true) {
                // // HACK: the order of the below variables matters!!
                var txtVal = barTxtVal();
                var fillVal = shaderFillVal();
                // // 

                // retrieve first 5 string characters of the float 
                var parsed = fillVal.ToString("F3"); // examples: 1.000; 0,999; 0.000

                // extract the integer used to compare against the text value
                var extractedInt = parsed switch {
                    "1.000" => 1,
                    "0.000" => 0,
                    _ => int.Parse(parsed[2..^1]) // return the two digits after the float dot notation
                };

                // perform non boundary assertions
                if (parsed != "1.000" && txtVal != 99) {
                    //perform approximate assertions based on the mid range  (number 5)
                    var thirdDigit = int.Parse(parsed[^1].ToString()); // ex: from 0.893 => 3

                    // // HACK: because fillVal is declared after txtVal, the delay causes different results
                    if (extractedInt == txtVal) {
                        Assert.AreEqual(extractedInt, txtVal);
                    } else {
                        if (extractedInt > txtVal) {
                            Assert.LessOrEqual(thirdDigit, 5, $"Third digit of {parsed} is {thirdDigit}, it should be lesser than 5. The txtVal value is {txtVal}.");
                        } else {
                            Assert.GreaterOrEqual(thirdDigit, 5, $"Third digit of {parsed} is {thirdDigit}, it should be greater than 5. The txtVal value is {txtVal}.");
                        }
                    }
                }

                // exit after specified seconds
                if (DateTime.Now > end) {
                    break;
                }
            }
        }
    }
}