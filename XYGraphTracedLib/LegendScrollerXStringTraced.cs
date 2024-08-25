using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using WpfTestbench;
using System.Windows;


namespace XYGraphLib {

  /// <summary>
  /// LegendScrollerXString class does not exist, but LegendScrollerXStringTraced is needed to allow the
  /// testing of LegendXString.
  /// </summary>
  public class LegendScrollerXStringTraced: LegendScrollerX, ITraceName {

    /// <summary>
    /// Name to be used for tracing
    /// </summary>
    public string TraceName { get; private set; }


    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor
    /// </summary>
    public LegendScrollerXStringTraced() : this("LegendScrollerXString") { }


    ///// <summary>
    ///// Default Constructor with special LegendX
    ///// </summary>
    //public LegendScrollerXStringTraced(LegendX newLegendX) : this("LegendScrollerXString", newLegendX) { }


    /// <summary>
    /// Constructor supporting tracing of multiple XLegendScrollers with different names
    /// </summary>
    public LegendScrollerXStringTraced(string traceName) : this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    ///// <summary>
    ///// Constructor supporting tracing of multiple XLegendScrollers with different names and special LegendX
    ///// </summary>
    //public LegendScrollerXStringTraced(string traceName, LegendX newLegendX) : this(TraceWPFEvents.TraceCreateStart(traceName), newLegendX) {
    //  TraceName = traceName;
    //  TraceWPFEvents.TraceCreateEnd(traceName);
    //}


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before constructor gets executed 
    /// </summary>
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
    //Chart1Plot1X1YLegendTraced is private and the other constructors invoking it set Name already
    private LegendScrollerXStringTraced(DummyTraceClass dummyArgument) : base(new LegendXStringTraced()) {}
    #pragma warning restore CS8618
    #endregion


    #region Event Tracing
    //      -------------

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
      TraceWPFEvents.OnPropertyChanged(this, e, base.OnPropertyChanged);
    }


    protected override Size MeasureOverrideTraced(Size constraint) {
      return TraceWPFEvents.MeasureOverride(this, constraint, base.MeasureOverrideTraced);
    }


    protected override Size ArrangeOverrideTraced(Size finalSize) {
      return TraceWPFEvents.ArrangeOverride(this, finalSize, base.ArrangeOverrideTraced);
    }


    protected override void OnRenderTraced(DrawingContext drawingContext) {
      TraceWPFEvents.OnRender(this, drawingContext, base.OnRenderTraced);
    }
    #endregion
  }
}
