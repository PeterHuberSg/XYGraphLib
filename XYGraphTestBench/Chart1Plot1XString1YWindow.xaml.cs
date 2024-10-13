using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTestbench;
using XYGraphLib;
using static XYGraphLib.Chart4Plots1X4YLegendsWindow;


namespace XYGraphTestBench {


  /// <summary>
  /// Interaction logic for Chart1Plot1XString1YWindow.xaml
  /// </summary>
  public partial class Chart1Plot1XString1YWindow: Window {


    ///// <summary>
    ///// Creates and opens a new Chart1Plot1XString1YWindow
    ///// </summary>
    public static void Show(Window ownerWindow) {
      new Chart1Plot1XString1YWindow { Owner = ownerWindow }.Show();
    }


    LegendXString legendXString;


    public Chart1Plot1XString1YWindow() {
      InitializeComponent();

      legendXString = (LegendXString)TestChart1Plot1XString1YLegend.LegendScrollerX.Legend;
      NumberOfStringsNumberScrollBar.Value = legendXString.LegendStrings.Count;
      MaxStringLenghtNumberScrollBar.Value = 10;//there is not property for this value

      updateParameters();
      NumberOfStringsNumberScrollBar.ValueChanged += NumberScrollBar_ValueChanged;
      MaxStringLenghtNumberScrollBar.ValueChanged += NumberScrollBar_ValueChanged;
    }


    private void NumberScrollBar_ValueChanged(object? sender, WpfTestbench.ValueChangedEventArgs<double> e) {
      TraceWPFEvents.TraceLineStart($"NumberScrollBar ValueChanged StringsCount: {NumberOfStringsNumberScrollBar.Value}; StringsLength: {MaxStringLenghtNumberScrollBar.Value}");
      updateParameters();
      TraceWPFEvents.TraceLineEnd("NumberScrollBar ValueChanged");
    }

    [NotNull]
    (double x, double y, string label)[] stringsLengths;


    /// <summary>
    /// whenever a setup parameter changes, regenerate the test data and refresh 
    /// </summary>
    private void updateParameters() {
      var strings = RandomText.GetStrings(stringsCount: (int)NumberOfStringsNumberScrollBar.Value,
        maxStringLength: (int)MaxStringLenghtNumberScrollBar.Value);
      //legendXString.LegendStrings = strings;

      ////add new line in middle of string for testing
      //for (var stringsIndex=0; stringsIndex<strings.Length; stringsIndex++) {
      //  if (strings[stringsIndex].Length>4) {
      //    strings[stringsIndex] = strings[stringsIndex][..4] + Environment.NewLine + strings[stringsIndex][4..];
      //  }
      //}

      stringsLengths = new (double x, double y, string label)[strings.Length];
      for (int i = 0; i < strings.Length; i++) {
        var label = strings[i];
        stringsLengths[i] = (i, label.Length, label);
      }
      TestChart1Plot1XString1YLegend.FillData(stringsLengths, 
        [new SerieSetting<(double x, double y, string label)>(getSeriesData, SerieStyleEnum.line, Brushes.Blue, 2, null)],
        getSeriesLabel);
    }


    private static void getSeriesData((double x, double y, string _) dataRecord, int _, [NotNull] ref double[]? dataExtracted) {
      dataExtracted ??= new double[2];
      dataExtracted[0] = dataRecord.x;
      dataExtracted[1] = dataRecord.y;
    }


    private static string getSeriesLabel((double x, double y, string label) dataRecord) {
      return dataRecord.label;
    }
  }
}
