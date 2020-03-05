using System;
using System.Globalization;
using System.Windows.Data;


namespace XYGraphLib {

  /// <summary>
  /// The converter just returns the same object it received. It can be used for debugging binding problems by setting breakpoints during 
  /// convertion. One can check if the converter gets called at all and what is the type of the value, Unfortunately, the Converter
  /// doesn't get any binding information. Throw an exception  to see the binding details in the error text.
  /// </summary>
  public class PassThroughConverter: IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return value;
    }

    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return value;
    }
  }
}
