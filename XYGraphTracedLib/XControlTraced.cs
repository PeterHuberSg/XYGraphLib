/*
Sample class showing how to add tracing of property changes and layouting to a class inheriting (indirectly) from Control.

1) Copy content to a new class file, name it 'your class name'Traced
2) Replace 'XControl' with 'your class name'
done :-)

*/
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
  /// Version of XControl which traces WPF events 
  /// </summary>
  public class XControlTraced: XControl, ITraceName {

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
    public XControlTraced(): this("XControl") {}


    /// <summary>
    /// Constructor supporting tracing of multiple XControls with different names
    /// </summary>
    public XControlTraced(string traceName): this(TraceWPFEvents.TraceCreateStart(traceName)) {
      TraceName = traceName;
      TraceWPFEvents.TraceCreateEnd(traceName);
    }


    /// <summary>
    /// Dummy constructor allowing public constructor to call TraceCreateStart() before construtor gets executed 
    /// </summary>
    private XControlTraced(DummyTraceClass dummyArgument):base() {
    }
    #endregion


    #region Event Tracing
    //      -------------

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
      TraceWPFEvents.OnPropertyChanged(this, e, base.OnPropertyChanged);
    }


    //protected override Size MeasureOverride(Size constraint) {
    //  return TraceWPFEvents.MeasureOverride(this, constraint, base.MeasureOverride);
    //}

    protected override Size ArrangeOverride(Size finalSize) {
      return TraceWPFEvents.ArrangeOverride(this, finalSize, base.ArrangeOverride);
    }

    
    protected override void OnRender(DrawingContext drawingContext) {
      TraceWPFEvents.OnRender(this, drawingContext, base.OnRender);
    }
    #endregion
  }
}
