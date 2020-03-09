/**************************************************************************************

XYGraphLib.TraceWpf
==================

Supports using TraceWPFEvents  

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Windows;


namespace XYGraphLib {

  /// <summary>
  /// Supports using TraceWPFEvents. This is useful during development
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
    static public event Action<FrameworkElement?, string>? LineAdded;


    /// <summary>
    /// Trace a line
    /// </summary>
    static public void Line(string traceLine) {
      Line(null, traceLine);
    }


    /// <summary>
    /// Trace a line, add Name of FrameworkElement
    /// </summary>
    static public void Line(FrameworkElement? frameworkElement, string traceLine) {
      LineAdded?.Invoke(frameworkElement, traceLine);
    }
  }
}
