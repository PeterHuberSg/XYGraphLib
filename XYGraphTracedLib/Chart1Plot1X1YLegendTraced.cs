﻿using System.Windows;
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
    public Chart1Plot1X1YLegendTraced(): this("XYGraph2") {}


    /// <summary>
    /// Constructor supporting tracing of multiple XYGraphs with different names
    /// </summary>
    public Chart1Plot1X1YLegendTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before construtor gets executed 
    /// </summary>
    //private Chart1Plot1X1YLegendTraced(DummyTraceClass dummyArgument): base(new PlotAreaTraced(), new LegendScrollerXTraced(), new LegendScrollerYTraced(), 
    //  new GridTraced("ZoomGrid")) {
    //}
    private Chart1Plot1X1YLegendTraced(DummyTraceClass dummyArgument) : base(new PlotAreaTraced(), new LegendScrollerXTraced(), new LegendScrollerYTraced()) {
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
