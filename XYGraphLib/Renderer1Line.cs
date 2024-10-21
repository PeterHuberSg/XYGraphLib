/**************************************************************************************

XYGraphLib.Renderer1Line
========================

Creates a Visual for a series of data to be displayed with 1 line in the PlotArea.

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
  /// Creates a Visual for a series of data to be displayed with 1 line in the PlotArea.
  /// </summary>
  public class Renderer1Line: RendererDataSeries {

    #region Constructor
    //      -----------

    readonly Brush? fillBrush;


    public Renderer1Line(Brush strokeBrush, double strokeThickness, Brush? fillBrush, double[][,] dataSeries,
      string? yName, string? yUnit) :
      base(strokeBrush, strokeThickness, DimensionMapXY, dataSeries, yName, yUnit) 
    {
      if (dataSeries.Length!=1) {
        throw new ArgumentException("Renderer1Line needs 1 DataSerie, but there were " + dataSeries.Length + ".");
      }
      if (dataSeries[0].GetLength(1)!=2) {
        throw new ArgumentException("Renderer1Line needs 2 values per ValuePoint, but there were " + dataSeries[0].GetLength(1) + ".");
      }
      this.fillBrush = fillBrush;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Renders the line graph to the drawingContext. The line gets scaled to the available height and width displaying only 
    /// values between minDisplayValueX and maxDisplayValueX, if the x-values are sorted.
    /// </summary>
    protected override void OnCreateVisual(DrawingContext drawingContext, double width, double height, DrawingVisual _) {
      StreamGeometry streamGeometry = new StreamGeometry();
      using StreamGeometryContext streamGeometryContext = streamGeometry.Open();
      bool isFirstPoint = true;
      double minDisplayValueX = MinDisplayValues[DimensionX];
      double maxDisplayValueX = MaxDisplayValues[DimensionX];
      double[,] dataSerie = DataSeries[0];
      int dataSerieLength = dataSerie.GetLength(0);
      int firstDataPointIndex = 0;
      if (IsDimensionSorted[DimensionX]) {
        //search biggest valueX smaller than minDisplayValueX. First point must be outside drawing area to get a nice line.
        for (int dataPointIndex = 0; dataPointIndex<dataSerieLength; dataPointIndex++) {
          double valueX = dataSerie[dataPointIndex, DimensionX];
          if (valueX>minDisplayValueX) {
            firstDataPointIndex = dataPointIndex - 1;
            break;
          }
        }
      }
      for (int dataPointIndex = firstDataPointIndex; dataPointIndex<dataSerieLength; dataPointIndex++) {
        double valueX = dataSerie[dataPointIndex, DimensionX];
        Point valuePoint = TranslateValueXYToPoint(dataSerie, dataPointIndex, width, height);
        if (isFirstPoint) {
          isFirstPoint = false;
          if (fillBrush==null) {
            //no fill colour, draw only line
            streamGeometryContext.BeginFigure(valuePoint, isFilled: false, isClosed: false);
          } else {
            if (IsDimensionSorted[DimensionX]) {
              //line is sorted and has a fill colour: area should fill to x-axis
              //draw borders Pen.Thickness outside of both axes to hide them from viewer
              //place first point at beginning of x-axis
              //                streamGeometryContext.BeginFigure(new Point(valuePoint.X, height), isFilled : true, isClosed : true);
              streamGeometryContext.BeginFigure(new Point(0-StrokePen.Thickness, height+StrokePen.Thickness), isFilled: true, isClosed: true);
              //place second point on the same y-height of first point, but outside of y-axis
              streamGeometryContext.LineTo(new Point(0-StrokePen.Thickness, valuePoint.Y), isStroked: true, isSmoothJoin: false);
              //first real point
              streamGeometryContext.LineTo(valuePoint, isStroked: true, isSmoothJoin: false);
            } else {
              //line is not sorted and has a fill colour: just fill the graphic
              streamGeometryContext.BeginFigure(valuePoint, isFilled: true, isClosed: true);
            }
          }
        } else {
          streamGeometryContext.LineTo(valuePoint, isStroked: true, isSmoothJoin: false);
        }
        if (IsDimensionSorted[DimensionX]) {
          if (valueX>maxDisplayValueX || dataPointIndex==dataSerieLength-1) {
            if (fillBrush!=null) {
              //line is sorted and has a fill colour: add last line leading back to x-axis
              //                streamGeometryContext.LineTo(new Point(valuePoint.X, height), isStroked : true, isSmoothJoin : false);
              streamGeometryContext.LineTo(new Point(width+StrokePen.Thickness, valuePoint.Y), isStroked: true, isSmoothJoin: false);
              streamGeometryContext.LineTo(new Point(width+StrokePen.Thickness, height+StrokePen.Thickness), isStroked: true, isSmoothJoin: false);
            }
            break;
          }
        }
      }

      streamGeometry.Freeze();
      drawingContext.DrawGeometry(fillBrush, StrokePen, streamGeometry);
    }
    #endregion
  }
}
