using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace XYGraphLib {


  public class LegendScrollerY: LegendScroller {

    #region Properties
    //      ----------

    /// <summary>
    /// Used as reference height for Charts zoom buttons
    /// </summary>
    public double ScrollBarWidth;


    protected override void OnReset() {
      ScrollBarWidth = double.NaN;
    }
    #endregion

    
    #region Constructor
    //      -----------

    /// <summary>
    /// Default constructor, adding a LegendY for doubles
    /// </summary>
    public LegendScrollerY() : this(new LegendY(Renderer.DimensionY)) { }


    /// <summary>
    /// Constructor using any x-axis legend
    /// </summary>
    public LegendScrollerY(LegendY legend): base(legend) {
    }


    protected override void OnButtonScrollbarCreated() {
      ScrollBar.Orientation = Orientation.Vertical;
      ScrollBar.VerticalAlignment = VerticalAlignment.Stretch;
      ScrollBar.HorizontalAlignment = HorizontalAlignment.Center;
    }
    #endregion


    #region Layout Overrides
    //      ----------------

    protected override Size MeasureContentOverride(Size availableSize) {
      CalculateScrollBarValues();

      ScrollBar.Measure(availableSize);
      ScrollBarWidth = ScrollBar.DesiredSize.Width;
      Size buttonSize = new Size(ScrollBarWidth, ScrollBarWidth);
      ZoomInButton.Measure(buttonSize);
      ZoomOutButton.Measure(buttonSize);

      double availableLegendWidth = Math.Max(0, availableSize.Width - ScrollBarWidth);
      Legend.Measure(new Size(availableLegendWidth, availableSize.Height));

      //use scrollbarDockPanel or Legend to deal with infinite space.
      return new Size(Legend.DesiredSize.Width + ScrollBarWidth,  Legend.DesiredSize.Height);
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      CalculateScrollBarValues();

      double legendWidth = arrangeRect.Size.Width - ScrollBarWidth;
      double legendX = 0;
      if (IsSizingWidthToExpandableContent()) {
        //use all the height, but only the width needed
        legendWidth = Math.Min(legendWidth, Legend.DesiredSize.Width);
      }
      double scrollbarX = legendWidth;

      if (!double.IsNaN(Width) || HorizontalAlignment==HorizontalAlignment.Stretch) {
        //HorizontalContentAlignment matters only if space available is different from the needed space, which is only possible if 
        //LegendY is stretched or its width is defined
        switch (HorizontalContentAlignment) {
        case HorizontalAlignment.Left:
        case HorizontalAlignment.Stretch:
          break;
        case HorizontalAlignment.Center:
          legendX    = (arrangeRect.Size.Width - legendWidth - ScrollBarWidth)/2;
          scrollbarX = (arrangeRect.Size.Width + legendWidth - ScrollBarWidth)/2;
          break;
        case HorizontalAlignment.Right:
          legendX    = arrangeRect.Size.Width - legendWidth - ScrollBarWidth;
          scrollbarX = arrangeRect.Size.Width               - ScrollBarWidth;
          break;
        default:
          throw new NotSupportedException();
        }
      }

      Legend.ArrangeBorderPadding(arrangeRect, legendX, 0, legendWidth, arrangeRect.Size.Height);
      ZoomInButton.ArrangeBorderPadding(arrangeRect, scrollbarX, 0, ScrollBarWidth, ScrollBarWidth);
      ScrollBar.ArrangeBorderPadding(arrangeRect, scrollbarX, ScrollBarWidth, ScrollBarWidth, Math.Max(0, arrangeRect.Size.Height-2*ScrollBarWidth));
      ZoomOutButton.ArrangeBorderPadding(arrangeRect, scrollbarX, arrangeRect.Height-ScrollBarWidth, ScrollBarWidth, ScrollBarWidth);


      //if (IsSizingHeightToExpandableContent()) {
      //  return new Size(arrangeRect.Size.Width, legendHeight + scrollHeight);
      //} else {
      return arrangeRect.Size;
      //}
    }
    #endregion
  }
}