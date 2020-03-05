using System;
using System.Windows;
using System.Windows.Media;


namespace XYGraphLib {
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
      //gridlineX are parallel to horizontal x-line, but the distance between them is controled by LegendY, therefore dimension Y
      base(strokeBrush, strokeThickness, DimensionMapY) 
    {
      LegendScrollerY = legendScrollerY;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Renders the horizontal x-gridline to the drawingContext, one line for each label in YLegend.
    /// </summary>
    protected override void OnCreateVisual(DrawingContext drawingContext, double width, double height) {
      LegendY legendy = (LegendY)LegendScrollerY.Legend;
      //gridlines use only 1 dimension. Both for x and y gridline, 
      double minDisplayValue = MinDisplayValues[0];

      // Create a GuidelineSet to get the lines exactly on a pixel
      GuidelineSet guidelines = new GuidelineSet();
      double halfPenWidth = StrokePen.Thickness / 2;
      foreach (double lableValue in legendy.LabelValues) {
        double yPos = height - (ScaleY * (lableValue - minDisplayValue));
        guidelines.GuidelinesY.Add(yPos + halfPenWidth);
      }

      drawingContext.PushGuidelineSet(guidelines);
      foreach (double lableValue in legendy.LabelValues) {
        double yPos = height - (ScaleY * (lableValue - minDisplayValue));
        drawingContext.DrawLine(StrokePen, new Point(0, yPos), new Point(width, yPos));
      }
      drawingContext.Pop();
    }
    #endregion
  }
}
