using System;
using System.Windows;
using WpfTestbench;


namespace XYGraphLib {

  /// <summary>
  /// Interaction logic for LegendXWindow.xaml
  /// </summary>
  public partial class LegendXWindow: Window {

    public static void Show(Window ownerWindow) {
      new LegendXWindow { Owner = ownerWindow }.Show();
    }


    public LegendXWindow() {
      InitializeComponent();

      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:10", () => testFunc(100, 0, 99, 0, 10, [0, 5, 10])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:20", () => testFunc(100, 0, 99, 0, 20, [0, 10, 20])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:30", () => testFunc(100, 0, 99, 0, 30, [0, 10, 20, 30])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:50", () => testFunc(100, 0, 99, 0, 50, [0, 20, 40])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:80", () => testFunc(100, 0, 99, 0, 80, [0, 50])));

      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:5", () => testFunc(100, 0, 99, 0, 5, [0, 2, 4])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:4", () => testFunc(100, 0, 99, 0, 4, [0, 2, 4])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:3", () => testFunc(100, 0, 99, 0, 3, [0, 1, 2, 3])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:2", () => testFunc(100, 0, 99, 0, 2, [0, 1, 2])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:1", () => testFunc(100, 0, 99, 0, 1, [0.0, 0.5, 1.0])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:0.5", () => testFunc(100, 0, 99, 0, 0.5, [0.0, 0.2, 0.4])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:0.3", () => testFunc(100, 0, 99, 0, 0.3, [0.0, 0.2])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:0.2", () => testFunc(100, 0, 99, 0, 0.2, [0.0, 0.1, 0.2])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:0.1", () => testFunc(100, 0, 99, 0, 0.1, [0.00, 0.05, 0.10])));

      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0.4, R:1", () => testFunc(100, 0, 99, 0.4, 1, [0.5, 1.0])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0.5, R:1", () => testFunc(100, 0, 99, 0.5, 1, [0.5, 1.0, 1.5])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0.6, R:1", () => testFunc(100, 0, 99, 0.6, 1, [1.0, 1.5])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0.9, R:1", () => testFunc(100, 0, 99, 0.9, 1, [1.0, 1.5])));
      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:1, R:1", () => testFunc(100, 0, 99, 1, 1, [1.0, 1.5, 2.0])));

      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:8, R:5", () => testFunc(100, 0, 99, 8, 5, [8, 10, 12])));

      TestBench.TestFunctions.Add(("W:73, minV:0, maxV:99, disp V:94, R:5", () => testFunc(73, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:79, minV:0, maxV:99, disp V:94, R:5", () => testFunc(79, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:85, minV:0, maxV:99, disp V:94, R:5", () => testFunc(85, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:91, minV:0, maxV:99, disp V:94, R:5", () => testFunc(91, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:97, minV:0, maxV:99, disp V:94, R:5", () => testFunc(97, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:103, minV:0, maxV:99, disp V:94, R:5", () => testFunc(103, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:109, minV:0, maxV:99, disp V:94, R:5", () => testFunc(109, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:115, minV:0, maxV:99, disp V:94, R:5", () => testFunc(115, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:121, minV:0, maxV:99, disp V:94, R:5", () => testFunc(121, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:127, minV:0, maxV:99, disp V:94, R:5", () => testFunc(127, 0, 99, 94, 5, [94, 96, 98])));
      TestBench.TestFunctions.Add(("W:133, minV:0, maxV:99, disp V:94, R:5", () => testFunc(133, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:139, minV:0, maxV:99, disp V:94, R:5", () => testFunc(139, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:145, minV:0, maxV:99, disp V:94, R:5", () => testFunc(145, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:151, minV:0, maxV:99, disp V:94, R:5", () => testFunc(151, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:157, minV:0, maxV:99, disp V:94, R:5", () => testFunc(157, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:163, minV:0, maxV:99, disp V:94, R:5", () => testFunc(163, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:169, minV:0, maxV:99, disp V:94, R:5", () => testFunc(169, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:175, minV:0, maxV:99, disp V:94, R:5", () => testFunc(175, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:181, minV:0, maxV:99, disp V:94, R:5", () => testFunc(181, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:187, minV:0, maxV:99, disp V:94, R:5", () => testFunc(187, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:193, minV:0, maxV:99, disp V:94, R:5", () => testFunc(193, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:200, minV:0, maxV:99, disp V:94, R:5", () => testFunc(200, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:207, minV:0, maxV:99, disp V:94, R:5", () => testFunc(207, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:214, minV:0, maxV:99, disp V:94, R:5", () => testFunc(214, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:221, minV:0, maxV:99, disp V:94, R:5", () => testFunc(221, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:228, minV:0, maxV:99, disp V:94, R:5", () => testFunc(228, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:235, minV:0, maxV:99, disp V:94, R:5", () => testFunc(235, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:242, minV:0, maxV:99, disp V:94, R:5", () => testFunc(242, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:249, minV:0, maxV:99, disp V:94, R:5", () => testFunc(249, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:256, minV:0, maxV:99, disp V:94, R:5", () => testFunc(256, 0, 99, 94, 5, [94, 95, 96, 97, 98, 99])));
      TestBench.TestFunctions.Add(("W:263, minV:0, maxV:99, disp V:94, R:5", () => testFunc(263, 0, 99, 94, 5, [94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99])));
      TestBench.TestFunctions.Add(("W:270, minV:0, maxV:99, disp V:94, R:5", () => testFunc(270, 0, 99, 94, 5, [94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99])));
      TestBench.TestFunctions.Add(("W:277, minV:0, maxV:99, disp V:94, R:5", () => testFunc(277, 0, 99, 94, 5, [94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99])));
      TestBench.TestFunctions.Add(("W:284, minV:0, maxV:99, disp V:94, R:5", () => testFunc(284, 0, 99, 94, 5, [94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99])));
      TestBench.TestFunctions.Add(("W:291, minV:0, maxV:99, disp V:94, R:5", () => testFunc(291, 0, 99, 94, 5, [94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99])));
      TestBench.TestFunctions.Add(("W:292, minV:0, maxV:99, disp V:94, R:5", () => testFunc(292, 0, 99, 94, 5, [94, 94.5, 95, 95.5, 96, 96.5, 97, 97.5, 98, 98.5, 99])));

      TestBench.TestFunctions.Add(("W:125, minV:0, maxV:1000000, disp V:0, R:1000000", () => testFunc(125, 0, 1000000, 0, 1000000, [0])));
      TestBench.TestFunctions.Add(("W:125, minV:0, maxV:1000000, disp V:1, R:1000000", () => testFunc(125, 0, 1000000, 1, 1000000, [1])));
      TestBench.TestFunctions.Add(("W:129, minV:0, maxV:1000000, disp V:0, R:1000000", () => testFunc(129, 0, 1000000, 0, 1000000, [0, 500000, 1000000])));
      TestBench.TestFunctions.Add(("W:129, minV:0, maxV:1000000, disp V:1, R:999999", () => testFunc(129, 0, 1000000, 1, 999999, [500000, 1000000])));
      TestBench.TestFunctions.Add(("W:129, minV:0, maxV:1000000, disp V:1, R:999998", () => testFunc(129, 0, 1000000, 1, 999998, [1]))); //there is only space for the 50000 label. If only 1 label, the first value gets displayed
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:0, disp V:-1000000, R:1000000", () => testFunc(140, -1000000, 0, -1000000, 1000000, [-1000000, -500000, 0])));
      TestBench.TestFunctions.Add(("W:140, minV:-900000, maxV:0, disp V:-900000, R:900000", () => testFunc(140, -900000, 0, -900000, 900000, [-500000, 0])));

      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:-1000000, disp V:-900000, R:100000", () => testFunc(140, -1000000, -1000000, -900000, 100000, [-900000, -850000, -800000])));
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:-100000, disp V:-900000, R:200000", () => testFunc(140, -1000000, -100000, -900000, 200000, [-900000, -800000, -700000])));
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:-100000, disp V:-900000, R:600000", () => testFunc(140, -1000000, -100000, -900000, 600000, [-900000])));//step: 500000. 1000000 is too big, 500000 too small =>
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:-100000, disp V:-855555, R:100000", () => testFunc(140, -1000000, -100000, -855555, 100000, [-850000, -800000])));
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:-100000, disp V:-855555, R:200000", () => testFunc(140, -1000000, -100000, -855555, 200000, [-800000, -700000])));
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:-100000, disp V:-855555, R:600000", () => testFunc(140, -1000000, -100000, -855555, 600000, [-855555])));//step: 500000. 1000000 is too big, 500000 too small =>

      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:1000000, disp V:-900000, R:1000000", () => testFunc(140, -1000000, 1000000, -900000, 1000000, [-500000, 0])));
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:1000000, disp V:-900000, R:180000", () => testFunc(140, -1000000, 1000000, -900000, 1800000, [-900000])));//step: 500000. 1000000 is too big, 500000 too small =>
      TestBench.TestFunctions.Add(("W:140, minV:-1000000, maxV:1000000, disp V:-855555, R:1000000", () => testFunc(140, -1000000, 1000000, -855555, 1000000, [-500000, 0])));

      TestBench.TestFunctions.Add(("W:200, minV:-0.000004, maxV:0.000000, disp V:-0.0000001, R:0.0000001", () => testFunc(200, -0.000004, 0.000000, -0.0000001, 0.0000001, [-0.0000001, -0.00000005, 0])));
      TestBench.TestFunctions.Add(("W:200, minV:-0.000004, maxV:0.000000, disp V:-0.0000001234, R:0.0000001", () => testFunc(200, -0.000004, 0.000000, -0.0000001234, 0.0000001, [-0.0000001, -0.00000005])));
      TestBench.TestFunctions.Add(("W:200, minV:-0.000004, maxV:0.000000, disp V:-0.0000001234, R:0.0000005", () => testFunc(200, -0.000004, 0.000000, -0.0000001234, 0.0000005, [0, 0.0000002])));

      TestBench.TestFunctions.Add(("W:200, minV:0.000000, maxV:0.000004, disp V:0.0000001, R:0.0000001", () => testFunc(200, 0.000000, 0.000004, 0.0000001, 0.0000001, [0.0000001, 0.00000015, 0.0000002])));
      TestBench.TestFunctions.Add(("W:200, minV:0.000000, maxV:0.000004, disp V:0.0000001234, R:0.0000001", () => testFunc(200, 0.000000, 0.000004, 0.0000001234, 0.0000001, [0.00000015, 0.0000002])));
      TestBench.TestFunctions.Add(("W:200, minV:0.000000, maxV:0.000004, disp V:0.0000001234, R:0.0000005", () => testFunc(200, 0.000000, 0.000004, 0.0000001234, 0.0000005, [0.0000002, 0.0000004, 0.0000006])));

      TestBench.TestFunctions.Add(("W:100, minV:0, maxV:99, disp V:0, R:10", () => testFunc(100, 0, 99, 0, 10, [0, 5, 10])));
    }


   Action? testFunc(double width, double minFullValue, double maxFullValue, double displayValue, double displayRange, double[] labels) {
      TestLegendXTraced.Width = width;
      MinFullValueNumberScrollBar.Value = minFullValue;
      MaxFullValueNumberScrollBar.Value  = maxFullValue;
      DisplayValueNumberScrollBar.Value = displayValue;
      DisplayRangeNumberScrollBar.Value = displayRange;

      if (labels==null) return null; //no test needed

      //test to be executed before next values get applies
      return () => {
        if (labels.Length!=(TestLegendXTraced.LabelValues is null ? 0 : TestLegendXTraced.LabelValues.Length)) {
          System.Diagnostics.Debugger.Break();
          throw new Exception($"There should be {labels.Length} legends, but there were {(TestLegendXTraced.LabelValues is null ? 0 : TestLegendXTraced.LabelValues.Length)}.");
        } else {
          for (int legendValueIndex = 0; legendValueIndex < labels.Length; legendValueIndex++) {
            if (labels[legendValueIndex]!=Math.Round(TestLegendXTraced.LabelValues![legendValueIndex], 8)) {
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