/**************************************************************************************

XYGraphLib.RendererGridLineX
============================

Creates a Visual for the horizontal grid lines displayed in the PlotArea.

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
  /// Creates a Visual for the horizontal grid lines displayed in the PlotArea.
  /// </summary>
  public class RendererGridLineX: Renderer {

    #region Properties
    //      ----------

    /// <summary>
    /// Creates horizontal grid lines, which are controlled by LegendScrollerY defining how many lines need to be displayed and where
    /// </summary>
    public LegendScrollerY LegendScrollerY { get; private set; }
    #endregion


    #region Constructor
    //      -----------

    public RendererGridLineX(LegendScrollerY legendScrollerY, System.Windows.Media.Brush strokeBrush, double strokeThickness):
      //gridlineX are parallel to horizontal x-line, but the distance between them is controlled by LegendY, therefore dimension Y
      base(strokeBrush, strokeThickness, DimensionMapY) 
    {
      LegendScrollerY = legendScrollerY;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Renders the horizontal x-grid-line to the drawingContext, one line for each label in YLegend.
    /// </summary>
    protected override void OnCreateVisual(DrawingContext drawingContext, double width, double height, DrawingVisual _) {
      var legendY = (LegendY)LegendScrollerY.Legend;
      //grid-lines use only 1 dimension. Both for x and y grid-line, 
      double minDisplayValue = MinDisplayValues[0];

      // Create a GuidelineSet to get the lines exactly on a pixel
      GuidelineSet guidelines = new GuidelineSet();
      double halfPenWidth = StrokePen.Thickness / 2;
      foreach (double labelValue in legendY.LabelValues!) {
        double yPos = height - (ScaleY * (labelValue - minDisplayValue));
        guidelines.GuidelinesY.Add(yPos + halfPenWidth);
      }

      drawingContext.PushGuidelineSet(guidelines);
      foreach (double labelValue in legendY.LabelValues) {
        double yPos = height - (ScaleY * (labelValue - minDisplayValue));
        drawingContext.DrawLine(StrokePen, new Point(0, yPos), new Point(width, yPos));
      }
      drawingContext.Pop();
    }
    #endregion
  }
}
