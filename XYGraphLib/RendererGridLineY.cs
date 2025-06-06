﻿/**************************************************************************************

XYGraphLib.RendererGridLineY
============================

Creates a Visual for the vertical grid lines displayed in the PlotArea.

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


namespace XYGraphLib {

  /// <summary>
  /// Creates a Visual for the vertical grid lines displayed in the PlotArea.
  /// </summary>
  public class RendererGridLineY: Renderer {

    #region Properties
    //      ----------

    /// <summary>
    /// Creates vertical grid lines, which are controlled by XLegendScroller defining how many lines need to be displayed and where
    /// </summary>
    public LegendScrollerX LegendScrollerX { get; private set; }
    #endregion


    #region Constructor
    //      -----------

    public RendererGridLineY(LegendScrollerX legendScrollerX, Brush strokeBrush, double strokeThickness):
      //gridlineY are parallel to vertical y-line, but the distance between them is controlled by LegendX, therefore dimension X
      base(strokeBrush, strokeThickness, DimensionMapX) 
    {
      LegendScrollerX = legendScrollerX;
    }
    #endregion


    #region Methods
    //      -------

    bool isFirstTime = true;//do the test only once


    /// <summary>
    /// Renders the vertical x-grid-line to the drawingContext, one line for each label in XLegend.
    /// </summary>
    protected override void OnCreateVisual(DrawingContext drawingContext, double width, double height, DrawingVisual _) {
      if (isFirstTime) {
        if (LegendScrollerX.Legend is not LegendX)
          throw new NotSupportedException($"RendererGridLineY works only with LegendX, but LegendScrollerX.Legend was {LegendScrollerX.Legend.GetType().Name}.");
        isFirstTime = false;
      }
      var legendX = (LegendX)LegendScrollerX.Legend;
      //grid-lines use only 1 dimension. Both for x and y grid-line, 
      double minDisplayValue = MinDisplayValues[0];

      // Create a GuidelineSet to get the lines exactly on a pixel
      var guidelines = new GuidelineSet();
      double halfPenWidth = StrokePen.Thickness / 2;
      foreach (double labelValue in legendX.LabelValues!) {
        double xPos = ScaleX * (labelValue - minDisplayValue);
        guidelines.GuidelinesX.Add(xPos + halfPenWidth);
      }

      drawingContext.PushGuidelineSet(guidelines);
      foreach (double labelValue in legendX.LabelValues) {
        double xPos = ScaleX * (labelValue - minDisplayValue);
        drawingContext.DrawLine(StrokePen, new Point(xPos, 0), new Point(xPos, height));
      }
      drawingContext.Pop();
    }
    #endregion
  }
}
