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
  /// Version of PlotArea which traces WPF events 
  /// </summary>
  public class PlotAreaTraced: PlotArea, ITraceName {

    /// <summary>
    /// Name to be used for tracing
    /// </summary>
    public string TraceName { get; private set; }


    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor
    /// </summary>
    public PlotAreaTraced(): this("PlotArea", new LegendScrollerYTraced("LegendScrollerY")) {}


    /// <summary>
    /// Constructor supporting tracing of multiple PlotAreas with different names
    /// </summary>
    public PlotAreaTraced(string traceName, LegendScrollerY legendScroller) : this(TraceWPFEvents.TraceCreateStart(traceName), legendScroller) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before constructor gets executed 
    /// </summary>
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
    //PlotAreaTraced is private and the other constructors invoking it set Name already
    private PlotAreaTraced(DummyTraceClass? _, LegendScrollerY legendScrollerY) :
      base(legendScrollerY){}
    #pragma warning restore CS8618
    #endregion


    #region Event Tracing
    //      -------------

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
      TraceWPFEvents.OnPropertyChanged(this, e, base.OnPropertyChanged);
    }


    protected override Size MeasureOverride(Size constraint) {
      return TraceWPFEvents.MeasureOverride(this, constraint, base.MeasureOverride);
    }

    
    protected override Size ArrangeOverride(Size finalSize) {
      return TraceWPFEvents.ArrangeOverride(this, finalSize, base.ArrangeOverride);
    }

    
    protected override void OnRender(DrawingContext drawingContext) {
      TraceWPFEvents.OnRender(this, drawingContext, base.OnRender);
    }
    #endregion
  }
}
