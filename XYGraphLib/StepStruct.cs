using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYGraphLib {
  /// <summary>
  /// Gives easy access to FirstDigit and Exponent of Value. Example: Value: 30000, FirstDigit: 3, Exponent: 4. Note that all
  /// digits except the first one must be 0.
  /// </summary>
  public struct StepStruct {

    public int FirstDigit {
      get { return firstDigit; }
    }
    private int firstDigit;

    public int Exponent {
      get { return exponent; }
    }
    private int exponent;

    public double Value {
      get { return value; }
    }
    private double value;

    static int maxExponent = (int)Math.Floor(Math.Log10(double.MaxValue)) - 1;

    public StepStruct(int newFirstDigit, int newExponent) {
      if (newExponent<-maxExponent || newExponent>maxExponent) {
        throw new ApplicationException(string.Format("Legend step: Exponent of amplitude '{0}' should be between {1} and {2}.", newExponent, -maxExponent, maxExponent));
      }
      firstDigit = newFirstDigit;
      exponent = newExponent;
      value =  firstDigit * Math.Pow(10.0, exponent);
    }

    public override string ToString() {
      return String.Format("firstDigit: '{0}'; exponent: '{1}'; value: '{2}'", firstDigit, exponent, value);
    }
  }
}
