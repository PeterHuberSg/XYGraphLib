using System.Windows;
using WpfTestbench;
using System;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for XLegend
  /// </summary>
  public partial class LegendXDateWindow: TestbenchWindow {


    /// <summary>
    /// Creates and opens a new YLegendWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      ShowProtected(() => new LegendXDateWindow(), ownerWindow);
    }


    /// <summary>
    /// Default constructor
    /// </summary>
    public LegendXDateWindow() {
      InitializeComponent();
    }


    protected override Func<Action>[] InitTestFuncs() {
      return new Func<Action>[] {
        //                           minDate                                minDisplayDate              timeSpanUnit
        //                                                  maxDate               labelsCount
        () => testFunc("01.01.2000 23:00:00", "03.01.2000 02:00:00", "02.01.2000 00:00:00", 5, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 00:00:00", "03.01.2000 03:00:00", "02.01.2000 00:00:00", 5, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 01:00:00", "03.01.2000 04:00:00", "02.01.2000 06:00:00", 4, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 02:00:00", "03.01.2000 05:00:00", "02.01.2000 06:00:00", 4, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 03:00:00", "03.01.2000 06:00:00", "02.01.2000 06:00:00", 5, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 04:00:00", "03.01.2000 07:00:00", "02.01.2000 06:00:00", 5, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 05:00:00", "03.01.2000 08:00:00", "02.01.2000 06:00:00", 5, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 06:00:00", "03.01.2000 09:00:00", "02.01.2000 06:00:00", 5, TimeSpanUnitEnum.hour6),
        () => testFunc("02.01.2000 07:00:00", "03.01.2000 10:00:00", "02.01.2000 12:00:00", 4, TimeSpanUnitEnum.hour6),

        () => testFunc("01.01.2000 23:00:00", "07.01.2000 00:00:00", "02.01.2000 00:00:00", 6, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 00:00:00", "07.01.2000 01:00:00", "02.01.2000 00:00:00", 6, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 01:00:00", "07.01.2000 02:00:00", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 02:00:00", "07.01.2000 03:00:00", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 03:00:00", "07.01.2000 04:00:00", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 04:00:00", "07.01.2000 05:00:00", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 05:00:00", "07.01.2000 06:00:00", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 06:00:00", "07.01.2000 07:00:00", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.day),
        () => testFunc("02.01.2000 07:00:00", "07.01.2000 08:00:00", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.day),

        //                           minDate                                minDisplayDate              timeSpanUnit
        //                                                  maxDate               labelsCount
        () => testFunc("01.01.2000", "31.01.2000", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.week),
        () => testFunc("02.01.2000", "01.02.2000", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.week),
        () => testFunc("03.01.2000", "02.02.2000", "03.01.2000 00:00:00", 5, TimeSpanUnitEnum.week),
        () => testFunc("04.01.2000", "03.02.2000", "10.01.2000 00:00:00", 4, TimeSpanUnitEnum.week),
        () => testFunc("05.01.2000", "04.02.2000", "10.01.2000 00:00:00", 4, TimeSpanUnitEnum.week),
        () => testFunc("06.01.2000", "05.02.2000", "10.01.2000 00:00:00", 4, TimeSpanUnitEnum.week),
        () => testFunc("07.01.2000", "06.02.2000", "10.01.2000 00:00:00", 4, TimeSpanUnitEnum.week),
        () => testFunc("08.01.2000", "07.02.2000", "10.01.2000 00:00:00", 5, TimeSpanUnitEnum.week),

        () => testFunc("15.08.1999", "14.02.2000", "01.09.1999 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("15.09.1999", "14.03.2000", "01.10.1999 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("15.10.1999", "14.04.2000", "01.11.1999 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("15.11.1999", "14.05.2000", "01.12.1999 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("15.12.1999", "14.06.2000", "01.01.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("15.01.2000", "14.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("16.01.2000", "15.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("17.01.2000", "16.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("18.01.2000", "17.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("19.01.2000", "18.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("20.01.2000", "19.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("21.01.2000", "20.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("22.01.2000", "21.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("23.01.2000", "22.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("24.01.2000", "23.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("25.01.2000", "24.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("26.01.2000", "25.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("27.01.2000", "26.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("28.01.2000", "27.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("29.01.2000", "28.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("30.01.2000", "29.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("31.01.2000", "30.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("01.02.2000", "31.07.2000", "01.02.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("02.02.2000", "01.08.2000", "01.03.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("03.02.2000", "02.08.2000", "01.03.2000 00:00:00", 6, TimeSpanUnitEnum.month),
        () => testFunc("04.02.2000", "03.08.2000", "01.03.2000 00:00:00", 6, TimeSpanUnitEnum.month),

        () => testFunc("01.01.2000", "31.12.2004", "01.01.2000 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.02.2000", "31.01.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.03.2000", "28.02.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.04.2000", "31.03.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.05.2000", "30.04.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.06.2000", "31.05.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.07.2000", "30.06.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.08.2000", "31.07.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.09.2000", "31.08.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.10.2000", "30.09.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.11.2000", "31.10.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.12.2000", "30.11.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),
        () => testFunc("01.01.2001", "31.12.2005", "01.01.2001 00:00:00", 5, TimeSpanUnitEnum.year),

        () => testFunc("01.01.2000", "31.12.2030", "01.01.2000 00:00:00", 7, TimeSpanUnitEnum.year5),
        () => testFunc("01.01.2003", "31.12.2033", "01.01.2005 00:00:00", 6, TimeSpanUnitEnum.year5),
        () => testFunc("01.01.2006", "31.12.2036", "01.01.2010 00:00:00", 6, TimeSpanUnitEnum.year5),
        () => testFunc("01.01.2009", "31.12.2039", "01.01.2010 00:00:00", 6, TimeSpanUnitEnum.year5),
        () => testFunc("01.01.2012", "31.12.2042", "01.01.2015 00:00:00", 6, TimeSpanUnitEnum.year5),
        () => testFunc("01.01.2015", "31.12.2045", "01.01.2015 00:00:00", 7, TimeSpanUnitEnum.year5),

        () => testFunc("02.01.2000", "31.12.2070", "01.01.2010 00:00:00", 7, TimeSpanUnitEnum.year10),
        () => testFunc("02.01.2007", "31.12.2077", "01.01.2010 00:00:00", 7, TimeSpanUnitEnum.year10),
        () => testFunc("02.01.2014", "31.12.2084", "01.01.2020 00:00:00", 7, TimeSpanUnitEnum.year10),
        () => testFunc("02.01.2021", "31.12.2091", "01.01.2030 00:00:00", 7, TimeSpanUnitEnum.year10),
        () => testFunc("02.01.2028", "31.12.2098", "01.01.2030 00:00:00", 7, TimeSpanUnitEnum.year10),
        () => testFunc("02.01.2035", "31.12.2105", "01.01.2040 00:00:00", 7, TimeSpanUnitEnum.year10),

      };
    }


    private Action testFunc(string minDate, string maxDate, string minDisplayDate, int? labelsCount, TimeSpanUnitEnum? timeSpanUnit) {
      if (minDate!=null) TestLegendXDateTraced.DisplayDate = DateTime.Parse(minDate);

      if (maxDate!=null) {
        DateTime endTime = DateTime.Parse(maxDate);
        TestLegendXDateTraced.DisplayDateRange = DateTime.Parse(maxDate) - TestLegendXDateTraced.DisplayDate;
      }

      return () => {
        if (minDisplayDate!=null && timeSpanUnit!=null){
          DateTime firstLabelDate = TestLegendXDateTraced.LabelValues[0].ToDateTime();
          DateTime expectedFirstLabelDate = DateTime.Parse(minDisplayDate);
          //          string firstLabelString = TestLegendXDateTraced.LabelValues[0].ToDateTime().ToFirstLabel(timeSpanUnit.Value);
          if (firstLabelDate!=expectedFirstLabelDate) {
            System.Diagnostics.Debugger.Break();
            throw new Exception("First label '" + firstLabelDate + "' should be '" + expectedFirstLabelDate + "'. " + TestLegendXDateTraced);
          }
        }
        if (labelsCount!=null && TestLegendXDateTraced.LabelValues.Length!=labelsCount.Value) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("LabelValues.Length '" + TestLegendXDateTraced.LabelValues.Length + "' should be '" + labelsCount + "'. " + TestLegendXDateTraced);
        }
        if (timeSpanUnit!=null && TestLegendXDateTraced.TimeSpanUnit!=timeSpanUnit.Value) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("TimeSpanUnit '" + TestLegendXDateTraced.TimeSpanUnit + "' should be '" + timeSpanUnit + "'. " + TestLegendXDateTraced);
        }
      };
    }
  }
}
