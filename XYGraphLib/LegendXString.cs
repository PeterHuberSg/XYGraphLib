/**************************************************************************************

XYGraphLib.LegendXString
========================

Horizontal Legend with string values

Written 2014-2024 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/

// Displays a horizontal legend consisting of strings instead of numbers for a PlotArea. LegendXString inherits
// from Legend which has parameters for MinValue, DisplayValue, DisplayValueRange and MaxValue, which are 
// doubles.  
// 
// Legend:
//            ┌────────────────────────────────────┐
//    0       │ 10              15             20  │     30
//    ↑       └────────────────────────────────────┘      ↑
// MinValue     ↑DisplayValue                   ↑        MaxValue
//              DisplayValue + DisplayValueRange│
//
// LegendXString stores for each x Value one string in legendStrings. LegendXString writes only integers into the
// Legend value properties, which are indices into legendStrings.
//
// MinValue     =  0 => Legend[ 0]:  0 => legendStrings[ 0]: "zero"
// DisplayValue = 10 => Legend[10]: 10 => legendStrings[10]: "ten"
// MaxValue     = 30 => Legend[30]: 30 => legendStrings[30]: "thirty"
//
// LegendXString:
//            ┌────────────────────────────────────┐
//    zero    │ ten             fifteen    twenty  │    thirty
//    ↑       └────────────────────────────────────┘      ↑
//    0         10              15             20        30
//    ↑                                                   ↑
// MinValue     ↑DisplayValue                   ↑        MaxValue
//              DisplayValue + DisplayValueRange│

using CustomControlBaseLib;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace XYGraphLib {
  /// <summary>
  /// Displays the legend below the PlotArea with string values.
  /// </summary>
  public class LegendXString: LegendX {


    #region Properties
    //      ----------


    private IReadOnlyList<string> legendStrings;

    public IReadOnlyList<string> LegendStrings {
      get { return legendStrings; }
      set {
        legendStrings = value;
        InvalidateMeasure(); //It seems InvalidateVisual() does not force Measure()
        InvalidateVisual();
        OnReset();
      }
    }



    ///// <summary>
    /////Returns the x pixel position of a label string, even if the label does not get displayed, because
    ///// there is not enough space. Returns int.MinValue if the string is not needed for the graph based 
    ///// on DisplayValue and DisplayValueRange.
    ///// </summary>
    //public double[] LegendStringXs { get; }

    public double MaxLegendHeight { get; private set; } = double.NaN;
    #endregion


    #region Constructor
    //      -----------

    public LegendXString() : this(RandomText.GetStrings(stringsCount: 10, maxStringLength: 10)) {
    }


    public LegendXString(IReadOnlyList<string> strings) : base(Renderer.DimensionX, legendAngle: 90) {
      legendStrings = strings;
    }
    #endregion


    #region Measurement Overrides
    //      ---------------------

    protected override void OnFontChanged(bool hasOnlyFontSizeChanged) {
      calculateMaxLegendHeight();
    }


    protected override void OnReset() {
      MaxLegendHeight = double.NaN;
      RenderWidthTracked = double.NaN;
    }


    protected override Size OnLegendMeasurement(Size requiredSize) {
      if (double.IsNaN(MaxLegendHeight)) calculateMaxLegendHeight();

      requiredSize.Height = MaxLegendHeight;
      return requiredSize;
    }


    private void calculateMaxLegendHeight() {
      MaxLegendHeight = LegendGlyphDrawer.GetMaxLength(legendStrings, FontSize);
    }
    #endregion


    #region Arrange Overrides
    //      -----------------

    protected override Size OnLegendArrange(Rect arrangeRect) {
      if (IsSizingHeightToFixedContent()) {
        //use all the width, but only the height needed
        return new Size(arrangeRect.Size.Width, MaxLegendHeight);
      } else {
        //use all available space
        return arrangeRect.Size;
      }
    }
    #endregion


    #region Render Overrides
    //      ----------------

    /// <summary>
    /// Width used during rendering
    /// </summary>
    protected double RenderWidthTracked { get; private set; }


    //no need to provide special default data
    //protected override void OnProvideDefaultValues(out double displayValue, out double displayValueRange) {
    //  base.OnProvideDefaultValues(out displayValue, out displayValueRange);
    //  return;
    //}


    protected override bool OnIsRecalculationNeeded(Size renderContentSize) {
      bool isRecalculationNeeded = RenderWidthTracked!=renderContentSize.Width;
      RenderWidthTracked = renderContentSize.Width;
      return isRecalculationNeeded;
    }


    const double precision = 0.001; //treat 2 doubles as equal if their difference is smaller than precision


    protected override void OnRecalculate(ref double[]? labelValues, ref string?[]? labelStrings, ref Point[]? labelPoints) {
      var stringsIndex = (int)Math.Ceiling(DisplayValue);//first label to display
      var labelsCount = (int)(DisplayValue + DisplayValueRange) - stringsIndex + 1;
      var stringWidth = FontFamily.LineSpacing*FontSize;
      if (stringWidth>=RenderWidthTracked || //not enough space to display even 1 string completely
        labelsCount<1) {                     //only part of a label gets displayed
        //only 1 or even only the part of a label can be displayed. Just display the very first value.
        if (labelValues==null || labelValues.Length!=1) {
          labelValues = new double[1];
          labelStrings = new string[1];
          labelPoints = new Point[1];
        }
        stringsIndex = (int)Math.Round(DisplayValue);
        labelValues[0] = stringsIndex;
        labelStrings![0] = legendStrings[stringsIndex];
        labelPoints![0] = new Point(0, 0);
        return;
      }

      var stringIndexOffset = stringsIndex - DisplayValue;
      var stringXIncrement = RenderWidthTracked / DisplayValueRange;
      var maxLabelCount = (int)(RenderWidthTracked / stringWidth);//maximum number of labels that can be displayed
      var stringIndexIncrement = 1;
      while (labelsCount>maxLabelCount) {
        labelsCount /= 2;
        stringXIncrement *= 2;
        stringIndexIncrement *= 2;
      }

      //add one more label at the very end. Otherwise that label gets never displayed 
      var isLastLabelNeeded = (int)(DisplayValue + DisplayValueRange) == legendStrings.Count - 1;

      if (isLastLabelNeeded) {
        labelsCount++;
      }
      if (labelValues==null || labelValues.Length!=labelsCount) {
        labelValues = new double[labelsCount];
        labelStrings = new string[labelsCount];
        labelPoints = new Point[labelsCount];
      }
      if (isLastLabelNeeded) {
        labelsCount--;
      }
 
      double stringX;
      if (stringIndexOffset<=precision) {
        //DisplayValue is (close) to an integer, i.e. the Legend starts exactly with stringsIndex 
        stringX = 0;
      } else {
        //DisplayValue is not close to an integer. The graph will display a value for stringsIndex, but
        //the legend will only display a string for stringsIndex+1
        stringX = stringIndexOffset * stringXIncrement;
      }

      //create labels
      var labelIndex = 0;
      for (; labelIndex<labelsCount; labelIndex++) {
        labelValues[labelIndex] = stringsIndex;
        labelStrings![labelIndex] = legendStrings[stringsIndex];
        labelPoints![labelIndex] = new Point(stringX, 0);
        stringsIndex += stringIndexIncrement;
        stringX += stringXIncrement;
      }

      if (isLastLabelNeeded) {
        labelValues[labelIndex] = stringsIndex;
        labelStrings![labelIndex] = legendStrings[^1];
        //draw very last label next to RenderWidth
        labelPoints![labelIndex] = new Point(RenderWidthTracked - FontFamily.Baseline*FontSize*0.8, 0);
      }
      ////number of labels that can be displayed in window
      //Array.Fill(LegendStringXs, int.MinValue);
      ////var firstStringsIndex = (int)Math.Round(DisplayValue, 0);
      //var stringsIndex = (int)DisplayValue;//first string that is needed for the label calculation, might be just
      //                                     //outside of the graph
      ////var stringIndexOffset = firstStringsIndex - DisplayValue;
      //var stringIndexOffset = DisplayValue - stringsIndex;
      //var stringXIncrement = RenderWidthTracked / DisplayValueRange;
      //var maxLabelCount = RenderWidthTracked / FontFamily.LineSpacing*FontSize;

      //if (maxLabelCount<=1) {
      //  //only 1 or even only the part of a label can be displayed. Just display the very first value.
      //  if (labelValues==null || labelValues.Length!=1) {
      //    labelValues = new double[1];
      //    labelStrings = new string[1];
      //    labelPoints = new Point[1];
      //  }
      //  labelValues[0] = DisplayValue;
      //  labelStrings![0] = LegendStrings[stringsIndex];
      //  labelPoints![0] = new Point(0, 0);
      //  return;
      //}

      ////calculate number of labels that need to be displayed
      //if (DisplayValueRange<=maxLabelCount) {
      //  //spread the labels, there is too much space available
      //  var labelsCount = (int)DisplayValueRange;
      //  if (labelValues==null || labelValues.Length!=labelsCount) {
      //    labelValues = new double[labelsCount];
      //    labelStrings = new string[labelsCount];
      //    labelPoints = new Point[labelsCount];
      //  }

      //  double stringX;
      //  if (stringIndexOffset<=precision) {
      //    //DisplayValue is (close) to an integer, i.e. the Legend starts exactly with stringsIndex 
      //    stringX = 0;
      //  } else {
      //    //DisplayValue is not close to an integer. The graph will display a value for stringsIndex, but
      //    //the legend will only display a string for stringsIndex+1
      //    stringX = -stringIndexOffset;
      //    LegendStringXs[stringsIndex++] = stringX;
      //    stringX += stringXIncrement;
      //  }

      //  //create labels
      //  for (var labelIndex = 0; labelIndex<labelsCount; labelsCount++) {
      //    labelValues[labelsCount] = stringsIndex++;
      //    labelStrings![labelsCount] = LegendStrings[stringsIndex];
      //    labelPoints![labelsCount] = new Point(stringX, 0);
      //    stringX += stringXIncrement;
      //  }

      //  if (stringX + precision < RenderWidthTracked) {
      //    //calculate one more x for first string outside the graph.
      //    LegendStringXs[stringsIndex] = stringX;
      //  }
      //} else {
      //  ////display only some labels, there is not enough space
      //  //labelIncrement = displayLabelsCount/maxLabelCount;
      //  //labelDistance = RenderWidthTracked / maxLabelCount;
      //  //displayLabelsCount = maxLabelCount;
      //}

      //if (labelValues==null || labelValues.Length!=displayLabelsCount) {
      //  labelValues = new double[displayLabelsCount];
      //  labelStrings = new string[displayLabelsCount];
      //  labelPoints = new Point[displayLabelsCount];
      //}

      //var labelValue = firstStringsIndex;
      //for (int stringsIndex = 0; stringsIndex < firstStringsIndex; stringsIndex++) {
      //  labelValues[stringsIndex] = labelValue++;
      //  labelStrings![stringsIndex] = LegendStrings[stringsIndex];
      //  labelPoints![stringsIndex] = new Point(0, 0);
      //}

      /*
      //calculate labels
      //double pixelPerValue = RenderWidthTracked / DisplayValueRange;
      for (int labelIndex = 0; labelIndex < labelValues.Length; labelIndex++) {
        //write label value
        labelValues[labelIndex] = labelValue;

        //calculate position
        double xPosition = (labelValue-DisplayValue) * pixelPerValue;
#if DEBUG
        if (labelIndex>0) {
          double labelDistance = xPosition-labelPoints[labelIndex-1].X;
          double labelLength = LegendGlyphDrawer.GetLength(labelValue.ToString(numberFormat), FontSize);
          if (labelDistance<labelLength) {
            System.Diagnostics.Debugger.Break();
            throw new Exception("label " + labelValue.ToString(numberFormat) + " needs " + labelLength +
            " pixels, but there are only " + labelDistance + " pixels distance between 2 labels.");
          }
        }
#endif
        if (xPosition> RenderWidthTracked) {
          if (xPosition<1.0001*RenderWidthTracked) {
            //probably rounding error
            xPosition = RenderWidthTracked;
          } else {
            //should not happen
#if DEBUG
            System.Diagnostics.Debugger.Break();
            throw new Exception();
#endif
          }
        }
        labelPoints[labelIndex] = new Point(xPosition, 0);

        //calculate label string
        labelStrings[labelIndex] = labelValue.ToString(numberFormat);

        //calculate next label
        labelValue += step.Value;
      }
      */
    }



    #endregion
  }
}
