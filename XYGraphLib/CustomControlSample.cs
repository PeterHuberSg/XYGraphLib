using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace XYGraphLib {


  /// <summary>
  /// CustomControlSample displays a a textbox on the right side and an elippse on the left. The textbox takes the size as
  /// needed by its content, while the ellipse has the same dimensions like the textbox.
  /// </summary>
  public class CustomControlSample: CustomControlBase {
    // CustomControlSample provides a code sample showing what an inheritor of CustomControlBase usually needs to do:
    // 1) add some FrameworkElements as childeren of CustomControlSample (=children)
    // 2) override MeasureContentOverride (instead of MeasurementOverrid). The provided Size constraint is reduced by Border and Padding
    // 3) override ArrangeContentOverride (instead of ArrangeOverride). The parameter arrangeRect is a rectangle, not a
    //    size, to provide an offset for Border and Padding. Use ArrangeBorderPadding() instead of Arrange() to arrange the children
    // 4) override OnRenderContent instead of OnRender to draw content directly to drawingContext
    //
    //There are 2 types of content a control inheriting from CustomControlBase can use:
    //+ FramworkElement: AddChild() in constructor and overwrite MeasureContentOverride and ArrangeContentOverride
    //+ draw to drawingContext: overwrite MeasureContentOverride, ArrangeContentOverride and OnRenderContent
    //
    //CustomControlSample has one TextBox child and draws a ellipse to demonstrate both cases


    /// <summary>
    /// Gives access to textBox for testing purpose
    /// </summary>
    public TextBox textBox; //child of CustomControlBase 


    #region 1) create and add Framework controls as children to constructor
    //      ---------------------------------------------------------------

    /// <summary>
    /// Default constructor
    /// </summary>
    public CustomControlSample() {
      //add FrameworkElement children, if any
      textBox = new TextBox{Text="Hi"};
      textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
      
      Binding newBinding = new Binding("Foreground");
      newBinding.Source = this;
      newBinding.Mode = BindingMode.OneWay;
      textBox.SetBinding(TextBox.ForegroundProperty, newBinding);      
      AddChild(textBox);

      //change some CustomControlBase properties
      Background = Brushes.Goldenrod;
      BorderBrush = Brushes.DarkGoldenrod;
    }


    void textBox_TextChanged(object sender, TextChangedEventArgs e) {
      if (HorizontalAlignment==HorizontalAlignment.Stretch && HorizontalContentAlignment!=HorizontalAlignment.Stretch) {
        //the width of the textbox and therefore of the ellipse is defined by the contnent of the textbox

        //FrameworkElement.ArrangeCore() only calls CustomControlSample.OnRender() if the size of CustomControlSample has
        //changed. Just changing the textbox content might not change the size of CustomControlSample, but the ellipse
        //has to be redrawn. Unfortunately, Microsoft does not allow in ArrangeOverride() to force a render. InvalidateVisual()
        //does not just force a render, but also measure and arrange.
        
        InvalidateVisual();
      }
    }
    #endregion


    #region 2) overwrite MeasureContentOverride instead of MeasurementOverrid
    //      -----------------------------------------------------------------
    
    protected override Size MeasureContentOverride(Size constraint) { 
      //constraint is already reduced by Border and Padding.

      textBox.HorizontalAlignment = HorizontalContentAlignment;
      textBox.VerticalAlignment = VerticalContentAlignment;

      //constraint can be between 0 and infinite, but textBox.Measure() is able to handle that.
      //the return value from textBox.Measure() is in DesiredSize, which tells how much space the textbox requests. If width is set, 
      //Textbox requests the same width. Otherwise it requests the size needed to display its content.

      //In this sample, TextBox.Width is always NAN. Give TextBox only half the available width
      textBox.Measure(new Size(constraint.Width/2, constraint.Height));

      //minimum size needed for TextBox and  equally sized ellipse
      Size returnSize = new Size(2*textBox.DesiredSize.Width, textBox.DesiredSize.Height);
      if (HorizontalAlignment==HorizontalAlignment.Stretch) {
        //if textbox does not need all the space but content(!) should be stretched, request full width
        returnSize.Width = Math.Max(returnSize.Width, constraint.Width);
      }
      if (VerticalAlignment==VerticalAlignment.Stretch) {
        //if textbox does not need all the space but content(!) should be stretched, request full height
        returnSize.Height = Math.Max(returnSize.Height, constraint.Height);
      }
      return returnSize;
    }
    #endregion


    #region 3) override ArrangeContentOverride instead of ArrangeOverride
    //      -------------------------------------------------------------
    //
    // 3.1) Don't use CustomControl.DesiredSize in Arrange(), because DesiredSize is with Margin. Use arrangeRect instead, which is 
    //      without Margin, Border and Padding. DesiredSize should only be used when dealing with the children of the CustomControl.
    // 3.2) use ArrangeBorderPadding() instead of Arrange() to arrange children frameworkElements
    // 3.3) if IsSizingToContent() then use only the space needed, otherwise use all space provided

    double ellipsRenderWidth = double.NegativeInfinity;


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      if (textBox.HorizontalAlignment!=HorizontalContentAlignment || textBox.VerticalAlignment!=VerticalContentAlignment) {
        //ensure that arrange and measure use the same ContentAlignment, i.e. that it has not changed between the 2 calls
        throw new Exception("ContentAlignment has changed between Measure() and Arrange().");
      }

      //arrange visual children here. This will also call their Render method.

      ////calculate total desired space. loop over all children. Include also the space needed for OnRender
      //double totalDesiredWidth = 2*textBox.DesiredSize.Width;
      //double totalDesiredHeight = textBox.DesiredSize.Height;
      
      //arrange: 
      //loop over all FrameworkElement children. This sample has only 1 FrameworkElement child. The
      //children rendered by CustomControlSample directly will be arranged in OnRender
      Size availableTextboxSize;
      if (IsSizingWidthToExpandableContent()) {
        //textbox should use only space really needed. To let Framework.ArrangeCore() to get the alignment right, all
        //the space needs to be given to textbox.Arrange(), except the space which is used by the ellipse.
        double ellipseWidth = textBox.DesiredSize.Width;
        //Warning: Actually, it would be better to use the RenderSize after arranging the textbox instead of DesiredSize. But the 
        //code here is executed before the arrange, meaning RenderSize is not updated yet. Luckily, in CustomControlSample the 
        //measured size and the arranged size will be the same when the textbox does not need to grow (i.e. not width defined nor 
        //content stretched)
        availableTextboxSize = new Size(arrangeRect.Size.Width-ellipseWidth, arrangeRect.Size.Height);
      } else {
        //Textbox has to use half of the avaialble space. No content alignment
        availableTextboxSize = new Size(arrangeRect.Size.Width/2, arrangeRect.Size.Height);
      }
      //bool isContentToUseAllSpace = HorizontalAlignment==HorizontalAlignment.Stretch && HorizontalContentAlignment!=HorizontalAlignment.Stretch;
      //if (isContentToUseAllSpace) {
      //  //textbox should use only space realy needed. To let Framework.ArrangeCore() to get the alignment right, all
      //  //the space needs to be given to textbix.Arrange(), except the space which is used by the ellipse.
      //  availableTextboxSize = new Size(arrangeRect.Size.Width-ellipseWidth, arrangeRect.Size.Height);
      //} else {
      //  //Textbox has to use half of the avaialble space. No content alignment
      //  availableTextboxSize = new Size(arrangeRect.Size.Width/2, arrangeRect.Size.Height);
      //}

      textBox.ArrangeBorderPadding(arrangeRect, 0, 0, availableTextboxSize.Width, availableTextboxSize.Height);

      if (ellipsRenderWidth!=textBox.RenderSize.Width) {
        ellipsRenderWidth = textBox.RenderSize.Width;
      }
      return arrangeRect.Size;
    }
    #endregion


    #region 4) override OnRenderContent instead of OnRender
    //      -----------------------------------------------
    // 4.1) drawingContext is moved already to the left and down to cater for Margin, Border and Padding.
    // 4.2) The CustomsControl's RenderSize cannot be used, since it includes Border and Padding. renderContentSize must be used instead, which
    // is exactly the same as what ArrangeContentOverride() returned. RenderSize would be used for children FrameworkElements.

    protected override void OnRenderContent(System.Windows.Media.DrawingContext drawingContext, Size renderContentSize) {
      //draws an ellipse next to the textbox using the same size
      double radiusX = textBox.RenderSize.Width/2;
      double radiusY = textBox.RenderSize.Height/2;
      double offsetX;
      double offsetY;
      if (!double.IsNaN(Width) || HorizontalAlignment==HorizontalAlignment.Stretch) {
        //HorizontalContentAlignment matters only if space available is different from the needed space, which is only possible if 
        //CustomControlSample is stretched or a width is defined
        switch (HorizontalContentAlignment) {
        case HorizontalAlignment.Left:
        case HorizontalAlignment.Stretch:
          offsetX = textBox.RenderSize.Width + radiusX;
          break;
        case HorizontalAlignment.Center:
          offsetX = renderContentSize.Width/2 + radiusX;
          break;
        case HorizontalAlignment.Right:
          offsetX = renderContentSize.Width - radiusX;
          break;
        default:
          throw new NotSupportedException();
        }
      } else {
        //if CustomControlSample is not stretched, renderContentSize.Width=2*textBox.RenderSize.Width
        offsetX = textBox.RenderSize.Width + radiusX;
      }
      if (!double.IsNaN(Height) || VerticalAlignment==VerticalAlignment.Stretch) {
        //VerticalContentAlignment matters only if space available is different from the needed space, which is only possible if 
        //CustomControlSample is stretched or height is defined
        switch (VerticalContentAlignment) {
        case VerticalAlignment.Top:
        case VerticalAlignment.Stretch:
          offsetY = radiusY;
          break;
        case VerticalAlignment.Center:
          offsetY = renderContentSize.Height/2;
          break;
        case VerticalAlignment.Bottom:
          offsetY = renderContentSize.Height - radiusY;
          break;
        default:
          throw new NotSupportedException();
        }
      } else {
        //if CustomControlSample is not stretched, renderContentSize.Height=textBox.RenderSize.Height
        offsetY = textBox.RenderSize.Height/2;
      }
      drawingContext.DrawEllipse(Brushes.LightGoldenrodYellow, null, new Point(offsetX, offsetY), radiusX, radiusY);
    }
    #endregion
  }
}
