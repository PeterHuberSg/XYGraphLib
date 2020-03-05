using System;
using System.Globalization;
using System.Windows.Data;


namespace XYGraphLib {

  /// <summary>
  /// Can be used for debugging binding problems. Once can check if the converter gets called at all and what is the type of the value
  /// </summary>
  public class DummyConverter: IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value==null) return null;

      return value.ToString();
    }

    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
