///**************************************************************************************

//XYGraphLib.ControlEventTrace
//============================

//Helper class to trace the events of a control

//Written 2014-2020 by Jürgpeter Huber 
//Contact: PeterCode at Peterbox dot com

//To the extent possible under law, the author(s) have dedicated all copyright and 
//related and neighboring rights to this software to the public domain worldwide under
//the Creative Commons 0 license (details see COPYING.txt file, see also
//<http://creativecommons.org/publicdomain/zero/1.0/>). 

//This software is distributed without any warranty. 
//**************************************************************************************/
//using System;


//namespace XYGraphLib {
//  /// <summary>
//  /// Helper class to trace the events of a control
//  /// </summary>
//  public class ControlEventTrace {
//    /// <summary>
//    /// Delegate for events
//    /// </summary>
//    public delegate void WriteDelegate(bool NewLine, string ClassName, string EventName, string Parameters, params Object[] ParameterArgs);


//    /// <summary>
//    /// This event is used for debugging, to investigate in which sequence the other events fire. 
//    /// </summary>
//    public event WriteDelegate? WriteEvent;


//    /// <summary>
//    /// Trace event and some event related information
//    /// </summary>
//    public void Write(string ControlTypeName, string EventName, string Parameters, params Object[] ParameterArgs) {
//      WriteEvent?.Invoke(false, ControlTypeName, EventName, Parameters, ParameterArgs);
//    }


//    /// <summary>
//    /// Trace event  and some event related information
//    /// </summary>
//    public void WriteLine(string ControlTypeName, string EventName, string Parameters, params Object[] ParameterArgs) {
//      WriteEvent?.Invoke(true, ControlTypeName, EventName, Parameters, ParameterArgs);
//    }


//    /// <summary>
//    /// Trace event  
//    /// </summary>
//    public void Write(string ControlTypeName, string EventName) {
//      WriteEvent?.Invoke(false, ControlTypeName, EventName, "");
//    }


//    /// <summary>
//    /// Trace event  
//    /// </summary>
//    public void WriteLine(string ControlTypeName, string EventName) {
//      WriteEvent?.Invoke(true, ControlTypeName, EventName, "");
//    }
//  }
//}
