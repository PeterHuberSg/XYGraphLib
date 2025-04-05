/**************************************************************************************

XYGraphLib.Chart4Plots1X4YLegends
=================================

Chart with 4 PlotArea, 1 XLegendScroller and 4 LegendScrollerY

Written 2021 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/

// Chart4Plots1X4YLegends displays 4 graphics stacked vertically, each having his own yLegend (Value), but sharing 1 XLegend (Time).
// 
// ┌────────────────┬───────────────────┐
// │ PlotArea0      │ LegendScrollerY0  │
// ├────────────────┼───────────────────┤
// │ PlotArea1      │ LegendScrollerY1  │
// ├────────────────┼───────────────────┤
// │ PlotArea2      │ LegendScrollerY2  │
// ├────────────────┼───────────────────┤
// │ PlotArea3      │ LegendScrollerY3  │
// ├────────────────┼───────────────────┤
// │XLegendScroller │Total Zoom Buttons │
// └────────────────┴───────────────────┘
//
//
// Usage:
// same VS project:  
//   xmlns:XYGraphLib="clr-namespace:XYGraphLib" 
//
// different VS project:  
//   xmlns:XYGraphLib="clr-namespace:XYGraphLib;assembly=XYGraphLib" 
//   ensure there is a project reference added to XYGraphLib 
//
// <XYGraphLib:Chart4Plots1X4YLegends/>


using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CustomControlBaseLib;

namespace XYGraphLib {
  /// <summary>
  /// Displays 2 graphics stacked vertically, each having his own yLegend (Value), but sharing 1 XLegend (Time).
  /// </summary>
  public class Chart4Plots1X4YLegends: Chart {


    #region Properties
    //      ----------

    /// <summary>
    /// Highest plotArea
    /// </summary>
    public PlotArea PlotArea0;


    /// <summary>
    /// Second highest plotArea
    /// </summary>
    public PlotArea PlotArea1;


    /// <summary>
    /// Third highest plotArea
    /// </summary>
    public PlotArea PlotArea2;


    /// <summary>
    /// Lowest plotArea
    /// </summary>
    public PlotArea PlotArea3;


    /// <summary>
    /// YLegend Scroller for highest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY0;


    /// <summary>
    /// YLegend Scroller for second highest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY1;


    /// <summary>
    /// YLegend Scroller for third highest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY2;


    /// <summary>
    /// YLegend Scroller for lowest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY3;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Only WPF Editor from Visual Studio should use this constructor
    /// </summary>
    public Chart4Plots1X4YLegends(): 
      this(
        new LegendScrollerX(),
        new PlotArea(new LegendScrollerY()), 
        new PlotArea(new LegendScrollerY()),
        new PlotArea(new LegendScrollerY()),
        new PlotArea(new LegendScrollerY())) 
    { }


    readonly double heightRatio0;
    readonly double heightRatio1;
    readonly double heightRatio2;
    readonly double heightRatio3;


    /// <summary>
    /// Constructor supporting XYGraph with plugged in components
    /// </summary>
    public Chart4Plots1X4YLegends(
      LegendScrollerX legendScrollerX,
      PlotArea plotArea0,
      PlotArea plotArea1,
      PlotArea plotArea2,
      PlotArea plotArea3, 
      int newHeightRatio0 = 1,
      int newHeightRatio1 = 1,
      int newHeightRatio2 = 1,
      int newHeightRatio3 = 1):
      base(legendScrollerX, plotArea0, plotArea1, plotArea2, plotArea3)
    {
      PlotArea0 = plotArea0;
      PlotArea1 = plotArea1;
      PlotArea2 = plotArea2;
      PlotArea3 = plotArea3;
      LegendScrollerY0 = plotArea0.LegendScrollerY;
      LegendScrollerY1 = plotArea1.LegendScrollerY;
      LegendScrollerY2 = plotArea2.LegendScrollerY;
      LegendScrollerY3 = plotArea3.LegendScrollerY;


      if (newHeightRatio0<=0 || newHeightRatio1<=0 || newHeightRatio2<=0 || newHeightRatio3<=0) {
        throw new ArgumentException("HeightRatio cannot be 0 or negative.");
      }
      double totalHight = newHeightRatio0 + newHeightRatio1 + newHeightRatio2 + newHeightRatio3;
      heightRatio0 = newHeightRatio0 / totalHight;
      heightRatio1 = newHeightRatio1 / totalHight;
      heightRatio2 = newHeightRatio2 / totalHight;
      heightRatio3 = newHeightRatio3 / totalHight;
    }
    #endregion

    
    #region Layout
    //      ------

    protected override Size MeasureContentOverride(Size constraint) {
      TotalZoom100Button!.Measure(new Size(constraint.Width, constraint.Height));
      double totalZoom100ButtonWidth = TotalZoom100Button.DesiredSize.Width;
      double totalZoom100ButtonHeight = TotalZoom100Button.DesiredSize.Height;
      LegendScrollerX.Legend.MinHeight = totalZoom100ButtonHeight;

      LegendScrollerX.Measure(constraint);
      double zoomButtonDimension = TotalZoomOutButton!.Width = TotalZoomOutButton.Height = TotalZoomInButton!.Height = TotalZoomInButton.Width = 
        LegendScrollerX.ScrollBarHeight;

      double legendHeight = Math.Min(constraint.Height, Math.Max(LegendScrollerX.DesiredSize.Height, totalZoom100ButtonHeight + zoomButtonDimension));
      double plotArea0Height = (constraint.Height - legendHeight) * heightRatio0;
      double plotArea1Height = (constraint.Height - legendHeight) * heightRatio1;
      double plotArea2Height = (constraint.Height - legendHeight) * heightRatio2;
      double plotArea3Height = (constraint.Height - legendHeight) * heightRatio3;
      LegendScrollerY0.Measure(new Size(constraint.Width, plotArea0Height));
      LegendScrollerY1.Measure(new Size(constraint.Width, plotArea1Height));
      LegendScrollerY2.Measure(new Size(constraint.Width, plotArea2Height));
      LegendScrollerY3.Measure(new Size(constraint.Width, plotArea3Height));
      double legendScrollerYMaxWidth = 
        Math.Max(
          Math.Max(
            Math.Max(LegendScrollerY0.DesiredSize.Width, LegendScrollerY1.DesiredSize.Width), 
            LegendScrollerY2.DesiredSize.Width), 
          LegendScrollerY3.DesiredSize.Width);

      double legendWidth = Math.Min(constraint.Width, Math.Max(legendScrollerYMaxWidth, totalZoom100ButtonWidth));
      double plotAreaWidth = constraint.Width-legendWidth;
      PlotArea0.Measure(new Size(plotAreaWidth, plotArea0Height));
      PlotArea1.Measure(new Size(plotAreaWidth, plotArea1Height));
      PlotArea2.Measure(new Size(plotAreaWidth, plotArea2Height));
      PlotArea3.Measure(new Size(plotAreaWidth, plotArea3Height));

      TotalZoomInButton.Measure(new Size(TotalZoomInButton.Width, TotalZoomInButton.Height));
      TotalZoomOutButton.Measure(new Size(TotalZoomOutButton.Width, TotalZoomOutButton.Height));

      //always remeasure xLegend because in the beginning it is given the whole width of Chart4Plots1X4YLegends
      LegendScrollerX.Measure(new Size(plotAreaWidth, legendHeight));

      Size returnedSize = constraint;
      if (double.IsInfinity(constraint.Height)) {
        returnedSize.Height = LegendScrollerX.DesiredSize.Height + LegendScrollerY0.DesiredSize.Height + 
          LegendScrollerY1.DesiredSize.Height + LegendScrollerY2.DesiredSize.Height + LegendScrollerY3.DesiredSize.Height;
      }
      if (double.IsInfinity(constraint.Width)) {
        returnedSize.Width = LegendScrollerX.DesiredSize.Width + legendScrollerYMaxWidth;
      }
      return returnedSize;
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      double legendWidth = 
        Math.Min(arrangeRect.Width,
          Math.Max(
            Math.Max(
              Math.Max(
                Math.Max(LegendScrollerY0.DesiredSize.Width, LegendScrollerY1.DesiredSize.Width),
              LegendScrollerY2.DesiredSize.Width),
            LegendScrollerY3.DesiredSize.Width),
          TotalZoom100Button!.DesiredSize.Width));
      double remainingWidth = arrangeRect.Width - legendWidth;
      double legendXHeight = Math.Min(arrangeRect.Height, 
        Math.Max(LegendScrollerX.DesiredSize.Height, TotalZoom100Button.DesiredSize.Height + TotalZoomOutButton!.DesiredSize.Height));
      double remainingHeight = arrangeRect.Height - legendXHeight;
      double plotArea0Height = remainingHeight * heightRatio0;
      double plotArea1Height = remainingHeight * heightRatio1;
      double plotArea2Height = remainingHeight * heightRatio2;
      double plotArea3Height = remainingHeight * heightRatio3;
      var y = 0.0;
      LegendScrollerY0.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea0Height);
      y += plotArea0Height;
      LegendScrollerY1.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea1Height);
      y += plotArea1Height;
      LegendScrollerY2.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea2Height);
      y += plotArea2Height;
      LegendScrollerY3.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea3Height);
      LegendScrollerX.ArrangeBorderPadding(arrangeRect, 0, remainingHeight, remainingWidth, legendXHeight);

      //arrange plot-area after scrollers, which might change the values plot-area has to display
      y = 0.0;
      PlotArea0.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea0Height);
      y += plotArea0Height;
      PlotArea1.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea1Height);
      y += plotArea1Height;
      PlotArea2.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea2Height);
      y += plotArea2Height;
      PlotArea3.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea3Height);
      TotalZoom100Button.ArrangeBorderPadding(arrangeRect, remainingWidth, remainingHeight, legendWidth, TotalZoom100Button.DesiredSize.Height);

      double zoomInOutY = remainingHeight + TotalZoom100Button.DesiredSize.Height;
      TotalZoomOutButton.ArrangeBorderPadding(arrangeRect, remainingWidth, zoomInOutY, 
        TotalZoomOutButton.DesiredSize.Width, TotalZoomOutButton.DesiredSize.Height);
      TotalZoomInButton!.ArrangeBorderPadding(arrangeRect, arrangeRect.Width - TotalZoomInButton!.DesiredSize.Width, zoomInOutY, TotalZoomInButton.DesiredSize.Width, TotalZoomInButton.DesiredSize.Height);

      return arrangeRect.Size;
    }
    #endregion
  }
}
