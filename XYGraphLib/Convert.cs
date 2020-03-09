/**************************************************************************************

XYGraphLib.Convert
==================

Helper class for data conversion and rounding

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
  /// Helper class for data conversion and rounding
  /// </summary>
  public static class  Convert {
    /// <summary>
    /// maximal numbers supported
    /// </summary>
    public const int DoubleToString_maxDigitsSupported = 15;

    /// <summary>
    /// converts a double into a string, showing the value in exponential format. DigitCount
    /// indicates how many characters can be used. if the number is negative, the returned
    /// string is one character longer then DigitCount
    /// 
    /// <example>
    /// digitCount = 14, Value = 1                           : 1.00000000E+00
    /// digitCount = 10, Value = 100000000000000000000.0 = 10: 1.0000E+20
    /// digitCount = 10, Value = 0.999999999                 : 1.0000E+00
    /// digitCount = 10, Value = 0.99999                     : 9.9999E-01
    /// digitCount = 10, Value = 0.0                         : 0.0000E+00
    /// digitCount = 10, Value = -100000000000000000000.0    : -1.0000E+20
    /// digitCount = 10, Value = -0.999999999                : -1.0000E+00
    /// digitCount = 10, Value = -0.99999                    : -9.9999E-01
    /// </example>
    /// </summary>
    public static string ExponentialToString(this double Value, int DigitCount) {
      //check validity of parameters
      if (double.IsNaN(Value)) {
        return "NaN";
      }
      if (DigitCount<1) {
        throw new ApplicationException(string.Format("ALib: Exponential to string, at least {0} chars needed, but was {1}.", 1, DigitCount));
      } else if (DigitCount>DoubleToString_maxDigitsSupported) {
        throw new ApplicationException(string.Format("ALib: Exponential to string, at most {0} chars allowed, but was {1}.", DoubleToString_maxDigitsSupported, DigitCount));
      }

      //create format string
      string formatString = "0." + new string('0', DigitCount) + "E+00";

      //convert value to string
      return Value.ToString(formatString);
    }


    /// <summary>
    /// Converts a double to a fixed point format string. The double gets rounded to the least significant
    /// digit.
    /// <example>
    /// <para>value      LSD  formatted string</para>
    /// <para>123456.789   2  123500</para>
    /// <para>123456.789   1  123460</para>
    /// <para>123456.789   0  123457</para>
    /// <para>123456.789  -1  123456.8</para>
    /// <para>123456.789  -2  123456.79</para>
    /// </example>
    /// </summary>
    public static string FixedPointLSDToString(this double value, int leastSignificantDigit) {
      double roundValue = value.Round(leastSignificantDigit);
      if (leastSignificantDigit>=0) {
        return roundValue.FixedPointToString(0);
      } else {
        return roundValue.FixedPointToString(-leastSignificantDigit);
      }
    }


    /// <summary>
    /// converts a double into a string, showing the value in fixed point format. First,
    /// the floating number gets rounded to x significant digits. Then enough zeros are
    /// added just before or after the decimal point to display the number correctly. For 
    /// details see examples:
    /// 
    /// <example>
    /// Format = 5; Value = 1234567.890      : "1234600"   
    /// Format = 5; Value =  999996.000      : "1000000"   
    /// Format = 5; Value =  999994.000      :  "999990"   
    /// Format = 5; Value =   99999.6        :  "100000"   
    /// Format = 5; Value =   99999.4        :   "99999"   
    /// Format = 5; Value =    9999.96       :   "10000.0" because of rounding 1 digits too many
    /// Format = 5; Value =    9999.94       :    "9999.9"
    /// Format = 5; Value =    9998.96       :    "9999.0"
    /// Format = 5; Value =     999.996      :    "1000.00"
    /// Format = 5; Value =     999.994      :     "999.99"
    /// Format = 5; Value =      99.9996     :     "100.000"
    /// Format = 5; Value =      99.9994     :      "99.999"
    /// Format = 5; Value =       9.99996    :      "10.0000"
    /// Format = 5; Value =       9.99994    :       "9.9999"
    /// Format = 5; Value =       0.999996   :       "1.00000"
    /// Format = 5; Value =       0.999994   :       "0.99999"
    /// Format = 5; Value =       0.0999996  :       "0.100000"
    /// Format = 5; Value =       0.0999994  :       "0.099999"
    /// Format = 5; Value =       0.00999996 :       "0.0100000"
    /// Format = 5; Value =       0.00999994 :       "0.0099999"
    /// 
    /// Format = 5; Value = -1234567.890      : "-1234600"   
    /// Format = 5; Value =  -999996.000      : "-1000000"   
    /// Format = 5; Value =  -999994.000      :  "-999990"   
    /// Format = 5; Value =   -99999.6        :  "-100000"   
    /// Format = 5; Value =   -99999.4        :   "-99999"   
    /// Format = 5; Value =    -9999.96       :   "-10000"
    /// Format = 5; Value =    -9999.94       :    "-9999.9"
    /// Format = 5; Value =    -9998.96       :    "-9999.0"
    /// Format = 5; Value =     -999.996      :    "-1000.0"
    /// Format = 5; Value =     -999.994      :     "-999.99"
    /// Format = 5; Value =      -99.9996     :     "-100.00"
    /// Format = 5; Value =      -99.9994     :      "-99.999"
    /// Format = 5; Value =       -9.99996    :      "-10.000"
    /// Format = 5; Value =       -9.99994    :       "-9.9999"
    /// Format = 5; Value =       -0.999996   :       "-1.0000"
    /// Format = 5; Value =       -0.999994   :       "-0.99999"
    /// Format = 5; Value =       -0.0999996  :       "-0.10000"
    /// Format = 5; Value =       -0.0999994  :       "-0.099999"
    /// Format = 5; Value =       -0.00999996 :       "-0.010000"
    /// Format = 5; Value =       -0.00999994 :       "-0.0099999"
    /// 
    /// Format = 4; Value = 1234567.890     : "1235000"   
    /// Format = 4; Value =  999960.000     : "1000000"   
    /// Format = 4; Value =  999940.000     :  "999900"   
    /// Format = 4; Value =   99996.00      :  "100000"   
    /// Format = 4; Value =   99994.0       :   "99990"   
    /// Format = 4; Value =    9999.6       :   "10000"
    /// Format = 4; Value =    9999.4       :    "9999"
    /// Format = 4; Value =    9998.6       :    "9999"
    /// Format = 4; Value =     999.96      :    "1000"
    /// Format = 4; Value =     999.94      :     "999.9"
    /// Format = 4; Value =      99.996     :     "100.0"
    /// Format = 4; Value =      99.994     :      "99.99"
    /// Format = 4; Value =       9.9996    :      "10.00"
    /// Format = 4; Value =       9.9994    :       "9.999"
    /// Format = 4; Value =       0.99996   :       "1.000"
    /// Format = 4; Value =       0.99994   :       "0.9999"
    /// Format = 4; Value =       0.099996  :       "0.1000"
    /// Format = 4; Value =       0.099994  :       "0.09999"
    /// Format = 4; Value =       0.0099996 :       "0.01000"
    /// Format = 4; Value =       0.0099994 :       "0.009999"
    /// 
    /// </example>
    /// </summary>
    /// <param name="Value">numeric value to be converted</param>
    /// <param name="DigitCountBeforePoint">number of significant digits. 0 means no rounding</param>
    /// <returns>converted string.</returns>
    public static string FixedPointSignificantDigitsToString(this double Value, int significantDigitsCount) {
      //check validity of parameters
      if (significantDigitsCount<=0) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, at least 1 significant digit needed, but was {0}.", significantDigitsCount));
      }
      if (significantDigitsCount>DoubleToString_maxDigitsSupported) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, at most {0} digits supported, but there were {1} digits.", DoubleToString_maxDigitsSupported, significantDigitsCount));
      }
      //find most significant digit
      int digitIndex;
      double decimalFactor;
      double positiveValue = Math.Abs(Value);
      if (positiveValue>=1.0) {
        digitIndex = 1;
        decimalFactor = 10;
        while (decimalFactor<positiveValue) {
          digitIndex++;
          decimalFactor *= 10;
        }
      } else {
        digitIndex = 0;
        decimalFactor = 0.1;
        while (decimalFactor>positiveValue && digitIndex>=-powerOf10ArrayOffset) {
          digitIndex--;
          decimalFactor /= 10;
        }
      };

      if (digitIndex<=-powerOf10ArrayOffset) {
        //value is considered to be zero
        return FixedPointToString(0, significantDigitsCount);
      } else {
        double roundedValue = Value.Round(digitIndex - significantDigitsCount);
        int digitCountAfterDeciPoint = significantDigitsCount - digitIndex;
        if (digitCountAfterDeciPoint>0) {
          return FixedPointToString(roundedValue, digitCountAfterDeciPoint);
        } else {
          return FixedPointToString(roundedValue, 0);
        }
      }
    }


    /// <summary>
    /// converts a double into a string, showing the value in fixed point format. For 
    /// the value before the decimal point, as many digits as needed are displayed. The
    /// parameter DigitCountAfterPoint decides how many digits are used after the decimal 
    /// point. DigitCountAfterPoint=0 means no decimal point
    /// 
    /// <example>
    /// Digits = 3; Value = 99.9996 : "100.000"
    /// Digits = 3; Value = 99.9994 :  "99.999"
    /// Digits = 3; Value =  9.9996 :  "10.000"
    /// Digits = 3; Value =  9.9994 :   "9.999"
    /// Digits = 3; Value =  0.9996 :   "1.000"
    /// Digits = 3; Value =  0.9994 :   "0.999"
    /// Digits = 3; Value =  0.0996 :   "0.100"
    /// Digits = 3; Value =  0.0994 :   "0.099"
    /// Digits = 3; Value =  0.0096 :   "0.010"
    /// Digits = 3; Value =  0.0094 :   "0.009"
    /// Digits = 3; Value =  0.0006 :   "0.001"
    /// Digits = 3; Value =  0.0004 :   "0.000"

    /// Digits = 3; Value = -99.9996 : "-100.000"
    /// Digits = 3; Value = -99.9994 :  "-99.999"
    /// Digits = 3; Value =  -9.9996 :  "-10.000"
    /// Digits = 3; Value =  -9.9994 :   "-9.999"
    /// Digits = 3; Value =  -0.9996 :   "-1.000"
    /// Digits = 3; Value =  -0.9994 :   "-0.999"
    /// Digits = 3; Value =  -0.0996 :   "-0.100"
    /// Digits = 3; Value =  -0.0994 :   "-0.099"
    /// Digits = 3; Value =  -0.0096 :   "-0.010"
    /// Digits = 3; Value =  -0.0094 :   "-0.009"
    /// Digits = 3; Value =  -0.0006 :   "-0.001"
    /// Digits = 3; Value =  -0.0004 :   "-0.000"

    /// Digits = 3; Value = 99.6 : "100"
    /// Digits = 3; Value = 99.4 :  "99"
    /// Digits = 3; Value =  9.6 :  "10"
    /// Digits = 3; Value =  9.4 :   "9"
    /// Digits = 3; Value =  0.6 :   "1"
    /// Digits = 3; Value =  0.4 :   "0"
    /// </example>
    /// </summary>
    public static string FixedPointToString(this double Value, int DigitCountAfterPoint) {
      //check validity of parameter
      if (DigitCountAfterPoint<0) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, at least 0 chars after decimal point needed, but was {0}.", 
          DigitCountAfterPoint));
      }
      if (DigitCountAfterPoint>DoubleToString_maxDigitsSupported) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, at most {0} digits supported, but was {1}.", 
          DoubleToString_maxDigitsSupported, DigitCountAfterPoint));
      }

      //create format string
      string formatString;
      //only digits after decimal point 0.0234
      formatString = "0." + new String('0', DigitCountAfterPoint);

      //convert value to string
      string resultString = Value.ToString(formatString);

      return resultString;
    }


    /// <summary>
    /// converts a double into a string, showing the value in fixed point format. If necessary,
    /// blanks are inserted in front of leading digits to align decimal points without using
    /// tabs.
    /// 
    /// <example>
    /// Format = 5, 0; Value = 99999 : "99999"
    /// Format = 5, 0; Value = 9999  : "  9999"
    /// Format = 5, 0; Value = 9     : "        9"
    /// Format = 5, 0; Value = 0.9   : "        1"  note: rounding
    /// 
    /// Format = 3, 2; Value = 9999  : "9999.00"  note: more digits returned than requested
    /// Format = 3, 2; Value = 999   : "999.00"
    /// Format = 3, 2; Value = 0.999 : "    1.00" note: rounding
    /// Format = 3, 2; Value = 0.99  : "    0.99"
    /// Format = 3, 2; Value = 0.9   : "    0.90"
    /// Format = 3, 2; Value = 0     : "    0.00" 
    /// Format = 3, 2; Value = -99   : "-99.00"
    /// Format = 3, 2; Value = -0.999: "-1.00"
    /// 
    /// Format = 0, 3; Value = 9999:      "9999.000"
    /// Format = 0, 3; Value =    9:      "9.000"
    /// Format = 0, 3; Value =    0.9:    "0.900"
    /// Format = 0, 3; Value =    0.999:  "0.999"
    /// Format = 0, 3; Value =    0.9999: "1.000"
    /// Format = xxx, xxx; Value = xxx: xxxx
    /// </example>
    /// </summary>
    /// <param name="Value">numeric value to be converted</param>
    /// <param name="DigitCountBeforePoint">number of digits before the decimal point. can be 0, but not negative</param>
    /// <param name="DigitCountAfterPoint">number of digits after the decimal point. can be 0, but not negative</param>
    /// <returns>converted string. if not enough leading digit, 2 blanks will be added for each missing digit.</returns>
    public static string FixedPointToStringColumn(this double Value, int DigitCountBeforePoint, int DigitCountAfterPoint) {
      //check validity of parameters
      if (DigitCountBeforePoint<0) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, at least 0 chars before decimal point needed, but was {0}.", DigitCountBeforePoint));
      }
      if (DigitCountAfterPoint<0) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, at least 0 chars after decimal point needed, but was {0}.", DigitCountAfterPoint));
      }
      if (DigitCountBeforePoint==0 && DigitCountAfterPoint==0) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, number of chars before or after decimal point needs to be bigger than 0, but both were 0.", DigitCountAfterPoint));
      }
      if (DigitCountBeforePoint + DigitCountAfterPoint>DoubleToString_maxDigitsSupported) {
        throw new ApplicationException(string.Format("ALib: FixedPoint to string, at most {0} digits supported, but before was {1} and after was {2}.", DoubleToString_maxDigitsSupported, DigitCountBeforePoint, DigitCountAfterPoint));
      }

      //create format string
      string formatString;
      if (DigitCountBeforePoint==0) {
        //only digits after decimal point 0.0234
        formatString = "0." + new String('0', DigitCountAfterPoint);
      } else if (DigitCountAfterPoint==0) {
        //only digits before decimal point 234
        formatString = new String('#', DigitCountBeforePoint-1) + "0";
      } else {
        //digits before and after the decimal point
        formatString = new String('#', DigitCountBeforePoint-1) + "0." + new String('0', DigitCountAfterPoint);
      }

      //convert value to string
      string resultString = Value.ToString(formatString);

      //check format
      if (DigitCountBeforePoint>0) {
        //fill up with empty spaces as needed (2 spaces for 1 missing digit)
        int leadingDigitCount;
        if (DigitCountAfterPoint==0) {
          leadingDigitCount = resultString.Length;
        } else {
//          leadingDigitCount = resultString.IndexOf('.');
          leadingDigitCount = resultString.IndexOf(
            System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }
        //if (Value<0.0) {
        //  leadingDigitCount -= 1;
        //}
        if (leadingDigitCount<0) {
          throw new ApplicationException(string.Format("ALib: FixedPoint to string, decimal point missing. Value {0} with DigitCountBeforePoint '{1}' and DigitCountAfterPoint '{2}' converted to '{3}'.",
          Value, DigitCountBeforePoint, DigitCountAfterPoint, resultString));
        } else if (leadingDigitCount>DigitCountBeforePoint) {
          throw new ApplicationException(string.Format("ALib: FixedPoint to string, too many digits before decimal point. Value {0} with DigitCountBeforePoint '{1}' and DigitCountAfterPoint '{2}' converted to '{3}'.",
            Value, DigitCountBeforePoint, DigitCountAfterPoint, resultString));
        } else if (leadingDigitCount<DigitCountBeforePoint) {
          resultString = new string(' ', 2 * (DigitCountBeforePoint-leadingDigitCount)) + resultString;
        }

      }
      return resultString;
    }


    /// <summary>
    /// returns the number of non zero digits after the decimal point. 
    /// </summary>
    public static int GetDigitCountAfterPoint(this double number) {
      string numberString = number.ToString();
      int indexOfE = numberString.IndexOf("E");
      if (indexOfE>=0) {
        throw new ApplicationException(string.Format("Convert.GetDigitCountAfterPoint(number: {0}): number too big or too small.", number));
      }
      int leadingDigitCount = numberString.IndexOf(
            System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
      if (leadingDigitCount<0) {
        //no decimal point
        return 0;
      }
      int charIndex = leadingDigitCount+1;
      int zeroCount = 0;
      int nineCount = 0;
      while (charIndex<numberString.Length){
        if (numberString[charIndex]=='0') {
          zeroCount++;
          if (zeroCount>=3) {
            return charIndex - leadingDigitCount - 3;
          }
        } else {
          zeroCount = 0;
        }
        if (numberString[charIndex]=='9') {
          nineCount++;
          if (nineCount>=3) {
            return charIndex - leadingDigitCount - 3;
          }
        } else {
          nineCount = 0;
        }
        charIndex++;
      }
      return numberString.Length - leadingDigitCount - 1;
    }


    /// <summary>
    /// returns the Bit Index of a Flag Enum 
    /// 
    /// value result
    ///     0     -1
    ///     1      0
    ///     2      1
    ///     4      2
    ///     8      3
    ///   ...
    ///  2^31     31
    /// </summary>
    public static int ToBitIndex(this UInt32 value) {
      if (value==0) {
        //no bit set
        return -1;
      }

      // loop through all 2^result
      UInt32 powerOfTwo = 1;
      int bitIndex = 0;
      do {
        if (value==powerOfTwo) {
          return bitIndex;
        }
        powerOfTwo <<= 1;  // same as powerOfTwo*2, but shouldn't generate an overflow for (2^31) * 2
        bitIndex++;
      } while (value>=powerOfTwo && bitIndex<32);
      throw new ApplicationException(string.Format("Converting Flag Enum to Index: Cannot convert '{0}', it is not 2^x", value));
    }


    /// <summary>
    /// returns the exponent of double value. If value is 0, exponent is also 0. If value is
    /// NaN or infinite, an exception is thrown.
    /// </summary>
    public static int GetDoubleExponent(this double value) {
      if (double.IsNaN(value) || double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value)){
        throw new ApplicationException(string.Format("Convert.GetDoubleExponent({0}: Cannot calculate exponent.", value));
      }else if (value==0) {
        //value is 0 and exponent could be any number. We just return 0
        return 0;
      }else if (value<0) {
        value = -value;
      }
      double exponentDouble = Math.Log10(value);
      if (exponentDouble>=0) {
        return (int)exponentDouble;
      } else {
        return (int)Math.Floor(exponentDouble);
      }
    }


    private static readonly int[] powerOf10ArrayInt = { 
                                  1, 
                                 10,
                                100,
                               1000,
                              10000,
                             100000,

                            1000000,
                           10000000,
                          100000000,
                         1000000000};


    /// <summary>
    /// Returns 10 to the power of the Exponent as int
    /// </summary>
    public static int PowerOf10Int(int Exponent) {
      if (Exponent<0) {
        throw new ApplicationException(string.Format("PowerOf10Int(): Exponent cannot be smaller than 0, but was: {0}.", Exponent));
      }
      return powerOf10ArrayInt[Exponent];
    }


    private static readonly double[] powerOf10ArrayDouble = { 
                                  0.0000000000000000000000000001,
                                  0.000000000000000000000000001,
                                  0.00000000000000000000000001,
                                  0.0000000000000000000000001,

                                  0.000000000000000000000001,
                                  0.00000000000000000000001,
                                  0.0000000000000000000001,
                                  0.000000000000000000001,
                                  0.00000000000000000001,
                                  0.0000000000000000001,

                                  0.000000000000000001,
                                  0.00000000000000001,
                                  0.0000000000000001,
                                  0.000000000000001,
                                  0.00000000000001,
                                  0.0000000000001,

                                  0.000000000001,
                                  0.00000000001,
                                  0.0000000001,
                                  0.000000001,
                                  0.00000001,
                                  0.0000001,

                                  0.000001,
                                  0.00001,
                                  0.0001,
                                  0.001,
                                  0.01,
                                  0.1,
                                  1.0, 
                                 10.0,
                                100.0,
                               1000.0,
                              10000.0,
                             100000.0,

                            1000000.0,
                           10000000.0,
                          100000000.0,
                         1000000000.0,
                        10000000000.0,
                       100000000000.0,

                      1000000000000.0,
                     10000000000000.0,
                    100000000000000.0,
                   1000000000000000.0,
                  10000000000000000.0,
                 100000000000000000.0,

                1000000000000000000.0,
               10000000000000000000.0,
              100000000000000000000.0,
             1000000000000000000000.0,
            10000000000000000000000.0,
           100000000000000000000000.0,

         10000000000000000000000000.0,
        100000000000000000000000000.0,
       1000000000000000000000000000.0,
      10000000000000000000000000000.0};

    const int powerOf10ArrayOffset = 28;  //index of 1.0M


    /// <summary>
    /// Returns 10 to the power of the Exponent as double
    /// </summary>
    public static double PowerOf10Double(int Exponent) {
      if (Exponent<-powerOf10ArrayOffset) {
        return Math.Pow(10.0, Exponent);
      }
      int PowerOf10ArrayIndex = Exponent + powerOf10ArrayOffset;
      if (PowerOf10ArrayIndex>=powerOf10ArrayDouble.Length) {
        return Math.Pow(10.0, Exponent);
      }
      return powerOf10ArrayDouble[PowerOf10ArrayIndex];
    }
    

    private static readonly decimal[] powerOf10ArrayDecimal = { 
                                  0.0000000000000000000000000001M,
                                  0.000000000000000000000000001M,
                                  0.00000000000000000000000001M,
                                  0.0000000000000000000000001M,

                                  0.000000000000000000000001M,
                                  0.00000000000000000000001M,
                                  0.0000000000000000000001M,
                                  0.000000000000000000001M,
                                  0.00000000000000000001M,
                                  0.0000000000000000001M,

                                  0.000000000000000001M,
                                  0.00000000000000001M,
                                  0.0000000000000001M,
                                  0.000000000000001M,
                                  0.00000000000001M,
                                  0.0000000000001M,

                                  0.000000000001M,
                                  0.00000000001M,
                                  0.0000000001M,
                                  0.000000001M,
                                  0.00000001M,
                                  0.0000001M,

                                  0.000001M,
                                  0.00001M,
                                  0.0001M,
                                  0.001M,
                                  0.01M,
                                  0.1M,
                                  1M, 
                                 10M,
                                100M,
                               1000M,
                              10000M,
                             100000M,

                            1000000M,
                           10000000M,
                          100000000M,
                         1000000000M,
                        10000000000M,
                       100000000000M,

                      1000000000000M,
                     10000000000000M,
                    100000000000000M,
                   1000000000000000M,
                  10000000000000000M,
                 100000000000000000M,

                1000000000000000000M,
               10000000000000000000M,
              100000000000000000000M,
             1000000000000000000000M,
            10000000000000000000000M,
           100000000000000000000000M,

         10000000000000000000000000M,
        100000000000000000000000000M,
       1000000000000000000000000000M,
      10000000000000000000000000000M};


    /// <summary>
    /// Returns 10 to the power of the Exponent as decimal
    /// </summary>
    public static decimal PowerOf10Decimal(int Exponent) {
      if (Exponent<-powerOf10ArrayOffset) {
        return (decimal)Math.Pow(10.0, Exponent);
      }
      int PowerOf10ArrayIndex = Exponent + powerOf10ArrayOffset;
      if (PowerOf10ArrayIndex>=powerOf10ArrayDecimal.Length) {
        return (decimal)Math.Pow(10.0, Exponent);
      }
      return powerOf10ArrayDecimal[PowerOf10ArrayIndex];
    }


    /// <summary>
    /// biggest double number Convert can round
    /// </summary>
    public static readonly double MaxDoubleRound = PowerOf10Double(Convert.powerOf10ArrayOffset-1);
    /// <summary>
    /// "smallest" number close to zero Convert can round
    /// </summary>
    public static readonly double DoubleRoundEpsilon = PowerOf10Double(-Convert.powerOf10ArrayOffset);

    /// <summary>
    /// supports rounding before (digits positive) and after the decimal point (digits negative)
    /// </summary>
    public static double Round(this double value, int digits) {
      if (digits>28) {
        throw new ApplicationException(string.Format("Convert.Round(): can only round up to 28 digits, but was {0}.", digits));
      }
      if (digits<-28) {
        throw new ApplicationException(string.Format("Convert.Round(): can only round up to 28 digits, but was {0}.", digits));
      }
      if (double.IsNaN(value) || double.IsInfinity(value)) {
      //////  || 
      //////  value>MaxDoubleRound || value<-MaxDoubleRound ||
      //////  (value>0 && value<DoubleRoundEpsilon) || (value<0 && value>-DoubleRoundEpsilon)) //
      //////{
        return value;
      }

      decimal divider = PowerOf10Decimal(digits);
      decimal returnValue;
      //modulo division doesn't round. the value needs to be adjusted
      if (value>=0.0) {
        //for positive values, add half a divider (round .5 to 1)
        returnValue = (decimal)value + (divider / 2M);
      } else {
        //for negative values, subtract half a divider (round -.5 to -1)
        returnValue= (decimal)value - (divider / 2M);
      }
      return (double)(returnValue - (returnValue % divider));
    }


    /// <summary>
    /// Rounds a DateTime to a time interval of milliseconds.
    /// 
    /// Example: 
    /// RoundingMillisec = 300 milliseconds
    /// 29.9.2007 14:00:03:149 => 29.9.2007 14:00:03:000
    /// 29.9.2007 14:00:03:150 => 29.9.2007 14:00:03:300
    /// 29.9.2007 14:00:05:000 => 29.9.2007 14:00:05:100
    /// 
    /// RoundingMillisec = 1 day
    /// 29.9.2007 14:00:05:000 => 30.9.2007 00:00:00:000
    /// </summary>
    public static DateTime Round(this DateTime RoundDateTime, int RoundingMillisec) {
      //if ((24*60*60*1000)%RoundingMillisec != 0) {
      //  Tracer.Warning(
      //    "Time rounding: A day ({0} milliseconds) cannot be divided by RoundingMillisec '{1}' without reminder '{2}'.",
      //    24*60*60*1000, RoundingMillisec, (24*60*60*1000)%RoundingMillisec);
      //}
      long roundingTicks = RoundingMillisec * TimeSpan.TicksPerMillisecond;
      long roundDateTimeTicks = RoundDateTime.Ticks + (roundingTicks / 2);
      return new DateTime(roundDateTimeTicks - (roundDateTimeTicks % roundingTicks));
    }


    /// <summary>
    /// Rounds a DateTime to a time interval of milliseconds. 
    /// 
    /// Example: 
    /// RoundingMillisec = 300 milliseconds
    /// 29.9.2007 14:00:03:149 => 29.9.2007 14:00:03:000
    /// 29.9.2007 14:00:03:150 => 29.9.2007 14:00:03:300
    /// 29.9.2007 14:00:05:000 => 29.9.2007 14:00:05:100
    /// 
    /// RoundingMillisec = 1 day
    /// 29.9.2007 14:00:05:000 => 30.9.2007 00:00:00:000
    /// 
    /// RoundingMillisec = 13579.123 milliseconds, will not create a warning
    /// 29.9.2007 14:00:05:000 => 29.09.2007 14:00:10:210
    /// </summary>
    public static DateTime Round(this DateTime RoundDateTime, double RoundingMillisec) {
      long roundingTicks = (long)(RoundingMillisec * TimeSpan.TicksPerMillisecond);
      long roundDateTimeTicks = RoundDateTime.Ticks + (roundingTicks / 2);
      return new DateTime(roundDateTimeTicks - (roundDateTimeTicks % roundingTicks));
    }


    /// <summary>
    /// converts a time duration into a value together with a time unit
    /// </summary>
    /// <example>
    ///      2: 2 msec
    ///   2000: 2 sec
    /// 120000: 2 min
    /// </example>
    public static string MilliSecondsToString(this long time) {
      if (time<1000) {
        return time.ToString() + " msec";
      } else if (time<1000*60) {
        return (time /1000.0).ToString() + " sec";
      } else if (time<1000*60*60) {
        return (time /1000.0/60.0).ToString() + " min";
      }
      return (time /1000.0/60.0/60.0).ToString() + " h";
    }


    /// <summary>
    /// converts a byte size into a byte value and its unit
    /// </summary>
    /// <example>
    ///  123: 123 byte
    /// 1234: 1.234 kbyte
    /// </example>
    public static string ByteSizeToString(this long byteSize) {
      if (byteSize<1024) {
        return byteSize.ToString() + " byte";
      } else if (byteSize<1024*1024) {
        return (byteSize /1024.0).ToString("0.###") + " kByte";
      } else if (byteSize<1024*1024*1024) {
        return (byteSize /1024.0/1024.0).ToString("0.###") + " MByte";
      }
      return (byteSize /1024.0/1024.0/1024.0).ToString("0.###") + " GByte";
    }

    public static readonly long TicksPerMillisecond = TimeSpan.FromMilliseconds(1).Ticks;


    /// <summary>
    /// Converts DateTime to double. Precision: 1 millisecond
    /// </summary>
    public static double ToDouble(this DateTime time) {
      return (double)(time.Ticks / TicksPerMillisecond);
    }


    /// <summary>
    /// Converts double to DateTime. Precision: 1 millisecond
    /// </summary>
    public static DateTime ToDateTime(this double ticksDouble) {
      return new DateTime((long)ticksDouble*TicksPerMillisecond);
    }


    /// <summary>
    /// Converts TimeSpan to double. Precision: 1 millisecond
    /// </summary>
    public static double ToDouble(this TimeSpan timeSpan) {
      return (double)(timeSpan.Ticks / TicksPerMillisecond);
    }


    /// <summary>
    /// Converts double to TimeSpan. Precision: 1 millisecond
    /// </summary>
    public static TimeSpan ToTimeSpan(this double ticksDouble) {
      return new TimeSpan((long)ticksDouble*TicksPerMillisecond);
    }


    /// <summary>
    /// Test if eventHandler is in the eventHandlers array
    /// </summary>
    public static bool IsRegistered(MulticastDelegate eventHandlers, Delegate eventHandler) {
      if (eventHandlers==null) {
        return false;
      }
      foreach (Delegate eventHandlerItem in eventHandlers.GetInvocationList()) {
        if (eventHandlerItem.Target==eventHandler.Target &&
            eventHandlerItem.Method.Name==eventHandler.Method.Name)//
        {
          return true;
        }
      }
      return false;
    }
  }
}
