/**************************************************************************************

XYGraphLib.DoubleDigitsExponent
===============================

Gives access to the significant digits and the power of 10 exponent of a double

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;


namespace XYGraphLib {
  /// <summary>
  /// Gives access to the significant digits and the power of 10 exponent of a double
  /// 
  /// double   digits  exponent
  /// 123      1.23     2
  ///  12.3    1.23     1
  ///   1.23   1.23     0
  ///   0.123  1.23    -1
  ///   0.0123 1.23    -2
  /// </summary>
  public class DoubleDigitsExponent {

    public double DoubleValue {
      get { return doubleValue; }
      set { setDoubleValue(value); }
    }
    private double doubleValue;


    public double Digits {
      get { return digits; }
      //set { digits = value; }
    }
    private double digits;


    public int Exponent {
      get { return exponent; }
      //set { exponent = value; }
    }
    private int exponent;


    public DoubleDigitsExponent(double value) {
      setDoubleValue(value);
    }


    private void setDoubleValue(double value) {
      if (double.IsNaN(value)) {
        throw new Exception("DoubleDigitsExponent does not support undefined double NaN.");
      }
      if (double.IsInfinity(value)) {
        throw new Exception("DoubleDigitsExponent does not support infinity.");
      }
      //normalise amplitude between 1.0 and 10.0
      doubleValue = digits = value;
      double absDigits = Math.Abs(digits);
      exponent = 0;
      if (absDigits==0) return;

      if (absDigits>=10) {
        //divide by 10, until step smaller 10
        while (absDigits>=10) {
          absDigits /= 10;
          digits /= 10;
          exponent += 1;
        }

      } else if (absDigits<1) {
        //multiply by 10, until step bigger than 1
        while (absDigits<1) {
          absDigits *= 10.0;
          digits *= 10.0;
          exponent -= 1;
        }
      } else {
        //nothing to do, amplitude is already between 1.0 and 10.0
      }
    }


    public override string ToString() {
      return "Value: " + doubleValue + "; Digits: " + digits + "; Exponent: " + Exponent + ";";
    }
  }

}
