/**************************************************************************************

XYGraphLib.LegendXDate
======================

Horizontal Legend with date values

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


namespace XYGraphLib {


  /// <summary>
  /// /// Displays the legend below the PlotArea with date values.
  /// </summary>
  public class LegendXDate: LegendX {

    #region Properties
    //      ----------

    /// <summary>
    /// Earliest date to be displayed in legend
    /// Default Value: DateTime.MaxValue
    /// </summary>
    public DateTime DisplayDate {
      get { return (DateTime)GetValue(DisplayDateProperty); }
      set { SetValue(DisplayDateProperty, value); }
    }
    DateTime displayDateTracked;
    static readonly DateTime initDisplayDate = DateTime.MaxValue;


    /// <summary>
    /// The DependencyProperty definition for DisplayDate property.
    /// </summary>
    public static readonly DependencyProperty DisplayDateProperty =
          DependencyProperty.RegisterAttached(
        "DisplayDate", // Property name
        typeof(DateTime), // Property type
        typeof(LegendXDate), // Property owner
        new FrameworkPropertyMetadata(initDisplayDate, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Time range to be displayed in legend
    /// Default Value: TimeSpan.MinValue
    /// </summary>
    public TimeSpan DisplayDateRange {
      get { return (TimeSpan)GetValue(DisplayDateRangeProperty); }
      set { SetValue(DisplayDateRangeProperty, value); }
    }
    TimeSpan displayDateRangeTracked;
    static readonly TimeSpan initDisplayDateRange = TimeSpan.MinValue;


    /// <summary>
    /// The DependencyProperty definition for DisplayDateRange property.
    /// </summary>
    public static readonly DependencyProperty DisplayDateRangeProperty =
          DependencyProperty.RegisterAttached(
        "DisplayDateRange", // Property name
        typeof(TimeSpan), // Property type
        typeof(LegendXDate), // Property owner
        new FrameworkPropertyMetadata(initDisplayDateRange, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Presently selected TimeSpanUnit, like years or minutes
    /// Default Value: 'none'
    /// </summary>
    public TimeSpanUnitEnum TimeSpanUnit {
      get { return (TimeSpanUnitEnum)GetValue(TimeSpanUnitProperty); }
      private set { SetValue(TimeSpanUnitProperty, value); }
    }

    /// <summary>
    /// The DependencyProperty definition for TimeSpanUnit property.
    /// </summary>
    public static readonly DependencyProperty TimeSpanUnitProperty =
      DependencyProperty.RegisterAttached(
        "TimeSpanUnit", // Property name
        typeof(TimeSpanUnitEnum), // Property type
        typeof(LegendXDate), // Property owner
        new FrameworkPropertyMetadata(TimeSpanUnitEnum.none));


    protected override void OnReset() {
      base.OnReset();

      DisplayDate = initDisplayDate;
      DisplayDateRange = initDisplayDateRange;
      displayDateTracked = initDisplayDate==DateTime.MaxValue ? DateTime.MinValue : DateTime.MaxValue;
      displayDateRangeTracked = initDisplayDateRange==TimeSpan.MaxValue ? TimeSpan.MinValue : TimeSpan.MaxValue;
      displayValueTracked = double.NaN;
      displayValueRangeTracked = double.NaN;
    }
    #endregion


    #region Measurement Overrides
    //      ---------------------

    readonly TimeSpanUnitFormatter timeSpanUnitFormatter = new TimeSpanUnitFormatter();


    protected override void OnFontChanged(bool hasOnlyFontSizeChanged) {
      timeSpanUnitFormatter.SetGlyphDrawer(LegendGlyphDrawer, FontSize);
    }
    #endregion


    #region Render Overrides
    //      ----------------

    protected override void OnProvideDefaultValues(out double displayValue, out double displayValueRange) {
TraceWpf.Line(">>>>> LegendXDate.OnProvideDefaultValues()");
      DisplayDate = DateTime.Now.AddDays(-7);
      displayValue = DisplayDate.ToDouble();
      DisplayDateRange = TimeSpan.FromDays(1);
      displayValueRange = DisplayDateRange.ToDouble();
    }


    double displayValueTracked = double.NaN;
    double displayValueRangeTracked = double.NaN;


    protected override bool OnIsRecalculationNeeded(Size renderContentSize) {
TraceWpf.Line(">>>>> LegendxDate.OnIsRecalculationNeeded()");
      //check first if DisplayValue has changed, which most likely comes from LegendScroller
      bool hasDisplayDateChanged = false;
      if (displayValueTracked!=DisplayValue) {
        displayValueTracked = DisplayValue;
        displayDateTracked = DisplayDate = DisplayValue.ToDateTime();
        hasDisplayDateChanged = true;
      }
      if (displayValueRangeTracked!=DisplayValueRange) {
        displayValueRangeTracked = DisplayValueRange;
        displayDateRangeTracked = DisplayDateRange = displayValueRangeTracked.ToTimeSpan();
        hasDisplayDateChanged = true;
      }
      if (!hasDisplayDateChanged) {
        //if DisplayValue (LegendScroller) hasn't changed, check if DisplayDate has been directly changed
        if (displayDateTracked!=DisplayDate) {
          displayDateTracked = DisplayDate;
          displayValueTracked =DisplayValue = displayDateTracked.ToDouble();
          hasDisplayDateChanged = true;
        }
        if (displayDateRangeTracked!=DisplayDateRange) {
          displayDateRangeTracked = DisplayDateRange;
          displayValueRangeTracked = DisplayValueRange = displayDateRangeTracked.ToDouble();
          hasDisplayDateChanged = true;
        }
      }

      return base.OnIsRecalculationNeeded(renderContentSize) || hasDisplayDateChanged;//OnIsRecalculationNeeded needs to come first to guarantee its execution
    }


    protected override void OnRecalculate(ref double[]? labelValues, ref string?[]? labelStrings, ref Point[]? labelPoints) {
      //choose time unit (seconds to years)
      TimeSpanUnitEnum newTimeSpanUnit = timeSpanUnitFormatter.GetDefaultTimeSpanUnit(DisplayDateRange, RenderWidthTracked);

      //check if even more labels can be displayed
      TimeSpan step = TimeSpan.MinValue;
      DateTime firstLabelDate = DateTime.MinValue;
      int labelsCount = int.MinValue;
      double spaceBetweenLabels = LegendGlyphDrawer.GetLength("    ", FontSize);
      while (true) {
        calculate(newTimeSpanUnit, out var newStep, out var newFirstLabelDate, out var newLabelsCount);
        TimeSpanUnitConfig timeSpanUnitConfig = timeSpanUnitFormatter.GetWidths(newTimeSpanUnit);
        double totalWidth = timeSpanUnitConfig.FirstLabelWidth;
        if (newLabelsCount>1) {
          totalWidth += (newLabelsCount - 1) * (timeSpanUnitConfig.FollowLabelWidth + spaceBetweenLabels) * 1.1;
        }
        if (totalWidth>RenderWidthTracked || newTimeSpanUnit<0) {
          if (labelsCount==int.MinValue) {
            //seems even the first estimated step is already too big. Just use the values
            step = newStep;
            firstLabelDate = newFirstLabelDate;
            labelsCount = newLabelsCount;
          }else{
            //take previous TimeSpanUnit
            newTimeSpanUnit += 1;
          }
          break;
        }
        step = newStep;
        firstLabelDate = newFirstLabelDate;
        labelsCount = newLabelsCount;
        newTimeSpanUnit -= 1;
      }
      TimeSpanUnit = newTimeSpanUnit;

      if (labelValues==null || labelValues.Length!=labelsCount) {
        labelValues = new double[labelsCount];
        labelStrings = new string[labelsCount];
        labelPoints = new Point[labelsCount];
      }

      //calculate labels
      double pixelPerValue = RenderWidthTracked / DisplayValueRange;
      TimeSpanUnitMask mask = newTimeSpanUnit.GetMask();
      double firstLabelWidth = double.MinValue;
      DateTime labelDate = firstLabelDate;
      for (int labelIndex = 0; labelIndex < labelsCount; labelIndex++) {
        //write label value
        labelValues[labelIndex] = labelDate.ToDouble();

        //calculate position
        double xPosition = (labelValues[labelIndex]-DisplayValue) * pixelPerValue;
#if DEBUG
        if (xPosition> RenderWidthTracked && labelsCount>1) {
          if (xPosition-RenderWidthTracked>0.0001) {//filter rounding problems
            //should not happen
            System.Diagnostics.Debugger.Break();
            throw new Exception();
          }
        }
#endif
        labelPoints![labelIndex] = new Point(xPosition, 0);

        //calculate label string
        if (labelIndex==1 && xPosition<firstLabelWidth) {
          //skip second label, it would overlap with first label
          labelStrings![labelIndex] = null;

        } else {
          string legendString;
          if (labelIndex==0) {
            legendString = labelDate.ToFirstLabel(newTimeSpanUnit);
          } else {
            legendString = labelDate.ToFollowLabel(newTimeSpanUnit);
          }
          labelStrings![labelIndex] = legendString;
          if (labelIndex==0) {
            firstLabelWidth = xPosition + LegendGlyphDrawer.GetLength(legendString, FontSize) * 1.05;
          }
        }
        
        //calculate next label
        if (newTimeSpanUnit>=TimeSpanUnitEnum.year) {
          int yearMultiplier = newTimeSpanUnit.GetYearMultiplier();
          labelDate = labelDate.AddYears(yearMultiplier);
        } else if (newTimeSpanUnit==TimeSpanUnitEnum.month) {
          labelDate = labelDate.AddMonths(1);
        } else {
          labelDate = labelDate.AddTicks(step.Ticks);
        }
      }
    }


    private void calculate(TimeSpanUnitEnum timeSpanUnit, out TimeSpan step, out DateTime firstLabelDate, out int labelsCount) {
      step = new TimeSpan(timeSpanUnit.GetTicks());
      //find start of first time unit after MinDisplayDate
      if (timeSpanUnit==TimeSpanUnitEnum.month) {
        firstLabelDate = DisplayDate.Date;
        if (firstLabelDate!=DisplayDate) {
          //day has already started. Take the next one for the first label
          firstLabelDate = firstLabelDate.Date.AddDays(1);
        }
        if (firstLabelDate.Day!=1) {
          if (firstLabelDate.Month<12) {
            firstLabelDate = new DateTime(firstLabelDate.Year, firstLabelDate.Month+1, 1);
          } else {
            firstLabelDate = new DateTime(firstLabelDate.Year+1, 1, 1);
          }
        }
      } else if (timeSpanUnit>=TimeSpanUnitEnum.year) {
        firstLabelDate = DisplayDate.Date;
        if (firstLabelDate!=DisplayDate) {
          //day has already started. Take the next one for the first label
          firstLabelDate = firstLabelDate.Date.AddDays(1);
        }
        if (firstLabelDate.Day!=1 || firstLabelDate.Month!=1) {
          firstLabelDate = new DateTime(firstLabelDate.Year+1, 1, 1);
        }
        if (timeSpanUnit>=TimeSpanUnitEnum.year5) {
          int yearMultiplier = timeSpanUnit.GetYearMultiplier();
          if (firstLabelDate.Year%yearMultiplier>0) {
            firstLabelDate = new DateTime(firstLabelDate.Year - firstLabelDate.Year%yearMultiplier + yearMultiplier, 1, 1); ;
          }
        }
      } else {
        // timeSpanUnit is a week or shorter
        firstLabelDate = new DateTime((DisplayDate.Ticks/step.Ticks) * step.Ticks);
        if (firstLabelDate<DisplayDate) {
          try {
          firstLabelDate = firstLabelDate.AddTicks(step.Ticks);
          } catch (Exception ex) {
            string s = ex.ToString();

          }
#if DEBUG
          if (firstLabelDate<DisplayDate) {
            System.Diagnostics.Debugger.Break();
            throw new Exception("firstLabelDate " + firstLabelDate + " should be greater than DisplayDate " + DisplayDate + ".");
          }
#endif
        }
      }

      DateTime maxDisplayDate = DisplayDate + DisplayDateRange;
      if (timeSpanUnit>=TimeSpanUnitEnum.year) {
        //a year or more
        int yearMultiplier = timeSpanUnit.GetYearMultiplier();
        int yearsDifference = maxDisplayDate.Year - firstLabelDate.Year;
        labelsCount = yearsDifference/yearMultiplier + 1;
        //lastLabelDate = firstLabelDate.AddYears(labelsCount);
        //labelsCount++;

      } else if (timeSpanUnit==TimeSpanUnitEnum.month) {
        labelsCount = maxDisplayDate.Month - firstLabelDate.Month + (maxDisplayDate.Year - firstLabelDate.Year)*12 + 1;
        //lastLabelDate = firstLabelDate.AddMonths(labelsCount);
        //labelsCount++;

      } else {
        //week or shorter. The number of ticks per TimeSpanUnit is fixed as opposed to months (28-31 days) or years (365/366 days)
        labelsCount =  (int)((maxDisplayDate.Ticks - firstLabelDate.Ticks) / step.Ticks) + 1;
      }
    }
    #endregion
  }
}
