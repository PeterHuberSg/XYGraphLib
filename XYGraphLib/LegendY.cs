/***************************************************************************************************************************
 * LegendY
 * =======
 * 
 * Displays the right or left legend for a PlotArea with a displayed value range of DisplayValue to DisplayValueRange. The values are 
 * displayed vertically (y-axis).
 * MinValue and MaxValue indicate the largest values the legend might have to display after scrolling. They are
 * used to calculate the required width during MeasureOverride().
 * 
 * ' 500' MaxValue
 * '    '                                     
 * +--------------------------+
 * |  30' ^ DisplayValueRange |
 * |  20' |                   | <- DisplayLegend                           
 * |  10' DisplayValue        |
 * +--------------------------+
 * '    '                                   
 * '-100' MinValue
 *    500'...MaxValue
 *       '
 * +-----'+
 * |  200'| DisplayValue + DisplayValueRange
 * |     '|
 * |  150'|
 * |     '|
 * |  100'| DisplayValue
 * +-----'+
 *       '
 *   -100'..MinValue * 
 * note that the values must be right aligned ==> there is some blank space before the displayed values 100-200. When the user
 * scrolls to -100, there is enough space to display it without changing the legend width. If the width of the legend would 
 * vary during scrolling, so would the width of the chart, which would be annoying for the user.
 * 
 ***************************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;


namespace XYGraphLib {


  /// <summary>
  /// Displays the the right or left legend for a PlotArea. The legend has labels displaying the value a pixel at this location corresponds to.  DisplayValue and 
  /// DisplayValueRange control the value range displayed. MinValue and MaxValue indicate the largest values the legend might have to display after scrolling. This
  /// information is needed to chose a number formating which is correct from Min- to MaxValue.
  /// </summary>
  public class LegendY: Legend {
    //LegendY:
    //   -100...MinValue
    //
    // +------+
    // |  100 | DisplayValue
    // |      |
    // |  150 |
    // |      |
    // |  200 | DisplayValue + DisplayValueRange
    // +------+
    // 
    //    500...MaxValue


    #region Constructor
    //      -----------

    /// <summary>
    /// Default constructor. A LegendY is written parallel to the y-axis (vertically) and shows per default y-values, 
    /// which is dimension 1
    /// </summary>
    public LegendY()
      : this(Renderer.DimensionY) {
    }


    /// <summary>
    /// Constructor if LegendY should be used for another dimension than y, although still written parallel to the y-axis (vertically)
    /// </summary>
    public LegendY(int dimension)
      : base(dimension, newNeedsMeasureWhenValuesChange: true, isWriteRightAligned: true) {
    }
    #endregion


    #region Measurement Overrides
    //      ---------------------

    bool hasFontChanged;


    protected override void OnFontChanged(bool hasOnlyFontSizeChanged) {
      hasFontChanged = true;
    }


    const double minHeightInFonts = 1.5;//multiplier used to calculate minimum height based on font size


    protected override Size OnLegendMeasurement(Size requiredSize) {
      if (requiredSize.Height==0) {
        //Legend was given infinite space and has converted it to 0 for the requestedSize. Request a bit more height.
        requiredSize.Height = minHeightInFonts*FontSize;//request at least minimum height
      } else {
        if (requiredSize.Height<FontSize) return requiredSize; //not high enough to display legend values
      }

      requiredSize.Width = calculateLegendWidth(requiredSize.Height);
      return requiredSize;
    }


    double heightTracked;
    double requiredWidth;
    StepStruct step;
    string numberFormat;
    bool isNewWidthCalculated;


    protected override void OnReset() {
      heightTracked = double.MinValue;
      requiredWidth = double.NaN;
      isNewWidthCalculated = false;
    }


    /// <summary>
    /// 1) Calculate the step value based on MinValue, MaxValue, DisplayRange and availableHeight. 
    /// 2) Width is calculated based on fontSize and number of didgits needed to display MinValue & MaxValue.
    /// </summary>
    private double calculateLegendWidth(double availableHeight) {
      //calculateLegendWidth() is called from within Measure and Arrange. Legend guarantees that MinValue, DisplayValue, DisplayValueRange and
      //MaxValue have reasonable values

      if (!hasFontChanged && heightTracked==availableHeight && !HasMinMaxOrDisplayValueRangeChanged && !double.IsNaN(requiredWidth)) 
      {
        return requiredWidth;//nothing has changed
      }

      isNewWidthCalculated = true;
      hasFontChanged = false;
      heightTracked = availableHeight;

      //calculate step value between 2 legend values
      //--------------------------------------------

      //value between 2 display labels (=step)
      double estimatedStepValue = DisplayValueRange * FontSize * 3 / availableHeight;

      if (estimatedStepValue<1000 * double.Epsilon) {
        throw new ApplicationException(string.Format("Legend: range '{0}' too close to zero." + this, estimatedStepValue));
      }

      //normalise amplitude between 1.0 and 10.0
      DoubleDigitsExponent stepDigitsExponent = new DoubleDigitsExponent(estimatedStepValue);
      if (stepDigitsExponent.Digits<1.00 || stepDigitsExponent.Digits>=10.00) {
        throw new ApplicationException(string.Format("Legend: Normalised stepValue should be between 1.0 and 9.999, but was '{0}'.", stepDigitsExponent.Digits));
      }

      if (stepDigitsExponent.Digits<1.00) {
        step = new StepStruct(1, stepDigitsExponent.Exponent);
      } else if (stepDigitsExponent.Digits<2.00) {
        step = new StepStruct(2, stepDigitsExponent.Exponent);
      } else if (stepDigitsExponent.Digits<5.00) {
        step = new StepStruct(5, stepDigitsExponent.Exponent);
      } else {
        step = new StepStruct(1, stepDigitsExponent.Exponent+1);
      }

      //calculate string format. It depends on:
      //---------------------------------------
      //1) biggest and smallest value in Min-/MaxValue Range
      //2) step size in DisplayValue Range

      //find maximum number of characters needed to display longest Value
      double minValue;
      if (double.IsNaN(MinValue)){
        minValue = DisplayValue;
      }else{
        minValue = MinValue;
      }
      DoubleDigitsExponent minValueDigitsExponent = new DoubleDigitsExponent(minValue);
      double maxValue;
      if (double.IsNaN(MaxValue)) {
        maxValue = DisplayValue + DisplayValueRange;
      } else {
        maxValue = MaxValue;
      }
      DoubleDigitsExponent maxValueDigitsExponent = new DoubleDigitsExponent(maxValue);
      int minExponent;
      int maxExponent;
      if (minValueDigitsExponent.Exponent<=maxValueDigitsExponent.Exponent) {
        //Example: Min=0.1, Max = 10000
        minExponent = minValueDigitsExponent.Exponent;
        maxExponent = maxValueDigitsExponent.Exponent;
      } else {
        //Example: Min=-10000, Max = 0.1
        minExponent = maxValueDigitsExponent.Exponent;
        maxExponent = minValueDigitsExponent.Exponent;
      }
      numberFormat = "#,0";

      if (step.Exponent<0) {
        //add required digits after decimal point
        numberFormat += '.' + new string('0', -step.Exponent);
      }

      string minValueString = minValue.ToString(numberFormat);
      double minValueStringWidth = LegendGlyphDrawer.GetLength(minValueString, FontSize);
      string maxValueString = maxValue.ToString(numberFormat);
      double maxValueStringWidth = LegendGlyphDrawer.GetLength(maxValueString, FontSize);
      requiredWidth = Math.Max(minValueStringWidth, maxValueStringWidth);

      return requiredWidth;
    }
    #endregion


    #region Arrange Overrides
    //      -----------------

    protected override Size OnLegendArrange(Rect arrangeRect) {
      //calculate width and step again, since available height might be different during measure and arrange
      double calculatedWidth = calculateLegendWidth(arrangeRect.Height);
      if (IsSizingWidthToFixedContent()) {
        //use all the height, but only the width needed
        return new Size(calculatedWidth, arrangeRect.Height);
      } else {
        //use all available space
        return arrangeRect.Size;
      }
    }
    #endregion


    #region Render Overrides
    //      ----------------

    protected override bool OnIsRecalculationNeeded(Size renderContentSize) {
      return isNewWidthCalculated;
    }


    protected override void OnRecalculate(ref double[] labelValues, ref string[] labelStrings, ref Point[] labelPoints) {
      isNewWidthCalculated = false; //prevents OnRecalculate to be called again
      double labelValue;
      FindFirstLastLabel(ref labelValues, ref labelStrings, ref labelPoints, step, out labelValue);

      //calculate label values
      double pixelPerValue = heightTracked / DisplayValueRange;
      for (int labelIndex = 0; labelIndex < labelValues.Length; labelIndex++) {
        //write label value
        labelValues[labelIndex] = labelValue;

        //calculate position
        double yPosition = heightTracked - (labelValue-DisplayValue) * pixelPerValue - FontSize / 3;
#if DEBUG
        if (yPosition> heightTracked) {
          //should not happen
          throw new Exception();
        }
#endif
        labelPoints[labelIndex] = new Point(0, yPosition);

        //calculate label string
        labelStrings[labelIndex] = labelValue.ToString(numberFormat); ;

        //calculate next label
        labelValue += step.Value;
      }
    }


    protected override Point OnContentAlignment(Size renderContentSize) {
      double posX = requiredWidth;
      if (!double.IsNaN(Width) || HorizontalAlignment==HorizontalAlignment.Stretch) {
        //HorizontalContentAlignment matters only if space available is different from the needed space, which is only possible if 
        //XLegend is stretched or its width is defined
        switch (HorizontalContentAlignment) {
        case HorizontalAlignment.Left:
          break;
        case HorizontalAlignment.Stretch:
        case HorizontalAlignment.Center:
          posX = (renderContentSize.Width + requiredWidth)/2;
          break;
        case HorizontalAlignment.Right:
          posX = renderContentSize.Width;
          break;
        default:
          throw new NotSupportedException();
        }
      }
      return new Point(posX, 0);
    }
    #endregion

  }
}
