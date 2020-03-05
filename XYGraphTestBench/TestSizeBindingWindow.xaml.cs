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
using TracerLib;


namespace XYGraphLib {

  public partial class TestSizeBindingWindow: Window {

    
    /// <summary>
    /// Creates and opens a new TestSizeBindingWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      TestSizeBindingWindow testSizeBindingWindow = new TestSizeBindingWindow();
      testSizeBindingWindow.Owner = ownerWindow;
      testSizeBindingWindow.Show();
    }



    public TestSizeBindingWindow() {
      InitializeComponent();

      MouseDown += new MouseButtonEventHandler(TestSizeBindingWindow_MouseDown);
    }


    void TestSizeBindingWindow_MouseDown(object sender, MouseButtonEventArgs e) {
      if (e.ChangedButton==MouseButton.Right) {
        var traceStringBuilder = new StringBuilder();
        foreach (TraceMessage traceMessage in Tracer.GetTrace()) {
          traceStringBuilder.AppendLine(traceMessage.ToString());
        }
        var traceString = traceStringBuilder.ToString();
      }
    }
  }
}
