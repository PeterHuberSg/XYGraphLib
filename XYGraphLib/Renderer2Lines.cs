/**************************************************************************************

XYGraphLib.Renderer2Lines
=========================

Creates a Visual for a series of data to be displayed with 2 lines in the PlotArea.

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;


namespace XYGraphLib {

  /// <summary>
  /// Creates a Visual for a series of data to be displayed with 2 lines in the PlotArea.
  /// </summary>
  public class Renderer2Lines: RendererDataSeries {

    #region Constructor
    //      -----------

    readonly Brush fillBrush;


    public Renderer2Lines(
      Brush strokeBrush, 
      double strokeThickness, 
      Brush areaLineFillBrush,
      RendererDataSeries.YSerie[] ySeries): 
        base(strokeBrush, strokeThickness, DimensionMapXY, ySeries) 
    {
      fillBrush = areaLineFillBrush;
    }
    #endregion


    #region Methods
    //      -------

    const int upperLine = 0;
    const int lowerLine = 1;

    /// <summary>
    /// Renders the area line graph to the drawingContext. The line gets scaled to the available height and width displaying only 
    /// x-values between MinDisplayValueX and MaxDisplayValueX.
    /// </summary>
    protected override void OnCreateVisual(DrawingContext drawingContext, double width, double height, DrawingVisual _) {
      StreamGeometry streamGeometry = new StreamGeometry();
      using StreamGeometryContext streamGeometryContext = streamGeometry.Open();
      bool isFirstPoint = true;
      double minDisplayValueX = MinDisplayValues[DimensionX];
      double maxDisplayValueX = MaxDisplayValues[DimensionX];

      //draw upperLine values from MinDisplayValueX to MaxDisplayValueX
      double[,] xYs = YSeries[upperLine].Values;
      int dataSerieLength = xYs.GetLength(0);
      int firstDataPointIndex = 0;
      if (IsYSerieSorted[DimensionX]) {
        //search biggest valueX smaller than minDisplayValueX. First point must be outside drawing area to get a nice line.
        for (int dataPointIndex = 0; dataPointIndex<dataSerieLength; dataPointIndex++) {
          double valueX = xYs[dataPointIndex, DimensionX];
          if (valueX>minDisplayValueX) {
            firstDataPointIndex = dataPointIndex - 1;
            break;
          }
        }
      }
      int lastDataPointIndex = dataSerieLength-1;
      for (int dataPointIndex = firstDataPointIndex; dataPointIndex<dataSerieLength; dataPointIndex++) {
        double valueX = xYs[dataPointIndex, DimensionX];
        drawPoint(streamGeometryContext, xYs, dataPointIndex, width, height, ref isFirstPoint);
        if (IsYSerieSorted[DimensionX]) {
          if (valueX>maxDisplayValueX) {
            lastDataPointIndex = dataPointIndex;//It would be possible just to use lastDataPointIndex-1 in next for loop. But using
                                                //lastDataPointIndex makes for clearer code.
            break;
          }
        }
      }

      //draw lowerLine values reversed from MaxDisplayValueX to MinDisplayValueX
      xYs = YSeries[lowerLine].Values;
      for (int dataPointIndex = lastDataPointIndex; dataPointIndex>=firstDataPointIndex; dataPointIndex--) {
        if (dataPointIndex<0 || dataPointIndex>=dataSerieLength) {
          System.Diagnostics.Debugger.Break();
        }
        drawPoint(streamGeometryContext, xYs, dataPointIndex, width, height, ref isFirstPoint);
      }
      streamGeometry.Freeze();
      drawingContext.DrawGeometry(fillBrush, StrokePen, streamGeometry);
    }


    private void drawPoint(StreamGeometryContext streamGeometryContext, double[,] dataSerie, int dataPointIndex, 
      double width, double height, ref bool isFirstPoint) 
    {
      Point valuePoint = TranslateValueXYToPoint(dataSerie, dataPointIndex, width, height);
      if (isFirstPoint) {
        isFirstPoint = false;
        streamGeometryContext.BeginFigure(valuePoint, isFilled : true, isClosed : true);
      } else {
        streamGeometryContext.LineTo(valuePoint, isStroked : true, isSmoothJoin : false);
      }
    }
    #endregion
  }
}
