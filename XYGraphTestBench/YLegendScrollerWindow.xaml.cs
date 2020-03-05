using System.Windows;
using WpfTestbench;
using System;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for YLegendScroller
  /// </summary>
  public partial class YLegendScrollerWindow: TestbenchWindow {
    
    
    /// <summary>
    /// Creates and opens a new YLegendScrollerWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      ShowProtected( () => new YLegendScrollerWindow(), ownerWindow);
    }


    /// <summary>
    /// Default constructor
    /// </summary>
    public YLegendScrollerWindow() {
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

        //() => testVals(100,  0, 99,   0,  10, new double[]{0,  5, 10}),
        //() => testVals(100,  0, 99,   0,  20, new double[]{0, 10, 20}),
        //() => testVals(100,  0, 99,   0,  30, new double[]{0, 20}),
        //() => testVals(100,  0, 99,   0,  50, new double[]{0, 20, 40}),
        //() => testVals(100,  0, 99,   0,  80, new double[]{0, 50}),

        //() => testVals(100, 0, 99,   0,   5, new double[]{0, 2, 4}),
        //() => testVals(100, 0, 99,   0,   4, new double[]{0, 2, 4}),
        //() => testVals(100, 0, 99,   0,   3, new double[]{0, 2}),
        //() => testVals(100, 0, 99,   0,   2, new double[]{0, 1, 2}),
        //() => testVals(100, 0, 99,   0,   1, new double[]{0.0, 0.5, 1.0}),
        //() => testVals(100, 0, 99,   0, 0.5, new double[]{0.0, 0.2, 0.4}),
        //() => testVals(100, 0, 99,   0, 0.3, new double[]{0.0, 0.2}),
        //() => testVals(100, 0, 99,   0, 0.2, new double[]{0.0, 0.1, 0.2}),
        //() => testVals(100, 0, 99,   0, 0.1, new double[]{0.00, 0.05, 0.10}),

        //() => testVals(100, 0, 99,   0.4,   1, new double[]{0.5, 1.0}),
        //() => testVals(100, 0, 99,   0.5,   1, new double[]{0.5, 1.0, 1.5}),
        //() => testVals(100, 0, 99,   0.6,   1, new double[]{1.0, 1.5}),
        //() => testVals(100, 0, 99,   0.9,   1, new double[]{1.0, 1.5}),
        //() => testVals(100, 0, 99,     1,   1, new double[]{1.0, 1.5, 2.0}),

        //() => testVals(100, 0, 99,   8,   5, new double[]{8, 10,  12}),
        //() => testVals(100, 0, 99,  94,   5, new double[]{94, 96, 98}),

        () => testVals(103, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(109, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(115, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(121, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(127, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(133, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(139, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(145, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(151, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(157, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(163, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(169, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(175, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(181, 0, 99,  94,   5, new double[]{94, 96, 98}),
        () => testVals(187, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),
        () => testVals(193, 0, 99,  94,   5, new double[]{94, 95, 96, 97, 98, 99}),

        () => testVals(100,  0, 1000000,          0,  1000000, new double[]{0,  500000, 1000000}),
        () => testVals(100, -1000000, 0,   -1000000,  1000000, new double[]{-1000000,  -500000, 0}),
        () => testVals(100, - 900000, 0,    -900000,   900000, new double[]{-500000, 0}),

        () => testVals(100,  0,  0,   0,   0, new double[]{-1,  0, 1}), //range 0 must be increased to display something
        () => testVals(100, 10, 10,  10,   0, new double[]{0,  10, 20}),
        () => testVals(100, -1, -1,  -1,   0, new double[]{-2, -1,  0}),

        //test zoom
        () => testVals(100, 0, 99,   0,  99, new double[]{0,  50}),
        () => testZoom(zoomIn,  zoomActive, new double[]{0,  20}),
        () => testZoom(zoomIn,  zoomActive, new double[]{0,  5}),
        () => testZoom(zoomIn,  zoomActive, new double[]{0,  2}),
        () => testZoom(zoomOut, zoomActive, new double[]{0,  5}),
        () => testZoom(zoomOut, zoomActive, new double[]{0,  20}),
        () => testZoom(zoomOut, zoomOff,    new double[]{0,  50}),
        () => testZoom(zoomOut, zoomOff,    new double[]{0,  50}),

        //test paging
        () => testVals(100, 0, 35,   0,  10, new double[]{0,  5, 10}),
        () => testPage(up,   new double[]{10, 15, 20}),
        () => testPage(up,   new double[]{20, 25, 30}),
        () => testPage(up,   new double[]{25, 30, 35}),
        () => testPage(up,   new double[]{25, 30, 35}),
        () => testPage(down, new double[]{15, 20, 25}),
        () => testPage(down, new double[]{ 5, 10, 15}),
        () => testPage(down, new double[]{ 0,  5, 10}),
        () => testPage(down, new double[]{ 0,  5, 10}),

        //test padding
        () => testVals(100, 0, 99,   0,  10, new double[]{0,  5, 10}),
        () => testPadd(1, new double[]{0,  5, 10}),
        () => testPadd(2, new double[]{0,  5, 10}),
        () => testPadd(3, new double[]{0,  5, 10}),
        () => testPadd(4, new double[]{0,  5, 10}),
        () => testPadd(5, new double[]{0,  5, 10}),
        () => testPadd(6, new double[]{0,  5, 10}),
        () => testPadd(7, new double[]{0,  5, 10}),
        () => testPadd(8, new double[]{0,  5, 10}),
        () => testPadd(0, new double[]{0,  5, 10}),
      };
    }


    private Action testVals(double height, double minFullValue, double maxFullValue, double displayValue, double displayRange, double[] labels) {
      TestLegendYScroller.Height = height;
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
        TestLegendYScroller.ZoomIn();
      } else {
        TestLegendYScroller.ZoomOut();
      }

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        if (isZoomActive!=TestLegendYScroller.CanZoomOut) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("IsZoomActive should be " + isZoomActive + ".");
        }
        verify(labels);
      };
    }


    private Action testPage(bool pageUp, double[] labels) {
      if (pageUp) {
        TestLegendYScroller.DisplayValue += TestLegendYScroller.Legend.DisplayValueRange;
      } else {
        TestLegendYScroller.DisplayValue -= TestLegendYScroller.Legend.DisplayValueRange;
      }

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        verify(labels);
      };
    }


    private Action testPadd(double padding, double[] labels) {
      TestLegendYScroller.Padding = new Thickness(padding);

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        verify(labels);
      };
    }


    private void verify(double[] labels) {
      LegendY legend = (LegendY)TestLegendYScroller.Legend;
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
