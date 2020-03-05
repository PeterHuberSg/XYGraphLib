﻿// Chart1Plot1X1YLegend displays some line graphics in the PlotArea with legends and scroll bars for eadh X and Y axis:
// 
// +-------------------------+---------+--------------+
// |PlotArea                 |LegendScrollerY         |
// |XYGridLines              |                        |
// +-------------------------+---------+--------------+
// |XLegendScroller          |Total Zoom Buttons      |
// +-------------------------+------------------------+
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace XYGraphLib {
  
  
  public class Chart1Plot1X1YLegend: Chart {


    #region Properties
    //      ----------

    /// <summary>
    /// PlotArea
    /// </summary>
    public PlotArea PlotArea {
      get { return (PlotArea)GetValue(PlotAreaProperty); }
      private set { SetValue(PlotAreaProperty, value); }
    }
    readonly PlotArea plotArea; //local copy for faster acces

    // DependencyProperty definition for PlotArea
    public static readonly DependencyProperty PlotAreaProperty = 
    DependencyProperty.Register("PlotArea", typeof(PlotArea), typeof(Chart1Plot1X1YLegend));


    /// <summary>
    /// YLegend Scroller
    /// </summary>
    public LegendScrollerY LegendScrollerY {
      get { return (LegendScrollerY)GetValue(LegendScrollerYProperty); }
      private set { SetValue(LegendScrollerYProperty, value); }
    }
    readonly LegendScrollerY legendScrollerY; //local copy for faster acces

    // DependencyProperty definition for LegendScrollerY
    public static readonly DependencyProperty LegendScrollerYProperty = 
    DependencyProperty.Register("LegendScrollerY", typeof(LegendScrollerY), typeof(Chart1Plot1X1YLegend));


    /// <summary>
    /// XLegend Scroller
    /// </summary>
    public LegendScrollerX LegendScrollerX {
      get { return (LegendScrollerX)GetValue(LegendScrollerXProperty); }
      private set { SetValue(LegendScrollerXProperty, value); }
    }
    readonly LegendScrollerX legendScrollerX; //local copy for faster acces

    // DependencyProperty definition for XLegendScroller
    public static readonly DependencyProperty LegendScrollerXProperty = 
    DependencyProperty.Register("LegendScrollerX", typeof(LegendScrollerX), typeof(Chart1Plot1X1YLegend));
    #endregion

    
    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor
    /// </summary>
    public Chart1Plot1X1YLegend(): 
      this(new PlotArea(), new LegendScrollerX(), new LegendScrollerY(), new Grid()) { }


    /// <summary>
    /// Constructor supporting Chart1Plot1X1YLegend with plugged in components
    /// </summary>
    public Chart1Plot1X1YLegend(PlotArea newPlotArea, LegendScrollerX newLegendScrollerX,
      LegendScrollerY newLegendScrollerY, Grid newZoomGrid) 
    {
      PlotArea = plotArea = Add(newPlotArea);

      LegendScrollerY = legendScrollerY = Add(newLegendScrollerY);
      legendScrollerY.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      legendScrollerY.Legend.HorizontalAlignment = HorizontalAlignment.Stretch;
      legendScrollerY.Legend.HorizontalContentAlignment = HorizontalAlignment.Left;

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
    public override void FillData<TRecord>(IEnumerable<TRecord> newRecords, SerieSetting<TRecord>[] newSerieSettings){
      plotArea.ClearRenderers();
      legendScrollerX.Reset();
      legendScrollerY.Reset();

      addGridLineRenderers();

      base.FillData<TRecord>(newRecords, newSerieSettings);

      for (int serieIndex = 0; serieIndex < newSerieSettings.Length; serieIndex++) {
        Renderer renderer = CreateGraphRenderer<TRecord>(serieIndex, newSerieSettings[serieIndex]);
        if (renderer!=null) {
          if (newSerieSettings[serieIndex].Group==0) {
            AddRenderer(renderer, plotArea, legendScrollerX, legendScrollerY);
          } else {
            throw new Exception("Only group 0 is supported. SerieSettings[" + serieIndex + "]: " + newSerieSettings[serieIndex]);
          }
        }
      }
    }


    /// <summary>
    /// Add strings in chart
    /// </summary>
    /// <param name="chartNotes">string to be displayed, font formatting and links to lists.</param>
    /// <param name="fontDefinitions">if null, chartNotes.FontDefinitionId must be 0. The Font information from this chart will be used </param>
    /// <param name="IsAddToUpper">if false, the notes will be added to to lower plotarea</param>
    protected void AddNotes(IEnumerable<ChartNote> chartNotes, FontDefinition[] fontDefinitions) {
      RendererNotes rendererNotes = CreateNotesRenderer(chartNotes, fontDefinitions);
      AddRenderer(rendererNotes, plotArea, legendScrollerX, legendScrollerY);
    }


    private void addGridLineRenderers() {
      //x-gridlines are controlled by y-legends and y-gridlines are controll by x-legends

      //link RendererGridLineX with legendScrollerY
      AddRenderer(new RendererGridLineX(legendScrollerY, Brushes.DarkGray, 1), plotArea, null, legendScrollerY);
      //link RendererGridLineY with legendScrollerX
      AddRenderer(new RendererGridLineY(legendScrollerX, Brushes.DarkGray, 1), plotArea, legendScrollerX, null);
    }
    #endregion

    
    #region Layouting
    //      ---------

    //double xLegendHeight = double.NegativeInfinity;
    //double zoomButtonSize = double.NegativeInfinity;


    const double plotAreaRatio = 0.7;


    protected override Size MeasureContentOverride(Size constraint) {
       if (plotArea.RendererCount==0) {
        //empty graphic. Add at least the grid lines
        addGridLineRenderers();
      }

      TotalZoom100Button.Measure(new Size(constraint.Width, constraint.Height));
      double totalZoom100ButtonWidth = TotalZoom100Button.DesiredSize.Width;
      double totalZoom100ButtonHeight = TotalZoom100Button.DesiredSize.Height;
      legendScrollerX.Legend.MinHeight = totalZoom100ButtonHeight;

      legendScrollerX.Measure(constraint);
      double zoomButtonDimension = TotalZoomOutButton.Width = TotalZoomOutButton.Height = TotalZoomInButton.Height = TotalZoomInButton.Width = 
        legendScrollerX.ScrollBarHeight;

      double legendHeight = Math.Min(constraint.Height, Math.Max(legendScrollerX.DesiredSize.Height, totalZoom100ButtonHeight + zoomButtonDimension));
      double plotAreaHeight = (constraint.Height - legendHeight) * plotAreaRatio;
      legendScrollerY.Measure(new Size(constraint.Width, plotAreaHeight));
      double legendScrollerYWidth = legendScrollerY.DesiredSize.Width;

      double legendWidth = Math.Min(constraint.Width, Math.Max(legendScrollerYWidth, totalZoom100ButtonWidth));
      double plotAreaWidth = constraint.Width-legendWidth;
      plotArea.Measure(new Size(plotAreaWidth, plotAreaHeight));

      TotalZoomInButton.Measure(new Size(TotalZoomInButton.Width, TotalZoomInButton.Height));
      TotalZoomOutButton.Measure(new Size(TotalZoomOutButton.Width, TotalZoomOutButton.Height));

      //always remeasure xLegend because in the beginning it is given the whole width of the chart
      legendScrollerX.Measure(new Size(plotAreaWidth, legendHeight));


      Size returnedSize = constraint;
      if (double.IsInfinity(constraint.Height)) {
        returnedSize.Height = legendScrollerX.DesiredSize.Height + legendScrollerY.DesiredSize.Height;
      }
      if (double.IsInfinity(constraint.Width)) {
        returnedSize.Width = legendScrollerX.DesiredSize.Width + legendScrollerYWidth;
      }
      return returnedSize;
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      double legendWidth = Math.Min(arrangeRect.Width, 
        Math.Max(legendScrollerY.DesiredSize.Width, TotalZoom100Button.DesiredSize.Width));
      double remainingWidth = arrangeRect.Width - legendWidth;
      double legendHeight = Math.Min(arrangeRect.Height, 
        Math.Max(legendScrollerX.DesiredSize.Height, TotalZoom100Button.DesiredSize.Height + TotalZoomOutButton.DesiredSize.Height));
      double remainingHeight = arrangeRect.Height - legendHeight;
      legendScrollerY.ArrangeBorderPadding(arrangeRect, remainingWidth, 0, legendWidth, remainingHeight);
      legendScrollerX.ArrangeBorderPadding(arrangeRect, 0, remainingHeight, remainingWidth, legendHeight);
      //arrange plotarea after scrollers, which might change the values plotarea has to display
      plotArea.ArrangeBorderPadding(arrangeRect, 0, 0, remainingWidth, remainingHeight);
      TotalZoom100Button.ArrangeBorderPadding(arrangeRect, remainingWidth, remainingHeight, legendWidth, TotalZoom100Button.DesiredSize.Height);

      double zoomInOutY = remainingHeight + TotalZoom100Button.DesiredSize.Height;
      TotalZoomOutButton.ArrangeBorderPadding(arrangeRect, remainingWidth, zoomInOutY, 
        TotalZoomOutButton.DesiredSize.Width, TotalZoomOutButton.DesiredSize.Height);
      TotalZoomInButton.ArrangeBorderPadding(arrangeRect, arrangeRect.Width - TotalZoomInButton.DesiredSize.Width, zoomInOutY, TotalZoomInButton.DesiredSize.Width, TotalZoomInButton.DesiredSize.Height);
      return arrangeRect.Size;
    }
    #endregion
  }
}
