/**************************************************************************************

XYGraphLib.DummyConverter
=========================

Can be used for debugging binding problems.

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Globalization;
using System.Windows.Data;


namespace XYGraphLib {


  /// <summary>
  /// Can be used for debugging binding problems. One can check if the converter gets called at all and what is the type of the value
  /// </summary>
  public class DummyConverter: IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value==null) return null!;

      return value.ToString()!;
    }

    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
