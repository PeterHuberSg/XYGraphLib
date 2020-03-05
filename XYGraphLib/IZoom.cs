using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYGraphLib {
  /// <summary>
  /// Inheritor supports zooming in/out and raises an event when CanZoomIn or CanZoomOut changes
  /// </summary>
  public interface IZoom {
    #region Properties
    //      ----------

    /// <summary>
    /// Is ZoomOut() possible ?
    /// </summary>
    bool CanZoomIn { get;}


    /// <summary>
    /// Is ZoomOut() possible ?
    /// </summary>
    bool CanZoomOut { get;}
    #endregion


    #region Eventhandler
    //      ------------

    /// <summary>
    /// Raised when CanZoomIn or CanZoomOut have changed
    /// </summary>
    event Action<IZoom> ZoomStateChanged;
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Zooms in one step, showing more details
    /// </summary>
    void ZoomIn();


    /// <summary>
    /// Zooms out one step, showing less details
    /// </summary>
    void ZoomOut();


    /// <summary>
    /// Zooms out as much as possible, showing complete graphic
    /// </summary>
    void ZoomReset();
    #endregion
  }
}
