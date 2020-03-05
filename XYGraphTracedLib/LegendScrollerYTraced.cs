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
  /// Version of YLegendScroller which traces WPF events 
  /// </summary>
  public class LegendScrollerYTraced: LegendScrollerY, ITraceName {

    #region ITraceName
    //      ----------

    /// <summary>
    /// Name to be used for tracing
    /// </summary>
    public string TraceName { get; private set; }
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor
    /// </summary>
    public LegendScrollerYTraced(): this("YLegendScroller") {}


    /// <summary>
    /// Constructor supporting tracing of multiple YLegendScrollers with different names
    /// </summary>
    public LegendScrollerYTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName), new LegendYTraced()) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }

    
    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before construtor gets executed 
    /// </summary>
    private LegendScrollerYTraced(DummyTraceClass dummyArgument, LegendY legendY):base(legendY) {
    }
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
