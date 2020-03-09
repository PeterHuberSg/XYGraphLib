/**************************************************************************************

XYGraphLib.IZoom
================

Inheritor supports zooming in/out and raises an event when CanZoomIn or CanZoomOut changes

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;


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


    #region Events
    //      ------

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
