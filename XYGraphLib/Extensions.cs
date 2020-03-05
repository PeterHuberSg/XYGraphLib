using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYGraphLib {
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
