using System.Windows;
using System.Windows.Media;
using WpfTestbench;
using System;
using System.Collections.Generic;


namespace XYGraphLib {

  /// <summary>
  /// Version of XYGraph which traces WPF events 
  /// </summary>
  public class Chart1Plot1XString1YLegendTraced: Chart1Plot1X1YLegend, ITraceName {
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
    public Chart1Plot1XString1YLegendTraced() : this("Graph1Plot1XString1YLegend") { }


    /// <summary>
    /// Constructor supporting tracing of multiple XYGraphs with different names
    /// </summary>
    public Chart1Plot1XString1YLegendTraced(string traceName) : this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before constructor gets executed 
    /// </summary>
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
    //Chart1Plot1XString1YLegendTraced is private and the other constructors invoking it set Name already
    private Chart1Plot1XString1YLegendTraced(DummyTraceClass? _) :
      base(new LegendScrollerXTraced(new LegendXStringTraced()), new PlotAreaTraced()) {
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
      base.FillData(records, serieSettings, xName, xUnit, xFormat, stringGetter);
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
