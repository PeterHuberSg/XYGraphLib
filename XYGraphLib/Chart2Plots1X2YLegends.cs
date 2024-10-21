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
// ┌─────────────────────────┬───────────────────┐
// │ PlotArea0               │ LegendScrollerY0  │
// ├─────────────────────────┼───────────────────┤
// │ PlotArea1               │ LegendScrollerY1  │
// ├─────────────────────────┼───────────────────┤
// │XLegendScroller          │Total Zoom Buttons │
// └─────────────────────────┴───────────────────┘
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
    public PlotArea PlotAreaUpper {
      get { return (PlotArea)GetValue(PlotAreaUpperProperty); }
      private set { SetValue(PlotAreaUpperProperty, value); }
    }
    readonly PlotArea plotAreaUpper; //local copy for faster access

    // DependencyProperty definition for PlotAreaUpper
    public static readonly DependencyProperty PlotAreaUpperProperty = 
    DependencyProperty.Register("PlotAreaUpper", typeof(PlotArea), typeof(Chart2Plots1X2YLegends));


    /// <summary>
    /// Lower plotArea
    /// </summary>
    public PlotArea PlotAreaLower {
      get { return (PlotArea)GetValue(PlotAreaLowerProperty); }
      private set { SetValue(PlotAreaLowerProperty, value); }
    }
    readonly PlotArea plotAreaLower; //local copy for faster access

    // DependencyProperty definition for PlotAreaLower
    public static readonly DependencyProperty PlotAreaLowerProperty = 
    DependencyProperty.Register("PlotAreaLower", typeof(PlotArea), typeof(Chart2Plots1X2YLegends));

    
    /// <summary>
    /// YLegend Scroller for upper plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerYUpper {
      get { return (LegendScrollerY)GetValue(LegendScrollerYUpperProperty); }
      private set { SetValue(LegendScrollerYUpperProperty, value); }
    }
    readonly LegendScrollerY legendScrollerYUpper; //local copy for faster access

    // DependencyProperty definition for LegendScrollerYUpper
    public static readonly DependencyProperty LegendScrollerYUpperProperty = 
    DependencyProperty.Register("LegendScrollerYUpper", typeof(LegendScrollerY), typeof(Chart2Plots1X2YLegends));


    /// <summary>
    /// YLegend Scroller for lower plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerYLower {
      get { return (LegendScrollerY)GetValue(LegendScrollerYLowerProperty); }
      private set { SetValue(LegendScrollerYLowerProperty, value); }
    }
    readonly LegendScrollerY legendScrollerYLower; //local copy for faster access

    // DependencyProperty definition for LegendScrollerYLower
    public static readonly DependencyProperty LegendScrollerYLowerProperty = 
    DependencyProperty.Register("LegendScrollerYLower", typeof(LegendScrollerY), typeof(Chart2Plots1X2YLegends));


    /// <summary>
    /// XLegend Scroller for upper plotArea
    /// </summary>
    public LegendScrollerX LegendScrollerX {
      get { return (LegendScrollerX)GetValue(LegendScrollerXProperty); }
      private set { SetValue(LegendScrollerXProperty, value); }
    }
    readonly LegendScrollerX legendScrollerX; //local copy for faster access

    // DependencyProperty definition for XLegendScroller
    public static readonly DependencyProperty LegendScrollerXProperty = 
    DependencyProperty.Register("LegendScrollerX", typeof(LegendScrollerX), typeof(Chart2Plots1X2YLegends));
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Only WPF Editor from Visual Studio should use this constructor
    /// </summary>
    public Chart2Plots1X2YLegends(): 
      this(new PlotArea(), new PlotArea(), new LegendScrollerX(), new LegendScrollerY(), new LegendScrollerY()) { }


    /// <summary>
    /// Constructor supporting XYGraph with plugged in components
    /// </summary>
    public Chart2Plots1X2YLegends(PlotArea newPlotAreaUpper, PlotArea newPlotAreaLower, 
    LegendScrollerX newLegendScrollerX, LegendScrollerY newLegendScrollerYUpper, LegendScrollerY newLegendScrollerYLower) : 
      base() 
    {
      PlotAreaUpper = plotAreaUpper = Add(newPlotAreaUpper);
      PlotAreaLower = plotAreaLower = Add(newPlotAreaLower);

      LegendScrollerYUpper = legendScrollerYUpper = Add(newLegendScrollerYUpper);
      legendScrollerYUpper.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerYUpper.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      legendScrollerYUpper.Legend.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerYUpper.Legend.HorizontalContentAlignment = HorizontalAlignment.Left;
      
      LegendScrollerYLower = legendScrollerYLower = Add(newLegendScrollerYLower);
      legendScrollerYLower.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerYLower.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      legendScrollerYLower.Legend.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerYLower.Legend.HorizontalContentAlignment = HorizontalAlignment.Left;

      LegendScrollerX = legendScrollerX = Add(newLegendScrollerX);

      AddZoomButtons();
    }
    #endregion

    
    #region Fill Data
    //      ---------

    public int MainSerieIndex = int.MinValue;


    /// <summary>
    /// Updates graphic with new data series 
    /// </summary>
    public override void FillData<TRecord>(IEnumerable<TRecord> newRecords, SerieSetting<TRecord>[] serieSettings) {
      plotAreaUpper.ClearRenderers();
      plotAreaLower.ClearRenderers();
      legendScrollerX.Reset();
      legendScrollerYUpper.Reset();
      legendScrollerYLower.Reset();

      addGridLineRenderers();

      base.FillData(newRecords, serieSettings);//ensures that SerieSettings is not null

      for (int serieIndex = 0; serieIndex < serieSettings!.Length; serieIndex++) {
        Renderer? renderer = CreateGraphRenderer(serieIndex, serieSettings[serieIndex]);
        if (renderer!=null) {
          if (serieSettings[serieIndex].Group==0) {
            AddRenderer(renderer, plotAreaUpper, legendScrollerX, legendScrollerYUpper);
          } else if (serieSettings[serieIndex].Group==1) {
            AddRenderer(renderer, plotAreaLower, legendScrollerX, legendScrollerYLower);
          } else {
            throw new Exception("Only group 0 and 1 are supported. SerieSettings[" + serieIndex + "]: " + serieSettings[serieIndex]);
          }
        }
      }
    }


    /// <summary>
    /// Add strings in chart
    /// </summary>
    /// <param name="chartNotes">string to be displayed, font formatting and links to lists.</param>
    /// <param name="fontDefinitions">if null, chartNotes.FontDefinitionId must be 0. The Font information from this chart will be used </param>
    /// <param name="IsAddToUpper">if false, the notes will be added to lower plot-area</param>
    public void AddNotes(IEnumerable<ChartNote> chartNotes, FontDefinition[] fontDefinitions, bool IsAddToUpper) {
      RendererNotes rendererNotes = CreateNotesRenderer(chartNotes, fontDefinitions);
      if (IsAddToUpper) {
        AddRenderer(rendererNotes, plotAreaUpper, legendScrollerX, legendScrollerYUpper);
      } else {
        AddRenderer(rendererNotes, plotAreaLower, legendScrollerX, legendScrollerYLower);
      }
    }


    private void addGridLineRenderers() {
      //x-grid-lines are controlled by y-legends and y-grid-lines are controlled by x-legends

      //link RendererGridLineXUpper with legendScrollerYUpper
      AddRenderer(new RendererGridLineX(legendScrollerYUpper, Brushes.DarkGray, 1), plotAreaUpper, null, legendScrollerYUpper);
      //link RendererGridLineYUpper with legendScrollerX
      AddRenderer(new RendererGridLineY(legendScrollerX, Brushes.DarkGray, 1), plotAreaUpper, legendScrollerX, null);
      //link RendererGridLineXLower with legendScrollerYLower
      AddRenderer(new RendererGridLineX(legendScrollerYLower, Brushes.DarkGray, 1), plotAreaLower, null, legendScrollerYLower);
      //link RendererGridLineYLower with legendScrollerX
      AddRenderer(new RendererGridLineY(legendScrollerX, Brushes.DarkGray, 1), plotAreaLower, legendScrollerX, null);
    }
    #endregion

    
    #region Layout
    //      ------

    //double xLegendHeight = double.NegativeInfinity;
    //double zoomButtonSize = double.NegativeInfinity;


    const double plotAreaRatio = 0.7;


    protected override Size MeasureContentOverride(Size constraint) {
       if (plotAreaUpper.RendererCount==0) {
#if DEBUG
         if (plotAreaLower.RendererCount!=0) {
           throw new Exception("plotArea1 is supposed to be in sync with plotArea0");
         }
#endif
        //empty graphic. Add at least the grid lines
        addGridLineRenderers();
      }

      TotalZoom100Button!.Measure(new Size(constraint.Width, constraint.Height));
      double totalZoom100ButtonWidth = TotalZoom100Button.DesiredSize.Width;
      double totalZoom100ButtonHeight = TotalZoom100Button.DesiredSize.Height;
      legendScrollerX.Legend.MinHeight = totalZoom100ButtonHeight;

      legendScrollerX.Measure(constraint);
      double zoomButtonDimension = TotalZoomOutButton!.Width = TotalZoomOutButton.Height = TotalZoomInButton!.Height = TotalZoomInButton.Width = 
        legendScrollerX.ScrollBarHeight;

      double legendHeight = Math.Min(constraint.Height, Math.Max(legendScrollerX.DesiredSize.Height, totalZoom100ButtonHeight + zoomButtonDimension));
      double plotArea0Height = (constraint.Height - legendHeight) * plotAreaRatio;
      double plotArea1Height = (constraint.Height - legendHeight) * (1-plotAreaRatio);
      legendScrollerYUpper.Measure(new Size(constraint.Width, plotArea0Height));
      legendScrollerYLower.Measure(new Size(constraint.Width, plotArea1Height));
      double legendScrollerYMaxWidth = Math.Max(legendScrollerYUpper.DesiredSize.Width, legendScrollerYLower.DesiredSize.Width);

      double legendWidth = Math.Min(constraint.Width, Math.Max(legendScrollerYMaxWidth, totalZoom100ButtonWidth));
      double plotAreaWidth = constraint.Width-legendWidth;
      plotAreaUpper.Measure(new Size(plotAreaWidth, plotArea0Height));
      plotAreaLower.Measure(new Size(plotAreaWidth, plotArea1Height));

      TotalZoomInButton.Measure(new Size(TotalZoomInButton.Width, TotalZoomInButton.Height));
      TotalZoomOutButton.Measure(new Size(TotalZoomOutButton.Width, TotalZoomOutButton.Height));

      //always remeasure xLegend because in the beginning it is given the whole width of Chart2Plots1X2YLegends
      legendScrollerX.Measure(new Size(plotAreaWidth, legendHeight));


      Size returnedSize = constraint;
      if (double.IsInfinity(constraint.Height)) {
        returnedSize.Height = legendScrollerX.DesiredSize.Height + legendScrollerYUpper.DesiredSize.Height + legendScrollerYLower.DesiredSize.Height;
      }
      if (double.IsInfinity(constraint.Width)) {
        returnedSize.Width = legendScrollerX.DesiredSize.Width + legendScrollerYMaxWidth;
      }
      return returnedSize;
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      double legendWidth = Math.Min(arrangeRect.Width, 
        Math.Max(Math.Max(legendScrollerYUpper.DesiredSize.Width, legendScrollerYLower.DesiredSize.Width), TotalZoom100Button!.DesiredSize.Width));
      double remainingWidth = arrangeRect.Width - legendWidth;
      double legendHeight = Math.Min(arrangeRect.Height, 
        Math.Max(legendScrollerX.DesiredSize.Height, TotalZoom100Button.DesiredSize.Height + TotalZoomOutButton!.DesiredSize.Height));
      double remainingHeight = arrangeRect.Height - legendHeight;
      double plotArea0Height = remainingHeight * plotAreaRatio;
      double plotArea1Height = remainingHeight * (1-plotAreaRatio);
      legendScrollerYUpper.ArrangeBorderPadding(arrangeRect, remainingWidth, 0,               legendWidth, plotArea0Height);
      legendScrollerYLower.ArrangeBorderPadding(arrangeRect, remainingWidth, plotArea0Height, legendWidth, plotArea1Height);
      legendScrollerX.ArrangeBorderPadding(arrangeRect, 0, remainingHeight, remainingWidth, legendHeight);
      //arrange plot-area after scrollers, which might change the values plot-area has to display
      plotAreaUpper.ArrangeBorderPadding(arrangeRect, 0, 0,               remainingWidth, plotArea0Height);
      plotAreaLower.ArrangeBorderPadding(arrangeRect, 0, plotArea0Height, remainingWidth, plotArea1Height);
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
