using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace XYGraphLib {

  /// <summary>
  /// Supports using TraceWPFEvents without linking XYGraphLibTraced.DLL. This is usefull during development
  /// to add more information to TraceWPFEvents. During production, there should be no tracing.
  /// </summary>
  public static class TraceWpf {

    /// <summary>
    /// Is tracing active ?
    /// </summary>
    public static bool IsTracing { get {return LineAdded!=null;}}

    /// <summary>
    /// Action to execute for tracing
    /// </summary>
    static public event Action<FrameworkElement, string> LineAdded;


    /// <summary>
    /// Trace a line
    /// </summary>
    static public void Line(string traceLine) {
      Line(null, traceLine);
    }


    /// <summary>
    /// Trace a line, add Name of FrameworkElement
    /// </summary>
    static public void Line(FrameworkElement frameworkElement, string traceLine) {
      if (LineAdded!=null) {
        LineAdded(frameworkElement, traceLine);
      }
    }
  }
}
