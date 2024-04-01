﻿using System.Windows;
using System.Windows.Media;
using WpfTestbench;
using System;


namespace XYGraphLib {


  public class Chart4Plots1X4YLegendsTraced: Chart4Plots1X4YLegends, ITraceName {

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
    public Chart4Plots1X4YLegendsTraced() : this("Graph4Plots1X4YLegends") { }


    /// <summary>
    /// Constructor supporting tracing of multiple XYGraphs with different names
    /// </summary>
    public Chart4Plots1X4YLegendsTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before constructor gets executed 
    /// </summary>
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
    //Chart4Plots1X4YLegendsTraced is private and the other constructors invoking it set Name already
    private Chart4Plots1X4YLegendsTraced(DummyTraceClass? dummyArgument)
      : base(
          new PlotAreaTraced("PlotArea0"), 
          new PlotAreaTraced("PlotArea1"),
          new PlotAreaTraced("PlotArea2"),
          new PlotAreaTraced("PlotArea3"), 
          new LegendScrollerXTraced(new LegendXDateTraced()),
          new LegendScrollerYTraced("YLegendScroller0"), 
          new LegendScrollerYTraced("YLegendScroller1"),
          new LegendScrollerYTraced("YLegendScroller2"),
          new LegendScrollerYTraced("YLegendScroller3")
          ) 
    {}
    #pragma warning restore CS8618 
    //private Chart4Plots1X4YLegendsTraced(DummyTraceClass dummyArgument)
    //  : base(new PlotAreaTraced("PlotArea0"), new PlotAreaTraced("PlotArea1"), new LegendScrollerXTraced(new LegendXDateTraced()), 
    //  new LegendScrollerYTraced("YLegendScroller0"), new LegendScrollerYTraced("YLegendScroller1"), new GridTraced("ZoomGrid")) 
    //{
    //}
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
