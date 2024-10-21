using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTestbench;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for Graph2Plots1X2YLegends
  /// </summary>
  public partial class Chart2Plots1X2YLegendsWindow: Window {


    /// <summary>
    /// Creates and opens a new Chart2Plots1X2YLegendsWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      new Chart2Plots1X2YLegendsWindow { Owner = ownerWindow }.Show();
    }



    public Chart2Plots1X2YLegendsWindow() {
      InitializeComponent();

      TestChart2Plots1X2YLegendsTraced.PlotAreaLower.Background = Brushes.LightGoldenrodYellow;
      TraceWpf.Line(">>>>> Chart2Plots1X2YLegendsWindow.fillDataSeries()");
      fillDataSeries();
    }


    /// <summary>
    /// Stores the data to be displayed in XYGraph for one point in time (=record)
    /// </summary>
    class DataRecord(DateTime date, double[] dataPoint) {
      public DateTime Date { get; private set; } = date;
      public double[] DataPoint { get; private set; } = dataPoint;

      public override string ToString() {
        string returnString = "Date " + Date;
        for (int valuesIndex = 0; valuesIndex < DataPoint.Length; valuesIndex++) {
          returnString += "; " + valuesIndex + ": " + DataPoint[valuesIndex];
        }
        return returnString;
      }
    }


    //const int lowerGraphLineIndex = 1;
    const int areaLineIndex = 2;
    const int seriesCountUI = 3;//number of series displayed to user
    const int seriesCountDraw = 4;//number of series passed as data. 1 more than UI, because in UI areaLine is only 1 serie, but
                                  //2 series need to be passed for drawing
    readonly int[] minNumbers = [100, 50, 100];
    readonly int[] maxNumbers = [0, 60, 10];


    private void fillDataSeries() {
      DateTime startTime = DateTime.Now.Date.AddYears(-1);
      DateTime time = startTime;
      double minutes = 60*24;
      int stepsCount = 365;
      var dataRecords = new DataRecord[stepsCount];

      //count selected series and prepare values for data calculation
      var serieValues = new double[seriesCountDraw];
      var increments = new double[seriesCountDraw];
      int selectSeriesCount = 0;
      for (int seriesUIIndex = 0; seriesUIIndex < seriesCountUI; seriesUIIndex++) {
        serieValues[selectSeriesCount] = minNumbers[seriesUIIndex];
        increments[selectSeriesCount] = (maxNumbers[seriesUIIndex] - serieValues[selectSeriesCount]) / (stepsCount-1);
        selectSeriesCount++;
        if (seriesUIIndex==areaLineIndex) {
          //areaLine needs 2 series for drawing
          serieValues[selectSeriesCount] = minNumbers[seriesUIIndex]*0.8;
          increments[selectSeriesCount] = (maxNumbers[seriesUIIndex]*0.9 - serieValues[selectSeriesCount]) / (stepsCount-1);
          selectSeriesCount++;
        }
      }

      //setup line settings
      var seriesSettings = new SerieSetting<DataRecord>[selectSeriesCount];
      var seriesBrushes = new Brush?[] { Brushes.Green, Brushes.Blue, Brushes.Gray, /*area2*/null };
      Color fillColor = Colors.LightSkyBlue;
      fillColor.A = 128;

      seriesSettings = [
        new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.line, Brushes.Green, 2, null, "Plot1Line"),
        new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.line, Brushes.Blue, 2, new SolidColorBrush(fillColor),
          "Plot2Line", null, 1),
        new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.area1, Brushes.Gray, 1, null, "Plot1Area1"),
        new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.area2, null, strokeThickness: 0, null, "Plot1Area2")];


      //fill serie values into records
      for (int stepIndex = 0; stepIndex < stepsCount; stepIndex++) {
        var recordValues = new double[selectSeriesCount];
        for (int selectSeriesIndex = 0; selectSeriesIndex < selectSeriesCount; selectSeriesIndex++) {
          recordValues[selectSeriesIndex] = serieValues[selectSeriesIndex];
          serieValues[selectSeriesIndex] += increments[selectSeriesIndex];
        }
        var dataRecord = new DataRecord(time, recordValues);
        dataRecords[stepIndex] = dataRecord;
        time = time.AddMinutes(minutes);
      }

      TestChart2Plots1X2YLegendsTraced.FillData<DataRecord>(dataRecords, seriesSettings, "Date");

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
      TestChart2Plots1X2YLegendsTraced.AddNotes(chartNotes, fontDefinitions, true);
    }


    private static void getSeriesData(DataRecord dataRecord, int index, [NotNull] ref double[]? dataExtracted) {
      dataExtracted ??= new double[2];
      dataExtracted[0] = dataRecord.Date.ToDouble();
      dataExtracted[1] = dataRecord.DataPoint[index];
    }
  }
}
