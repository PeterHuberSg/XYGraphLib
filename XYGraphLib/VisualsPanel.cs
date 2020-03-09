/**************************************************************************************

XYGraphLib.VisualsPanel
=======================

A FrameworkElement which can host DrawingVisuals  

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System;


namespace XYGraphLib {

  /// <summary>
  /// VisualsPanel is a FrameworkElement which can host DrawingVisuals. This allows for very fast Graphics without the overhead of
  /// Templates, WPF Layout, gazillions of Dependency Properties and events.
  /// VisualsPanel stores DrawingVisuals in a VisualsCollection, which WPF uses to draw the VisualsPanel and its content. Just adding or removing
  /// a DrawingVisuals will cause WPF to render VisualsPanel again.
  /// 
  /// VisualsPanel can have a simple border (no rounded corners), Padding and Background. It can be used as is or more often by inheriting classes 
  /// wanting to render directly using DrawingVisuals, which is not supported by plain FrameworkElements.
  /// DrawingVisual.Offest must be zero. VisualsPanel uses the DrawingVisual's Offset property to position it inside of Border and Padding while
  /// executing ArrangeOverride. Inheritors should do their layouts in MeasureVisualsOverride and ArrangeVisualsOverride.
  /// 
  /// VisualsPanel is a FrameworkElement and does not inherit from Panel ! Panel in its name just indicates that it has similar functionality like 
  /// a Panel. A Panel supports UIElements as children, while VisualsPanel supports DrawingVisuals.
  /// 
  /// 
  /// </summary>
  public class VisualsPanel: FrameworkElement {

    #region Properties
    //      ----------

    /// <summary>
    ///   The DependencyProperty for the BorderBrush property.
    /// </summary>
    public static readonly DependencyProperty BorderBrushProperty = Border.BorderBrushProperty.AddOwner(typeof(VisualsPanel),
      new FrameworkPropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    ///  Brush used to draw a border around VisualsPanel if BorderPenThickness>0.
    ///  Default Value: Brushes.Transparent
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush BorderBrush {
      get { return (Brush)GetValue(BorderBrushProperty); }
      set { SetValue(BorderBrushProperty, value); }
    }


    /// <summary>
    ///   The DependencyProperty for the BorderPenThickness property.
    /// </summary>
    public static readonly DependencyProperty BorderPenThicknessProperty= DependencyProperty.Register(
      "BorderPenThickness", typeof(double), typeof(VisualsPanel),
      new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, borderPenThickness_Changed));


    private static void borderPenThickness_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      double thickness = (double)e.NewValue;
      if (double.IsNaN(thickness) || double.IsInfinity(thickness) || thickness < 0.0){
        throw new Exception("Thickness " + thickness + " must be greater equal 0 and cannot be infinite.");
      }
    }


    /// <summary>
    ///   Thickness of border drawn around VisualsPanel. Must be greater than or equal 0.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public double BorderPenThickness {
      get { return (double)GetValue(BorderPenThicknessProperty); }
      set { SetValue(BorderPenThicknessProperty, value); }
    }


    static readonly Thickness thickness0 = new Thickness(0);

    /// <summary>
    /// PaddingProperty
    /// </summary>
    public static readonly DependencyProperty PaddingProperty= DependencyProperty.Register("Padding", typeof(Thickness), typeof(VisualsPanel),
      new FrameworkPropertyMetadata(thickness0, FrameworkPropertyMetadataOptions.AffectsMeasure, thickness_Changed));


    private static void thickness_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      Thickness thickness = (Thickness)e.NewValue;
      if (double.IsNaN(thickness.Left) || double.IsNaN(thickness.Right) || 
        double.IsNaN(thickness.Top) || double.IsNaN(thickness.Bottom) ||
        double.IsInfinity(thickness.Left) || double.IsInfinity(thickness.Right) || 
        double.IsInfinity(thickness.Top) || double.IsInfinity(thickness.Bottom) ||
        thickness.Left < 0.0 || thickness.Right < 0.0 || thickness.Top < 0.0 || thickness.Bottom < 0.0)
      {
        throw new Exception("Thickness " + thickness.Left + ", " + thickness.Top + ", " + thickness.Right + ", " + thickness.Bottom + 
          " must be greater equal 0 and cannot be infinite.");
      }
    }


    /// <summary>
    /// Defines the size of the empty space between Border and Children. It must be greater than or equal 0.
    /// Default: 0
    /// </summary>
    [Bindable(true), Category("Layout")]
    public Thickness Padding {
      get { return (Thickness)GetValue(PaddingProperty); }
      set { SetValue(PaddingProperty, value); }
    }


    /// <summary>
    ///   The DependencyProperty for the Background property.
    /// </summary>
    public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(VisualsPanel),
      new FrameworkPropertyMetadata(Panel.BackgroundProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    ///   Brush used to draw the background of VisualsPanel. If null, no background will be painted.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush Background {
      get { return (Brush)GetValue(BackgroundProperty); }
      set { SetValue(BackgroundProperty, value); }
    }


    /// <summary>
    /// HorizontalContentAlignment Dependency Property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      HorizontalAlignment.Left
    /// </summary>
    public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register(
      "HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(VisualsPanel), new FrameworkPropertyMetadata(HorizontalAlignment.Left));


    /// <summary>
    ///     The horizontal alignment of the VisualsPanel.
    ///     This will only affect inheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [Bindable(true), Category("Layout")]
    public HorizontalAlignment HorizontalContentAlignment {
      get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
      set { SetValue(HorizontalContentAlignmentProperty, value); }
    }


    /// <summary>
    /// VerticalContentAlignment Dependency Property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      VerticalAlignment.Top
    /// </summary>
    public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register(
      "VerticalContentAlignment", typeof(VerticalAlignment), typeof(VisualsPanel), new FrameworkPropertyMetadata(VerticalAlignment.Top));


    /// <summary>
    ///     The vertical alignment of the VisualsPanel.
    ///     This will only affect inheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [Bindable(true), Category("Layout")]
    public VerticalAlignment VerticalContentAlignment {
      get { return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty); }
      set { SetValue(VerticalContentAlignmentProperty, value); }
    }


    // Tab properties are not supported, because VisualsPanel is used for drawing quickly to the screen, but not
    // for keyboard input
    ///// <summary>
    /////     The DependencyProperty for the TabIndex property.
    ///// </summary>
    //public static readonly DependencyProperty TabIndexProperty
    //            = KeyboardNavigation.TabIndexProperty.AddOwner(typeof(VisualsPanel));

    ///// <summary>
    /////     TabIndex property change the order of Tab navigation between Controls.
    /////     Control with lower TabIndex will get focus before the Control with higher index
    ///// </summary>
    //[Bindable(true), Category("Behavior")]
    //public int TabIndex {
    //  get { return (int)GetValue(TabIndexProperty); }
    //  set { SetValue(TabIndexProperty, value); }
    //}

    ///// <summary>
    /////     The DependencyProperty for the IsTabStop property.
    ///// </summary>
    //public static readonly DependencyProperty IsTabStopProperty
    //            = KeyboardNavigation.IsTabStopProperty.AddOwner(typeof(VisualsPanel));

    ///// <summary>
    /////     Determine is the Control should be considered during Tab navigation.
    /////     If IsTabStop is false then it is excluded from Tab navigation
    ///// </summary>
    //[Bindable(true), Category("Behavior")]
    //public bool IsTabStop {
    //  get { return (bool)GetValue(IsTabStopProperty); }
    //  set { SetValue(IsTabStopProperty, BooleanBoxes.Box(value)); }
    //}

    /// <summary>
    /// The space the children visuals can use for drawing. They should us Offset=0.
    /// </summary>
    public Size ContentSize { get; private set; }


    /// <summary>
    /// Raised if ContentSize size.
    /// </summary>
    public event Action<Size>? ContentSizeChanged;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Constructor used for WPF event tracing
    /// </summary>
    public VisualsPanel(string visualsPanelName): this() {
      if (visualsPanelName!=null) {
        Name = visualsPanelName;
      }
    }


    /// <summary>
    /// Default Constructor
    /// </summary>
    public VisualsPanel() {
      visuals = new VisualCollection(this);
      LayoutUpdated += visualsPanel_LayoutUpdated;
    }
    #endregion


    #region Layout
    //      ------

    /// <summary>
    /// TraceMeasureOverride is for the support of TraceWPFEvents.
    /// Input: MeasureOverride(), constraint; Output: actually desired size
    /// </summary>
    protected Func<Func<Size, Size>, Size, Size>? TraceMeasureOverride = null;


    /// <summary>
    /// It is sealed because inheritors should not override MeasureOverride but MeasureVisualsOverride instead.
    /// 
    /// MeasureOverride subtracts the space needed for Border and Padding, then calls MeasureVisualsOverride with the remaining size.
    /// </summary>
    protected override sealed Size MeasureOverride(Size constraint) {
      if (TraceMeasureOverride!=null) {
        // do the actual work in doMeasureOverride to allow TraceWPFEvents to hook in first
        return TraceMeasureOverride(doMeasureOverride, constraint);
      } else {
        return doMeasureOverride(constraint);
      }
    }


    private Size doMeasureOverride(Size constraint) {
      calculateBorderWidth(constraint, BorderPenThickness, Padding, out var borderWidth, out var contentAvailableWidth);

      calculateBorderHeight(constraint, BorderPenThickness, Padding, out var borderHeight, out var contentAvailableHeight);

      //measure content within the size left, which might be 0.
      Size contentRequiredSize = MeasureVisualsOverride(new Size (contentAvailableWidth, contentAvailableHeight));

      //request from the parent the full size needed for the border and the content, which might be greater than constraint size.
      return new Size(borderWidth + contentRequiredSize.Width, borderHeight + contentRequiredSize.Height);
    }


    private void calculateBorderWidth(Size constraint, double borderPenThickness, Thickness padding, out double borderWidth, out double contentAvailableWidth) {
      borderWidth = 2 * borderPenThickness + padding.Left + padding.Right;
      contentAvailableWidth = Math.Max(constraint.Width - borderWidth, 0);
    }


    private void calculateBorderHeight(Size constraint, double borderPenThickness, Thickness padding, out double borderHeight, out double contentAvailableHeight) {
      borderHeight = 2 * borderPenThickness  + padding.Top + padding.Bottom;
      contentAvailableHeight = Math.Max(constraint.Height - borderHeight, 0);
    }


    //private void adjustSpace(double constrainingSpace, double requiredSpace, out double availableSpace) {
    //  if (requiredSpace>constrainingSpace) {
    //    availableSpace = 0;
    //  } else {
    //    availableSpace = constrainingSpace - requiredSpace;
    //  }
    //}


    bool isWidthInfinity;
    bool isHeightInfinity;


    /// <summary>
    /// Called by VisualsPanel.MeasureOverride to find out the size needed by all Visuals. If
    /// the Visuals use all available size, return 0; if a dimension of availableSize is infinite, return
    /// the real size or zero, but not infinite !
    /// </summary>
    protected virtual Size MeasureVisualsOverride(Size availableSize) {
      isWidthInfinity = false;
      isHeightInfinity = false;

      if (double.IsInfinity(availableSize.Width)){
        //parent pretends that there is unlimited space available.
        //we would like to use all the space the parent can give. But there is no way to find out at this point how much this is. So
        //we just claim a big amount and fix the size in Arrange
        availableSize.Width = System.Windows.SystemParameters.PrimaryScreenWidth;;
        isWidthInfinity = true;
      }
      if (double.IsInfinity(availableSize.Height)){
        availableSize.Height = System.Windows.SystemParameters.PrimaryScreenHeight;;
        isHeightInfinity = true;
      }
       return availableSize;
    }


    Size actualContentSize;
    bool hasContentSizeChanged;
    double actualOffsetLeft = double.MinValue;
    double actualOffsetTop  = double.MinValue;
    static readonly Vector illegalVector = new Vector(double.MinValue, double.MinValue);
    Vector offsetVector = illegalVector;

    static readonly Size illegalSize = new Size(double.NaN, double.NaN);
    Size actualArrangeBounds = illegalSize;
    Brush actualBackground = Brushes.Transparent;
    Brush actualBorderBrush = Brushes.Transparent; 
    double actualBorderPenThickness = double.MinValue;
    DrawingVisual? backgroundDrawingVisual;
    bool hasBackgroundVisual;


    /// <summary>
    /// TraceArrangeOverride is for the support of TraceWPFEvents.
    /// Input: ArrangeOverride(), arrangeBounds; Output: actually used size
    /// </summary>
    protected Func<Func<Size, Size>, Size, Size>? TraceArrangeOverride = null;


    /// <summary>
    /// It is sealed because inheritors should not  override ArrangeOverride but ArrangeVisualsOverride instead.
    /// 
    /// ArrangeOverride subtracts the space needed for Border and Padding, then calls ArrangeVisualsOverride with the remaining size.
    /// </summary>
    protected override sealed Size ArrangeOverride(Size arrangeBounds) {
      if (TraceArrangeOverride!=null) {
        // do the actual work in doArrangeOverride to allow TraceWPFEvents to hook in first
        return TraceArrangeOverride(doArrangeOverride, arrangeBounds);
      } else {
        return doArrangeOverride(arrangeBounds);
      }
    }


    private Size doArrangeOverride(Size arrangeBounds) {
      if (double.IsNaN(arrangeBounds.Width) || double.IsInfinity(arrangeBounds.Width) || arrangeBounds.Width<0 ||
        double.IsNaN(arrangeBounds.Height) || double.IsInfinity(arrangeBounds.Height) || arrangeBounds.Height<0) 
      {
        //just curious if this ever happens.
        throw new Exception("Illegal dimensions of arrangeBounds: " + arrangeBounds);
      }

      Size adjustedBounds = arrangeBounds;
      if (isWidthInfinity || isHeightInfinity) {
        DependencyObject current = this;
        do {
          current = VisualTreeHelper.GetParent(current);
          if (current is UIElement parentUIElement) {
            double adjustedWidth, adjustedHeigth;
            adjustedWidth = isWidthInfinity ? parentUIElement.DesiredSize.Width : adjustedBounds.Width;
            adjustedHeigth = isHeightInfinity ? parentUIElement.DesiredSize.Height : adjustedBounds.Height;
            adjustedBounds = new Size(adjustedWidth, adjustedHeigth);
            break;
          }
        } while (current!=null);
      }


      calculateBorderWidth(adjustedBounds, BorderPenThickness, Padding, out var borderWidth, out var contentAvailableWidth);

      calculateBorderHeight(adjustedBounds, BorderPenThickness, Padding, out var borderHeight, out var contentAvailableHeight);

      //arrange content within the size left, which might be 0.
      Size contentRequiredSize = ArrangeVisualsOverride(new Size (contentAvailableWidth, contentAvailableHeight));

      //draw background and border if necessary
      if (actualArrangeBounds!=adjustedBounds || actualBackground!=Background || actualBorderBrush!=BorderBrush || 
        actualBorderPenThickness!=BorderPenThickness) 
      {
        //border and/or FrameWorkElementSize have changed. Redraw background
        actualArrangeBounds = adjustedBounds;
        actualBackground = Background;
        actualBorderBrush = BorderBrush;
        actualBorderPenThickness = BorderPenThickness;
        //remove old BackgroundDrawingVisual
        if (visuals.Count>0 && ((DrawingVisual)visuals[0])==backgroundDrawingVisual){
          visuals.RemoveAt(0);
        }
        if (Background==null && BorderPenThickness<=0 && Padding==thickness0) {
          //no BackgroundDrawingVisual needed
          backgroundDrawingVisual = null;
          hasBackgroundVisual = false;
        } else {
          //create updated background
          backgroundDrawingVisual = new DrawingVisual();
          using (DrawingContext drawingContext = backgroundDrawingVisual.RenderOpen()) {
            if (BorderPenThickness>0) {
              if (TraceWpf.IsTracing) {
                TraceWpf.Line(this, "DrawBackgroundBorder(Width: " + actualArrangeBounds.Width + ", Height: " + actualArrangeBounds.Height + ", Border: " + BorderPenThickness + ")");
              }
              double halfBorderWidth = BorderPenThickness / 2.0;
              var drawingRect = new Rect(halfBorderWidth, halfBorderWidth, actualArrangeBounds.Width-BorderPenThickness, actualArrangeBounds.Height-BorderPenThickness);
              drawingContext.DrawRectangle(Background, new Pen(BorderBrush, BorderPenThickness), drawingRect);
            } else {
              if (TraceWpf.IsTracing) {
                TraceWpf.Line(this, "DrawBackground(Width: " + actualArrangeBounds.Width + ", Height: " + actualArrangeBounds.Height + ")");
              }
              drawingContext.DrawRectangle(Background, null, new Rect(0, 0, actualArrangeBounds.Width, actualArrangeBounds.Height));
            }
          }
          visuals.Insert(0, backgroundDrawingVisual);
          hasBackgroundVisual = true;
        }
      }

      //update DrawingVisual Offsets if necessary
      double offsetLeft = Math.Min(BorderPenThickness + Padding.Left, adjustedBounds.Width);
      double offsetTop = Math.Min(BorderPenThickness + Padding.Top, adjustedBounds.Height);
      if (actualOffsetLeft!=offsetLeft || actualOffsetTop!=offsetTop){
        //border size has changed => calculate new offset
        actualOffsetLeft = offsetLeft;
        actualOffsetTop = offsetTop;
        offsetVector = new Vector(offsetLeft, offsetTop);
        for (int visualIndex = 0; visualIndex < visuals.Count; visualIndex++) {
          if (visualIndex==0 && hasBackgroundVisual) continue;

          DrawingVisual updateVisual = (DrawingVisual)visuals[visualIndex];
          if (updateVisual!=null){
            updateVisual.Offset = offsetVector;
          }
        }
      }

      ContentSize = new Size(contentAvailableWidth, contentAvailableHeight);
      if (actualContentSize!=ContentSize) {
        actualContentSize = ContentSize;
        if (ContentSizeChanged!=null) {
          hasContentSizeChanged = true;
        }
      }

      //report to the parent that at most arrangeBounds size was used.
      return new Size(Math.Min(borderWidth + contentRequiredSize.Width, adjustedBounds.Width), 
        Math.Min(borderHeight + contentRequiredSize.Height, adjustedBounds.Height));
    }


    /// <summary>
    /// Called by VisualsPanel.ArrangeOverride to let the inheritor arrange its content. If
    /// the Visuals use all available size, return arrangeBounds; if a dimension of availableSize is infinite, return
    /// the real size or 0, but not infinite !
    /// </summary>
    protected virtual Size ArrangeVisualsOverride(Size arrangeBounds) {
      if (double.IsInfinity(arrangeBounds.Width)){
        arrangeBounds.Width = 0;
      }
      if (double.IsInfinity(arrangeBounds.Height)){
        arrangeBounds.Height = 0;
      }
       return arrangeBounds;
    }


    void visualsPanel_LayoutUpdated(object? sender, EventArgs e) {
      if (hasContentSizeChanged) {
        hasContentSizeChanged = false;
        ContentSizeChanged?.Invoke(ContentSize);
      }
    }
    #endregion


    #region Visual Collection
    //      -----------------

    /// <summary>
    /// Holds all the DrawingVisuals of this VisualsPanel
    /// </summary>
    private readonly VisualCollection visuals;


    /// <summary>
    /// Returns the number of DrawingVisuals in this VisualsPanel. Used by WPF for drawing.
    /// </summary>
    protected sealed override int VisualChildrenCount {
      get {
        return visuals.Count;
      }
    }

    
    /// <summary>
    /// Returns the indexed DrawingVisual. Is used by WPF for drawing
    /// </summary>
    protected sealed override Visual GetVisualChild(int index) {
      return visuals[index];
    }


    /// <summary>
    /// Number of DrawingVisual children this VisualsPanel holds.
    /// </summary>
    public int ChildrenCount {
      get {
        if (hasBackgroundVisual) {
          return visuals.Count - 1;
        } else {
          return visuals.Count;
        }
      }
    }


    /// <summary>
    /// Removes all DrawingVisuals from VisualsPanel except the Visual for the VisualsPanel's Background. 
    /// </summary>
    public void Clear() {
      visuals.Clear();
      if (backgroundDrawingVisual!=null) {
        visuals.Add(backgroundDrawingVisual);
      }
    }

    
    /// <summary>
    /// Adds a DrawingVisual at the end of VisualsPanel' children.
    /// </summary>
    public void AddChild(DrawingVisual visual){
      handleOffset(visual);
      visuals.Add(visual);
    }


    private void handleOffset(DrawingVisual visual) {
      if (visual.Offset.X!=0 || visual.Offset.Y!=0) {
        throw new Exception("The offset (" + visual.Offset.X + ", " + visual.Offset.Y + ") of the VisualDrawing to be added to VisualsPanel " +
          "needs to be zero.");
      }
      if (offsetVector!=illegalVector) {
        visual.Offset = offsetVector;
      }
    }


    /// <summary>
    /// Inserts a DrawingVisual to VisualsPanel at the specified index.
    /// </summary>
    public void Insert(int index, DrawingVisual visual) {
      handleOffset(visual);
      if (hasBackgroundVisual) index++;

      visuals.Insert(index, visual);
    }


    /// <summary>
    /// Replace a DrawingVisual in VisualsPanel at the specified index.
    /// </summary>
    public void Replace(int index, DrawingVisual visual) {
      handleOffset(visual);
      if (hasBackgroundVisual) index++;

      visuals.RemoveAt(index);
      visuals.Insert(index, visual);
    }
    #endregion
  }
}
