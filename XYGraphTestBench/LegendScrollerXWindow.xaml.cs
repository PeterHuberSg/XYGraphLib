using System.Windows;
using WpfTestbench;
using System;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for LegendScrollerX
  /// </summary>
  public partial class LegendScrollerXWindow: TestbenchWindow {
    
    
    /// <summary>
    /// Creates and opens a new LegendScrollerXWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      ShowProtected( () => new LegendScrollerXWindow(), ownerWindow);
    }


    /// <summary>
    /// Default constructor
    /// </summary>
    public LegendScrollerXWindow() {
      InitializeComponent();
    }


    const bool up = true;
    const bool down = false;
    const bool zoomIn = true;
    const bool zoomOut = false;
    const bool zoomActive = true;
    const bool zoomOff = false;


    protected override Func<Action>[] InitTestFuncs() {
      return new Func<Action>[] {

        //            Height  MaxFV     DispR
        //                minFV    DispV

        () => testVals(400,  0, 99,   0,  10, new double[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}),
        () => testVals(400,  0, 99,   0,  20, new double[]{0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20}),
        () => testVals(400,  0, 99,   0,  30, new double[]{0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30}),
        () => testVals(400,  0, 99,   0,  50, new double[]{0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50}),
        () => testVals(400,  0, 99,   0,  80, new double[]{0, 10, 20, 30, 40, 50, 60, 70, 80}),
        () => testVals(400,  0, 99,   0,  99, new double[]{0, 10, 20, 30, 40, 50, 60, 70, 80, 90}),

        () => testVals(400, 0, 99,   0,   5, new double[]{0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0}),
        () => testVals(400, 0, 99,   0,   4, new double[]{0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0}),
        () => testVals(400, 0, 99,   0,   3, new double[]{0, 0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0}),
        () => testVals(400, 0, 99,   0,   2, new double[]{0, 0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0}),
        () => testVals(400, 0, 99,   0,   1, new double[]{0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0}),
        () => testVals(400, 0, 99,   0, 0.5, new double[]{0.00, 0.05, 0.10, 0.15, 0.20, 0.25, 0.30, 0.35, 0.40, 0.45, 0.50}),
        () => testVals(400, 0, 99,   0, 0.3, new double[]{0.0, 0.05, 0.10, 0.15, 0.20, 0.25, 0.30}),
        () => testVals(400, 0, 99,   0, 0.2, new double[]{0.0, 0.02, 0.04, 0.06, 0.08, 0.10, 0.12, 0.14, 0.16, 0.18, 0.2}),
        () => testVals(400, 0, 99,   0, 0.1, new double[]{0.00, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.10}),

        () => testVals(400, 0, 99,   0.4,   1, new double[]{0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.1, 1.2, 1.3, 1.4}),
        () => testVals(400, 0, 99,   0.5,   1, new double[]{0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.1, 1.2, 1.3, 1.4, 1.5}),
        () => testVals(400, 0, 99,   0.6,   1, new double[]{0.6, 0.7, 0.8, 0.9, 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6}),
        () => testVals(400, 0, 99,   0.9,   1, new double[]{0.9, 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9}),
        () => testVals(400, 0, 99,     1,   1, new double[]{1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0}),

        () => testVals(400, 0, 99,   8,   5, new double[]{8, 8.5, 9.0, 9.5, 10, 10.5, 11.0, 11.5, 12, 12.5, 13.0}),
        () => testVals(400, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),

        () => testVals(235, 0, 99,  94,   5, new double[]{94, 95.0, 96, 97.0, 98, 99}),
        () => testVals(241, 0, 99,  94,   5, new double[]{94, 95.0, 96, 97.0, 98, 99}),
        () => testVals(247, 0, 99,  94,   5, new double[]{94, 95.0, 96, 97.0, 98, 99}),
        () => testVals(253, 0, 99,  94,   5, new double[]{94, 95.0, 96, 97.0, 98, 99}),
        () => testVals(259, 0, 99,  94,   5, new double[]{94, 95.0, 96, 97.0, 98, 99}),
        () => testVals(265, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(271, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(277, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(283, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(289, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(295, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(301, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(307, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(313, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),
        () => testVals(319, 0, 99,  94,   5, new double[]{94, 94.5, 95.0, 95.5, 96, 96.5, 97.0, 97.5, 98, 98.5, 99}),

        () => testVals(400,  0, 1000000,          0,  1000000, new double[]{0, 200000, 400000, 600000, 800000, 1000000}),
        () => testVals(400, -1000000, 0,   -1000000,  1000000, new double[]{-1000000, -800000, -600000, -400000, -200000, 0}),
        () => testVals(400, - 900000, 0,    -900000,   900000, new double[]{-800000, -600000, -400000, -200000, 0}),
        () => testVals(400, - 900000, 0,    -899999,   899998, new double[]{-800000, -600000, -400000, -200000, 0}),

        () => testVals(400,  0,  0,   0,   0, new double[]{-1, -0.8, -0.6, -0.4, -0.2, 0, 0.2, 0.4, 0.6, 0.8, 1}), //range 0 must be increased to display something
        () => testVals(400, 10, 10,  10,   0, new double[]{0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20}),
        () => testVals(400, -1, -1,  -1,   0, new double[]{-2, -1.8, -1.6, -1.4, -1.2, -1.0, -0.8, -0.6, -0.4, -0.2,  0}),

        //test zoom
        () => testVals(400, 0, 99,   0,  99, new double[]{0, 10, 20, 30, 40, 50, 60, 70, 80, 90}),
        () => testZoom(zoomIn,  zoomActive, new double[]{0, 5, 10, 15, 20, 25, 30}),
        () => testZoom(zoomIn,  zoomActive, new double[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9}),
        () => testZoom(zoomIn,  zoomActive, new double[]{0, 0.5, 1, 1.5, 2, 2.5, 3}),
        () => testZoom(zoomOut, zoomActive, new double[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9}),
        () => testZoom(zoomOut, zoomActive, new double[]{0, 5, 10, 15, 20, 25, 30}),
        () => testZoom(zoomOut, zoomOff,    new double[]{0, 10, 20, 30, 40, 50, 60, 70, 80, 90}),
        () => testZoom(zoomOut, zoomOff,    new double[]{0, 10, 20, 30, 40, 50, 60, 70, 80, 90}),

        //test paging
        () => testVals(400, 0, 35,   0,  10, new double[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}),
        () => testPage(up,   new double[]{10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20}),
        () => testPage(up,   new double[]{20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30}),
        () => testPage(up,   new double[]{25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35}),
        () => testPage(up,   new double[]{25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35}),
        () => testPage(down, new double[]{15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25}),
        () => testPage(down, new double[]{5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}),
        () => testPage(down, new double[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}),
        () => testPage(down, new double[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}),
      };
    }


    private Action testVals(double width, double minFullValue, double maxFullValue, double displayValue, double displayRange, double[] labels) {
      TestLegendXScroller.Width = width;
      MinValueNumberScrollBar.Value = minFullValue;
      MaxValueNumberScrollBar.Value  = maxFullValue;
      DisplayValueNumberScrollBar.Value = displayValue;
      DisplayRangeNumberScrollBar.Value = displayRange;

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        verify(labels);
      };
    }


    private Action testZoom(bool isZoomIn, bool isZoomActive, double[] labels) {
      if (isZoomIn) {
        TestLegendXScroller.ZoomIn();
      } else {
        TestLegendXScroller.ZoomOut();
      }

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        if (isZoomActive!=TestLegendXScroller.CanZoomOut) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("IsZoomActive should be " + isZoomActive + ".");
        }
        verify(labels);
      };
    }


    private Action testPage(bool pageUp, double[] labels) {
      if (pageUp) {
        TestLegendXScroller.DisplayValue += TestLegendXScroller.Legend.DisplayValueRange;
      } else {
        TestLegendXScroller.DisplayValue -= TestLegendXScroller.Legend.DisplayValueRange;
      }

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        verify(labels);
      };
    }


    private void verify(double[] labels) {
      LegendX legend = (LegendX)TestLegendXScroller.Legend;
      if (labels.Length!=legend.LabelValues.Length) {
        System.Diagnostics.Debugger.Break();
        throw new Exception("There should be " + labels.Length + " legends, but there were " + legend.LabelValues.Length + ".");
      }
      for (int legendValueIndex = 0; legendValueIndex < labels.Length; legendValueIndex++) {
        if (labels[legendValueIndex]!=Math.Round(legend.LabelValues[legendValueIndex], 3)) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("Legend[" + legendValueIndex + "] should be '" + labels[legendValueIndex] + 
            "' but was '" + Math.Round(legend.LabelValues[legendValueIndex], 3) + "'");
        }
      }
    }

  }
}
