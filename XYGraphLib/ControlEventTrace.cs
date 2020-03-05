using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYGraphLib {
  /// <summary>
  /// Helper class to trace the events of a control
  /// </summary>
  public class ControlEventTrace {
    /// <summary>
    /// Delegate for events
    /// </summary>
    public delegate void WriteDelegate(bool NewLine, string ClassName, string EventName, string Parameters, params Object[] ParameterArgs);


    /// <summary>
    /// This event is used for debugging, to investigate in which sequence the other events fire. 
    /// </summary>
    public event WriteDelegate WriteEvent;


    /// <summary>
    /// Trace event and some event related information
    /// </summary>
    public void Write(string ControlTypeName, string EventName, string Parameters, params Object[] ParameterArgs) {
      if (WriteEvent!=null) {
        WriteEvent(false, ControlTypeName, EventName, Parameters, ParameterArgs);
      }
    }


    /// <summary>
    /// Trace event  and some event related information
    /// </summary>
    public void WriteLine(string ControlTypeName, string EventName, string Parameters, params Object[] ParameterArgs) {
      if (WriteEvent!=null) {
        WriteEvent(true, ControlTypeName, EventName, Parameters, ParameterArgs);
      }
    }


    /// <summary>
    /// Trace event  
    /// </summary>
    public void Write(string ControlTypeName, string EventName) {
      if (WriteEvent!=null) {
        WriteEvent(false, ControlTypeName, EventName, "");
      }
    }


    /// <summary>
    /// Trace event  
    /// </summary>
    public void WriteLine(string ControlTypeName, string EventName) {
      if (WriteEvent!=null) {
        WriteEvent(true, ControlTypeName, EventName, "");
      }
    }
  }
}
