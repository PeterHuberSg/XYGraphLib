using System;
using System.Collections.Generic;
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



    /// <summary>
    /// whenever a setup parameter changes, regenerate the test data and refresh 
    /// </summary>
    private void updateParameters() {
      var strings = RandomText.GetStrings(stringsCount: (int)NumberOfStringsNumberScrollBar.Value,
        maxStringLength: (int)MaxStringLenghtNumberScrollBar.Value);
      legendXString.LegendStrings = strings;

      double[][] stringsLengths = new double[strings.Length][];
      for (int i = 0; i < strings.Length; i++) {
        stringsLengths[i] = [i, strings[i].Length];
      }
      TestChart1Plot1XString1YLegend.FillData(stringsLengths, 
        [new SerieSetting<double[]>(i => i, SerieStyleEnum.line, Brushes.Blue, 2, null)]);
    }
  }
}
