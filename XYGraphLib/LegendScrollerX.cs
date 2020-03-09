/**************************************************************************************

XYGraphLib.LegendScrollerX
==========================

Horizontal ScrollBar and Legend

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
using System.Windows.Controls;
using CustomControlBaseLib;


namespace XYGraphLib {


  public class LegendScrollerX: LegendScroller {

    #region Properties
    //      ----------

    /// <summary>
    /// Used as reference height for Charts zoom buttons
    /// </summary>
    public double ScrollBarHeight;


    protected override void OnReset() {
      ScrollBarHeight = double.NaN;
    }
    #endregion



    #region Constructor
    //      -----------

    /// <summary>
    /// Default constructor, adding a LegendX for doubles
    /// </summary>
    public LegendScrollerX(): this(new LegendX(Renderer.DimensionX)) {}


    /// <summary>
    /// Constructor using any x-axis legend
    /// </summary>
    public LegendScrollerX(LegendX legend): base(legend) {
    }


    protected override void OnButtonScrollbarCreated() {
      ScrollBar.Orientation = Orientation.Horizontal;
      ScrollBar.HorizontalAlignment = HorizontalAlignment.Stretch;
      ScrollBar.VerticalAlignment = VerticalAlignment.Center;
    }
    #endregion


    #region Layout Overrides
    //      ----------------

    protected override Size MeasureContentOverride(Size availableSize) {
      ScrollBar.Measure(availableSize);
      ScrollBarHeight = ScrollBar.DesiredSize.Height;
      Size buttonSize = new Size(ScrollBarHeight, ScrollBarHeight);
      ZoomInButton.Measure(buttonSize);
      ZoomOutButton.Measure(buttonSize);

      var availableLegendHeight = Math.Max(0, availableSize.Height - ScrollBarHeight);
      Legend.Measure(new Size(availableSize.Width, availableLegendHeight));

      //use scrollbar and Legend to deal with infinite space.
      return new Size(Legend.DesiredSize.Width, ScrollBarHeight + Legend.DesiredSize.Height);
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      CalculateScrollBarValues();

      double legendHeight = Math.Max(0, arrangeRect.Size.Height - ScrollBarHeight);
      double legendY = 0;
      if (IsSizingHeightToExpandableContent()) {
        //use all the width, but only the height needed
        legendHeight = Math.Min(legendHeight, Legend.DesiredSize.Height);
      }
      double scrollbarY = legendHeight;

      if (!double.IsNaN(Height) || VerticalAlignment==VerticalAlignment.Stretch) {
        //VerticalContentAlignment matters only if space available is different from the needed space, which is only possible if 
        //LegendX is stretched or its height is defined
        switch (VerticalContentAlignment) {
        case VerticalAlignment.Top:
        case VerticalAlignment.Stretch:
          break;
        case VerticalAlignment.Center:
          legendY    = (arrangeRect.Size.Height - legendHeight - ScrollBarHeight)/2;
          scrollbarY = (arrangeRect.Size.Height - legendHeight + ScrollBarHeight)/2;
          break;
        case VerticalAlignment.Bottom:
          legendY    = arrangeRect.Size.Height - legendHeight - ScrollBarHeight;
          scrollbarY = arrangeRect.Size.Height                - ScrollBarHeight;
          break;
        default:
          throw new NotSupportedException();
        }
      }

      Legend.ArrangeBorderPadding(arrangeRect, 0, legendY, arrangeRect.Size.Width, legendHeight);
      ZoomOutButton.ArrangeBorderPadding(arrangeRect, 0, scrollbarY, ScrollBarHeight, ScrollBarHeight);
      ScrollBar.ArrangeBorderPadding(arrangeRect, ScrollBarHeight, scrollbarY, Math.Max(0,  arrangeRect.Size.Width-2*ScrollBarHeight), ScrollBarHeight);
      ZoomInButton.ArrangeBorderPadding(arrangeRect, arrangeRect.Width-ScrollBarHeight, scrollbarY, ScrollBarHeight, ScrollBarHeight);


      //if (IsSizingHeightToExpandableContent()) {
      //  return new Size(arrangeRect.Size.Width, legendHeight + scrollHeight);
      //} else {
      return arrangeRect.Size;
      //}
    }
    #endregion
  }
}
