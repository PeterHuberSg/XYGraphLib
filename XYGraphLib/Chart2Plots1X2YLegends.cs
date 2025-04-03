/**************************************************************************************

XYGraphLib.Chart2Plots1X2YLegends
=================================

Chart with 2 PlotArea, 1 XLegendScroller and 2 LegendScrollerY

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/

// Chart2Plots1X2YLegends displays 2 graphics stacked vertically, each having his own yLegend (Value), but sharing 1 XLegend (Time).
// 
// ┌────────────────┬───────────────────┐
// │ PlotArea0      │ LegendScrollerY0  │
// ├────────────────┼───────────────────┤
// │ PlotArea1      │ LegendScrollerY1  │
// ├────────────────┼───────────────────┤
// │LegendScrollerX │Total Zoom Buttons │
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
// <XYGraphLib:Chart2Plots1X2YLegends/>


using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using CustomControlBaseLib;


namespace XYGraphLib {
  /// <summary>
  /// Displays 2 graphics stacked vertically, each having his own yLegend (Value), but sharing 1 XLegend (Time).
  /// </summary>
  public class Chart2Plots1X2YLegends: Chart {


    #region Properties
    //      ----------

    /// <summary>
    /// Upper plotArea
    /// </summary>
    public readonly PlotArea PlotAreaUpper;


    /// <summary>
    /// Lower plotArea
    /// </summary>
    public readonly PlotArea PlotAreaLower ;

    
    /// <summary>
    /// YLegend Scroller for upper plotArea
    /// </summary>
    public readonly LegendScrollerY LegendScrollerYUpper;


    /// <summary>
    /// YLegend Scroller for lower plotArea
    /// </summary>
    public readonly LegendScrollerY LegendScrollerYLower;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Only WPF Editor from Visual Studio should use this constructor
    /// </summary>
    public Chart2Plots1X2YLegends(): 
      this(new LegendScrollerX(), new PlotArea(new LegendScrollerY()), new PlotArea(new LegendScrollerY())) { }


    /// <summary>
    /// Constructor supporting XYGraph with plugged in components
    /// </summary>
    public Chart2Plots1X2YLegends(LegendScrollerX legendScrollerX, PlotArea plotAreaUpper, PlotArea plotAreaLower):
      base(legendScrollerX, plotAreaUpper, plotAreaLower)  
    {
      PlotAreaUpper = plotAreaUpper;
      PlotAreaLower = plotAreaLower;
      LegendScrollerYUpper = PlotAreaUpper.LegendScrollerY;
      LegendScrollerYLower = PlotAreaLower.LegendScrollerY;
    }
    #endregion

    
    #region Layout
    //      ------

    const double plotAreaRatio = 0.7;


    protected override Size MeasureContentOverride(Size constraint) {
      //Debug.WriteLine("");
      //Debug.WriteLine($"-Chart2.MeasureContentOverride({constraint})");
      TotalZoom100Button!.Measure(new Size(constraint.Width, constraint.Height));
      double totalZoom100ButtonWidth = TotalZoom100Button.DesiredSize.Width;
      double totalZoom100ButtonHeight = TotalZoom100Button.DesiredSize.Height;
      LegendScrollerX.Legend.MinHeight = totalZoom100ButtonHeight;

      LegendScrollerX.Measure(constraint);
      double zoomButtonDimension = TotalZoomOutButton!.Width = TotalZoomOutButton.Height = TotalZoomInButton!.Height = TotalZoomInButton.Width = 
        LegendScrollerX.ScrollBarHeight;

      double legendXHeight = Math.Min(constraint.Height, Math.Max(LegendScrollerX.DesiredSize.Height, totalZoom100ButtonHeight + zoomButtonDimension));
      double plotArea0Height = (constraint.Height - legendXHeight) * plotAreaRatio;
      double plotArea1Height = (constraint.Height - legendXHeight) * (1-plotAreaRatio);
      LegendScrollerYUpper.Measure(new Size(constraint.Width, plotArea0Height));
      LegendScrollerYLower.Measure(new Size(constraint.Width, plotArea1Height));
      double legendScrollerYMaxWidth = Math.Max(LegendScrollerYUpper.DesiredSize.Width, LegendScrollerYLower.DesiredSize.Width);
      //Debug.WriteLine($"-legendScrollerYMaxWidth: {legendScrollerYMaxWidth:N0} = Max(UpperDesiredWidth: {LegendScrollerYUpper.DesiredSize.Width:N0}, Lower.DesiredWidth: {LegendScrollerYLower.DesiredSize.Width:N0})");

      double legendWidth = Math.Min(constraint.Width, Math.Max(legendScrollerYMaxWidth, totalZoom100ButtonWidth));
      //Debug.WriteLine($"-legendWidth: {legendWidth:N0}");
      double plotAreaWidth = constraint.Width-legendWidth;
      PlotAreaUpper.Measure(new Size(plotAreaWidth, plotArea0Height));
      PlotAreaLower.Measure(new Size(plotAreaWidth, plotArea1Height));

      TotalZoomInButton.Measure(new Size(TotalZoomInButton.Width, TotalZoomInButton.Height));
      TotalZoomOutButton.Measure(new Size(TotalZoomOutButton.Width, TotalZoomOutButton.Height));

      //always remeasure xLegend because in the beginning it is given the whole width of Chart2Plots1X2YLegends
      LegendScrollerX.Measure(new Size(plotAreaWidth, legendXHeight));

      //////////allow Chart to measure its own controls
      ////////base.MeasureChartControls(constraint);


      Size returnedSize = constraint;
      if (double.IsInfinity(constraint.Height)) {
        returnedSize.Height = LegendScrollerX.DesiredSize.Height + LegendScrollerYUpper.DesiredSize.Height + LegendScrollerYLower.DesiredSize.Height;
      }
      if (double.IsInfinity(constraint.Width)) {
        returnedSize.Width = LegendScrollerX.DesiredSize.Width + legendScrollerYMaxWidth;
      }
      //Debug.WriteLine($"-return {returnedSize.Width:N0}, {returnedSize.Height:N0}");
      return returnedSize;
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      //Debug.WriteLine("");
      //Debug.WriteLine($".Chart2.ArrangeContentOverride(Width: {arrangeRect.Width:N0}, Height: {arrangeRect.Height:N0})");
      double legendWidth = Math.Min(arrangeRect.Width, 
        Math.Max(Math.Max(LegendScrollerYUpper.DesiredSize.Width, LegendScrollerYLower.DesiredSize.Width), TotalZoom100Button!.DesiredSize.Width));
      //Debug.WriteLine($".legendWidth: {legendWidth:N0} = Min(arrangeWidth: {arrangeRect.Width}, Max(UpperDesiredWidth: {LegendScrollerYUpper.DesiredSize.Width:N0}, Lower.DesiredWidth: {LegendScrollerYLower.DesiredSize.Width:N0}, ZoomButton.DesiredWidth: {TotalZoom100Button!.DesiredSize.Width:N0}))");
      double remainingWidth = arrangeRect.Width - legendWidth;
      double legendXHeight = Math.Min(arrangeRect.Height, 
        Math.Max(LegendScrollerX.DesiredSize.Height, TotalZoom100Button.DesiredSize.Height + TotalZoomOutButton!.DesiredSize.Height));
      double remainingHeight = arrangeRect.Height - legendXHeight;
      double plotArea0Height = remainingHeight * plotAreaRatio;
      double plotArea1Height = remainingHeight * (1-plotAreaRatio);
      LegendScrollerYUpper.ArrangeBorderPadding(arrangeRect, remainingWidth, 0,               legendWidth, plotArea0Height);
      LegendScrollerYLower.ArrangeBorderPadding(arrangeRect, remainingWidth, plotArea0Height, legendWidth, plotArea1Height);
      LegendScrollerX.ArrangeBorderPadding(arrangeRect, 0, remainingHeight, remainingWidth, legendXHeight);
      //arrange plot-area after scrollers, which might change the values plot-area has to display
      PlotAreaUpper.ArrangeBorderPadding(arrangeRect, 0, 0,               remainingWidth, plotArea0Height);
      PlotAreaLower.ArrangeBorderPadding(arrangeRect, 0, plotArea0Height, remainingWidth, plotArea1Height);
      TotalZoom100Button.ArrangeBorderPadding(arrangeRect, remainingWidth, remainingHeight, legendWidth, TotalZoom100Button.DesiredSize.Height);

      double zoomInOutY = remainingHeight + TotalZoom100Button.DesiredSize.Height;
      TotalZoomOutButton.ArrangeBorderPadding(arrangeRect, remainingWidth, zoomInOutY, 
        TotalZoomOutButton.DesiredSize.Width, TotalZoomOutButton.DesiredSize.Height);
      TotalZoomInButton!.ArrangeBorderPadding(arrangeRect, arrangeRect.Width - TotalZoomInButton!.DesiredSize.Width, zoomInOutY, TotalZoomInButton.DesiredSize.Width, TotalZoomInButton.DesiredSize.Height);
      //Debug.WriteLine($".return {arrangeRect.Width:N0}, {arrangeRect.Height:N0}");

      //////////allow Chart to arrange its own controls
      ////////base.ArrangeChartControls(arrangeRect);

      return arrangeRect.Size;
    }
    #endregion
  }
}
