using System.Windows;
using System.Windows.Media;
using WpfTestbench;
using System;
using System.Collections.Generic;


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
    private Chart4Plots1X4YLegendsTraced(DummyTraceClass? _):
    base(
      new LegendScrollerXTraced(new LegendXDateTraced()),
      new PlotAreaTraced("PlotArea0", new LegendScrollerYTraced("LegendScrollerY0")), 
      new PlotAreaTraced("PlotArea1", new LegendScrollerYTraced("LegendScrollerY1")), 
      new PlotAreaTraced("PlotArea2", new LegendScrollerYTraced("LegendScrollerY2")), 
      new PlotAreaTraced("PlotArea3", new LegendScrollerYTraced("LegendScrollerY3"))) 
    { }
#pragma warning restore CS8618
    #endregion


    #region Event Tracing
    //      -------------

    public override void FillData<TRecord>(
      IEnumerable<TRecord> records, 
      SerieSetting<TRecord>[] serieSettings,
      string? xName = null,
      string? xFormat = null,
      string? xUnit = null, 
      Func<TRecord, string>? stringGetter = null) 
    {
      TraceWPFEvents.TraceLineStart($"{Name}.FillData()");
      base.FillData(records, serieSettings, xName, xFormat, xUnit, stringGetter);
      TraceWPFEvents.TraceLineEnd($"{Name}.FillData()");
    }


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
