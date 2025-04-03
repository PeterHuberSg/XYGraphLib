/**************************************************************************************

XYGraphLib.Crosshair
===================

Creates a Visual displaying a vertical line where the mouse is and the values of the x and y data at this x.

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

  public class Crosshair {

    #region Properties
    //      ----------
    #endregion


    #region Constructor
    //      -----------

    readonly Pen strokePen;


    public Crosshair(Pen? crosshairPen) {
      strokePen = crosshairPen??new() { Brush=Brushes.DimGray, DashStyle=DashStyles.Dash, Thickness=1 };
    }
    #endregion


    #region Methods
    //      -------

    public DrawingVisual CreateVisual(double width, double height) {
      DrawingVisual drawingVisual = new();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
        // Create a GuidelineSet to get the lines exactly on a pixel
        //GuidelineSet guidelines = new GuidelineSet();
        //double halfPenWidth = strokePen.Thickness / 2;
        //foreach (double labelValue in legendY.LabelValues!) {
        //  double yPos = height - (ScaleY * (labelValue - minDisplayValue));
        //  guidelines.GuidelinesX.Add(xPos + halfPenWidth);
        //}

        //drawingContext.PushGuidelineSet(guidelines);
        //foreach (double labelValue in legendY.LabelValues) {
        //  double yPos = height - (ScaleY * (labelValue - minDisplayValue));
        //  drawingContext.DrawLine(strokePen, new Point(0, yPos), new Point(width, yPos));
        //}
        //drawingContext.Pop();
        GuidelineSet guidelines = new();
        double halfPenWidth = strokePen.Thickness / 2;
        guidelines.GuidelinesX.Add(0 + halfPenWidth);

        drawingContext.PushGuidelineSet(guidelines);
        drawingContext.DrawLine(strokePen, new Point(0, 0), new Point(0, height));
        drawingContext.Pop();
      }
      return drawingVisual;
    }

    //todo:delete
    public DrawingVisual CreateVisual(double xPos, double width, double height) {
      DrawingVisual drawingVisual = new();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
        // Create a GuidelineSet to get the lines exactly on a pixel
        //GuidelineSet guidelines = new GuidelineSet();
        //double halfPenWidth = strokePen.Thickness / 2;
        //foreach (double labelValue in legendY.LabelValues!) {
        //  double yPos = height - (ScaleY * (labelValue - minDisplayValue));
        //  guidelines.GuidelinesX.Add(xPos + halfPenWidth);
        //}

        //drawingContext.PushGuidelineSet(guidelines);
        //foreach (double labelValue in legendY.LabelValues) {
        //  double yPos = height - (ScaleY * (labelValue - minDisplayValue));
        //  drawingContext.DrawLine(strokePen, new Point(0, yPos), new Point(width, yPos));
        //}
        //drawingContext.Pop();
        GuidelineSet guidelines = new();
        double halfPenWidth = strokePen.Thickness / 2;
        guidelines.GuidelinesX.Add(xPos + halfPenWidth);

        drawingContext.PushGuidelineSet(guidelines);
        drawingContext.DrawLine(strokePen, new Point(xPos, 0), new Point(xPos, height));
        drawingContext.Pop();
      }
      return drawingVisual;
    }
    #endregion
  }
}
