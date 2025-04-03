/**************************************************************************************

XYGraphLib.LegendX
==================

Horizontal Legend

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/

using System;
using System.Windows;
using System.Windows.Media;


// Displays a horizontal legend for a PlotArea. LegendX serves a base class for various specialised legends
// like displaying dates, numbers or strings. 
// 
//            ┌────────────────────────────────────┐
// -100       │ 100             150            200 │     500
//    ↑       └────────────────────────────────────┘      ↑
// MinValue     ↑DisplayValue                   ↑        MaxValue
//              DisplayValue + DisplayValueRange│
//


namespace XYGraphLib {


  public abstract class LegendX: Legend {


    #region Constructor
    //      -----------

    /// <summary>
    /// Default constructor. A LegendX is written parallel to the x-axis (horizontally) and shows x-values, 
    /// which is dimension 1
    public LegendX()
      : this(Renderer.DimensionX) {
    }


    /// <summary>
    /// Constructor if LegendX should be used for another dimension than x, although still written parallel to the x-axis (horizontally)
    /// </summary>
    public LegendX(int dimension,
      bool needsMeasureWhenValuesChange = false,
      bool isWriteRightAligned = false,
      double legendAngle = 0)
      : base(dimension, needsMeasureWhenValuesChange, isWriteRightAligned, legendAngle) 
    {}
    #endregion
  }
}
