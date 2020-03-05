using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;


namespace XYGraphLib {


  public class LegendX: Legend {

    #region Constructor
    //      -----------

    /// <summary>
    /// Default constructor. A LegendX is written parallel to the x-axis (horizontally) and shows per default x-values, 
    /// which is dimension 1
    public LegendX()
      : this(Renderer.DimensionX) {
    }


    /// <summary>
    /// Constructor if LegendX should be used for another dimension than x, although still written parallel to the x-axis (horizontally)
    /// </summary>
    public LegendX(int dimension)
      : base(dimension) {
    }
    #endregion

    
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


    const double freeSpaceFactor = 1.3;


    protected override void OnRecalculate(ref double[] labelValues, ref string[] labelStrings, ref Point[] labelPoints) {
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
      //the first digits of a step (=value difference between 2 lables) can be 1, 2 or 5. All other digits of a step are 0.
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
      string numberFormat = null;
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
          //equal to previousStep. Assigning it agin doesn't hurt. Even step is in this case not a meaningful value,
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
        labelStrings[0] = DisplayValue.ToString();
        labelPoints[0] = new Point(0, 0);
        return;
      }


      double labelValue;
      FindFirstLastLabel(ref labelValues, ref labelStrings, ref labelPoints, step, out labelValue);
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


//protected  void OnRecalculateOld(ref double[] labelValues, ref string[] labelStrings, ref Point[] labelPoints) {
//      double smallestValue;
//      double largestValue;
//      double range;
//      if (double.IsNaN(MinValue) || double.IsNaN(MaxValue)) {
//        //Min- and MaxValue are not defined. Use DisplayValue and DisplayValueRange instead
//        smallestValue = DisplayValue;
//        largestValue = smallestValue + DisplayValueRange; //DisplayValueRange is guaranteed to be greater 0
//        range = DisplayValueRange;
//      } else {
//        //Min- and MaxValue are defined, use them.
//        smallestValue = MinValue;
//        largestValue = MaxValue;
//        range = MaxValue - MinValue;//MaxValue is guaranteed to be greater than MinValue
//      }

//      string numberFormat = calculateNumberFormat(range);
//      //find first which label will need the most digits. It is MinValue or MaxValue. MinValue might need more digits, because it
//      // can be negative (-10000) and Max might be a small number (0).
//      //convert smallest and highest value to string
//      string smallestValueString = smallestValue.ToString(numberFormat);
//      string largestValueString = largestValue.ToString(numberFormat);

//      //select longer string. Sometimes the smallest label needs more digits than the highest, for example when the numbers are negative
//      string longestValueString;
//      double longestStringValue;
//      if (smallestValueString.Length>largestValueString.Length) {
//        longestValueString = smallestValueString;
//        longestStringValue = smallestValue;
//      } else {
//        longestValueString = largestValueString;
//        longestStringValue = largestValue;
//      }

//      //pixel length of longest label
//      double maxLabelWidth = LegendGlyphDrawer.GetLength(longestValueString, FontSize) * freeSpaceFactor;

//      //from here on all calculations are based on available pixel and DisplayRange, not MinMax
//      //pixel width and DisplayRange provide a first estimate for the distance between 2 labels (=step)
//      //step gets made smaller and smaller, until the space needed to display the label is bigger than the space available
//      //---------------------------------------------------------------------------------------

//      //maximum labels that can be displayed in window
//      int maxLabelCount = (int)(RenderWidthTracked / maxLabelWidth);

//      if (maxLabelCount>1) {
//        //if there is more than one label, even more digits after the decimal point might need to be displayed.
//        double estimatedDifferenceBetweenLabels = DisplayValueRange / maxLabelCount;
//        numberFormat = calculateNumberFormat(estimatedDifferenceBetweenLabels);
//        longestValueString = longestStringValue.ToString(numberFormat); //still use MinMax to find widest label
//        maxLabelWidth = LegendGlyphDrawer.GetLength(longestValueString, FontSize) * freeSpaceFactor;
//        maxLabelCount = (int)(RenderWidthTracked / maxLabelWidth);
//      }

//      double estimatedStepValue;
//      if (maxLabelCount<=1) {
//        //only 1 or even only the part of a label can be displayed. In this case it is better to just display the 
//        //very first value and not trying to find the first nicely rounded number
//        if (labelValues==null || labelValues.Length!=1) {
//          labelValues = new double[1];
//          labelStrings = new string[1];
//          labelPoints = new Point[1];
//        }
//        labelValues[0] = DisplayValue;
//        labelStrings[0] = DisplayValue.ToString();
//        labelPoints[0] = new Point(0, 0);
//        return;
//      }

//      estimatedStepValue = DisplayValueRange / maxLabelCount;

//      //normalise amplitude between 1.0 and 10.0
//      DoubleDigitsExponent stepDigitsExponent = new DoubleDigitsExponent(estimatedStepValue);
//      if (stepDigitsExponent.Digits<1.00 || stepDigitsExponent.Digits>=10.00) {
//        throw new ApplicationException(string.Format("Legend: Normalised stepValue should be between 1.0 and 9.999, but was '{0}'.", stepDigitsExponent.Digits));
//      }

//      StepStruct step;
//      if (stepDigitsExponent.Digits<1.00) {
//        step = new StepStruct(1, stepDigitsExponent.Exponent);
//      } else if (stepDigitsExponent.Digits<2.00) {
//        step = new StepStruct(2, stepDigitsExponent.Exponent);
//      } else if (stepDigitsExponent.Digits<5.00) {
//        step = new StepStruct(5, stepDigitsExponent.Exponent);
//      } else {
//        step = new StepStruct(1, stepDigitsExponent.Exponent+1);
//      }

//      numberFormat = "#,0";
//      if (step.Exponent<0) {
//        //add required digits after decimal point
//        numberFormat += '.' + new string('0', -step.Exponent);
//      }

//      double labelValue;
//      FindFirstLastLabel(ref labelValues, ref labelStrings, ref labelPoints, step, out labelValue);

//      //calculate labels
//      double pixelPerValue = RenderWidthTracked / DisplayValueRange;
//      for (int labelIndex = 0; labelIndex < labelValues.Length; labelIndex++) {
//        //write label value
//        labelValues[labelIndex] = labelValue;

//        //calculate position
//        double xPosition = (labelValue-DisplayValue) * pixelPerValue;
//#if DEBUG
//        if (labelIndex>0) {
//          if (xPosition-labelPoints[labelIndex-1].X<LegendGlyphDrawer.GetLength(labelValue.ToString(numberFormat), FontSize)) {
//            System.Diagnostics.Debugger.Break();
//            throw new Exception("label " + labelValue.ToString(numberFormat) + " needs " + LegendGlyphDrawer.GetLength(labelValue.ToString(numberFormat), FontSize) +
//            " pixels, but there are only " + (xPosition-labelPoints[labelIndex-1].X) + " pixels distance between 2 labels.");
//          }
//        }
//#endif
//        if (xPosition> RenderWidthTracked) {
//          if (xPosition<1.0001*RenderWidthTracked) {
//            //probably rounding error
//            xPosition = RenderWidthTracked;
//          } else {
//            //should not happen
//            throw new Exception();
//          }
//        }
//        labelPoints[labelIndex] = new Point(xPosition, 0);

//        //calculate label string
//        labelStrings[labelIndex] = labelValue.ToString(numberFormat);

//        //calculate next label
//        labelValue += step.Value;
//      }
//    }


    private string calculateNumberFormat(double range) {
      //DoubleDigitsExponent maxValueDigitsExponent = new DoubleDigitsExponent(maxValue);
      ////calculate number of digits before decimal point
      //int leadingDigits; 
      //if (maxValueDigitsExponent.Exponent<0){
      //  leadingDigits = 1;
      //}else{
      //  leadingDigits = maxValueDigitsExponent.Exponent + 1;
      //}

      //calculate number of digits after decimal point
      //int afterDigits;
      //if (maxValueDigitsExponent.Exponent>=0) {
      //  afterDigits = 0;
      //} else {
      //  afterDigits = -maxValueDigitsExponent.Exponent;
      //}
      //DoubleDigitsExponent rangeDigitsExponent = new DoubleDigitsExponent(range);
      //if (rangeDigitsExponent.Exponent<0) {
      //  //range needs some digits after the comma
      //  if (maxValueDigitsExponent.Exponent>rangeDigitsExponent.Exponent) {
      //    afterDigits = -rangeDigitsExponent.Exponent;
      //  }
      //}
      //string numberFormat = "#,0";
      //if (afterDigits>0) {
      //  //add required digits after decimal point
      //  numberFormat += '.' + new string('0', afterDigits);
      //}

      //calculate number of digits after decimal point. double.ToString() generates digits as needed in front of decimal 
      //point, but will not display zeros after the decimal point. However, the legend looks nicer is every label has
      //the same number of digits after the decimal point.
      DoubleDigitsExponent rangeDigitsExponent = new DoubleDigitsExponent(range);
      string numberFormat = "#,0";
      if (rangeDigitsExponent.Exponent<0) {
        //add required digits after decimal point
        numberFormat += '.' + new string('0', -rangeDigitsExponent.Exponent);
      }
      return numberFormat;
    }


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
    //protected override void OnRenderContent(DrawingContext drawingContext, Size renderContentSize) {
    //  //// Control declares Background, but does not do anything with it. We have to draw the background ourselves.
    //  //Brush background = Background;
    //  //if (background!=null) {
    //  //  Rect legendRectangle = new Rect(0, 0, renderContentSize.Width, renderContentSize.Height);
    //  //  drawingContext.DrawRectangle(background, null, legendRectangle);
    //  //}
    //  bool hasWidthChanged = false;
    //  if (widthTracked!=renderContentSize.Width) {
    //    //available number of pixels has changed.
    //    widthTracked = renderContentSize.Width;
    //    hasWidthChanged = true;
    //  }
    //  if (widthTracked==0) return;

    //  if (MinDisplayDate==minDisplayDateInit || MaxDisplayDate==maxDisplayDateInit) {
    //    //Replace default data with some data which can be displayed.
    //    MaxDisplayDate = DateTime.Now.Date;
    //    MinDisplayDate = MaxDisplayDate.AddMonths(-1);
    //  }

    //  if (MaxDisplayDate<MinDisplayDate) throw new ApplicationException("MaxDisplayDate " + MaxDisplayDate + " must be greater than MinDisplayDate " + MinDisplayDate + ".");

    //  bool isDisplayIndexChanged = false;
    //  if (minDisplayValueTracked!=MinDisplayDate || maxDisplayValueTracked!=MaxDisplayDate || isNewGlyphDrawer || hasWidthChanged) {

    //    minDisplayValueTracked = MinDisplayDate;
    //    maxDisplayValueTracked = MaxDisplayDate;
    //    isNewGlyphDrawer = false;

    //    //choose time unit (seconds to years)
    //    TimeSpanUnit = timeSpanUnitFormatter.GetDefaultTimeSpanUnit(MaxDisplayDate - MinDisplayDate, widthTracked);
    //    TimeSpan step = new TimeSpan(TimeSpanUnit.GetTicks());
    //    //find start of first time unit after MinDisplayDate
    //    DateTime minDisplayLegendLabel;
    //    if (TimeSpanUnit==TimeSpanUnitEnum.month) {
    //      minDisplayLegendLabel = MinDisplayDate.Date;
    //      if (minDisplayLegendLabel!=MinDisplayDate) {
    //        //day has already started. Take the next one for the first label
    //        minDisplayLegendLabel = minDisplayLegendLabel.Date.AddDays(1);
    //      }
    //      if (minDisplayLegendLabel.Day!=1) {
    //        if (minDisplayLegendLabel.Month<12) {
    //          minDisplayLegendLabel = new DateTime(minDisplayLegendLabel.Year, minDisplayLegendLabel.Month+1, 1);
    //        } else {
    //          minDisplayLegendLabel = new DateTime(minDisplayLegendLabel.Year+1, 1, 1);
    //        }
    //      }
    //    } else if (TimeSpanUnit>=TimeSpanUnitEnum.year) {
    //      minDisplayLegendLabel = MinDisplayDate.Date;
    //      if (minDisplayLegendLabel!=MinDisplayDate) {
    //        //day has already started. Take the next one for the first label
    //        minDisplayLegendLabel = minDisplayLegendLabel.Date.AddDays(1);
    //      }
    //      if (minDisplayLegendLabel.Day!=1 || minDisplayLegendLabel.Month!=1) {
    //        minDisplayLegendLabel = new DateTime(minDisplayLegendLabel.Year+1, 1, 1);
    //      }
    //      if (TimeSpanUnit>=TimeSpanUnitEnum.year5) {
    //        int yearMultiplier = TimeSpanUnit.GetYearMultiplier();
    //        if (minDisplayLegendLabel.Year%yearMultiplier>0) {
    //          minDisplayLegendLabel = new DateTime(minDisplayLegendLabel.Year - minDisplayLegendLabel.Year%yearMultiplier + yearMultiplier, 1, 1); ;
    //        }
    //      }
    //    } else {
    //      // timeSpanUnit is a week or shorter
    //      minDisplayLegendLabel = new DateTime((MinDisplayDate.Ticks/step.Ticks) * step.Ticks);
    //      if (minDisplayLegendLabel<MinDisplayDate) {
    //        minDisplayLegendLabel = minDisplayLegendLabel.AddTicks(step.Ticks);
    //      }
    //    }

    //    int labelsCount;
    //    DateTime maxDisplayLegendLabel;
    //    if (TimeSpanUnit>=TimeSpanUnitEnum.year) {
    //      //a year or more
    //      int yearMultiplier = TimeSpanUnit.GetYearMultiplier();
    //      int yearsDifference = MaxDisplayDate.Year - minDisplayLegendLabel.Year;
    //      labelsCount = yearsDifference / yearMultiplier;
    //      maxDisplayLegendLabel = MinDisplayDate.AddYears(labelsCount);
    //      labelsCount++;

    //    } else if (TimeSpanUnit==TimeSpanUnitEnum.month) {
    //      labelsCount = MaxDisplayDate.Month - minDisplayLegendLabel.Month + (MaxDisplayDate.Year - minDisplayLegendLabel.Year)*12;
    //      maxDisplayLegendLabel = MinDisplayDate.AddMonths(labelsCount);
    //      labelsCount++;

    //    } else {
    //      //week or shorter. The number of ticks per TimeSpanUnit is fixed as opposed to months (28-31 days) or years (365/366 days)
    //      maxDisplayLegendLabel = new DateTime((MaxDisplayDate.Ticks/step.Ticks) * step.Ticks);
    //      if (maxDisplayLegendLabel>MaxDisplayDate) {
    //        maxDisplayLegendLabel = maxDisplayLegendLabel.AddTicks(-step.Ticks);
    //      }
    //      labelsCount =  (int)(Math.Round(((double)(maxDisplayLegendLabel - minDisplayLegendLabel).Ticks) / step.Ticks, 6)) + 1;
    //      DateTime verifyMaxDisplayDate = new DateTime(minDisplayLegendLabel.Ticks + (labelsCount-1)*step.Ticks);
    //      if (verifyMaxDisplayDate.Ticks > maxDisplayLegendLabel.Ticks / 999 * 1000 || 
    //      verifyMaxDisplayDate.Ticks < maxDisplayLegendLabel.Ticks / 1001 * 1000) {
    //        throw new ApplicationException("maxDisplayLabelDate " + maxDisplayLegendLabel + " should be equal to minDisplayLabelDate " + 
    //        minDisplayLegendLabel + " + labelsCount " + labelsCount + " * step " + step + ".");
    //      }
    //    }

    //    LabelsCount = labelsCount;
    //    MinDisplayLegendLabel = minDisplayLegendLabel;
    //    LabelStep = step;

    //    if (legendStrings==null || legendStrings.Length!=labelsCount) {
    //      legendStrings = new string[labelsCount];
    //      labelsDates = new DateTime[labelsCount];
    //      LabelValues = new double[labelsCount];
    //    }

    //    //calculate label dates strings
    //    DateTime labelDate = minDisplayLegendLabel;
    //    for (int legendDateStringsIndex = 0; legendDateStringsIndex < labelsCount; legendDateStringsIndex++) {
    //      string legendString;
    //      labelsDates[legendDateStringsIndex] = labelDate;
    //      if (legendDateStringsIndex==0) {
    //        legendString = labelDate.ToFirstLabel(TimeSpanUnit);
    //      } else {
    //        legendString = labelDate.ToFollowLabel(TimeSpanUnit);
    //      }
    //      legendStrings[legendDateStringsIndex] = legendString;
    //      if (TimeSpanUnit>=TimeSpanUnitEnum.year) {
    //        int yearMultiplier = TimeSpanUnit.GetYearMultiplier();
    //        labelDate = labelDate.AddYears(yearMultiplier);
    //      } else if (TimeSpanUnit==TimeSpanUnitEnum.month) {
    //        labelDate = labelDate.AddMonths(1);
    //      } else {
    //        labelDate = labelDate.AddTicks(step.Ticks);
    //      }
    //    }
    //    TraceWpf.Line("Min: " + MinDisplayDate + ", Label: " + MinDisplayLegendLabel + ", Max: " + MaxDisplayDate);
    //    isDisplayIndexChanged = true;
    //  }

    //  double yPos = FontFamily.Baseline*FontSize;
    //  if (!double.IsNaN(Height) || VerticalAlignment==VerticalAlignment.Stretch) {
    //    //VerticalContentAlignment matters only if space available is different from the needed space, which is only possible if 
    //    //Legend is stretched or its height is defined
    //    switch (VerticalContentAlignment) {
    //    case VerticalAlignment.Top:
    //      break;
    //    case VerticalAlignment.Stretch:
    //    case VerticalAlignment.Center:
    //      yPos += (renderContentSize.Height - FontFamily.LineSpacing*FontSize)/2;
    //      break;
    //    case VerticalAlignment.Bottom:
    //      yPos += renderContentSize.Height - FontFamily.LineSpacing*FontSize;
    //      break;
    //    default:
    //      throw new NotSupportedException();
    //    }
    //  }

    //  TimeSpan displayRange = MaxDisplayDate - MinDisplayDate;
    //  double pixelPerTick = widthTracked / displayRange.Ticks;
    //  double firstLabelWidth = double.MinValue;
    //  TimeSpanUnitMask mask = TimeSpanUnit.GetMasks();
    //  for (int legendDateStringsIndex = 0; legendDateStringsIndex < legendStrings.Length; legendDateStringsIndex++) {
    //    double xPosition = (labelsDates[legendDateStringsIndex]-MinDisplayDate).Ticks * pixelPerTick;
    //    LabelCoordinates[legendDateStringsIndex] = xPosition;
    //    if (xPosition> widthTracked) break;

    //    //skip second label if first label is too long
    //    if (legendDateStringsIndex!=1 || firstLabelWidth<xPosition) {
    //      LegendGlyphDrawer.Write(drawingContext, new Point(xPosition, yPos), legendStrings[legendDateStringsIndex], FontSize, Foreground);
    //    }
    //    if (legendDateStringsIndex==0) {
    //      //      double firstLabelWidth = glyphDrawer.GetLength(mask.FirstLabelMask, FontSize) * 1.10;
    //      firstLabelWidth = xPosition + LegendGlyphDrawer.GetLength(legendStrings[0], FontSize) * 1.05;
    //    }
    //  }

    //  if (isDisplayIndexChanged && DisplayValueChanged!=null) {
    //    XLegendScroller xLegendScroller = Parent as XLegendScroller;
    //    if (xLegendScroller!=null) {
    //      DisplayValueChanged(xLegendScroller);
    //    }
    //  }
    //}


    #endregion

  }
}
