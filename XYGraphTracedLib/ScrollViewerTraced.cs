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
  /// Version of ScrollViewer which traces WPF events 
  /// </summary>
  public class ScrollViewerTraced: ScrollViewer, ITraceName {

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
    public ScrollViewerTraced(): this("ScrollViewer") {}


    /// <summary>
    /// Constructor supporting tracing of multiple ScrollViewers with different names
    /// </summary>
    public ScrollViewerTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before construtor gets executed 
    /// </summary>
    private ScrollViewerTraced(DummyTraceClass dummyArgument):base() {
    }
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
