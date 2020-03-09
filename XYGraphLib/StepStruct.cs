/**************************************************************************************

XYGraphLib.StepStruct
=====================

Gives easy access to FirstDigit and Exponent of numerical values in Legend 

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
  /// Gives easy access to FirstDigit and Exponent of Value. Example: Value: 30000, FirstDigit: 3, Exponent: 4. Note that all
  /// digits except the first one must be 0.
  /// </summary>
  public struct StepStruct {

    public int FirstDigit {
      get { return firstDigit; }
    }
    readonly int firstDigit;

    public int Exponent {
      get { return exponent; }
    }
    readonly int exponent;

    public double Value {
      get { return value; }
    }
    readonly double value;

    static readonly int maxExponent = (int)Math.Floor(Math.Log10(double.MaxValue)) - 1;

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
