/**************************************************************************************

XYGraphLib.Extensions
=====================

Helper class defining double.IsEqual()

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/


namespace XYGraphLib {


  /// <summary>
  /// Helper class defining double.IsEqual()
  /// </summary>
  public static class Extensions {

    /// <summary>
    /// Stupidly, C# defines NAN!=NAN ! Use IsEqual instead if NAN should be equal NAN
    /// </summary>
    public static bool IsEqual(this double double1, double double2) {
      if (double.IsNaN(double1)){
        return double.IsNaN(double2);
      }else if (double.IsNaN(double2)){
        //double1 is not a NAN
        return false;
      }else{
        return double1==double2;
      }
    }
  }
}
