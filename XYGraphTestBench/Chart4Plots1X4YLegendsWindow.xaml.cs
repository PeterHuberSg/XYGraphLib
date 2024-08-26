using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using WpfTestbench;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for Graph4Plots1X4YLegends
  /// </summary>
  public partial class Chart4Plots1X4YLegendsWindow: Window {


    /// <summary>
    /// Creates and opens a new Chart4Plots1X4YLegendsWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      new Chart4Plots1X4YLegendsWindow { Owner = ownerWindow }.Show();
    }


    public Chart4Plots1X4YLegendsWindow() {
      InitializeComponent();

      TestChart4Plots1X4YLegendsTraced.PlotArea1.Background = Brushes.SeaShell;
      TestChart4Plots1X4YLegendsTraced.PlotArea2.Background = Brushes.Cornsilk;
      TestChart4Plots1X4YLegendsTraced.PlotArea3.Background = Brushes.Honeydew;
      TraceWpf.Line(">>>>> Chart4Plots1X4YLegendsWindow.fillDataSeries()");
      fillDataSeries();
    }


    /// <summary>
    /// Stores the data to be displayed in XYGraph for one point in time (=record)
    /// </summary>
    public class DataRecord(DateTime date, double[] dataPoint) {
      public DateTime Date { get; private set; } = date;
      public double[] DataPoint { get; private set; } = dataPoint;

      public override string ToString() {
        var returnString = "Date " + Date;
        for (var valuesIndex = 0; valuesIndex < DataPoint.Length; valuesIndex++) {
          returnString += "; " + valuesIndex + ": " + DataPoint[valuesIndex];
        }
        return returnString;
      }
    }


    const int groupCount = 4;
    const int seriesCount = groupCount * 3;


    private void fillDataSeries() {
      DateTime startTime = DateTime.Now.Date.AddYears(-1);
      DateTime time = startTime;
      double minutes = 60*24;
      int stepsCount = 365;
      var dataRecords = new DataRecord[stepsCount];
      var serieValues = new double[seriesCount];
      var random = new Random();

      //prepare values for data calculation
      var serieIndex = 0;
      var seriesSettings = new SerieSetting<DataRecord>[seriesCount];
      //var seriesBrushes = new Brush[] { new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00)) };
      //var seriesFillBrushes = new Brush[] { Brushes.Green, Brushes.Blue, Brushes.Gray };
      //for (var groupIndex = 0; groupIndex < groupCount; groupIndex++) {
      //  var lambdaIndex = serieIndex; //A new instance within the loop is needed, otherwise the lambda expression will use the latest value of serieIndex (i.e. 12), see C# reference "Outer Variables"
      //  seriesSettings[serieIndex] =
      //    new SerieSetting<DataRecord>(record => [record.Date.ToDouble(), record.DataPoint[lambdaIndex]],
      //      SerieStyleEnum.line, groupIndex,
      //      new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)), 2,
      //      new SolidColorBrush(Color.FromArgb(0x30, 0xA0, 0xA0, 0xA0)));
      //  serieValues[serieIndex++] = random.NextDouble() * 100;
      //  var lambdaIndex1 = serieIndex;
      //  seriesSettings[serieIndex] =
      //    new SerieSetting<DataRecord>(record => [record.Date.ToDouble(), record.DataPoint[lambdaIndex1]],
      //      SerieStyleEnum.line, groupIndex,
      //      new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80)), 2,
      //      new SolidColorBrush(Color.FromArgb(0x30, 0x80, 0x80, 0x80)));
      //  serieValues[serieIndex++] = random.NextDouble() * 100;
      //  var lambdaIndex2 = serieIndex;
      //  seriesSettings[serieIndex] =
      //    new SerieSetting<DataRecord>(record => [record.Date.ToDouble(), record.DataPoint[lambdaIndex2]],
      //      SerieStyleEnum.line, groupIndex,
      //      new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00)), 2,
      //      new SolidColorBrush(Color.FromArgb(0x30, 0x00, 0x00, 0x00)));
      //  serieValues[serieIndex] = serieValues[serieIndex-1] + serieValues[serieIndex-2];
      //  serieIndex++;
      //}
      for (var groupIndex = 0; groupIndex < groupCount; groupIndex++) {
        seriesSettings[serieIndex] = new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.line, groupIndex,
          new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)), 2, new SolidColorBrush(Color.FromArgb(0x30, 0xA0, 0xA0, 0xA0)));
        serieValues[serieIndex++] = random.NextDouble() * 100;
        seriesSettings[serieIndex] = new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.line, groupIndex,
          new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80)), 2, new SolidColorBrush(Color.FromArgb(0x30, 0x80, 0x80, 0x80)));
        serieValues[serieIndex++] = random.NextDouble() * 100;
        seriesSettings[serieIndex] = new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.line, groupIndex,
          new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00)), 2, new SolidColorBrush(Color.FromArgb(0x30, 0x00, 0x00, 0x00)));
        serieValues[serieIndex] = serieValues[serieIndex-1] + serieValues[serieIndex-2];
        serieIndex++;
      }

      //fill serie values into records
      for (var stepIndex = 0; stepIndex < stepsCount; stepIndex++) {
        var recordValues = new double[seriesCount];
        serieIndex = 0;
        for (var groupIndex = 0; groupIndex < groupCount; groupIndex++) {
          recordValues[serieIndex] = serieValues[serieIndex];
          serieValues[serieIndex++] += random.NextDouble() * 10 - 5;
          recordValues[serieIndex] = serieValues[serieIndex];
          serieValues[serieIndex++] += random.NextDouble() * 10 - 5;
          recordValues[serieIndex] = serieValues[serieIndex-1] + serieValues[serieIndex-2];
          serieIndex++;
        }
        var dataRecord = new DataRecord(time, recordValues);
        dataRecords[stepIndex] = dataRecord;
        time = time.AddMinutes(minutes);
      }

      TestChart4Plots1X4YLegendsTraced.FillData<DataRecord>(dataRecords, seriesSettings);

      FontDefinition[] fontDefinitions = [
        new FontDefinition(Brushes.DarkBlue, FontFamily, null, null, null, FontWeights.Bold),
        new FontDefinition(Brushes.DarkRed, null, 18, null, FontStyles.Italic, FontWeights.Bold),
        new FontDefinition(Brushes.DarkOrange, null, 32, FontStretches.Condensed , null, FontWeights.Normal),
      ];
      ChartNote[] chartNotes = new ChartNote[stepsCount/20];
      time = startTime;
      for (int chartNoteIndex = 0; chartNoteIndex < chartNotes.Length; chartNoteIndex++) {
        chartNotes[chartNoteIndex] =chartNoteIndex<3
          ? new ChartNote([time.ToDouble(), double.PositiveInfinity], chartNoteIndex.ToString(), chartNoteIndex%3)
          : new ChartNote([time.ToDouble(), chartNoteIndex*10], chartNoteIndex.ToString(), chartNoteIndex%3);
        time = time.AddMinutes(20*minutes);
      }
      TestChart4Plots1X4YLegendsTraced.AddNotes(chartNotes, fontDefinitions, 0);
    }


    private static void getSeriesData(DataRecord dataRecord, int index, [NotNull] ref double[]? dataExtracted) {
      dataExtracted ??= new double[2];
      dataExtracted[0] = dataRecord.Date.ToDouble();
      dataExtracted[1] = dataRecord.DataPoint[index];
    }
  }
}
