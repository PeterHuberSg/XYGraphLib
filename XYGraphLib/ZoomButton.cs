/**************************************************************************************

XYGraphLib.VisualsPanel
=======================

WPF Button displaying a plus or minus sign for zooming in or out  

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace XYGraphLib {
  /// <summary>
  /// WPF Button displaying a plus or minus sign. ZoomButton uses all available space. Therefore it is good if its height and width is 
  /// set explicitly.
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

    readonly Grid grid;
    readonly Line xLine;
    readonly Line yLine;

    
    public ZoomButton() {
      grid = new Grid {
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch
      };
      Content = grid;

      xLine = new Line {
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top,
        Stretch = Stretch.None
      };
      grid.Children.Add(xLine);

      yLine = new Line {
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top,
        Stretch = Stretch.None
      };
      grid.Children.Add(yLine);
      
      IsEnabledChanged += zoomButton_IsEnabledChanged;
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


    void zoomButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
      reConstruct(RenderSize);
    }

    
    private void reConstruct(Size size) {
      //somehow the Button ContentPresenter doesn't give the grid all the space available, but only what the grid needs. It then centers
      //the grid which spoils the positioning. By assigning explicitly the grid all the space available, there will be not positioning problem.
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

        yLine.Stroke = StrokeBrush;
        yLine.StrokeThickness = availableWidth/10;
      } else {
        yLine.Visibility = Visibility.Collapsed;
      }
    }
    #endregion
  }
}
