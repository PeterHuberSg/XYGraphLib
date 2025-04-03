/**************************************************************************************

XYGraphLib.LegendScrollerY
==========================

Vertical ScrollBar and Legend

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CustomControlBaseLib;

//                                     Legend
//                                      ↓
//                                    ┌──┬───┐
// DisplayValue + DisplayValueRange → │10│ + │ ← Zoom In Button
//                                    │  ├───┤
//                                    │  │Scr│
//                                    │ 5│oll│
//                                    │  │Bar│
//                                    │  ├───┤
//                     DisplayValue → │ 0│ - │ ← Zoom Out Button 
//                                    └──┴───┘
//
//LegendScrollerY informs Renderers by raising the DisplayIndexRangeChanged event which data needs to be displayed

namespace XYGraphLib {

  /// <summary>
  /// Displays an y axis legend (normally a double), Scrollbar and 2 ZoomButton. It can be used to select which data-samples 
  /// should be displayed in a Renderer based on a value range, displayed in the legend. The user choses with the scrollbar 
  /// which is the first sample to display (DisplayValue) and with the zoom buttons how many samples should be displayed 
  /// (DisplayRangeIndex). LegendScroller calculates DisplayValue and DisplayValueRange based on MinValue and MaxValue and sets 
  /// the Legend accordingly.
  /// </summary>
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
      //Debug.WriteLine($"--{Name}.MeasureContentOverride({availableSize})");
      CalculateScrollBarValues();

      ScrollBar.Measure(availableSize);
      ScrollBarWidth = ScrollBar.DesiredSize.Width;
      Size buttonSize = new (ScrollBarWidth, ScrollBarWidth);
      ZoomInButton.Measure(buttonSize);
      ZoomOutButton.Measure(buttonSize);

      double availableLegendWidth = Math.Max(0, availableSize.Width - ScrollBarWidth);
      //Debug.WriteLine($"--availableLegendWidth {availableLegendWidth:N0} = Max(0, availableWidth {availableSize.Width:N0} - ScrollBarWidth {ScrollBarWidth:N0})");
      Legend.Measure(new Size(availableLegendWidth, availableSize.Height));
      //Debug.WriteLine($"--LegendDesiredWidth {Legend.DesiredSize.Width:N0} = Legend.Measure(availableLegendWidth {availableLegendWidth:N0}, availableSizeHeight {availableSize.Height:N0})");
      //Debug.WriteLine($"--return {Legend.DesiredSize.Width + ScrollBarWidth:N0} = LegendDesiredWidth {Legend.DesiredSize.Width:N0} + ScrollBarWidth {ScrollBarWidth:N0}, LegendDesiredHeight {Legend.DesiredSize.Height:N0})");

      //use scrollbarDockPanel or Legend to deal with infinite space.
      return new Size(Legend.DesiredSize.Width + ScrollBarWidth,  Legend.DesiredSize.Height);
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      //Debug.WriteLine($"..{Name}.ArrangeContentOverride(Width: {arrangeRect.Width:N0}, Height: {arrangeRect.Height:N0})");
      CalculateScrollBarValues();

      double legendWidth = arrangeRect.Size.Width - ScrollBarWidth;
      //Debug.WriteLine($"..legendWidth {legendWidth:N0} = arrangeRect.Width {arrangeRect.Size.Width:N0} - ScrollBarWidth {ScrollBarWidth:N0}");
      double legendX = 0;
      //Debug.WriteLine($"..IsSizingWidthToExpandableContent: {IsSizingWidthToExpandableContent()}");
      if (IsSizingWidthToExpandableContent()) {
        //use all the height, but only the width needed
        //Debug.WriteLine($"..legendWidth {Math.Min(legendWidth, Legend.DesiredSize.Width):N0} = Min(legendWidth: {legendWidth:N0}, Legend.DesiredWidth: {Legend.DesiredSize.Width:N0})");
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
      //Debug.WriteLine($"..return arrangeRect {arrangeRect.Size.Width:N0}, {arrangeRect.Size.Height:N0}");
      return arrangeRect.Size;
      //}
    }
    #endregion
  }
}