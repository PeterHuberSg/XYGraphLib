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
  /// Version of XLegend which traces WPF events 
  /// </summary>
  public class LegendXTraced: LegendX, ITraceName {

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
    public LegendXTraced(): this("LegendX") {}


    /// <summary>
    /// Constructor supporting tracing of multiple XLegends with different names
    /// </summary>
    public LegendXTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before constructor gets executed 
    /// </summary>
    private LegendXTraced(DummyTraceClass dummyArgument):base() {}
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
