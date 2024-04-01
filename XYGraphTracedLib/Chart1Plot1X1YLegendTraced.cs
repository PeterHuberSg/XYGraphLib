using System.Windows;
using System.Windows.Media;
using WpfTestbench;
using System;


namespace XYGraphLib {

  /// <summary>
  /// Version of XYGraph which traces WPF events 
  /// </summary>
  public class Chart1Plot1X1YLegendTraced: Chart1Plot1X1YLegend, ITraceName {

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
    public Chart1Plot1X1YLegendTraced(): this("Graph1Plot1X1YLegend") {}


    /// <summary>
    /// Constructor supporting tracing of multiple XYGraphs with different names
    /// </summary>
    public Chart1Plot1X1YLegendTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before constructor gets executed 
    /// </summary>
    //private Chart1Plot1X1YLegendTraced(DummyTraceClass dummyArgument): base(new PlotAreaTraced(), new LegendScrollerXTraced(), new LegendScrollerYTraced(), 
    //  new GridTraced("ZoomGrid")) {
    //}
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
    //Chart1Plot1X1YLegendTraced is private and the other constructors invoking it set Name already
    private Chart1Plot1X1YLegendTraced(DummyTraceClass? _) : 
      base(new PlotAreaTraced(), new LegendScrollerXTraced(new LegendXDateTraced()), new LegendScrollerYTraced()) {
    }
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
