using System.Windows;
using WpfTestbench;
using System.Windows.Data;
using System.Globalization;
using System;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for LegendScrollerX
  /// </summary>
  public partial class LegendScrollerXDateWindow: Window {
    
    
  //  /// <summary>
  //  /// Creates and opens a new XLegendScrollerWindow
  //  /// </summary>
  //  public static void Show(Window ownerWindow) {
  //    ShowProtected( () => new LegendScrollerXDateWindow(), ownerWindow);
  //  }


  //  /// <summary>
  //  /// Default constructor
  //  /// </summary>
  //  public LegendScrollerXDateWindow() {
  //    InitializeComponent();
  //    Loaded += XLegendScrollerWindow_Loaded;
  //  }


  //  void XLegendScrollerWindow_Loaded(object sender, RoutedEventArgs e) {
  //    //the test results depend on the available width. TestbenchWindow uses 85% of the Monitor width, which leads to different 
  //    //XLegendScrollerWindow.Widths on different monitor sizes. To make the tests to pass on any monitor, the XLegendScrollerWindow.Width
  //    //gets fixed to the width of the monitor the test was written for.
  //    Width = 1161;
  //  }


  //  //protected override Func<Action>[] InitTestFuncs() {
  //  //  return new Func<Action>[] {
  //  //    //     displayIndex      maxIndex               maxDate                              legendMaxDate
  //  //    //       displayIndexRange        minDate                     legendMinDate
  //  //    () => testFunc(   0,    34,   34, "21.12.1999", "24.01.2000", "21.12.1999 00:00:00", "24.01.2000 00:00:00"),
  //  //    () => testFunc(null,    14, null, null,         null,         "21.12.1999 00:00:00", "24.01.2000 00:00:00"), //not enough pixels to zoom in
  //  //    () => testFunc(null,  1000, 1000, null,         null,         "21.12.1999 00:00:00", "24.01.2000 00:00:00"),
  //  //    () => testFunc(null,   500, null, null,         null,         "21.12.1999 00:00:00", "07.01.2000 00:00:00"),
  //  //    () => testFunc(null,   250, null, null,         null,         "21.12.1999 00:00:00", "29.12.1999 12:00:00"),
  //  //    () => testFunc(null,   128, null, null,         null,         "21.12.1999 00:00:00", "25.12.1999 08:26:52"),
  //  //    () => testFunc(null,    64, null, null,         null,         "21.12.1999 00:00:00", "23.12.1999 04:13:26"),
  //  //    () => testFunc(null,    32, null, null,         null,         "21.12.1999 00:00:00", "22.12.1999 20:52:48"),

  //  //    () => testFunc( 200,  1000, null, null,         null,         "21.12.1999 00:00:00", "24.01.2000 00:00:00"),
  //  //    () => testFunc(200,    500, null, null,         null,         "27.12.1999 19:12:00", "13.01.2000 19:12:00"),
  //  //    () => testFunc(null,   250, null, null,         null,         "27.12.1999 19:12:00", "05.01.2000 07:12:00"),
  //  //    () => testFunc(null,   128, null, null,         null,         "27.12.1999 19:12:00", "01.01.2000 03:38:52"),
  //  //    () => testFunc(null,    64, null, null,         null,         "27.12.1999 19:12:00", "29.12.1999 23:25:26"),
  //  //    () => testFunc(null,    32, null, null,         null,         "27.12.1999 19:12:00", "29.12.1999 16:04:48"),

  //  //    () => testFunc(   0,  1000, 1000, "01.01.1900", "31.12.2999", "01.01.1900 00:00:00", "31.12.2999 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "31.12.2999", "01.01.2000 00:00:00", "31.12.2999 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "31.12.2100", "01.01.2000 00:00:00", "31.12.2100 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "31.12.2050", "01.01.2000 00:00:00", "31.12.2050 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "31.12.2010", "01.01.2000 00:00:00", "31.12.2010 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "31.12.2005", "01.01.2000 00:00:00", "31.12.2005 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "31.12.2000", "01.01.2000 00:00:00", "31.12.2000 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "30.06.2000", "01.01.2000 00:00:00", "30.06.2000 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "31.01.2000", "01.01.2000 00:00:00", "31.01.2000 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "10.01.2000", "01.01.2000 00:00:00", "10.01.2000 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "02.01.2000", "01.01.2000 00:00:00", "02.01.2000 00:00:00"),
  //  //    () => testFunc(null,  null, null, "01.01.2000", "01.01.2000", "01.01.2000 00:00:00", "08.01.2000 00:00:00"), //cannot display 0 daterange
  //  //  };
  //  //}


  //  //private Action testFunc(
  //  //  int? displayIndex, int? displayIndexRange, int? maxIndex, 
  //  //  string minDate, string maxDate,
  //  //  string legendMinDate, string legendMaxDate)
  //  //{
  //  //  if (displayIndex!=null) TestLegendXScroller.DisplayIndex = displayIndex.Value;
  //  //  if (displayIndexRange!=null) TestLegendXScroller.DisplayIndexRange = displayIndexRange.Value;
  //  //  if (maxIndex!=null) TestLegendXScroller.MaxIndex = maxIndex.Value;
  //  //  if (minDate!=null) TestLegendXScroller.MinDate = DateTime.Parse(minDate);
  //  //  if (maxDate!=null) TestLegendXScroller.MaxDate = DateTime.Parse(maxDate);

  //  //  return () => { 
  //  //    if (legendMinDate!=null && TestLegendXScroller.Legend.MinDisplayDate.ToString()!=legendMinDate) {
  //  //      throw new Exception("Legend.MinDisplayDate '" + TestLegendXScroller.Legend.MinDisplayDate + "' should be '" + DateTime.Parse(legendMinDate) + "'. " + TestLegendXScroller);
  //  //    }
  //  //    if (legendMaxDate!=null && TestLegendXScroller.Legend.MaxDisplayDate.ToString()!=legendMaxDate) {
  //  //      throw new Exception("Legend.MaxDisplayDate '" + TestLegendXScroller.Legend.MaxDisplayDate + "' should be '" + DateTime.Parse(legendMaxDate) + "'. " + TestLegendXScroller);
  //  //    }
  //  //  };
  //  //}

  //}


  ///// <summary>
  ///// Supports the conversion between DateTime and double.
  ///// </summary>
  //public class DoubleDateTimeConverter: DependencyObject, IValueConverter {
  //  public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
  //    double? timeRangeDouble = value as double?;
  //    if (timeRangeDouble==null) return null;

  //    return TimeSpan.FromDays(timeRangeDouble.Value);
  //  }


  //  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
  //    TimeSpan? timeRange = value as TimeSpan?;
  //    if (timeRange==null) return null;

  //    return timeRange.Value.TotalDays;
  //  }
  //}


  ///// <summary>
  ///// Supports the conversion between TimeSpan and double.
  ///// </summary>
  //public class DateRangeConverter: DependencyObject, IValueConverter {

  //  //// Allows XAML to provide a link to XLegendScroller
  //  //public XLegendScroller XLegendScroller
  //  //{
  //  //    get { return (XLegendScroller) GetValue(XLegendScrollerProperty); }
  //  //    set { SetValue(XLegendScrollerProperty, value); }
  //  //}

  //  //// Dependency property definition for XLegendScroller
  //  //public static readonly DependencyProperty XLegendScrollerProperty =
  //  //    DependencyProperty.Register(
  //  //    "XLegendScroller",
  //  //    typeof(XLegendScroller),
  //  //    typeof(DateRangeConverter));


  //  public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
  //    double? timeRangeDouble = value as double?;
  //    if (timeRangeDouble==null) return null;

  //    return TimeSpan.FromDays(timeRangeDouble.Value);
  //  }

    
  //  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
  //    TimeSpan? timeRange = value as TimeSpan?;
  //    if (timeRange==null) return null;

  //    return timeRange.Value.TotalDays;
  //  }
  }
}
