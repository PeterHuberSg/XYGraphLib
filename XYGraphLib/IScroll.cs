//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Media;

//namespace XYGraphLib {

//  /// <summary>
//  /// Inheritor can scroll its content in the x-axis.
//  /// </summary>
//  public interface IScrollX {

//    /// <summary>
//    /// Smallest X Value that gets actually displayed
//    /// </summary>
//    double MinDisplayValueX{get; set;}


//    /// <summary>
//    /// Biggest X Value that gets actually displayed
//    /// </summary>
//    double MaxDisplayValueX{get; set;}
//  }


//  /// <summary>
//  /// Inheritor can scroll its content in the y-axis. It will display graphics in the range from
//  /// MinDisplayValue to MaxDisplayValue, using all the height available.
//  /// </summary>
//  public interface IScrollY {

//    /// <summary>
//    /// Smallest Y Value found in ValueList
//    /// </summary>
//    double MinValueY { get; }

//    /// <summary>
//    /// Biggest Y Value Renderer will ever display
//    /// </summary>
//    double MaxValueY { get; }

//    /// <summary>
//    /// Sets Min and Max DisplayValueY in one call. This allows to render the visual only once, even if
//    /// both values change.
//    /// </summary>
//    void SetDisplayValueRangeY(double min, double max);
//  }


//  /// <summary>
//  /// Inheritor renders some graphic into a visual
//  /// </summary>
//  public interface IRender: IScrollY {

//    /// <summary>
//    /// Renderer Id. Used to differentiate the WPF events of multiply renderers
//    /// </summary>
//    int RendererId { get; }


//    /// <summary>
//    /// Smallest Y Value that gets actually displayed
//    /// </summary>
//    double MinDisplayValueY {get;}


//    /// <summary>
//    /// Biggest Y Value that gets actually displayed
//    /// </summary>
//    double MaxDisplayValueY {get;}
      
      
//    /// <summary>
//    /// Called by YLegendScroller.DisplayRangeChanged event to set MinDisplayValueY and MaxDisplayValueY of Renderer
//    /// </summary>
//    void ChangeDisplayRangeY(YLegendScroller sender);


//    ///// <summary>
//    ///// Constructor completed. Used for WPF event tracing
//    ///// </summary>
//    //event Action<IRender> Created;

    
//    /// <summary>
//    /// Raised if rendering is needed
//    /// </summary>
//    event Action<IRender> RenderingRequested;


//    /// <summary>
//    /// Renders some graphic into a Visual. The graphic gets scaled to the available height 
//    /// and width displaying only values according to IScrollX and IScrollY.
//    /// </summary>
//    Visual Render(double width, double height);

  
//    /// <summary>
//    /// Creation completed. Used for WPF event tracing
//    /// </summary>
//    event Action<IRender> Rendered;
//  }

//}
