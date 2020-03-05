using System;
using System.Windows;
using System.Windows.Media;


namespace XYGraphLib {

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

    public RendererGridLineY(LegendScrollerX legendScrollerx, Brush strokeBrush, double strokeThickness):
      //gridlineY are parallel to vertical y-line, but the distance between them is controled by LegendX, therefore dimension X
      base(strokeBrush, strokeThickness, DimensionMapX) 
    {
      LegendScrollerX = legendScrollerx;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Renders the vertical x-gridline to the drawingContext, one line for each label in XLegend.
    /// </summary>
    protected override void OnCreateVisual(DrawingContext drawingContext, double width, double height) {
      LegendX legendx = (LegendX)LegendScrollerX.Legend;
      //gridlines use only 1 dimension. Both for x and y gridline, 
      double minDisplayValue = MinDisplayValues[0];

      // Create a GuidelineSet to get the lines exactly on a pixel
      GuidelineSet guidelines = new GuidelineSet();
      double halfPenWidth = StrokePen.Thickness / 2;
      foreach (double lableValue in legendx.LabelValues) {
        double xPos = ScaleX * (lableValue - minDisplayValue);
        guidelines.GuidelinesX.Add(xPos + halfPenWidth);
      }

      drawingContext.PushGuidelineSet(guidelines);
      foreach (double lableValue in legendx.LabelValues) {
        double xPos = ScaleX * (lableValue - minDisplayValue);
        drawingContext.DrawLine(StrokePen, new Point(xPos, 0), new Point(xPos, height));
      }
      drawingContext.Pop();
    }
    #endregion
  }
}
