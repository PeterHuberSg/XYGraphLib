using System.Windows;
using WpfTestbench;
using System;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for LegendX
  /// </summary>
  public partial class LegendXWindow: TestbenchWindow {
    
    
    /// <summary>
    /// Creates and opens a new LegendXWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      ShowProtected( () => new LegendXWindow(), ownerWindow);
    }


    /// <summary>
    /// Default constructor
    /// </summary>
    public LegendXWindow() {
      InitializeComponent();
    }



    protected override Func<Action>[] InitTestFuncs() {
      return new Func<Action>[] {
        //            Width MaxFV     DispR
        //                minFV    DispV
        () => testFunc(100, 0, 99,   0,  10, new double[]{0, 5, 10}),
        () => testFunc(100, 0, 99,   0,  20, new double[]{0, 10, 20}),
        () => testFunc(100, 0, 99,   0,  30, new double[]{0, 10, 20, 30}),
        () => testFunc(100, 0, 99,   0,  50, new double[]{0, 20, 40}),
        () => testFunc(100, 0, 99,   0,  80, new double[]{0, 50}),

        () => testFunc(100, 0, 99,   0,   5, new double[]{0, 2, 4}),
        () => testFunc(100, 0, 99,   0,   4, new double[]{0, 2, 4}),
        () => testFunc(100, 0, 99,   0,   3, new double[]{0, 1, 2, 3}),
        () => testFunc(100, 0, 99,   0,   2, new double[]{0, 1, 2}),
        () => testFunc(100, 0, 99,   0,   1, new double[]{0.0, 0.5, 1.0}),
        () => testFunc(100, 0, 99,   0, 0.5, new double[]{0.0, 0.2, 0.4}),
        () => testFunc(100, 0, 99,   0, 0.3, new double[]{0.0, 0.2}),
        () => testFunc(100, 0, 99,   0, 0.2, new double[]{0.0, 0.1, 0.2}),
        () => testFunc(100, 0, 99,   0, 0.1, new double[]{0.00, 0.05, 0.10}),

        () => testFunc(100, 0, 99,   0.4,   1, new double[]{0.5, 1.0}),
        () => testFunc(100, 0, 99,   0.5,   1, new double[]{0.5, 1.0, 1.5}),
        () => testFunc(100, 0, 99,   0.6,   1, new double[]{1.0, 1.5}),
        () => testFunc(100, 0, 99,   0.9,   1, new double[]{1.0, 1.5}),
        () => testFunc(100, 0, 99,     1,   1, new double[]{1.0, 1.5, 2.0}),

        () => testFunc(100, 0, 99,   8,   5, new double[]{8, 10, 12}),

        () => testFunc( 73, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc( 79, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc( 85, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc( 91, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc( 97, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc(103, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc(109, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc(115, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc(121, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc(127, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testFunc(133, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(139, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(145, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(151, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(157, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(163, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(169, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(175, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(181, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(187, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(193, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(200, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(207, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(214, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(221, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(228, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(235, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(242, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(249, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(256, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testFunc(263, 0, 99,  94,   5, new double[]{94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99}),
        () => testFunc(270, 0, 99,  94,   5, new double[]{94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99}),
        () => testFunc(277, 0, 99,  94,   5, new double[]{94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99}),
        () => testFunc(284, 0, 99,  94,   5, new double[]{94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99}),
        () => testFunc(291, 0, 99,  94,   5, new double[]{94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99}),
        () => testFunc(292, 0, 99,  94,   5, new double[]{94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99}),

        () => testFunc(125,  0, 1000000,          0,  1000000, new double[]{0}),
        () => testFunc(125,  0, 1000000,          1,  1000000, new double[]{1}),
        () => testFunc(129,  0, 1000000,          0,  1000000, new double[]{0, 500000, 1000000}),
        () => testFunc(129,  0, 1000000,          1,  999999, new double[]{500000, 1000000}),
        () => testFunc(129,  0, 1000000,          1,   999998, new double[]{1}), //there is only space for the 50000 label. If only 1 label, the first value gets displayed
        () => testFunc(140, -1000000, 0,   -1000000,  1000000, new double[]{-1000000, -500000, 0}),
        () => testFunc(140,  -900000, 0,    -900000,   900000, new double[]{-500000, 0}),

        () => testFunc(140, -1000000, -100000, -900000,  100000, new double[]{-900000, -850000, -800000}),
        () => testFunc(140, -1000000, -100000, -900000,  200000, new double[]{-900000, -800000, -700000}),
        () => testFunc(140, -1000000, -100000, -900000,  600000, new double[]{-900000}),//step: 500000. 1000000 is too big, 500000 too small =>
        () => testFunc(140, -1000000, -100000, -855555,  100000, new double[]{-850000, -800000}),
        () => testFunc(140, -1000000, -100000, -855555,  200000, new double[]{-800000, -700000}),
        () => testFunc(140, -1000000, -100000, -855555,  600000, new double[]{-855555}),//step: 500000. 1000000 is too big, 500000 too small =>

        () => testFunc(140, -1000000, 1000000, -900000,  1000000, new double[]{-500000, 0}),
        () => testFunc(140, -1000000, 1000000, -900000,  1800000, new double[]{-900000}),//step: 500000. 1000000 is too big, 500000 too small =>
        () => testFunc(140, -1000000, 1000000, -855555,  1000000, new double[]{-500000, 0}),

        () => testFunc(200, -0.000004, 0.000000, -0.0000001,     0.0000001, new double[]{-0.0000001, -0.00000005, 0}),
        () => testFunc(200, -0.000004, 0.000000, -0.0000001234,  0.0000001, new double[]{-0.0000001, -0.00000005}),
        () => testFunc(200, -0.000004, 0.000000, -0.0000001234,  0.0000005, new double[]{0, 0.0000002}),

        () => testFunc(200,  0.000000, 0.000004,  0.0000001,     0.0000001, new double[]{0.0000001, 0.00000015, 0.0000002}),
        () => testFunc(200,  0.000000, 0.000004,  0.0000001234,  0.0000001, new double[]{0.00000015, 0.0000002}),
        () => testFunc(200,  0.000000, 0.000004,  0.0000001234,  0.0000005, new double[]{0.0000002, 0.0000004, 0.0000006}),

        () => testFunc(100, 0, 99,   0,  10, new double[]{0,  5, 10})
      };
    }


    Action testFunc(double width, double minFullValue, double maxFullValue, double displayValue, double displayRange, double[] labels) {
      TestLegendXTraced.Width = width;
      MinFullValueNumberScrollBar.Value = minFullValue;
      MaxFullValueNumberScrollBar.Value  = maxFullValue;
      DisplayValueNumberScrollBar.Value = displayValue;
      DisplayRangeNumberScrollBar.Value = displayRange;

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        if (labels.Length!=TestLegendXTraced.LabelValues.Length) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("There should be " + labels.Length + " legends, but there were " + TestLegendXTraced.LabelValues.Length + ".");
        } else {
          for (int legendValueIndex = 0; legendValueIndex < labels.Length; legendValueIndex++) {
            if (labels[legendValueIndex]!=Math.Round(TestLegendXTraced.LabelValues[legendValueIndex], 8)) {
              System.Diagnostics.Debugger.Break();
              throw new Exception("Legend[" + legendValueIndex + "] should be '" + labels[legendValueIndex] + 
                "' but was '" + Math.Round(TestLegendXTraced.LabelValues[legendValueIndex], 8) + "'");
            }
          }
        }
      };
    }
  }
}
