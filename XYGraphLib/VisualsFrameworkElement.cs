using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;


namespace XYGraphLib {

  /// <summary>
  /// FrameworkElement with Visuals Collection. Is used by inheriting classes wanting to render directly
  /// to the DrawingContext, which is not supported for FrameworkElements.
  /// </summary>
  public class VisualsFrameworkElement: FrameworkElement {

    #region Properties
    //      ----------

    /// <summary>
    /// Used by inheritors to add visuals to the Visual Collection
    /// </summary>
    internal readonly VisualCollection Visuals;


    /// <summary>
    /// Returns the number of Visuals in this VisualsFrameworkElement. Used by WPF for drawing.
    /// </summary>
    protected override int VisualChildrenCount {
      get {
        return Visuals.Count;
      }
    }
    #endregion
    

    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor
    /// </summary>
    public VisualsFrameworkElement() {
      Visuals = new VisualCollection(this);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Returns the indexed Visual. Is used by WPF for drawing
    /// </summary>
    protected override Visual GetVisualChild(int index) {
      return Visuals[index];
    }
    #endregion
  }
}
