/**************************************************************************************

XYGraphLib.Legend
=================

Base class for LegendX and LegendY

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using CustomControlBaseLib;


namespace XYGraphLib {

  /// <summary>
  /// Displays the legend next to a PlotArea. The legend has labels displaying the value a pixel at this location corresponds to. DisplayValue and 
  /// DisplayValueRange control the value range displayed. MinValue and MaxValue indicate the largest values the legend might have to display after scrolling. This
  /// information is needed to chose a number formating which is correct from Min- to MaxValue.</br>
  /// Legend is the abstract base class for LegendX (horizontal) and LegendY (vertical).
  /// </summary>
  public abstract class Legend: CustomControlBase {
    //
    //LegendX:
    //            +------------------------------------+
    // -100       | 100             150            200 |     500
    //            +------------------------------------+
    // :MinValue   :DisplayValue                    :        MaxValue
    //              DisplayValue + DisplayValueRange:
    // 
    //
    //LegendY:
    //    100'...MinValue
    //       '
    // +-----'+
    // |  200'| DisplayValue
    // |     '|
    // |  150'|
    // |     '|
    // |  100'| DisplayValue + DisplayValueRange
    // +-----'+
    //       '
    //   -100'..MaxValue

    #region Properties
    //      ----------

    /// <summary>
    /// A dataPoint has several values associated with it, one for every dimension. 0:x-axis, 1: y-axis, but there can be even more dimensions
    /// </summary>
    public readonly int Dimension;


    /// <summary>
    /// Lowest value stored in the data-records for the supported dimension, allows to have the same formatting when displayed values 
    /// are within Min- and MaxValue. If this functionality is not needed, set it to NaN. Infinity throws an exception
    ///  
    /// Default Value: double.Nan
    /// </summary>
    public double MinValue {
      get { return (double)GetValue(MinValueProperty); }
      set { SetValue(MinValueProperty, throwExceptionIfInfite(value)); }
    }
    double minValueTracked;
    const double minValueInit = double.NaN;


    private double throwExceptionIfInfite(double value) {
      if (double.IsInfinity(value)) {
        throw new Exception("Infinity is not supported in a legend.");
      }
      return value;
    }


    /// <summary>
    /// The DependencyProperty definition for MinValue.
    /// </summary>
    public static readonly DependencyProperty MinValueProperty =
      DependencyProperty.RegisterAttached(
        "MinValue", // Property name
        typeof(double), // Property type
        typeof(Legend), // Property owner
        new FrameworkPropertyMetadata(minValueInit, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Lowest value displayed in legend
    /// Default: double.MaxValue
    /// </summary>
    public double DisplayValue {
      get { return (double)GetValue(DisplayValueProperty); }
      set { SetValue(DisplayValueProperty, throwExceptionIfInfite(value)); }
    }
    double displayValueTracked;
    static readonly double displayValueInit = double.NaN;


    /// <summary>
    /// The DependencyProperty definition for DisplayValue property.
    /// </summary>
    public static readonly DependencyProperty DisplayValueProperty =
      DependencyProperty.RegisterAttached(
        "DisplayValue", // Property name
        typeof(double), // Property type
        typeof(Legend), // Property owner
        new FrameworkPropertyMetadata(displayValueInit, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Value range displayed in legend.
    /// Default: double.MinValue
    /// </summary>
    public double DisplayValueRange {
      get { return (double)GetValue(DisplayValueRangeProperty); }
      set { SetValue(DisplayValueRangeProperty, throwExceptionIfInfite(value)); }
    }
    double displayValueRangeTracked;
    static readonly double displayValueRangeInit = double.NaN;


    /// <summary>
    /// The DependencyProperty definition for DisplayValueRange property.
    /// </summary>
    public static readonly DependencyProperty DisplayValueRangeProperty =
      DependencyProperty.RegisterAttached(
        "DisplayValueRange", // Property name
        typeof(double), // Property type
        typeof(Legend), // Property owner
        // Changing DisplayValueRange can change the required width of an LegendY, for which a new Measurement() must be called.
        new FrameworkPropertyMetadata(displayValueRangeInit, FrameworkPropertyMetadataOptions.AffectsRender, valueChangeNeedsMeasurement));


    /// <summary>
    /// Highest value stored in the data-records for the supported dimension, allows to have the same formatting when displayed values 
    /// are within Min- and MaxValue.
    /// Default Value: double.MinValue
    /// </summary>
    public double MaxValue {
      get { return (double)GetValue(MaxValueProperty); }
      set { SetValue(MaxValueProperty, throwExceptionIfInfite(value)); }
    }
    double maxValueTracked;
    const double maxValueInit = double.NaN;


    /// <summary>
    /// The DependencyProperty definition for MaxValue.
    /// </summary>
    public static readonly DependencyProperty MaxValueProperty =
      DependencyProperty.RegisterAttached(
        "MaxValue", // Property name
        typeof(double), // Property type
        typeof(Legend), // Property owner
        new FrameworkPropertyMetadata(maxValueInit, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Set to true if changing Range should invoke Measurement(). This is needed for vertical LegendScrollers
    /// </summary>
    public readonly bool NeedsMeasureWhenValuesChange = false;


    private static void valueChangeNeedsMeasurement(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      Legend legend = (Legend)d;
      if (legend.NeedsMeasureWhenValuesChange) legend.InvalidateMeasure();
    }


    /// <summary>
    /// Set to true if legend should be written right aligned. This is used for vertically written numbers
    /// </summary>
    public readonly bool IsWriteRightAligned = false;


    /// <summary>
    /// coordinates of all value labels, used to draw grid lines
    /// Default Value: null
    /// </summary>
    public double[]? LabelValues {
      get { return (double[])GetValue(LabelValuesProperty); }
      private set { SetValue(LabelValuesProperty, value); }
    }
    const double[]? labelValuesInit = null;


    /// <summary>
    /// The DependencyProperty definition for LabelValues property.
    /// </summary>
    public static readonly DependencyProperty LabelValuesProperty = 
    DependencyProperty.Register(
      "LabelValues", // Property name 
      typeof(double[]), // Property type
      typeof(Legend), // Property owner
      new UIPropertyMetadata(labelValuesInit));


    /// <summary>
    /// True if MinValue, MaxValue or DisplayValueRange have changed and render is not executed yet. Inheritors can use it during 
    /// measurement and arrange.
    /// </summary>
    protected bool HasMinMaxOrDisplayValueRangeChanged { 
      get { return 
        DisplayValueRange!=displayValueRangeTracked || 
        (!double.IsNaN(minValueTracked) && minValueTracked!=MinValue) || 
        (!double.IsNaN(maxValueTracked) && maxValueTracked!=MaxValue); 
      } 
    }
    #endregion


    #region Public methods and events
    //      -------------------------

    /// <summary>
    /// Reset Legend properties and removes all event listeners from DisplayValueChanged.
    /// </summary>
    public void Reset() {
      if (DisplayValueChanged!=null) {
        foreach (Action<Legend> eventHandler in DisplayValueChanged.GetInvocationList()) {
          DisplayValueChanged -= eventHandler;
        }
      }

      //The original and the tracked value get assigned NaN. We use the strange behavior of doubles that
      //comparing 2 NaN-values is always false, even they have actually the same value. In the code, we 
      //check first if the original value (DisplayVale. FidplayValueRange) is NaN. If it is, we use some 
      //default value. When we later check original against tracked, we will always detect a change as long
      //the tracked value is NaN. After detecting the change for the first time, the tracked value gets
      //the original value assigned.
#if DEBUG
      if (!double.IsNaN(minValueInit) || !double.IsNaN(minValueInit) || !double.IsNaN(minValueInit) || !double.IsNaN(minValueInit)) {
        throw new Exception("code needs to be rewritten if the Init values are not NaN.");
      }
#endif
      minValueTracked = MinValue = minValueInit;
      displayValueTracked = DisplayValue = displayValueInit;
      displayValueRangeTracked = DisplayValueRange = displayValueRangeInit;
      maxValueTracked = MaxValue = maxValueInit;

      /*code version if init values are double.Max-/MinValues
        -----------------------------------------------------
        MinValue = minValueInit;
        minValueTracked = minValueInit==double.MaxValue ? double.MinValue : double.MaxValue;
        DisplayValue = displayValueInit;
        displayValueTracked = displayValueInit==double.MaxValue ? double.MinValue : double.MaxValue;
        DisplayValueRange = displayValueRangeInit;
        displayValueRangeTracked = displayValueRangeInit==double.MaxValue ? double.MinValue : double.MaxValue;
        MaxValue = maxValueInit;
        maxValueTracked = maxValueInit==double.MaxValue ? double.MinValue : double.MaxValue;
      */
      LabelValues = labelValuesInit;

      resetLocalData();
      OnReset();
    }


    protected virtual void OnReset() {
    }


    /// <summary>
    /// Raised when DisplayValue or DisplayValue Range has changed, but only once new legend values are calculated during
    /// rendering
    /// </summary>
    public event Action<Legend>? DisplayValueChanged;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor. newNeedsMeasureWhenValuesChange should be true when changing Min, 
    /// Max or DisplayRange should invoke Measure(), which is needed for vertical legends.
    /// </summary>
    public Legend(int Dimension, bool newNeedsMeasureWhenValuesChange = false, bool isWriteRightAligned = false) {
      this.Dimension = Dimension;
      NeedsMeasureWhenValuesChange = newNeedsMeasureWhenValuesChange;
      IsWriteRightAligned = isWriteRightAligned;

      ClipToBounds = true;

      Reset();
    }
    #endregion


    #region Arrange Overrides
    //      -----------------

    sealed protected override Size ArrangeContentOverride(Rect arrangeRect) {
      return OnLegendArrange(arrangeRect);
    }


    /// <summary>
    /// During the arrangement of the legend, inheritor should return arranged size
    /// </summary>
    protected virtual Size OnLegendArrange(Rect arrangeRect) {
      return arrangeRect.Size;
    }
    #endregion


    #region Measurement Overrides
    //      ---------------------

    [AllowNull]
    protected GlyphDrawer LegendGlyphDrawer = null;
    FontFamily? fontFamilyTracked = null; //there is no need to reset font tracking
    double fontSizeTracked = double.NaN;
    bool isNewGlyphDrawer = false;


    sealed protected override Size MeasureContentOverride(Size availableSize) {
      if (double.IsNaN(DisplayValue) || double.IsNaN(DisplayValueRange)) {
        //Replace default data with some data which can be displayed.
        OnProvideDefaultValues(out var displayValue, out var displayValueRange);
        DisplayValue = displayValue;
        DisplayValueRange = displayValueRange;
      }

      //ensure that DisplayValue, DisplayValueRange, MinValue and MaxValue all have legal values
      if (double.IsInfinity(DisplayValue)) throw new Exception("DisplayValue " + DisplayValue + " cannot be infinite.");

      if (double.IsInfinity(DisplayValueRange)) throw new Exception("DisplayValueRange " + DisplayValueRange + " cannot be infinite.");

      if (DisplayValueRange < double.Epsilon*1e+9) throw new ApplicationException("DisplayValueRange " + DisplayValueRange + " must be positive.");

      if (double.IsNaN(MinValue)!=double.IsNaN(MaxValue)) throw new ApplicationException("MinValue " + MinValue + " and MaxValue " + MaxValue + " must be both NaN or not.");

      if (!double.IsNaN(MinValue)) {
        //min and max values are defined. Ensure that Max>Min and they are bigger or equal DisplayValue and DisplayValueRange
        if (MaxValue<MinValue) {
          throw new ApplicationException("MaxValue " + MaxValue + " must be greater than MinValue " + MinValue + " .");
        }
        if (MinValue>DisplayValue) {
          MinValue = DisplayValue;
        }
        if (MaxValue<DisplayValue + DisplayValueRange) {
          MaxValue = DisplayValue + DisplayValueRange;
        }
      }

      Size requiredSize = availableSize;
      if (double.IsInfinity(requiredSize.Width)) {
        //evaluate a minimum value if host gives unlimited space. The size requested as indicated in the return value of 
        //MeasureContentOverride() can be changed in OnLegendMeasurement(() by an inheritor
        //
        //Normally, the legend should be in a container telling the legend the available space. If it is unlimited (Scroll Region or host
        //wants Legend to take all available space), arrange will later provide the space available, even requiredSize was 0. Another 
        //reason for unlimited space is that Grid sometimes makes 2 measurement calls, in which the first one has infinite space to get 
        //the requested size, followed by second measurement call with the available size, after processing the measurement information
        //from other GridCells

        if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) {
          //in Visual Studio require some width to display something
          requiredSize.Width = 200;
        } else {
          //normally, the legend should be in a container telling the legend the available width. If it is unlimited (Scroll Container),
          //arrange will return all width available, even requiredSize.Width is 0. 
          requiredSize.Width = 0;
        }
      }
      //do the same for required Height
      if (double.IsInfinity(requiredSize.Height)) {
        if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) {
          requiredSize.Height = 200;
        } else {
          requiredSize.Height = 0;
        }
      }

      if (double.IsNaN(FontSize) ) {
        throw new Exception("FontSize cannot be NaN.");
      }
      if (FontFamily==null) {
        throw new Exception("FontFamily cannot be null.");
      }

      if (fontFamilyTracked!=FontFamily || fontSizeTracked!=FontSize) {
        bool hasOnlyFontSizeChanged = fontFamilyTracked==FontFamily && fontSizeTracked!=FontSize;
        fontFamilyTracked = FontFamily;
        fontSizeTracked = FontSize;

        if (FontSize<1 || FontSize>200) {
          throw new Exception("FontSize: " + FontSize + " should be between 1 and 200.");
        }

        //font might have different dimension than before. Recalculate space needed
        isNewGlyphDrawer = true;
        LegendGlyphDrawer = new GlyphDrawer(FontFamily, FontStyle, FontWeight, FontStretch, VisualTreeHelper.GetDpi(this).PixelsPerDip);
        OnFontChanged(hasOnlyFontSizeChanged);
      }

      return OnLegendMeasurement(requiredSize);
    }


    /// <summary>
    /// called when FontFamily or FontSize has been detected during legend measurement
    /// </summary>
    protected virtual void OnFontChanged(bool hasOnlyFontSizeChanged) {
    }


    /// <summary>
    /// During the measurement of the legend, inheritor should update requiredSize. If the available space given by the host is
    /// indefinite, Legend will replace it with 0. Inheritor should return a different size if it can be calculated
    /// how much is needed in the minimum
    /// </summary>
    protected virtual Size OnLegendMeasurement(Size requiredSize) {
      return requiredSize;
    }
    #endregion


    #region Render Overrides
    //      ----------------

    string?[]? labelStrings;
    Point[]? labelPoints;


    private void resetLocalData() {
      labelStrings = null;
    }


    sealed protected override void OnRenderContent(DrawingContext drawingContext, Size renderContentSize) {
      if (double.IsNaN(DisplayValueRange) || DisplayValueRange<0) throw new ApplicationException("DisplayValueRange " + DisplayValueRange + " must be a positive number.");

      bool haveDisplayValuesChanged = false;
      //OnIsRecalculationNeeded must be first to guarantee that it gets called
      if (OnIsRecalculationNeeded(renderContentSize) || displayValueTracked!=DisplayValue || displayValueRangeTracked!=DisplayValueRange || 
        (!double.IsNaN(MinValue) && minValueTracked!=MinValue) || (!double.IsNaN(MaxValue) && maxValueTracked!=MaxValue) ||  isNewGlyphDrawer) 
      {
        displayValueTracked = DisplayValue;
        displayValueRangeTracked = DisplayValueRange;
        minValueTracked = MinValue;
        maxValueTracked = MaxValue;
        isNewGlyphDrawer = false;
        haveDisplayValuesChanged = true;

        double[]? labelValues = LabelValues;
        OnRecalculate(ref labelValues, ref labelStrings, ref labelPoints);
        LabelValues = labelValues;
      }

      if (labelStrings!=null) {
        Point offset = OnContentAlignment(renderContentSize);
        for (int labelIndex = 0; labelIndex < labelStrings.Length; labelIndex++) {
          var labelString = labelStrings[labelIndex];
          if (labelString is null) continue;

          Point labelPoint = labelPoints![labelIndex];
          Point labelPointWithOffset = new Point(offset.X + labelPoint.X, offset.Y +labelPoint.Y);
          if (IsWriteRightAligned) {
            LegendGlyphDrawer.WriteRightAligned(drawingContext, labelPointWithOffset, labelString, FontSize, Foreground);
          } else {
            LegendGlyphDrawer.Write(drawingContext, labelPointWithOffset, labelString, FontSize, Foreground);
          }
        }

        if (haveDisplayValuesChanged && DisplayValueChanged!=null) {
          DisplayValueChanged(this);
        }
      }
    }


    const int round6Digits = 6;
    const double roundingError = 0.0000001;


    /// <summary>
    /// Calculates first and last label in legend and how many labels are needed. There is always at least 1 label
    /// </summary>
    protected void FindFirstLastLabel(
      [NotNull]ref double[]? labelValues, 
      [NotNull]ref string?[]? labelStrings, 
      [NotNull]ref Point[]? labelPoints, 
      StepStruct step, 
      out double firstLabel) 
    {
      //find value of first label
//      firstLabel = ((int)(DisplayValue/step.Value)) * step.Value;
      firstLabel = (Math.Ceiling(DisplayValue/step.Value)) * step.Value;
      if (firstLabel<DisplayValue) {
#if DEBUG
        if ((DisplayValue-firstLabel)/DisplayValueRange<roundingError) {
          //this is most likely a rounding problem due to double arithmetic
          //move first label to DisplayValue so that it gets displayed. This is important, otherwise the first label will be missing
          firstLabel = DisplayValue;
        } else {
          //example: DisplayValue:6, FirstLabel: 5. FirstLabel would come before first value. Take next label
          firstLabel += step.Value;
          if (firstLabel<DisplayValue) {
            System.Diagnostics.Debugger.Break();
            throw new Exception("firstLabel " + firstLabel + " should be greater than DisplayValue " + DisplayValue + ".");
          }
        }
#else 
          firstLabel += step.Value;
#endif
      }
      //find value of last label
      double maxDisplayValue = DisplayValue + DisplayValueRange;
//      double lastLabel = ((int)Math.Round(maxDisplayValue/step.Value, round6Digits)) * step.Value;
      double lastLabel = (Math.Floor(Math.Round(maxDisplayValue/step.Value, round6Digits))) * step.Value;
      //doubles have problems representing decimal point numbers correctly. Example:
      //(3*0.2)- 0.6 = 0.00000000000000011102230246251565
      //meaning (3*0.2) != 0.6 !!!
      //
      //double lastLabel = Math.Round(((int)Math.Round(maxDisplayValue/step.Value, round6Digits)) * step.Value, -step.Exponent-1);
      if (lastLabel>maxDisplayValue) {
#if DEBUG
        if ((lastLabel-maxDisplayValue)/DisplayValueRange<roundingError) {
          //this is most likely a rounding problem due to double arithmetic
          //even the values are slightly different, just leave them. Last grid line will not be shown, but would anyway hardly be visible
        } else {
          lastLabel -= step.Value;
          if (lastLabel>maxDisplayValue) {
            System.Diagnostics.Debugger.Break();
            throw new Exception("lastLabel " + lastLabel + " should be smaller than maxDisplayValue " + maxDisplayValue + ".");
          }
        }
#else 
          lastLabel -= step.Value;
#endif
      }

      //calculate number of labels needed
      //round to 6 digits. It is impossible that more than 1 million label need to be displayed in 1 window.
      int labelsCount =  (int)(Math.Round((lastLabel-firstLabel) / step.Value, round6Digits)) + 1;
      if (labelValues==null || labelValues.Length!=labelsCount) {
        labelValues = new double[labelsCount];
        labelStrings = new string[labelsCount];
        labelPoints = new Point[labelsCount];
      }
    }


    protected virtual void OnProvideDefaultValues(out double displayValue, out double displayValueRange) {
      displayValue = 0;
      displayValueRange = 10;
    }


    protected virtual bool OnIsRecalculationNeeded(Size renderContentSize) { 
      return false; 
    }


    /// <summary>
    /// Legend calls OnRecalculate before OnLegendMeasurement() if one of the following changed:       
    /// OnIsRecalculationNeeded(), DisplayValue, DisplayValueRange, MinValue, MaxValue or GlyphDrawer 
    /// Inheritor should recalculate the label values
    /// </summary>
    protected abstract void OnRecalculate(ref double[]? labelValues, ref string?[]? labelStrings, ref Point[]? labelPoints);


    protected virtual Point OnContentAlignment(Size renderContentSize) {
      return new Point(0, 0);
    }
    #endregion
  }
}