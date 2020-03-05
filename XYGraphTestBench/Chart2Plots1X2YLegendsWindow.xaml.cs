using System;
using System.Collections.Generic;
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
  public partial class Chart2Plots1X2YLegendsWindow: TestbenchWindow {

    
    /// <summary>
    /// Creates and opens a new Chart2Plots1X2YLegendsWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      ShowProtected( () => new Chart2Plots1X2YLegendsWindow(), ownerWindow);
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
    class DataRecord {
      public DateTime Date { get; private set; }
      public double[] DataPoint { get; private set; }


      public DataRecord(DateTime date, double[] dataPoint) {
        Date = date;
        DataPoint = dataPoint;
      }


      public override string ToString() {
        string returnString = "Date " + Date;
        for (int valuesIndex = 0; valuesIndex < DataPoint.Length; valuesIndex++) {
          returnString += "; " + valuesIndex + ": " + DataPoint[valuesIndex];
        }
        return returnString;
      }
    }


    const int lowerGraphLineIndex = 1;
    const int areaLineIndex = 2;
    const int seriesCountUI = 3;//number of series displayed to user
    const int seriesCountDraw = 4;//number of series passed as data. 1 more than UI, because in UI areaLine is only 1 serie, but
    //2 series need to be passed for drawing
    int[] minNumbers = new int[] { 100, 50, 100 };
    int[] maxNumbers =  new int[] {  0, 60,  10 };


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
      var seriesBrushes = new Brush[] { Brushes.Green, Brushes.Blue, Brushes.Gray, /*area2*/null };
      int seriesSettingsIndex = 0;
      for (int seriesUIIndex = 0; seriesUIIndex < seriesCountUI; seriesUIIndex++) {
        int lambdaIndex = seriesSettingsIndex; //we need to create a new instance within the loop, otherwise the lambda expression will use the latest value of seriesSettingsIndex (i.e. max(seriesSettingsIndex)), see C# reference "Outer Variables"
        if (seriesUIIndex==areaLineIndex) {
          seriesSettings[seriesSettingsIndex] = 
            new SerieSetting<DataRecord>(record => new double[] { record.Date.ToDouble(), record.DataPoint[lambdaIndex] },
            SerieStyleEnum.area1, 0, seriesBrushes[seriesUIIndex], 1, null);
          seriesSettingsIndex++;
          int lambdaIndex2 = lambdaIndex+1;
          seriesSettings[seriesSettingsIndex] = 
            new SerieSetting<DataRecord>(record => new double[] { record.Date.ToDouble(), record.DataPoint[lambdaIndex2]},
            SerieStyleEnum.area2, 0, seriesBrushes[seriesUIIndex], 1, null);
          seriesSettingsIndex++;
        } else {
          int group;
          Brush fillBrush;
          if (seriesUIIndex==lowerGraphLineIndex) {
            group = 1;
            Color fillColor = Colors.LightSkyBlue;
            fillColor.A = 128;
            fillBrush = new SolidColorBrush(fillColor);
          } else {
            group = 0;
            fillBrush = null;
          }
          seriesSettings[seriesSettingsIndex] = 
            new SerieSetting<DataRecord>(record => new double[] { record.Date.ToDouble(), record.DataPoint[lambdaIndex]},
            SerieStyleEnum.line, group, seriesBrushes[seriesUIIndex], 2, fillBrush);
          seriesSettingsIndex++;
        }
      }
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

      TestChart2Plots1X2YLegendsTraced.FillData<DataRecord>(dataRecords, seriesSettings);

      FontDefinition[] fontDefinitions = new FontDefinition[]{
        new FontDefinition(Brushes.DarkBlue, FontFamily, null, null, null, FontWeights.Bold),
        new FontDefinition(Brushes.DarkRed, null, 18, null, FontStyles.Italic, FontWeights.Bold),
        new FontDefinition(Brushes.DarkOrange, null, 32, FontStretches.Condensed , null, FontWeights.Normal),
      };
      ChartNote[] chartNotes = new ChartNote[stepsCount/20];
      time = startTime;
      for (int chartNoteIndex = 0; chartNoteIndex < chartNotes.Length; chartNoteIndex++) {
        if (chartNoteIndex<3) {
          chartNotes[chartNoteIndex] = new ChartNote(new double[] { time.ToDouble(), double.PositiveInfinity }, chartNoteIndex.ToString(), chartNoteIndex%3);
        } else {
          chartNotes[chartNoteIndex] = new ChartNote(new double[] { time.ToDouble(), chartNoteIndex*10 }, chartNoteIndex.ToString(), chartNoteIndex%3);
        }
        time = time.AddMinutes(20*minutes);
      }
      TestChart2Plots1X2YLegendsTraced.AddNotes(chartNotes, fontDefinitions, true);
    }
  }
}
