/*
 Test window for Chart1Plot1X1YLegend.
 
 Visual Studio's Visual Designer hangs when Chart1Plot1X1YLegendWindow is made in XAML. But it runs without problems. Therefore, only
 code behind is used.
  
 Chart1Plot1X1YLegend specific parameters displayed in Chart1Plot1X1YLegendWindow:
 
 +-----------------------------------------------------+
 |      Start Date: [1.1.2011]  Time:      [1] minutes |
 |      Steps:      [       5]  Increment: [1] minutes |
 |                                                     |
 | Serie 0 [x]  Min: [  0]  Max: [100]                 |
 | Serie 1 [x]  Min: [ 50]  Max: [ 60]                 |
 | Serie 2 [x]  Min: [100]  Max: [100]                 |  serie 3 gets displayed as areaLine
 +-----------------------------------------------------+
 
*/

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
using System.Windows.Controls.Primitives;
using System.Diagnostics.CodeAnalysis;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for Chart1Plot1X1YLegend
  /// </summary>
  public partial class Chart1Plot1X1YWindow: Window {


    /// <summary>
    /// Creates and opens a new Chart1Plot1X1YLegendWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      new Chart1Plot1X1YWindow { Owner = ownerWindow }.Show();
    }


    readonly Chart1Plot1X1YLegendTraced chart1Plot1X1YLegendTraced;

    //common setup data
    readonly DatePicker startDateDatePicker;
    readonly NumberScrollBar timeNumberScrollBar;
    readonly NumberScrollBar stepsNumberScrollBar;
    readonly NumberScrollBar incrementNumberScrollBar;

    //setup data for series
    const int seriesCountUI = 3;//number of series displayed to user
    const int seriesCountDraw = 4;//number of series passed as data. 1 more than UI, because in UI areaLine is only 1 serie, but
                                  //2 series need to be passed for drawing

    CheckBox[] serieCheckBoxes = new CheckBox[seriesCountUI];
    readonly NumberScrollBar[] minNumberScrollBars = new NumberScrollBar[seriesCountUI];
    readonly NumberScrollBar[] maxNumberScrollBars = new NumberScrollBar[seriesCountUI];


    public Chart1Plot1X1YWindow() {
      Title = "Test Chart1Plot1X1YLegend";

      //use wpfControlTestbench as content of Chart1Plot1X1YLegendWindow
      TestBench testbench = new ();
      Content = testbench;

      //test Chart1Plot1X1YLegendTraced in testbench
      chart1Plot1X1YLegendTraced = new Chart1Plot1X1YLegendTraced();
      testbench.TestControl = chart1Plot1X1YLegendTraced;

      //add controls to set Chart1Plot1X1YLegend specific properties
      Grid grid = new();
      testbench.TestProperties = grid;

      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Auto });
      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Auto });
      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=new GridLength(125) });
      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=new GridLength(5) });
      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Auto });
      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=new GridLength(125) });
      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Auto });
      grid.ColumnDefinitions.Add(new ColumnDefinition { Width=new GridLength(1, GridUnitType.Star) });

      grid.RowDefinitions.Add(new RowDefinition { Height=GridLength.Auto });
      grid.RowDefinitions.Add(new RowDefinition { Height=GridLength.Auto });
      grid.RowDefinitions.Add(new RowDefinition { Height=GridLength.Auto });
      grid.RowDefinitions.Add(new RowDefinition { Height=GridLength.Auto });
      grid.RowDefinitions.Add(new RowDefinition { Height=GridLength.Auto });
      grid.RowDefinitions.Add(new RowDefinition { Height=GridLength.Auto });
      grid.RowDefinitions.Add(new RowDefinition { Height=GridLength.Auto });
      grid.RowDefinitions.Add(new RowDefinition { Height=new GridLength(1, GridUnitType.Star) });

      //Styles
      Style textBoxStyle = new (typeof(TextBox));
      textBoxStyle.Setters.Add(new Setter(TextBox.VerticalAlignmentProperty, VerticalAlignment.Center));
      textBoxStyle.Setters.Add(new Setter(TextBox.HorizontalAlignmentProperty, HorizontalAlignment.Left));
      textBoxStyle.Setters.Add(new Setter(TextBox.MarginProperty, new Thickness(4, 2, 0, 2)));
      grid.Resources.Add(typeof(TextBox), textBoxStyle);

      Style textBlockStyle = new (typeof(TextBlock));
      textBlockStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
      textBlockStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left));
      grid.Resources.Add(typeof(TextBlock), textBlockStyle);

      Style labelStyle = new (typeof(Label));
      labelStyle.Setters.Add(new Setter(Label.VerticalAlignmentProperty, VerticalAlignment.Center));
      labelStyle.Setters.Add(new Setter(Label.HorizontalAlignmentProperty, HorizontalAlignment.Left));
      grid.Resources.Add(typeof(Label), labelStyle);

      //Row0: startDate, time
      startDateDatePicker = new DatePicker { SelectedDate = DateTime.Today };
      startDateDatePicker.SelectedDateChanged += StartDatePicker_SelectedDateChanged;
      Label startDateLabel = new() { Content = "_Start Date", Target = startDateDatePicker };

      timeNumberScrollBar = new NumberScrollBar { Value = 1, DecimalPlaces = 0, Minimum = 1, Maximum = 1440, SmallChange = 1, LargeChange = 60 };
      timeNumberScrollBar.ValueChanged += NumberScrollBar_ValueChanged;
      Label timeLabel = new() { Content = "_Time", Target = timeNumberScrollBar };
      TextBlock timeTextBlock = new() { Text = "minutes" };

      int row = 0;
      int column = 1;
      addToGrid(grid, row, column++, startDateLabel);
      addToGrid(grid, row, column++, startDateDatePicker);
      column++;
      addToGrid(grid, row, column++, timeLabel);
      addToGrid(grid, row, column++, timeNumberScrollBar);
      addToGrid(grid, row, column++, timeTextBlock);


      //Row 1: Steps, Increment
      stepsNumberScrollBar = new NumberScrollBar { Value = 3000, Minimum = 1, Maximum = 100000, SmallChange = 30, LargeChange = 300 };
      stepsNumberScrollBar.ValueChanged += NumberScrollBar_ValueChanged;
      Label stepsLabel = new() { Content = "_Steps", Target = stepsNumberScrollBar };

      incrementNumberScrollBar = new NumberScrollBar { Value = 1, Minimum = -10, Maximum = 500, SmallChange = 1, LargeChange = 200 };
      incrementNumberScrollBar.ValueChanged += NumberScrollBar_ValueChanged;
      Label incrementLabel = new() { Content = "_Increment", Target = incrementNumberScrollBar };
      TextBlock incrementTextBlock = new() {Text = "minutes"};

      row++; column = 1;
      addToGrid(grid, row, column++, stepsLabel);
      addToGrid(grid, row, column++, stepsNumberScrollBar);
      column++;
      addToGrid(grid, row, column++, incrementLabel);
      addToGrid(grid, row, column++, incrementNumberScrollBar);
      addToGrid(grid, row, column++, incrementTextBlock);

      //Row 2..: Series with selection Checkbox and min/max values A-->
      addDataSerie(0, row++, 0, 100, grid);
      addDataSerie(1, row++, 60, 60, grid);
      addDataSerie(2, row++, 20, 50, grid);

      TextBlock lineAreaTextBlock = new() {Text = "Style: LineArea"};
      addToGrid(grid, row, 6, lineAreaTextBlock);

      updateParameters();
    }


    private void addDataSerie(int serieIndex, int row, int min, int max, Grid grid) {
      DockPanel serieDockPanel = new DockPanel { HorizontalAlignment = HorizontalAlignment.Right, Margin=new Thickness(0, 0, 5, 0) };
      serieCheckBoxes[serieIndex] = new CheckBox { IsChecked = true, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left };
      serieCheckBoxes[serieIndex].Click += CheckBox_Click;
      Label serieLabel = new Label { Content = "Serie _" + serieIndex, Target = serieCheckBoxes[serieIndex] };
      serieDockPanel.Children.Add(serieLabel);
      serieDockPanel.Children.Add(serieCheckBoxes[serieIndex]);

      minNumberScrollBars[serieIndex] = new NumberScrollBar { Value = min, Minimum = -11000, Maximum = 11000, SmallChange = 10, LargeChange = 100 };
      minNumberScrollBars[serieIndex].ValueChanged += NumberScrollBar_ValueChanged;
      Label minLabel = new() { Content = "Mi_n", Target = minNumberScrollBars[serieIndex] };

      maxNumberScrollBars[serieIndex] = new NumberScrollBar { Value = max, Minimum = -11000, Maximum = 11000, SmallChange = 10, LargeChange = 100 };
      maxNumberScrollBars[serieIndex].ValueChanged += NumberScrollBar_ValueChanged;
      Label maxLabel = new() { Content = "Ma_x", Target = maxNumberScrollBars[serieIndex] };

      row++; int column = 0;
      addToGrid(grid, row, column++, serieDockPanel);
      addToGrid(grid, row, column++, minLabel);
      addToGrid(grid, row, column++, minNumberScrollBars[serieIndex]);
      column++;
      addToGrid(grid, row, column++, maxLabel);
      addToGrid(grid, row, column++, maxNumberScrollBars[serieIndex]);
    }


    private static void addToGrid(Grid grid, int row, int column, UIElement uIElement) {
      grid.Children.Add(uIElement);
      Grid.SetRow(uIElement, row);
      Grid.SetColumn(uIElement, column++);
    }


    void NumberScrollBar_ValueChanged(object? sender, ValueChangedEventArgs<double> e) {
      updateParameters();
    }


    private void StartDatePicker_SelectedDateChanged(object? sender, SelectionChangedEventArgs e) {
      updateParameters();
    }


    private void CheckBox_Click(object? sender, RoutedEventArgs e) {
      updateParameters();
    }


    /// <summary>
    /// Stores the data to be displayed in Chart1Plot1X1YLegend for one point in time (=record)
    /// </summary>
    class DataRecord(DateTime date, double[] values) {
      public DateTime Date { get; private set; } = date;
      public double[] Values { get; private set; } = values;

      public override string ToString() {
        string returnString = "Date " + Date;
        for (int valuesIndex = 0; valuesIndex < Values.Length; valuesIndex++) {
          returnString += "; " + valuesIndex + ": " + Values[valuesIndex];
        }
        return returnString;
      }
    }


    const int areaLineIndex = 2;


    /// <summary>
    /// whenever a setup parameter changes, regenerate the test data and refresh 
    /// </summary>
    private void updateParameters() {
      DateTime time = startDateDatePicker.SelectedDate!.Value;
      double minutes = timeNumberScrollBar.Value;
      int stepsCount = (int)stepsNumberScrollBar.Value;
      var dataRecords = new DataRecord[stepsCount];

      //count selected series and prepare values for data calculation
      var serieValues = new double[seriesCountDraw];
      var increments = new double[seriesCountDraw];
      int selectSeriesCount = 0;
      for (int seriesUIIndex = 0; seriesUIIndex < seriesCountUI; seriesUIIndex++) {
        if (serieCheckBoxes[seriesUIIndex].IsChecked!.Value) {
          serieValues[selectSeriesCount] = minNumberScrollBars[seriesUIIndex].Value;
          increments[selectSeriesCount] = (maxNumberScrollBars[seriesUIIndex].Value - serieValues[selectSeriesCount]) / (stepsCount-1);
          selectSeriesCount++;
          if (seriesUIIndex==areaLineIndex) {
            //areaLine needs 2 series for drawing
            serieValues[selectSeriesCount] = minNumberScrollBars[seriesUIIndex].Value * 0.8;
            increments[selectSeriesCount] = (maxNumberScrollBars[seriesUIIndex].Value * 0.9 - serieValues[selectSeriesCount]) / (stepsCount-1);
            selectSeriesCount++;
          }
        }
      }

      //setup line settings
      var seriesSettings = new SerieSetting<DataRecord>[selectSeriesCount];
      int seriesSettingsIndex = 0;
      for (int seriesUIIndex = 0; seriesUIIndex < seriesCountUI; seriesUIIndex++) {
        if (serieCheckBoxes[seriesUIIndex].IsChecked!.Value) {

          seriesSettings[seriesSettingsIndex++] = seriesUIIndex switch {
            0 => new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.line, Brushes.Green, 2, null),
            1 => new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.line, Brushes.Blue, 2, null),
            2 => new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.area1, Brushes.Gray, 1, null),
            //3 => new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.area2, null!, 1, null),
            _ => throw new NotSupportedException($"seriesUIIndex: {seriesUIIndex}"),
          };
          if (seriesUIIndex==areaLineIndex) {
            seriesSettings[seriesSettingsIndex++] = new SerieSetting<DataRecord>(getSeriesData, SerieStyleEnum.area2, null!, 1, null);
          }


          //int lambdaIndex = seriesSettingsIndex; //we need to create a new instance within the loop, otherwise the lambda expression will use the latest value of seriesSettingsIndex (i.e. max(seriesSettingsIndex)), see C# reference "Outer Variables"
          //if (seriesUIIndex==areaLineIndex) {
          //  //an area graphic has 2 lines and needs two seriesSettings
          //  var areaBrush = seriesBrushes[seriesUIIndex]!;
          //  seriesSettings[seriesSettingsIndex] = new SerieSetting<DataRecord>(getSeries1Data, SerieStyleEnum.area1, areaBrush, 1, null);
          //  seriesSettingsIndex++;
          //  int lambdaIndex2 = lambdaIndex+1;
          //  seriesSettings[seriesSettingsIndex] = new SerieSetting<DataRecord>(record => [record.Date.ToDouble(), record.Values[lambdaIndex2]],
          //    SerieStyleEnum.area2, areaBrush, 1, null);
          //  seriesSettingsIndex++;
          //} else {
          //  seriesSettings[seriesSettingsIndex] = new SerieSetting<DataRecord>(record => [record.Date.ToDouble(), record.Values[lambdaIndex]],
          //    SerieStyleEnum.line, seriesBrushes[seriesUIIndex]!, 2, null);
          //  seriesSettingsIndex++;
          //}
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

      chart1Plot1X1YLegendTraced.FillData<DataRecord>(dataRecords, seriesSettings);
    }


    private static void getSeriesData(DataRecord dataRecord, int index, [NotNull] ref double[]? dataExtracted) {
      dataExtracted ??= new double[2];
      dataExtracted[0] = dataRecord.Date.ToDouble();
      dataExtracted[1] = dataRecord.Values[index];
    }
  }
}

