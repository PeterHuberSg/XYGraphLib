/**************************************************************************************

XYGraphLib.LegendXDouble
========================

Horizontal Legend

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

// Displays a horizontal legend for a PlotArea based on double data. More often, LegendXDate day is used, 
// which inherits from LegendXDouble, but displays the double values as dates.
// MinValue and MaxValue indicate the largest values the legend can display.
// 
//            ┌────────────────────────────────────┐
// -100       │ 100             150            200 │     500
//    ↑       └────────────────────────────────────┘      ↑
// MinValue     ↑DisplayValue                   ↑        MaxValue
//              DisplayValue + DisplayValueRange│
//


namespace XYGraphLib {

  /// <summary>
  /// Displays the legend below the PlotArea (often time axis).
  /// </summary>
  public class LegendXDouble: LegendX {


    #region Measurement Overrides
    //      ---------------------

    //nothing needs to be done OnFontChanged()
    //protected override void OnFontChanged(bool hasOnlyFontSizeChanged) {
    //}

    protected override Size OnLegendMeasurement(Size requiredSize) {
      requiredSize.Height = FontFamily.LineSpacing*FontSize;
      return requiredSize;
    }
    #endregion


    #region Arrange Overrides
    //      -----------------

    protected override Size OnLegendArrange(Rect arrangeRect) {
      if (IsSizingHeightToFixedContent()) {
        //use all the width, but only the height needed
        return new Size(arrangeRect.Size.Width, FontFamily.LineSpacing*FontSize);
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


    protected override void OnReset() {
      RenderWidthTracked = double.NaN;
    }


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


    protected override void OnRecalculate(ref double[]? labelValues, ref string?[]? labelStrings, ref Point[]? labelPoints) {
      //find first which label will need the most digits. Use MinValue or MaxValue, not DisplayValue and range, because the format
      //should stay the same for all values between min to max.
      double lowestValue;
      double highestValue;
      if (double.IsNaN(MinValue) || double.IsNaN(MaxValue)) {
        //Min- and MaxValue are not defined. Use DisplayValue and DisplayValueRange instead
        lowestValue = DisplayValue;
        highestValue = lowestValue + DisplayValueRange; //DisplayValueRange is guaranteed to be greater 0
      } else {
        //Min- and MaxValue are defined, use them.
        lowestValue = MinValue;
        highestValue = MaxValue;
      }

      //use DisplayValueRange as first estimate for value difference between 2 labels.
      //normalise amplitude between 1.0 and 10.0
      DoubleDigitsExponent stepDigitsExponent = new DoubleDigitsExponent(DisplayValueRange);
#if DEBUG
      if (stepDigitsExponent.Digits<1.00 || stepDigitsExponent.Digits>=10.00) {
        System.Diagnostics.Debugger.Break();
        throw new Exception(string.Format("Legend: Normalised stepValue should be between 1.0 and 9.999, but was '{0}'.", stepDigitsExponent.Digits));
      }
#endif
      //the first digits of a step (=value difference between 2 labels) can be 1, 2 or 5. All other digits of a step are 0.
      //chose a first step which is smaller than DisplayValueRange
      StepStruct step;
      if (stepDigitsExponent.Digits<1.00) {
        step = new StepStruct(5, stepDigitsExponent.Exponent-1);
      } else if (stepDigitsExponent.Digits<2.00) {
        step = new StepStruct(1, stepDigitsExponent.Exponent);
      } else if (stepDigitsExponent.Digits<5.00) {
        step = new StepStruct(2, stepDigitsExponent.Exponent);
      } else {
        step = new StepStruct(5, stepDigitsExponent.Exponent);
      }
      string minMaxnumberFormat = getNumberFormat(step);

      //find the label which needs the most digits
      //MinValue might need more digits, because it can be negative (-10000) and Max might be a small number (0).
      //convert smallest and highest value to string
      string lowestValueString = lowestValue.ToString(minMaxnumberFormat);
      string highestValueString = highestValue.ToString(minMaxnumberFormat);

      //select longer string. 
      string longestValueString;
      double longestStringValue;
      if (lowestValueString.Length>highestValueString.Length) {
        longestValueString = lowestValueString;
        longestStringValue = lowestValue;
      } else {
        longestValueString = highestValueString;
        longestStringValue = highestValue;
      }

      //DisplayRange provides a first estimate for the distance between 2 labels (=step)
      //step gets made smaller and smaller, until the space needed to display the label is bigger than the space available between
      //2 labels
      string? numberFormat = null;
      double labelWidth = double.NaN;
      double pixelPerValue = RenderWidthTracked / DisplayValueRange;
      StepStruct previousStep = step;
      string longestValuePlusSpace = longestValueString + "    ";
      while (true) {
        string newNumberFormat = getNumberFormat(step);
        longestValueString = longestStringValue.ToString(newNumberFormat);
        labelWidth = LegendGlyphDrawer.GetLength(longestValuePlusSpace, FontSize);

        if (labelWidth>step.Value*pixelPerValue) {
          //not enough space for new format. Use previous step and its format.
          //if there is not enough space for even 1 label: we come here first time going through the loop. step is already
          //equal to previousStep. Assigning it again doesn't hurt. Even step is in this case not a meaningful value,
          //it is ok, because the dingle label displayed will not use step for formatting.
          step = previousStep;
          break;
        }

        numberFormat = newNumberFormat;
        //try next smaller step
        previousStep = step;
        if (step.FirstDigit==1) {
          step = new StepStruct(5, step.Exponent-1);
        } else if (step.FirstDigit==2) {
          step = new StepStruct(1, step.Exponent);
        } else if (step.FirstDigit==5) {
          step = new StepStruct(2, step.Exponent);
        } else {
          throw new Exception("Illegal FirstDigit of step: " + step + ". It should be 1, 2 or 5.");
        }
      }

      //number of labels that can be displayed in window
      int estimatedLabelCount;
      if (numberFormat==null) {
        //not enough space to display even 1 label properly.
        estimatedLabelCount = 1;
      } else {
#if DEBUG
        if (double.IsNaN(labelWidth)) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("labelWidth cannot be NaN.");
        }
#endif
        estimatedLabelCount = (int)(RenderWidthTracked / labelWidth);
      }

      if (estimatedLabelCount<=1) {
        //only 1 or even only the part of a label can be displayed. In this case it is better to just display the 
        //very first value and not trying to find the first nicely rounded step-number
        if (labelValues==null || labelValues.Length!=1) {
          labelValues = new double[1];
          labelStrings = new string[1];
          labelPoints = new Point[1];
        }
        labelValues[0] = DisplayValue;
        labelStrings![0] = DisplayValue.ToString();
        labelPoints![0] = new Point(0, 0);
        return;
      }


      FindFirstLastLabel(ref labelValues, ref labelStrings, ref labelPoints, step, out var labelValue);
      if (labelValues.Length==1) {
        //only 1 or even only the part of a label can be displayed. In this case it is better to just display the 
        //very first value and not trying to find the first nicely rounded step-number
        labelValues[0] = DisplayValue;
        labelStrings[0] = DisplayValue.ToString();
        labelPoints[0] = new Point(0, 0);
        return;
      }

      //calculate labels
      //double pixelPerValue = RenderWidthTracked / DisplayValueRange;
      for (int labelIndex = 0; labelIndex < labelValues.Length; labelIndex++) {
        //write label value
        labelValues[labelIndex] = labelValue;

        //calculate position
        double xPosition = (labelValue-DisplayValue) * pixelPerValue;
#if DEBUG
        if (labelIndex>0){
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
    }


    private string getNumberFormat(StepStruct step) {
      string numberFormat = "#,0";
      if (step.Exponent<0) {
        //add required digits after decimal point
        numberFormat += '.' + new string('0', -step.Exponent);
      }
      return numberFormat;
    }


    //private string calculateNumberFormat(double range) {

    //  //calculate number of digits after decimal point. double.ToString() generates digits as needed in front of decimal 
    //  //point, but will not display zeros after the decimal point. However, the legend looks nicer is every label has
    //  //the same number of digits after the decimal point.
    //  DoubleDigitsExponent rangeDigitsExponent = new DoubleDigitsExponent(range);
    //  string numberFormat = "#,0";
    //  if (rangeDigitsExponent.Exponent<0) {
    //    //add required digits after decimal point
    //    numberFormat += '.' + new string('0', -rangeDigitsExponent.Exponent);
    //  }
    //  return numberFormat;
    //}


    protected override Point OnContentAlignment(Size renderContentSize) {
      double posY = FontFamily.Baseline*FontSize;
      if (!double.IsNaN(Height) || VerticalAlignment==VerticalAlignment.Stretch) {
        //VerticalContentAlignment matters only if space available is different from the needed space, which is only possible if 
        //Legend is stretched or its height is defined
        switch (VerticalContentAlignment) {
        case VerticalAlignment.Top:
          break;
        case VerticalAlignment.Stretch:
        case VerticalAlignment.Center:
          posY += (renderContentSize.Height - FontFamily.LineSpacing*FontSize)/2;
          break;
        case VerticalAlignment.Bottom:
          posY += renderContentSize.Height - FontFamily.LineSpacing*FontSize;
          break;
        default:
          throw new NotSupportedException();
        }
      }
      return new Point(0, posY);
    }
    #endregion

  }
}
