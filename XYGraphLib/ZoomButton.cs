using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace XYGraphLib {
  /// <summary>
  /// WPF Button displaying a plus or minus sign. ZoomButton uses all available space. Therefore it is good if its height and width is set explictely.
  /// </summary>
  public class ZoomButton: Button {

    #region Properties
    //      ----------

    /// <summary>
    /// If IsZoomIn is true, a plus sign gets displayed (=default), otherwise a minus sign
    /// </summary>
    public bool IsZoomIn {
      get { return (bool)GetValue(IsZoomInProperty); }
      set { SetValue(IsZoomInProperty, value);}
    }


    /// <summary>
    /// The DependencyProperty definition for IsZoomIn property.
    /// </summary>
    public static readonly DependencyProperty IsZoomInProperty =
      DependencyProperty.RegisterAttached(
        "IsZoomIn", // Property name
        typeof(bool), // Property type
        typeof(ZoomButton), // Property owner
        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

    
    /// <summary>
    /// Brush used to draw the plus or minus sign. Default colour: DarkSlateGray
    /// </summary>
    public Brush StrokeBrush {
      get { return (Brush)GetValue(StrokeBrushProperty); }
      set { SetValue(StrokeBrushProperty, value);}
    }


    /// <summary>
    /// The DependencyProperty definition for StrokeBrush property.
    /// </summary>
    public static readonly DependencyProperty StrokeBrushProperty =
      DependencyProperty.RegisterAttached(
        "StrokeBrush", // Property name
        typeof(Brush), // Property type
        typeof(ZoomButton), // Property owner
        new FrameworkPropertyMetadata(Brushes.DarkSlateGray, FrameworkPropertyMetadataOptions.AffectsRender));
    #endregion


    #region Constructor
    //      -----------

    public ZoomButton(bool isZoomIn, Brush strokeBrush):this() {
      IsZoomIn = isZoomIn;
      StrokeBrush = strokeBrush;
    }


    Grid grid;
    Line xLine;
    Line yLine;

    
    public ZoomButton() {
      grid = new Grid();
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid.VerticalAlignment = VerticalAlignment.Stretch;
      Content = grid;

      xLine = new Line();
      xLine.HorizontalAlignment = HorizontalAlignment.Left;
      xLine.VerticalAlignment = VerticalAlignment.Top;
      xLine.Stretch = Stretch.None;
      grid.Children.Add(xLine);
      
      yLine = new Line();
      yLine.HorizontalAlignment = HorizontalAlignment.Left;
      yLine.VerticalAlignment = VerticalAlignment.Top;
      yLine.Stretch = Stretch.None;
      grid.Children.Add(yLine);
      
      IsEnabledChanged += ZoomButton_IsEnabledChanged;
    }
    #endregion


    #region Methods
    //      -------

    protected override Size MeasureOverride(Size constraint) {
      return base.MeasureOverride(constraint);
    }


    protected override Size ArrangeOverride(Size arrangeBounds) {
      return base.ArrangeOverride(arrangeBounds);
    }


    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
      reConstruct(sizeInfo.NewSize);
      base.OnRenderSizeChanged(sizeInfo);
    }


    void ZoomButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
      reConstruct(RenderSize);
    }

    
    private void reConstruct(Size size) {
      //somehow the Button ContentPresenter doesn't give the grid all the space available, but only what the grid needs. It then centers
      //the grid which spoils the positioning. By assigning explicitely the grid all the space available, there will be not positioning porblem.
      grid.Width = size.Width;
      grid.Height = size.Height;

      const double borderOffset = 6;
      double availableWidth = size.Width - borderOffset;
      double availableHeight = size.Height - borderOffset;
      double startX = 0.1 * availableWidth;
      double midX   = 0.5 * availableWidth;
      double endX   = 0.9 * availableWidth;
      double startY = 0.1 * availableHeight;
      double midY   = 0.5 * availableHeight;
      double endY   = 0.9 * availableHeight;

      xLine.X1 = startX;
      xLine.Y1 = midY;
      xLine.X2 = endX;
      xLine.Y2 = midY;

      //const double borderOffset = 8;
      //double availableWidth = size.Width - borderOffset;
      //double availableHeight = size.Height - borderOffset;
      //double startX = 0.2 * availableWidth;
      //double midX = 0.5 * availableWidth;
      //double endX   = 0.8 * availableWidth;
      //double startY = 0.2 * availableHeight;
      //double midY = 0.5 * availableHeight;
      //double endY   = 0.8 * availableHeight;

      //xLine.X1 = startX - borderOffset;
      //xLine.Y1 = midY - borderOffset;
      //xLine.X2 = endX - borderOffset;
      //xLine.Y2 = midY - borderOffset;

      Brush brush;
      if (IsEnabled) {
        brush = StrokeBrush;
      } else {
        brush = StrokeBrush.Clone();
        brush.Opacity = 0.3;
      }
      xLine.Stroke = brush;
      xLine.StrokeThickness = availableHeight/10;

      if (IsZoomIn) {
        yLine.Visibility = Visibility.Visible;
        yLine.X1 = midX;
        yLine.Y1 = startY;
        yLine.X2 = midX;
        yLine.Y2 = endY;

        //yLine.X1 = midX - borderOffset;
        //yLine.Y1 = startY - borderOffset;
        //yLine.X2 = midX - borderOffset;
        //yLine.Y2 = endY - borderOffset;
        yLine.Stroke = StrokeBrush;
        yLine.StrokeThickness = availableWidth/10;
      } else {
        yLine.Visibility = Visibility.Collapsed;
      }
    }

    //protected override void OnRender(DrawingContext drawingContext) {
    //  base.OnRender(drawingContext);
    //  if (double.IsNaN(RenderSize.Width) || double.IsNaN(RenderSize.Height)) return;

    //  //double startX = 0.2 * RenderSize.Width * 2;
    //  //double midX = 0.5 * RenderSize.Width * 2;
    //  //double endX   = 0.8 * RenderSize.Width * 2;
    //  //double startY = 0.2 * RenderSize.Height * 2;
    //  //double midY = 0.5 * RenderSize.Height * 2;
    //  //double endY   = 0.8 * RenderSize.Height * 2;

    //  double startX = -0.5*RenderSize.Width;
    //  double midX = 0.5 * RenderSize.Width;
    //  double endX   = RenderSize.Width * 1.5;
    //  double startY = -0.5*RenderSize.Height;
    //  double midY = 0.5 * RenderSize.Height;
    //  double endY   = RenderSize.Height * 1.5;
    //  drawingContext.DrawLine(StrokePen, new Point(startX, midY), new Point(endX, midY));
    //  if (IsZoomIn) {
    //    drawingContext.DrawLine(StrokePen, new Point(midX, startY), new Point(midX, endY));
    //  }
    //}


    //private void Redraw(Size size) {
    //  Visuals.Clear();
    //  if (double.IsNaN(size.Width) || double.IsNaN(Height)) return;
    //  DrawingVisual drawingVisual = new DrawingVisual();
    //  double startX = 0.2 * size.Width;
    //  double midX = 0.5 * size.Width;
    //  double endX   = 0.8 * size.Width;
    //  double startY = 0.2 * size.Height;
    //  double midY = 0.5 * size.Height;
    //  double endY   = 0.8 * size.Height;
    //  using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
    //    drawingContext.DrawLine(StrokePen, new Point(startX, midY), new Point(endX, midY));
    //    if (IsZoomIn) {
    //      drawingContext.DrawLine(StrokePen, new Point(midX, startY), new Point(midX, endY));
    //    }
    //  }
    //  Visuals.Add((drawingVisual));
    //}


    //bool isMouseClicked;
    //private bool isPlus;

    //protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e) {
    //  isMouseClicked = true;
    //  base.OnMouseDown(e);
    //}


    //protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e) {
    //  isMouseClicked = false;
    //  base.OnMouseEnter(e);
    //}

    
    //protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e) {
    //  isMouseClicked = false;
    //  base.OnMouseEnter(e);
    //}


    //protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e) {
    //  if (isMouseClicked) {
    //    isMouseClicked = false;
    //    if (Click!=null) {
    //      Click(this);
    //    }
    //  }
    //}
    #endregion
  }
}
