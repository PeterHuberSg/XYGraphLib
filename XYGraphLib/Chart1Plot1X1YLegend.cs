/**************************************************************************************

XYGraphLib.Chart1Plot1X1YLegend
===============================

Chart with 1 PlotArea, 1 XLegendScroller and 1 LegendScrollerY

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
// Chart1Plot1X1YLegend displays some line graphics in the PlotArea with legends and scroll bars for each X and Y axis:
// 
// ┌─────────────────────────┬────────────────────────┐
// │PlotArea                 │LegendScrollerY         │
// │XYGridLines              │                        │
// ├─────────────────────────┼────────────────────────┤
// │XLegendScroller          │Total Zoom Buttons      │
// └─────────────────────────┴────────────────────────┘
//
// 
// Usage:
// same VS project:  
//   xmlns:XYGraphLib="clr-namespace:XYGraphLib" <br/>
//
// different VS project:  
//   xmlns:XYGraphLib="clr-namespace:XYGraphLib;assembly=XYGraphLib" <br/>
//   ensure there is a project reference added to XYGraphLib
//
// <XYGraphLib:Chart1Plot1X1YLegend/>
//

//http://tech.pro/tutorial/856/wpf-tutorial-using-a-visual-collection


using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Media;
using CustomControlBaseLib;


namespace XYGraphLib {

  /// <summary>
  /// Chart with 1 PlotArea, 1 XLegendScroller and 1 LegendScrollerY
  /// </summary>
  public class Chart1Plot1X1YLegend: Chart{


    #region Properties
    //      ----------

    /// <summary>
    /// PlotArea
    /// </summary>
    public readonly PlotArea PlotArea;


    /// <summary>
    /// YLegend Scroller
    /// </summary>
    public readonly LegendScrollerY LegendScrollerY;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Only WPF Editor from Visual Studio should use this constructor
    /// </summary>
    public Chart1Plot1X1YLegend(): 
      this(new LegendScrollerX(), new PlotArea(new LegendScrollerY())) { }


    /// <summary>
    /// Constructor supporting Chart1Plot1X1YLegend with plugged in components
    /// </summary>
    public Chart1Plot1X1YLegend(LegendScrollerX legendScrollerX, PlotArea plotArea):
      base(legendScrollerX, plotArea) 
    {
      PlotArea = plotArea;
      LegendScrollerY = plotArea.LegendScrollerY;
    }
    #endregion

    
    #region Layout
    //      ------

    const double plotAreaRatio = 0.7;


    protected override Size MeasureContentOverride(Size constraint) {
      TotalZoom100Button!.Measure(new Size(constraint.Width, constraint.Height));
      double totalZoom100ButtonWidth = TotalZoom100Button.DesiredSize.Width;
      double totalZoom100ButtonHeight = TotalZoom100Button.DesiredSize.Height;
      LegendScrollerX.Legend.MinHeight = totalZoom100ButtonHeight;

      LegendScrollerX.Measure(constraint);
      double zoomButtonDimension = TotalZoomOutButton!.Width = TotalZoomOutButton.Height = TotalZoomInButton!.Height = TotalZoomInButton.Width = 
        LegendScrollerX.ScrollBarHeight;

      double legendHeight = Math.Min(constraint.Height, Math.Max(LegendScrollerX.DesiredSize.Height, totalZoom100ButtonHeight + zoomButtonDimension));
      double plotAreaHeight = (constraint.Height - legendHeight) * plotAreaRatio;
      LegendScrollerY.Measure(new Size(constraint.Width, plotAreaHeight));
      double legendScrollerYWidth = LegendScrollerY.DesiredSize.Width;

      double legendWidth = Math.Min(constraint.Width, Math.Max(legendScrollerYWidth, totalZoom100ButtonWidth));
      double plotAreaWidth = constraint.Width-legendWidth;
      PlotArea.Measure(new Size(plotAreaWidth, plotAreaHeight));

      TotalZoomInButton.Measure(new Size(TotalZoomInButton.Width, TotalZoomInButton.Height));
      TotalZoomOutButton.Measure(new Size(TotalZoomOutButton.Width, TotalZoomOutButton.Height));

      //always remeasure xLegend because in the beginning it is given the whole width of the chart
      LegendScrollerX.Measure(new Size(plotAreaWidth, legendHeight));

      //////////allow Chart to measure its own controls
      ////////base.MeasureChartControls(constraint);

      Size returnedSize = constraint;
      if (double.IsInfinity(constraint.Height)) {
        returnedSize.Height = LegendScrollerX.DesiredSize.Height + LegendScrollerY.DesiredSize.Height;
      }
      if (double.IsInfinity(constraint.Width)) {
        returnedSize.Width = LegendScrollerX.DesiredSize.Width + legendScrollerYWidth;
      }
      return returnedSize;
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      double legendWidth = Math.Min(arrangeRect.Width, 
        Math.Max(LegendScrollerY.DesiredSize.Width, TotalZoom100Button!.DesiredSize.Width));
      double remainingWidth = arrangeRect.Width - legendWidth;
      double legendXHeight = Math.Min(arrangeRect.Height, 
        Math.Max(LegendScrollerX.DesiredSize.Height, TotalZoom100Button.DesiredSize.Height + TotalZoomOutButton!.DesiredSize.Height));
      double remainingHeight = arrangeRect.Height - legendXHeight;
      LegendScrollerY.ArrangeBorderPadding(arrangeRect, remainingWidth, 0, legendWidth, remainingHeight);
      LegendScrollerX.ArrangeBorderPadding(arrangeRect, 0, remainingHeight, remainingWidth, legendXHeight);
      //arrange plot-area after scrollers, which might change the values plot-area has to display
      PlotArea.ArrangeBorderPadding(arrangeRect, 0, 0, remainingWidth, remainingHeight);
      TotalZoom100Button.ArrangeBorderPadding(arrangeRect, remainingWidth, remainingHeight, legendWidth, TotalZoom100Button.DesiredSize.Height);

      double zoomInOutY = remainingHeight + TotalZoom100Button.DesiredSize.Height;
      TotalZoomOutButton.ArrangeBorderPadding(arrangeRect, remainingWidth, zoomInOutY, 
        TotalZoomOutButton.DesiredSize.Width, TotalZoomOutButton.DesiredSize.Height);
      TotalZoomInButton!.ArrangeBorderPadding(arrangeRect, arrangeRect.Width - TotalZoomInButton!.DesiredSize.Width, zoomInOutY, TotalZoomInButton.DesiredSize.Width, TotalZoomInButton.DesiredSize.Height);

      //////////allow Chart to arrange its own controls
      ////////base.ArrangeChartControls(arrangeRect);

      return arrangeRect.Size;
    }
    #endregion
  }
}
