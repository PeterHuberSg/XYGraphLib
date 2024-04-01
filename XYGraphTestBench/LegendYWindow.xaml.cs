using System.Windows;
using WpfTestbench;
using System;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for LegendY
  /// </summary>
  public partial class LegendYWindow: Window {

    ///// <summary>
    ///// Creates and opens a new LegendYWindow
    ///// </summary>
    public static void Show(Window ownerWindow) {
      new LegendYWindow { Owner = ownerWindow }.Show();
    }


    /// <summary>
    /// Default constructor
    /// </summary>
    public LegendYWindow() {
      InitializeComponent();

      //TestBench.TestFunctions.Add(("H:100, minV:0, maxV:99, disp V:0, R:10", () => testFunc(100, 0, 99, 0, 10, [0, 5, 10])));
      //      Height  MaxFV     DispR
      //          minFV    DispV
      addTest(100, 0, 99,   0,  10, [0,  5, 10]);
      addTest(100, 0, 99,   0,  20, [0, 10, 20]);
      addTest(100, 0, 99,   0,  30, [0, 20]);
      addTest(100, 0, 99,   0,  50, [0, 20, 40]);
      addTest(100, 0, 99,   0,  80, [0, 50]);

      addTest(100, 0, 99,   0,   5, [0, 2, 4]);
      addTest(100, 0, 99,   0,   4, [0, 2, 4]);
      addTest(100, 0, 99,   0,   3, [0, 2]);
      addTest(100, 0, 99,   0,   2, [0, 1, 2]);
      addTest(100, 0, 99,   0,   1, [0.0, 0.5, 1.0]);
      addTest(100, 0, 99,   0, 0.5, [0.0, 0.2, 0.4]);
      addTest(100, 0, 99,   0, 0.3, [0.0, 0.2]);
      addTest(100, 0, 99,   0, 0.2, [0.0, 0.1, 0.2]);
      addTest(100, 0, 99,   0, 0.1, [0.00, 0.05, 0.10]);

      addTest(100, 0, 99,   0.4,   1, [0.5, 1.0]);
      addTest(100, 0, 99,   0.5,   1, [0.5, 1.0, 1.5]);
      addTest(100, 0, 99,   0.6,   1, [1.0, 1.5]);
      addTest(100, 0, 99,   0.9,   1, [1.0, 1.5]);
      addTest(100, 0, 99,     1,   1, [1.0, 1.5, 2.0]);

      addTest(100, 0, 99,   8,   5, [8, 10,  12]);
      addTest(100, 0, 99,  94,   5, [94, 96, 98]);

      addTest(103, 0, 99,  94,   5, [94, 96, 98]);
      addTest(109, 0, 99,  94,   5, [94, 96, 98]);
      addTest(115, 0, 99,  94,   5, [94, 96, 98]);
      addTest(121, 0, 99,  94,   5, [94, 96, 98]);
      addTest(127, 0, 99,  94,   5, [94, 96, 98]);
      addTest(133, 0, 99,  94,   5, [94, 96, 98]);
      addTest(139, 0, 99,  94,   5, [94, 96, 98]);
      addTest(145, 0, 99,  94,   5, [94, 96, 98]);
      addTest(151, 0, 99,  94,   5, [94, 96, 98]);
      addTest(157, 0, 99,  94,   5, [94, 96, 98]);
      addTest(163, 0, 99,  94,   5, [94, 96, 98]);
      addTest(169, 0, 99,  94,   5, [94, 96, 98]);
      addTest(175, 0, 99,  94,   5, [94, 96, 98]);
      addTest(181, 0, 99,  94,   5, [94, 95, 96, 97, 98, 99]);
      addTest(187, 0, 99,  94,   5, [94, 95, 96, 97, 98, 99]);
      addTest(193, 0, 99,  94,   5, [94, 95, 96, 97, 98, 99]);

      addTest(100,  0, 1000000,          0,  1000000, [0,  500000, 1000000]);
      addTest(100, -1000000, 0,   -1000000,  1000000, [-1000000,  -500000, 0]);
      addTest(100, -900000, 0,   -900000,  900000, [-500000, 0]);

      addTest(100, 0, 99,   0,  10, [0,  5, 10]);
    }



    //protected override Func<Action>[] InitTestFuncs() {
    //  return new Func<Action>[] {
    //    //            Height  MaxFV     DispR
    //    //                minFV    DispV
    //    () => testFunc(100, 0, 99,   0,  10, new double[]{0,  5, 10}),
    //    () => testFunc(100, 0, 99,   0,  20, new double[]{0, 10, 20}),
    //    () => testFunc(100, 0, 99,   0,  30, new double[]{0, 20}),
    //    () => testFunc(100, 0, 99,   0,  50, new double[]{0, 20, 40}),
    //    () => testFunc(100, 0, 99,   0,  80, new double[]{0, 50}),

    //    () => testFunc(100, 0, 99,   0,   5, new double[]{0, 2, 4}),
    //    () => testFunc(100, 0, 99,   0,   4, new double[]{0, 2, 4}),
    //    () => testFunc(100, 0, 99,   0,   3, new double[]{0, 2}),
    //    () => testFunc(100, 0, 99,   0,   2, new double[]{0, 1, 2}),
    //    () => testFunc(100, 0, 99,   0,   1, new double[]{0.0, 0.5, 1.0}),
    //    () => testFunc(100, 0, 99,   0, 0.5, new double[]{0.0, 0.2, 0.4}),
    //    () => testFunc(100, 0, 99,   0, 0.3, new double[]{0.0, 0.2}),
    //    () => testFunc(100, 0, 99,   0, 0.2, new double[]{0.0, 0.1, 0.2}),
    //    () => testFunc(100, 0, 99,   0, 0.1, new double[]{0.00, 0.05, 0.10}),

    //    () => testFunc(100, 0, 99,   0.4,   1, new double[]{0.5, 1.0}),
    //    () => testFunc(100, 0, 99,   0.5,   1, new double[]{0.5, 1.0, 1.5}),
    //    () => testFunc(100, 0, 99,   0.6,   1, new double[]{1.0, 1.5}),
    //    () => testFunc(100, 0, 99,   0.9,   1, new double[]{1.0, 1.5}),
    //    () => testFunc(100, 0, 99,     1,   1, new double[]{1.0, 1.5, 2.0}),

    //    () => testFunc(100, 0, 99,   8,   5, new double[]{8, 10,  12}),
    //    () => testFunc(100, 0, 99,  94,   5, new double[]{94, 96, 98}),

    //    () => testFunc(103, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(109, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(115, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(121, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(127, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(133, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(139, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(145, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(151, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(157, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(163, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(169, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(175, 0, 99,  94,   5, new double[]{94, 96, 98}),
    //    () => testFunc(181, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
    //    () => testFunc(187, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
    //    () => testFunc(193, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),

    //    () => testFunc(100,  0, 1000000,          0,  1000000, new double[]{0,  500000, 1000000}),
    //    () => testFunc(100, -1000000, 0,   -1000000,  1000000, new double[]{-1000000,  -500000, 0}),
    //    () => testFunc(100, -900000, 0,   -900000,  900000, new double[]{-500000, 0}),

    //    () => testFunc(100, 0, 99,   0,  10, new double[]{0,  5, 10})
    //  };
    //}


    //      TestBench.TestFunctions.Add(("H:100, minV:0, maxV:99, disp V:0, R:10", () => testFunc(100, 0, 99, 0, 10, [0, 5, 10])));
    void addTest(double height, double minFullValue, double maxFullValue, double displayValue, double displayRange, double[] labels) {
      TestBench.TestFunctions.Add(($"H:{height}, minV:{minFullValue}, maxV:{maxFullValue}, disp V:{displayValue}, R:{displayRange}",
        () => testFunc(height, minFullValue, maxFullValue, displayValue, displayRange, labels)));
    }


    Action? testFunc(double height, double minFullValue, double maxFullValue, double displayValue, double displayRange, double[] labels) {
      TestLegendYTraced.Height = height;
      MinFullValueNumberScrollBar.Value = minFullValue;
      MaxFullValueNumberScrollBar.Value  = maxFullValue;
      DisplayValueNumberScrollBar.Value = displayValue;
      DisplayRangeNumberScrollBar.Value = displayRange;

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        if (labels.Length!=(TestLegendYTraced.LabelValues is null ? 0 : TestLegendYTraced.LabelValues.Length)) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("There should be " + labels.Length + " legends, but there were " + 
            (TestLegendYTraced.LabelValues is null ? 0 : TestLegendYTraced.LabelValues.Length) + ".");
        } else {
          for (int legendValueIndex = 0; legendValueIndex < labels.Length; legendValueIndex++) {
            if (labels[legendValueIndex]!=Math.Round(TestLegendYTraced.LabelValues![legendValueIndex], 3)) {
              System.Diagnostics.Debugger.Break();
              throw new Exception("Legend[" + legendValueIndex + "] should be '" + labels[legendValueIndex] +
                "' but was '" + Math.Round(TestLegendYTraced.LabelValues[legendValueIndex], 3) + "'");
            }
          }
        }
      };
    }
  }
}
