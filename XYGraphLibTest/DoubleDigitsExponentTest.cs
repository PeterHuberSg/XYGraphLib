using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYGraphLib;


namespace XYGraphLibTest {


  [TestClass]
  public class DoubleDigitsExponentTest {
    [TestMethod()]
    [DeploymentItem("XYGraphLib.dll")]
    public void TestDoubleDigitsExponent() {
      //                             value,  digits,     exponent
      assertDoubleDigitsExponent(4.5E+123, 4.5, 123);
      assertDoubleDigitsExponent(10000000, 1, 7);
      assertDoubleDigitsExponent(99999999, 9.9999999, 7);
      assertDoubleDigitsExponent(12345678.901, 1.2345678901, 7);
      assertDoubleDigitsExponent(1.234, 1.234, 0);
      assertDoubleDigitsExponent(1, 1, 0);
      assertDoubleDigitsExponent(0.1, 1, -1);
      assertDoubleDigitsExponent(0.0001, 1, -4);
      assertDoubleDigitsExponent(0, 0, 0);
      assertDoubleDigitsExponent(-0.0001, -1, -4);
      assertDoubleDigitsExponent(-0.1, -1, -1);
      assertDoubleDigitsExponent(-1, -1, 0);
      assertDoubleDigitsExponent(-1.234, -1.234, 0);
      assertDoubleDigitsExponent(-12345678.901, -1.2345678901, 7);
      assertDoubleDigitsExponent(-99999999, -9.9999999, 7);
      assertDoubleDigitsExponent(-10000000, -1, 7);
      assertDoubleDigitsExponent(-4.5E+123, -4.5, 123);

    }

    private void assertDoubleDigitsExponent(double testValue, double expectedDigits, double expectedExponent) {
      DoubleDigitsExponent doubleDigitsExponent = new DoubleDigitsExponent(testValue);
      Assert.AreEqual(testValue, doubleDigitsExponent.DoubleValue);
      Assert.AreEqual(expectedExponent, doubleDigitsExponent.Exponent);
      if (testValue>=0) {
        Assert.IsTrue(0.9999999999*expectedDigits<=doubleDigitsExponent.Digits);
        Assert.IsTrue(1.0000000001*expectedDigits>=doubleDigitsExponent.Digits);
      } else {
        Assert.IsTrue(0.9999999999*expectedDigits>=doubleDigitsExponent.Digits);
        Assert.IsTrue(1.0000000001*expectedDigits<=doubleDigitsExponent.Digits);
      }
    }
  }
}
