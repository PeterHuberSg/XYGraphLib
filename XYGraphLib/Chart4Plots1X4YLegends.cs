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
    public PlotArea PlotArea0 {
      get { return (PlotArea)GetValue(PlotArea0Property); }
      private set { SetValue(PlotArea0Property, value); }
    }
    readonly PlotArea plotArea0; //local copy for faster access

    // DependencyProperty definition for PlotArea0
    public static readonly DependencyProperty PlotArea0Property = 
    DependencyProperty.Register("PlotArea0", typeof(PlotArea), typeof(Chart4Plots1X4YLegends));


    /// <summary>
    /// Second highest plotArea
    /// </summary>
    public PlotArea PlotArea1 {
      get { return (PlotArea)GetValue(PlotArea1Property); }
      private set { SetValue(PlotArea1Property, value); }
    }
    readonly PlotArea plotArea1; //local copy for faster access

    // DependencyProperty definition for PlotArea1
    public static readonly DependencyProperty PlotArea1Property =
    DependencyProperty.Register("PlotArea1", typeof(PlotArea), typeof(Chart4Plots1X4YLegends));


    /// <summary>
    /// Third highest plotArea
    /// </summary>
    public PlotArea PlotArea2 {
      get { return (PlotArea)GetValue(PlotArea2Property); }
      private set { SetValue(PlotArea2Property, value); }
    }
    readonly PlotArea plotArea2; //local copy for faster access

    // DependencyProperty definition for PlotArea2
    public static readonly DependencyProperty PlotArea2Property =
    DependencyProperty.Register("PlotArea2", typeof(PlotArea), typeof(Chart4Plots1X4YLegends));


    /// <summary>
    /// Lowest plotArea
    /// </summary>
    public PlotArea PlotArea3 {
      get { return (PlotArea)GetValue(PlotArea3Property); }
      private set { SetValue(PlotArea3Property, value); }
    }
    readonly PlotArea plotArea3; //local copy for faster access

    // DependencyProperty definition for PlotArea3
    public static readonly DependencyProperty PlotArea3Property =
    DependencyProperty.Register("PlotArea3", typeof(PlotArea), typeof(Chart4Plots1X4YLegends));


    /// <summary>
    /// YLegend Scroller for highest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY0 {
      get { return (LegendScrollerY)GetValue(LegendScrollerY0Property); }
      private set { SetValue(LegendScrollerY0Property, value); }
    }
    readonly LegendScrollerY legendScrollerY0; //local copy for faster access

    // DependencyProperty definition for LegendScrollerY0
    public static readonly DependencyProperty LegendScrollerY0Property = 
    DependencyProperty.Register("LegendScrollerY0", typeof(LegendScrollerY), typeof(Chart4Plots1X4YLegends));


    /// <summary>
    /// YLegend Scroller for second highest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY1 {
      get { return (LegendScrollerY)GetValue(LegendScrollerY1Property); }
      private set { SetValue(LegendScrollerY1Property, value); }
    }
    readonly LegendScrollerY legendScrollerY1; //local copy for faster access

    // DependencyProperty definition for LegendScrollerY1
    public static readonly DependencyProperty LegendScrollerY1Property =
    DependencyProperty.Register("LegendScrollerY1", typeof(LegendScrollerY), typeof(Chart4Plots1X4YLegends));


    /// <summary>
    /// YLegend Scroller for third highest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY2 {
      get { return (LegendScrollerY)GetValue(LegendScrollerY2Property); }
      private set { SetValue(LegendScrollerY2Property, value); }
    }
    readonly LegendScrollerY legendScrollerY2; //local copy for faster access

    // DependencyProperty definition for LegendScrollerY2
    public static readonly DependencyProperty LegendScrollerY2Property =
    DependencyProperty.Register("LegendScrollerY2", typeof(LegendScrollerY), typeof(Chart4Plots1X4YLegends));


    /// <summary>
    /// YLegend Scroller for lowest plotArea
    /// </summary>
    public LegendScrollerY LegendScrollerY3 {
      get { return (LegendScrollerY)GetValue(LegendScrollerY3Property); }
      private set { SetValue(LegendScrollerY3Property, value); }
    }
    readonly LegendScrollerY legendScrollerY3; //local copy for faster access

    // DependencyProperty definition for LegendScrollerY3
    public static readonly DependencyProperty LegendScrollerY3Property =
    DependencyProperty.Register("LegendScrollerY3", typeof(LegendScrollerY), typeof(Chart4Plots1X4YLegends));


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
    DependencyProperty.Register("LegendScrollerX", typeof(LegendScrollerX), typeof(Chart4Plots1X4YLegends));
    #endregion

    
    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor
    /// </summary>
    public Chart4Plots1X4YLegends(): 
      this(
        new PlotArea(), 
        new PlotArea(),
        new PlotArea(),
        new PlotArea(), 
        new LegendScrollerX(), 
        new LegendScrollerY(),
        new LegendScrollerY(),
        new LegendScrollerY(), 
        new LegendScrollerY()) 
    { }


    readonly double heightRatio0;
    readonly double heightRatio1;
    readonly double heightRatio2;
    readonly double heightRatio3;


    /// <summary>
    /// Constructor supporting XYGraph with plugged in components
    /// </summary>
    public Chart4Plots1X4YLegends(
      PlotArea newPlotArea0,
      PlotArea newPlotArea1,
      PlotArea newPlotArea2,
      PlotArea newPlotArea3, 
      LegendScrollerX newLegendScrollerX,
      LegendScrollerY newLegendScrollerY0, 
      LegendScrollerY newLegendScrollerY1, 
      LegendScrollerY newLegendScrollerY2, 
      LegendScrollerY newLegendScrollerY3,
      int newHeightRatio0 = 1,
      int newHeightRatio1 = 1,
      int newHeightRatio2 = 1,
      int newHeightRatio3 = 1) 
    {
      PlotArea0 = plotArea0 = Add(newPlotArea0);
      PlotArea1 = plotArea1 = Add(newPlotArea1);
      PlotArea2 = plotArea2 = Add(newPlotArea2);
      PlotArea3 = plotArea3 = Add(newPlotArea3);

      LegendScrollerY0 = legendScrollerY0 = Add(newLegendScrollerY0);
      legendScrollerY0.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY0.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      legendScrollerY0.Legend.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY0.Legend.HorizontalContentAlignment = HorizontalAlignment.Left;

      LegendScrollerY1 = legendScrollerY1 = Add(newLegendScrollerY1);
      legendScrollerY1.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY1.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      legendScrollerY1.Legend.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY1.Legend.HorizontalContentAlignment = HorizontalAlignment.Left;

      LegendScrollerY2 = legendScrollerY2 = Add(newLegendScrollerY2);
      legendScrollerY2.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY2.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      legendScrollerY2.Legend.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY2.Legend.HorizontalContentAlignment = HorizontalAlignment.Left;

      LegendScrollerY3 = legendScrollerY3 = Add(newLegendScrollerY3);
      legendScrollerY3.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY3.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      legendScrollerY3.Legend.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY3.Legend.HorizontalContentAlignment = HorizontalAlignment.Left;

      LegendScrollerX = legendScrollerX = Add(newLegendScrollerX);

      if (newHeightRatio0<=0 || newHeightRatio1<=0 || newHeightRatio2<=0 || newHeightRatio3<=0) {
        throw new ArgumentException("HeightRatio cannot be 0 or negative.");
      }
      double totalHight = newHeightRatio0 + newHeightRatio1 + newHeightRatio2 + newHeightRatio3;
      heightRatio0 = newHeightRatio0 / totalHight;
      heightRatio1 = newHeightRatio1 / totalHight;
      heightRatio2 = newHeightRatio2 / totalHight;
      heightRatio3 = newHeightRatio3 / totalHight;

      AddZoomButtons();
    }
    #endregion

    
    #region Fill Data
    //      ---------

    public int MainSerieIndex = int.MinValue;


    /// <summary>
    /// Updates graphic with new data series 
    /// </summary>
    public override void FillData<TRecord>(
      IEnumerable<TRecord> newRecords,
      SerieSetting<TRecord>[] newSerieSettings,
      string? xName = null,
      string? xUnit = null,
      Func<TRecord, string>? stringGetter = null) 
    {
      plotArea0.ClearRenderers();
      plotArea1.ClearRenderers();
      plotArea2.ClearRenderers();
      plotArea3.ClearRenderers();
      legendScrollerX.Reset();
      legendScrollerY0.Reset();
      legendScrollerY1.Reset();
      legendScrollerY2.Reset();
      legendScrollerY3.Reset();

      addGridLineRenderers();

      base.FillData<TRecord>(newRecords, newSerieSettings, xName, xUnit, stringGetter);

      for (int serieIndex = 0; serieIndex < newSerieSettings.Length; serieIndex++) {
        Renderer? renderer = CreateGraphRenderer<TRecord>(serieIndex, newSerieSettings[serieIndex]);
        if (renderer!=null) {
          if (newSerieSettings[serieIndex].Group==0) {
            AddRenderer(renderer, plotArea0, legendScrollerX, legendScrollerY0);
          } else if (newSerieSettings[serieIndex].Group==1) {
            AddRenderer(renderer, plotArea1, legendScrollerX, legendScrollerY1);
          } else if (newSerieSettings[serieIndex].Group==2) {
            AddRenderer(renderer, plotArea2, legendScrollerX, legendScrollerY2);
          } else if (newSerieSettings[serieIndex].Group==3) {
            AddRenderer(renderer, plotArea3, legendScrollerX, legendScrollerY3);
          } else {
            throw new Exception("Only group 0 to 3 are supported. SerieSettings[" + serieIndex + "]: " + newSerieSettings[serieIndex]);
          }
        }
      }
    }


    /// <summary>
    /// Add strings in chart
    /// </summary>
    /// <param name="chartNotes">string to be displayed, font formatting and links to lists.</param>
    /// <param name="fontDefinitions">if null, chartNotes.FontDefinitionId must be 0. The Font information from this chart will be used </param>
    /// <param name="group">0: highest plot area ... 3: lowest plot area</param>
    public void AddNotes(IEnumerable<ChartNote> chartNotes, FontDefinition[] fontDefinitions, int group) {
      RendererNotes rendererNotes = CreateNotesRenderer(chartNotes, fontDefinitions);
      if (group==0) {
        AddRenderer(rendererNotes, plotArea0, legendScrollerX, legendScrollerY0);
      } else if (group==1) {
        AddRenderer(rendererNotes, plotArea1, legendScrollerX, legendScrollerY1);
      } else if (group==2) {
        AddRenderer(rendererNotes, plotArea2, legendScrollerX, legendScrollerY2);
      } else if (group==3) {
        AddRenderer(rendererNotes, plotArea3, legendScrollerX, legendScrollerY3);
      }
    }


    private void addGridLineRenderers() {
      //x-grid-lines are controlled by y-legends and y-grid-lines are controlled by x-legends

      //link highest RendererGridLineX with LegendScrollerY0
      AddRenderer(new RendererGridLineX(legendScrollerY0, Brushes.DarkGray, 1), plotArea0, null, legendScrollerY0);
      //link highest RendererGridLineY with legendScrollerX
      AddRenderer(new RendererGridLineY(legendScrollerX, Brushes.DarkGray, 1), plotArea0, legendScrollerX, null);

      //link second highest RendererGridLineX with LegendScrollerY1
      AddRenderer(new RendererGridLineX(legendScrollerY1, Brushes.DarkGray, 1), plotArea1, null, legendScrollerY1);
      //link second highest RendererGridLineY with legendScrollerX
      AddRenderer(new RendererGridLineY(legendScrollerX, Brushes.DarkGray, 1), plotArea1, legendScrollerX, null);

      //link third highest RendererGridLineX with LegendScrollerY2
      AddRenderer(new RendererGridLineX(legendScrollerY2, Brushes.DarkGray, 1), plotArea2, null, legendScrollerY2);
      //link third highest RendererGridLineY with legendScrollerX
      AddRenderer(new RendererGridLineY(legendScrollerX, Brushes.DarkGray, 1), plotArea2, legendScrollerX, null);

      //link lowest RendererGridLineX with LegendScrollerY3
      AddRenderer(new RendererGridLineX(legendScrollerY3, Brushes.DarkGray, 1), plotArea3, null, legendScrollerY3);
      //link lowest RendererGridLineY with legendScrollerX
      AddRenderer(new RendererGridLineY(legendScrollerX, Brushes.DarkGray, 1), plotArea3, legendScrollerX, null);
    }
    #endregion


    #region Layout
    //      ------

    //double xLegendHeight = double.NegativeInfinity;
    //double zoomButtonSize = double.NegativeInfinity;


    protected override Size MeasureContentOverride(Size constraint) {
       if (plotArea0.RendererCount==0) {
#if DEBUG
         if (plotArea1.RendererCount!=0 || plotArea2.RendererCount!=0 || plotArea3.RendererCount!=0) {
           throw new Exception("plotArea1..3 are supposed to be in sync with plotArea0");
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
      double plotArea0Height = (constraint.Height - legendHeight) * heightRatio0;
      double plotArea1Height = (constraint.Height - legendHeight) * heightRatio1;
      double plotArea2Height = (constraint.Height - legendHeight) * heightRatio2;
      double plotArea3Height = (constraint.Height - legendHeight) * heightRatio3;
      legendScrollerY0.Measure(new Size(constraint.Width, plotArea0Height));
      legendScrollerY1.Measure(new Size(constraint.Width, plotArea1Height));
      legendScrollerY2.Measure(new Size(constraint.Width, plotArea2Height));
      legendScrollerY3.Measure(new Size(constraint.Width, plotArea3Height));
      double legendScrollerYMaxWidth = 
        Math.Max(
          Math.Max(
            Math.Max(legendScrollerY0.DesiredSize.Width, legendScrollerY1.DesiredSize.Width), 
            legendScrollerY2.DesiredSize.Width), 
          legendScrollerY3.DesiredSize.Width);

      double legendWidth = Math.Min(constraint.Width, Math.Max(legendScrollerYMaxWidth, totalZoom100ButtonWidth));
      double plotAreaWidth = constraint.Width-legendWidth;
      plotArea0.Measure(new Size(plotAreaWidth, plotArea0Height));
      plotArea1.Measure(new Size(plotAreaWidth, plotArea1Height));
      plotArea2.Measure(new Size(plotAreaWidth, plotArea2Height));
      plotArea3.Measure(new Size(plotAreaWidth, plotArea3Height));

      TotalZoomInButton.Measure(new Size(TotalZoomInButton.Width, TotalZoomInButton.Height));
      TotalZoomOutButton.Measure(new Size(TotalZoomOutButton.Width, TotalZoomOutButton.Height));

      //always remeasure xLegend because in the beginning it is given the whole width of Chart4Plots1X4YLegends
      legendScrollerX.Measure(new Size(plotAreaWidth, legendHeight));


      Size returnedSize = constraint;
      if (double.IsInfinity(constraint.Height)) {
        returnedSize.Height = legendScrollerX.DesiredSize.Height + legendScrollerY0.DesiredSize.Height + 
          legendScrollerY1.DesiredSize.Height + legendScrollerY2.DesiredSize.Height + legendScrollerY3.DesiredSize.Height;
      }
      if (double.IsInfinity(constraint.Width)) {
        returnedSize.Width = legendScrollerX.DesiredSize.Width + legendScrollerYMaxWidth;
      }
      return returnedSize;
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      double legendWidth = 
        Math.Min(arrangeRect.Width,
          Math.Max(
            Math.Max(
              Math.Max(
                Math.Max(legendScrollerY0.DesiredSize.Width, legendScrollerY1.DesiredSize.Width),
              legendScrollerY2.DesiredSize.Width),
            legendScrollerY3.DesiredSize.Width),
          TotalZoom100Button!.DesiredSize.Width));
      double remainingWidth = arrangeRect.Width - legendWidth;
      double legendHeight = Math.Min(arrangeRect.Height, 
        Math.Max(legendScrollerX.DesiredSize.Height, TotalZoom100Button.DesiredSize.Height + TotalZoomOutButton!.DesiredSize.Height));
      double remainingHeight = arrangeRect.Height - legendHeight;
      double plotArea0Height = remainingHeight * heightRatio0;
      double plotArea1Height = remainingHeight * heightRatio1;
      double plotArea2Height = remainingHeight * heightRatio2;
      double plotArea3Height = remainingHeight * heightRatio3;
      var y = 0.0;
      legendScrollerY0.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea0Height);
      y += plotArea0Height;
      legendScrollerY1.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea1Height);
      y += plotArea1Height;
      legendScrollerY2.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea2Height);
      y += plotArea2Height;
      legendScrollerY3.ArrangeBorderPadding(arrangeRect, remainingWidth, y, legendWidth, plotArea3Height);
      legendScrollerX.ArrangeBorderPadding(arrangeRect, 0, remainingHeight, remainingWidth, legendHeight);

      //arrange plot-area after scrollers, which might change the values plot-area has to display
      y = 0.0;
      plotArea0.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea0Height);
      y += plotArea0Height;
      plotArea1.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea1Height);
      y += plotArea1Height;
      plotArea2.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea2Height);
      y += plotArea2Height;
      plotArea3.ArrangeBorderPadding(arrangeRect, 0, y, remainingWidth, plotArea3Height);
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
