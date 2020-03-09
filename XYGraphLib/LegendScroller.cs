/**************************************************************************************

XYGraphLib.LegendScroller
=========================

Base class for LegendScrollerX and LegendScrollerY

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
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using CustomControlBaseLib;


namespace XYGraphLib {
  /// <summary>
  /// Displays an x axis legend (time), scrollbar and 2 zoom-buttons. It can be used to select which data-samples should be displayed in a 
  /// Renderer and to display the dates. The user choses with the scrollbar which is the first sample to display (DisplayIndex) and with 
  /// the zoom buttons how many samples should be displayed (DisplayRangeIndex). LegendScroller calculates DisplayDate and DisplayDateRange 
  /// based on MinDate and MaxDate and sets the Legend accordingly.
  /// </summary>
  /// <remarks>
  ///              +---------------------+
  ///              |1.1.2000   1.2.2000  |  Legend
  ///              +-+-----------------+-+
  ///              |-|      ####       |+|
  ///              +-+-----------------+-+
  ///       Zoom Out^     Scrollbar     ^Zoom In Button
  ///              :                     :
  /// 1.1.2000     1.2.2000   1.3.2000   1.4.2000   1.5.2000   1.6.2000
  /// MinDate      DisplayDate           MaxDisplayDate        MaxDate
  /// 0            DisplayIndex                                MaxIndex
  ///              <--------------------->
  ///                 DisplayRangeIndex
  /// 
  /// LegendScroller informs Renderers by raising the DisplayIndexRangeChanged event which data needs to be displayed
  /// </remarks>
  public abstract class LegendScroller: CustomControlBase, IZoom {

    #region Properties
    //      ----------

    /// <summary>
    /// A dataPoint has several values associated with it, one for every dimension. 0:x-axis, 1: y-axis, but there can be even more dimensions
    /// </summary>
    public readonly int Dimension;


    /// <summary>
    /// Lowest value stored in the data-records
    /// Default: double.MaxValue
    /// </summary>
    public double MinValue {
      get { return (double)GetValue(MinValueProperty);}
      set { SetValue(MinValueProperty, value); }
    }
    double minValueTracked;
    const double minValueInit = double.MaxValue;


    /// <summary>
    /// The DependencyProperty definition for MinValue property.
    /// </summary>
    public static readonly DependencyProperty MinValueProperty =
          DependencyProperty.RegisterAttached(
        "MinValue", // Property name
        typeof(double), // Property type
        typeof(LegendScroller), // Property owner
        new FrameworkPropertyMetadata(minValueInit, FrameworkPropertyMetadataOptions.AffectsRender, valueChangeNeedsMeasurement));


    /// <summary>
    /// Set to true if changing Min, Range or Max should invoke Measurement(). This is needed for vertical LegendScrollers
    /// </summary>
    public readonly bool NeedsMeasureWhenValuesChange = false;


    private static void valueChangeNeedsMeasurement(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      LegendScroller legendScroller = (LegendScroller)d;
      if (legendScroller.NeedsMeasureWhenValuesChange) legendScroller.InvalidateMeasure();
    }


    /// <summary>
    /// Lowest value displayed in legend
    /// Default: double.MaxValue
    /// </summary>
    public double DisplayValue {
      get { return (double)GetValue(DisplayValueProperty); }
      set { SetValue(DisplayValueProperty, value); }
    }
    double displayValueTracked;
    const double displayValueInit = double.MaxValue;


    /// <summary>
    /// The DependencyProperty definition for DisplayValue.
    /// </summary>
    public static readonly DependencyProperty DisplayValueProperty =
      DependencyProperty.RegisterAttached(
        "DisplayValue", // Property name
        typeof(double), // Property type
        typeof(LegendScroller), // Property owner
        new FrameworkPropertyMetadata(displayValueInit, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Value range displayed in legend.
    /// Default: double.MinValue
    /// </summary>
    public double DisplayValueRange {
      get { return (double)GetValue(DisplayValueRangeProperty); }
      set { SetValue(DisplayValueRangeProperty, value); }
    }
    double displayValueRangeTracked;
    const double displayValueRangeInit = double.MinValue;


    /// <summary>
    /// The DependencyProperty definition for DisplayValueRange.
    /// </summary>
    public static readonly DependencyProperty DisplayValueRangeProperty =
      DependencyProperty.RegisterAttached(
        "DisplayValueRange", // Property name
        typeof(double), // Property type
        typeof(LegendScroller), // Property owner
        new FrameworkPropertyMetadata(displayValueRangeInit, FrameworkPropertyMetadataOptions.AffectsRender, valueChangeNeedsMeasurement));


    /// <summary>
    /// Highest value stored in the data-records
    /// Default: double.MaxValue
    /// </summary>
    public double MaxValue {
      get { return (double)GetValue(MaxValueProperty); }
      set { SetValue(MaxValueProperty, value); }
    }
    double maxValueTracked;
    const double maxValueInit = double.MinValue;


    /// <summary>
    /// The DependencyProperty definition for MaxValue.
    /// </summary>
    public static readonly DependencyProperty MaxValueProperty =
      DependencyProperty.RegisterAttached(
        "MaxValue", // Property name
        typeof(double), // Property type
        typeof(LegendScroller), // Property owner
        new FrameworkPropertyMetadata(maxValueInit, FrameworkPropertyMetadataOptions.AffectsRender, valueChangeNeedsMeasurement));


    /// <summary>
    /// Factor used for zooming
    /// Default: Math.Sqrt(10), zooming twice zooms by factor of 10
    /// </summary>
    public double ZoomFactor {
      get { return (double)GetValue(ZoomFactorProperty); }
      set { SetValue(ZoomFactorProperty, value); }
    }


    /// <summary>
    /// The DependencyProperty definition for ZoomFactor.
    /// </summary>
    public static readonly DependencyProperty ZoomFactorProperty =
      DependencyProperty.RegisterAttached(
        "ZoomFactor", // Property name
        typeof(double), // Property type
        typeof(LegendScroller), // Property owner
        new FrameworkPropertyMetadata(Math.Sqrt(10), FrameworkPropertyMetadataOptions.AffectsRender, valueChangeNeedsMeasurement));


    /// <summary>
    /// Legend
    /// </summary>
    public Legend Legend {
      get { return (Legend)GetValue(LegendProperty); }
      private set { SetValue(LegendProperty, value); }
    }
    readonly Legend legend;


    /// <summary>
    /// The DependencyProperty definition for Legend property.
    /// </summary>
    public static readonly DependencyProperty LegendProperty =
      DependencyProperty.RegisterAttached(
        "Legend", // Property name
        typeof(Legend), // Property type
        typeof(LegendScroller));


    /// <summary>
    /// Zoom can zoom in until highest resolution is reached. In theory this could be the precision of double, but
    /// in practice it is better to restrict zooming in to a factor like 1 million
    /// </summary>
    public bool CanZoomIn { get; private set; }


    /// <summary>
    /// Maximum factor for zooming in. 
    /// </summary>
    public double ZoomInLimit = 1000000;


    /// <summary>
    /// Zoom can zoom out when only part of the data is displayed
    /// </summary>
    public bool CanZoomOut { get; private set; }


    private void updateZoomState(bool newCanZoomIn, bool newCanZoomOut) {
      bool hasZoomStateChanged = false;
      if (CanZoomIn!=newCanZoomIn) {
        CanZoomIn = newCanZoomIn;
        hasZoomStateChanged = true;
        ZoomInButton.IsEnabled = newCanZoomIn;
      }
      if (CanZoomOut!=newCanZoomOut) {
        CanZoomOut = newCanZoomOut;
        hasZoomStateChanged = true;
        ZoomOutButton.IsEnabled = newCanZoomOut;
      }
      if (hasZoomStateChanged && ZoomStateChanged!=null) {
        ZoomStateChanged(this);
      }
    }


    /// <summary>
    /// Raised when IsZoomActive changes
    /// </summary>
    public event Action<IZoom>? ZoomStateChanged;


    /// <summary>
    /// Provides access to a ZoomButton, which the Charts use to give their zoom buttons the same dimensions
    /// </summary>
    public Button ZoomButton {
      get { return ZoomInButton; }
    }
    #endregion


    #region Public Methods
    //      --------------

    /// <summary>
    /// Shrinks the value range displayed by square root of 10. Zooming in twice makes the range 10 times smaller.
    /// </summary>
    public void ZoomIn() {
      if (CanZoomIn) {
        DisplayValueRange /= ZoomFactor;
      }
    }


    /// <summary>
    /// Shrinks the value range displayed by square root of 10. Zooming out twice makes the range 10 times bigger. Once 
    /// all data-records are displayed, no further zooming out is possible.
    /// </summary>
    public void ZoomOut() {
      if (CanZoomOut) {
        DisplayValueRange *= ZoomFactor;
      }
    }


    /// <summary>
    /// Zooms out as much as possible, i.e. shows all data
    /// </summary>
    public void ZoomReset() {
      if (CanZoomOut) {
        DisplayValue = MinValue;
        DisplayValueRange = MaxValue;
      }
    }


    bool areNewMinMax;

    
    /// <summary>
    /// Removes all Renderers from LegendScroller and reinitialises data
    /// </summary>
    public void Reset() {
      areNewMinMax = true;
      MinValue = minValueInit;
      minValueTracked = minValueInit==double.MaxValue ? double.MinValue : double.MaxValue;
      DisplayValue = displayValueInit;
      displayValueTracked = displayValueInit==double.MaxValue ? double.MinValue : double.MaxValue;
      DisplayValueRange = displayValueRangeInit;
      displayValueRangeTracked = displayValueRangeInit==double.MaxValue ? double.MinValue : double.MaxValue;
      MaxValue = maxValueInit;
      maxValueTracked = maxValueInit==double.MaxValue ? double.MinValue : double.MaxValue;

      OnReset();
      if (legend!=null) {
        legend.Reset();
      }
    }


    protected virtual void OnReset() {
    }
    #endregion


    #region Constructor
    //      -----------

    protected ZoomButton ZoomInButton;
    protected ScrollBar ScrollBar;
    protected ZoomButton ZoomOutButton;


    /// <summary>
    /// Constructor using externally provided Legend. NeedsMeasureWhenValuesChange should be true when changing Min, 
    /// Max or DisplayRange should invoke Measure().
    /// </summary>
    public LegendScroller(Legend newLegend) {
      Reset(); //resetting before adding the legend prevents legend.Reset() to be called here and in the legend on initialisation

      legend = Legend = newLegend;
      legend.Background = Brushes.WhiteSmoke;
      legend.BorderBrush = Brushes.Gray;
      legend.BorderThickness = new Thickness(1);
      NeedsMeasureWhenValuesChange = legend.NeedsMeasureWhenValuesChange;
      AddChild(legend);
      Dimension = legend.Dimension;

      ZoomInButton = new ZoomButton();
      ZoomInButton.Click += zoomInButton_Click;
      AddChild(ZoomInButton);

      ZoomOutButton = new ZoomButton { IsZoomIn=false };
      ZoomOutButton.IsEnabled = false;
      ZoomOutButton.Click += zoomOutButton_Click;
      AddChild(ZoomOutButton);

      ScrollBar = new ScrollBar { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Stretch };
      ScrollBar.ValueChanged += scrollBar_ValueChanged;
      AddChild(ScrollBar);

      OnButtonScrollbarCreated();
    }


    /// <summary>
    /// inheritor should set docking-position for the zoom buttons
    /// </summary>
    protected abstract void OnButtonScrollbarCreated();


    /// <summary>
    /// Installs event handler for Renderer's DisplayRangeChanged and updates its own MinDate and MaxDate if the Renderer is a
    /// RendererDataSeries and if the values from the Renderer are outside the present range.
    /// </summary>
    public void AddRenderer(Renderer renderer) {
      Legend.DisplayValueChanged += renderer.DisplayValueChanged;
      if (renderer is RendererDataSeries rendererDataSeries) {
        if (MinValue > rendererDataSeries.MinValues[Dimension]) {
          MinValue = rendererDataSeries.MinValues[Dimension];
          TraceWpf.Line(">>>>> LegendScroller.AddRenderer(): MinValue: " + MinValue.ToDateTime());
        }
        if (MaxValue < rendererDataSeries.MaxValues[Dimension]) {
          MaxValue = rendererDataSeries.MaxValues[Dimension];
          TraceWpf.Line(">>>>> LegendScroller.AddRenderer(): MaxValue: " + MaxValue.ToDateTime());
        }
      }
    }
    #endregion


    #region Events
    //      ------

    //Scrollbar event execution is stopped while arrange changes index values
    bool isArranging = false; // prevents that scrollbar event does anything when it gets raised due to changed during Arrange()
    //    bool isScrollBarEvent = false; // prevents properties from activating Arrange() when they get changed by the scrollbar


    void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
      if (isArranging) return;// scrollbar gets updated by Arrange(). There is no need the copy the scrollbar values back to properties

      //      isScrollBarEvent = true;
      // copy ScrollbarValues to properties
      if (ScrollBar.Orientation==Orientation.Horizontal) {
        DisplayValue = ScrollBar.Value;
      } else {
        DisplayValue = ScrollBar.Minimum + ScrollBar.Maximum - ScrollBar.Value;
      }
      DisplayValueRange = ScrollBar.ViewportSize;
      //      isScrollBarEvent = false;
    }


    void zoomInButton_Click(object sender, RoutedEventArgs e) {
      ZoomIn();
    }


    void zoomOutButton_Click(object sender, RoutedEventArgs e) {
      ZoomOut();
    }
    #endregion


    #region Local Methods
    //      -------------

    const double largeSmallChangeRatio = 10;


    bool isExceptionThrown = false; //prevents throwing exceptions repeatedly


    /// <summary>
    /// Ensures that the various Value properties are consistent. 
    /// </summary>
    protected void ValidateValues() {
    }


    /// <summary>
    /// Updates the scrollbar to their values. Inheritors should call 
    /// CalculateScrollBarValues latest during Arrange()
    /// </summary>
    protected void CalculateScrollBarValues() {
      if (double.IsInfinity(MinValue) || double.IsInfinity(MaxValue) || double.IsInfinity(DisplayValue) || double.IsInfinity(DisplayValueRange)) {
        if (isExceptionThrown) return;

        isExceptionThrown = true;
        throw new ApplicationException("Infinity is not supported: MinValue " + MinValue + ", MaxValue " + MaxValue + 
          ", DisplayValue " + DisplayValue + ", DisplayRange " + DisplayValueRange + ".");
      }

      //if max value range is not set (yet), use some default values to display something, in case it remains empty. It helps the
      //user to understand the GUI better, if he can see the legend even he has no data selected yet to be displayed
      if (MinValue==minValueInit || MaxValue==maxValueInit) {
        //there are no values. Use some default values
        MinValue = 0;
        MaxValue = 10;
      }

      double maxValueRange = MaxValue - MinValue;
      if (maxValueRange<0) {
        if (isExceptionThrown) return;

        isExceptionThrown = true;
        throw new ApplicationException("MaxValue " + MaxValue + " - MinValue " + MinValue + " should be greater 0, but was " + maxValueRange + ".");
      }
      if (maxValueRange==0) {
        //Min and Max are the same. This happens when the graphic displays only 1 record. In this case, the
        //range must be increased, otherwise nothing would get displayed. 
        if (MinValue==0) {
          MinValue = -1;
          MaxValue = 1;
        } else if (MinValue>0) {
          MaxValue = MinValue * 2;
          MinValue = 0;
        } else {
          MinValue *= 2;
          MaxValue = 0;
        }
        maxValueRange = MaxValue - MinValue;
      }

      //ensure that DisplayValue and DisplayValueRange define a range between MinValue and MaxValue, otherwise
      //correct them accordingly. This correction is done without error message, since scrolling and zooming can
      //lead to illegal values
      if (DisplayValue==displayValueInit || DisplayValueRange==displayValueRangeInit || DisplayValueRange<=0) {
        DisplayValue = MinValue;
        DisplayValueRange = maxValueRange;
      }
      if (DisplayValue<MinValue) {
        DisplayValue = MinValue;
      }
      if (DisplayValueRange>=(maxValueRange)*0.99) {
        //when nearly everything needs to be displayed, display everything
        DisplayValueRange = maxValueRange;
      }
      bool canZoomIn;
      bool canZoomOut;
      if (DisplayValueRange>=maxValueRange) {
        DisplayValueRange = maxValueRange;
        canZoomOut = false;
        canZoomIn = true;
      } else {
        canZoomOut = true;
        if (DisplayValueRange<(maxValueRange / ZoomInLimit)) {
          //limit zooming in to 1 million times
          DisplayValueRange = maxValueRange / ZoomInLimit;
          canZoomIn = false;
        } else {
          canZoomIn = true;
        }
      }
      if (DisplayValue+DisplayValueRange>MaxValue) {
        DisplayValue = MaxValue - DisplayValueRange;
      }
      updateZoomState(canZoomIn, canZoomOut);

      isExceptionThrown = false;

      if (!areNewMinMax && minValueTracked==MinValue && displayValueTracked==DisplayValue && displayValueRangeTracked==DisplayValueRange && 
        maxValueTracked==MaxValue) return; //nothing to do

      //update the tracked values, they might have changed
      minValueTracked = MinValue;
      displayValueTracked = DisplayValue;
      displayValueRangeTracked = DisplayValueRange;
      maxValueTracked = MaxValue;

      //set scrollbar values
      isArranging = true;
      ScrollBar.Minimum = MinValue;
      ScrollBar.Maximum =  MaxValue - DisplayValueRange;
      if (ScrollBar.Orientation==Orientation.Horizontal) {
        ScrollBar.Value = DisplayValue;
      } else {
        ScrollBar.Value = ScrollBar.Minimum + ScrollBar.Maximum - DisplayValue;
      }
      double largeChange = DisplayValueRange;
      if (ScrollBar.LargeChange!=largeChange) {
        ScrollBar.LargeChange = largeChange;
        ScrollBar.SmallChange =  largeChange / largeSmallChangeRatio;
        ScrollBar.ViewportSize = largeChange;
      }
      isArranging = false;
      areNewMinMax = false;
TraceWpf.Line(">>>>> LegendScroller.CalculateScrollBarValues(): Copy to legend.MinValue");
      legend.MinValue = minValueTracked;
      legend.DisplayValue = displayValueTracked;
      legend.DisplayValueRange = displayValueRangeTracked;
      legend.MaxValue = maxValueTracked;
    }


    #endregion
  }
}
