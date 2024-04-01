using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using XYGraphLib;
using WpfTestbench;


namespace XYGraphTestBench {


  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App: Application {

    /// <summary>
    /// Static constructor
    /// </summary>
    public App() {
    //double d = 1.29E-20;
    //d *= 1E+20;
    //d = Math.Round(d, 1);
    //d /= 1E+20;
    //double diff = d-1.3E-20;
    //if (diff!=0) System.Diagnostics.Debugger.Break();
    //string s = diff.ToString();

    //d = 1.29E-10;
    //d *= 1E+10;
    //d = Math.Round(d, 1);
    //d /= 1E+10;
    //double diff1 = d-1.3E-10;
    //if (diff1!=0) System.Diagnostics.Debugger.Break();
    //s = diff1.ToString();

      //double n1 = double.NaN;
      //double n2 = double.NaN;
      //double number = 1;
      //Console.WriteLine("n1==n2: " + (n1==n2));
      //Console.WriteLine("n1!=n2: " + (n1!=n2));
      //Console.WriteLine("n1==number: " + (n1==number));
      //Console.WriteLine("n1!=number: " + (n1!=number));
      //Console.WriteLine("n1>number: " + (n1>number));
      //Console.WriteLine("n1<number: " + (n1<number));
      DispatcherUnhandledException += App_DispatcherUnhandledException;
      TraceWpf.LineAdded += TraceWpf_LineAdded;
    }


    void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
      Tracer.Exception(e.Exception);

      e.Handled = true;
    }

  
    void TraceWpf_LineAdded(FrameworkElement? frameworkElement, string line) {
      TraceWPFEvents.TraceLine(frameworkElement, line);
    }
  }
}
