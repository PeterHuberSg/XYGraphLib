using System.Windows;
using System.Windows.Media;
using WpfTestbench;
using System;
using System.Collections.Generic;


namespace XYGraphLib {


  public class Chart2Plots1X2YLegendsTraced: Chart2Plots1X2YLegends, ITraceName {

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
    public Chart2Plots1X2YLegendsTraced() : this("Graph2Plots1X2YLegends") { }


    /// <summary>
    /// Constructor supporting tracing of multiple XYGraphs with different names
    /// </summary>
    public Chart2Plots1X2YLegendsTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before constructor gets executed 
    /// </summary>
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
    private Chart2Plots1X2YLegendsTraced(DummyTraceClass? _)
      : base(new LegendScrollerXTraced(new LegendXDateTraced()), 
          new PlotAreaTraced("PlotAreaUpper", new LegendScrollerYTraced("LegendScrollerYUpper")), 
          new PlotAreaTraced("PlotAreaLower", new LegendScrollerYTraced("LegendScrollerYLower"))) {
    }
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
