using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Reflection;
using System.Windows.Controls.Primitives;


namespace XYGraphLib {

  
  /// <summary>
  /// Displays all standard WPF colors in a dropdown.
  /// </summary>
  public class StandardColorComboBox: ComboBox  {

    public static readonly DependencyProperty SelectedColorBrushProperty = 
    DependencyProperty.Register("SelectedColorBrush", typeof(Brush), typeof(StandardColorComboBox), new PropertyMetadata(BorderPenThickness_Changed));

private static void BorderPenThickness_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e) {
}

    /// <summary>
    /// Can be used for binding to the select color brush
    /// </summary>
    public Brush SelectedColorBrush {
      get { return (Brush)GetValue(SelectedColorBrushProperty); }
      private set { SetValue(SelectedColorBrushProperty, value); }
    }


    ///////// <summary>
    ///////// Can be used to set dfault colour from XAML
    ///////// </summary>
    //////public string SelectedColorName {
    //////  get { return (string)GetValue(SelectedColorNameProperty); }
    //////  set {
    //////    string valueLowercase = value.ToLowerInvariant();
    //////    for (int sampleIndex = 0; sampleIndex < Items.Count; sampleIndex++) {
    //////      var colorSamplePanel = (ColorSamplePanel)Items[sampleIndex];
    //////      if (colorSamplePanel.ColorName.ToLowerInvariant()==valueLowercase) {
    //////        SelectedIndex = sampleIndex;
    //////        SetValue(SelectedColorNameProperty, colorSamplePanel.ColorName); 
    //////        return;
    //////      }
    //////    }
    //////    throw new Exception("Unknown ColorName: " + value + ".");
    //////  }
    //////}

    
    //////public static readonly DependencyProperty SelectedColorNameProperty = 
    //////DependencyProperty.Register("SelectedColorName", typeof(string), typeof(ColorSamplePanel), new FrameworkPropertyMetadata("Green"));

    
    public StandardColorComboBox() {
      foreach (PropertyInfo brushPropertyInfo in typeof(Brushes).GetProperties()) {
        SolidColorBrush brush = (SolidColorBrush)brushPropertyInfo.GetValue(null, null);
        Items.Add(new ColorSamplePanel(brushPropertyInfo.Name, brush)); 
      }
//////      TextSearch.SetTextPath(this, "ColorName");
      SelectedValuePath = "ColorBrush";
      SelectionChanged += StandardColorComboBox_SelectionChanged;
    }


    void StandardColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (e.AddedItems.Count>0) {
        var colorSamplePanel = (ColorSamplePanel)e.AddedItems[0];
        SelectedColorBrush = colorSamplePanel.ColorBrush;
        //////SetValue(SelectedColorNameProperty, colorSamplePanel.ColorName);
      }
    }
  }



  public class ColorSamplePanel: StackPanel {


    public string ColorName { get; private set; }
    public Brush ColorBrush { get; private set; }

    Rectangle colorRectangle;
    TextBlock textBlock;


    public ColorSamplePanel(string colorName, Brush colorBrush) {
      ColorName = colorName;
      ColorBrush = colorBrush;
      Orientation = Orientation.Horizontal;
      //TextSearch.SetText(this, colorName);

      colorRectangle = new Rectangle();
      colorRectangle.Fill = colorBrush;
      colorRectangle.Margin = new Thickness(2, 0, 2, 0);
      colorRectangle.VerticalAlignment = VerticalAlignment.Center;
      Children.Add(colorRectangle);

      this.VerticalAlignment = VerticalAlignment.Stretch;
      textBlock = new TextBlock();
      textBlock.Text = colorName;
      Children.Add(textBlock);

      SizeChanged += ColorSamplePanel_SizeChanged;
    }


    void ColorSamplePanel_SizeChanged(object sender, SizeChangedEventArgs e) {
      double dimensions = e.NewSize.Height * 2 / 3;
      colorRectangle.Height = dimensions;
      colorRectangle.Width = dimensions;
    }


    public override string ToString() {
      return textBlock.Text;
    }
  }
}
