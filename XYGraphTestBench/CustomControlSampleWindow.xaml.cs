using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTestbench;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for CustomControlSample
  /// </summary>
  public partial class CustomControlSampleWindow: TestbenchWindow {

    
    /// <summary>
    /// Creates and opens a new CustomControlSampleWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      ShowProtected( () => new CustomControlSampleWindow(), ownerWindow);
    }


    public CustomControlSampleWindow() {
      InitializeComponent();
    }


    const double nan = double.NaN;
    readonly static FontFamily arial = new FontFamily("Arial");
    readonly static FontFamily couri = new FontFamily("Courier New");
    const HorizontalAlignment hstr = HorizontalAlignment.Stretch;
    const HorizontalAlignment left = HorizontalAlignment.Left;
    const HorizontalAlignment hcen = HorizontalAlignment.Center;
    const HorizontalAlignment rigt = HorizontalAlignment.Right;
    const VerticalAlignment vst = VerticalAlignment.Stretch;
    const VerticalAlignment top = VerticalAlignment.Top;
    const VerticalAlignment vce = VerticalAlignment.Center;
    const VerticalAlignment bot = VerticalAlignment.Bottom;

    
    protected override Func<Action>[] InitTestFuncs() {
      return new Func<Action>[] {
        //            Height     Horizontal  Vertical  Left        Right       Top         Bottom      Text         Font
        //                Width  Contr Conte Cntr Cnte Mar Bor Pad Pad Bor Mar Mar Bor Pad Pad Bor Mar 
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10,  0,  0,  0,  0,  0,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10,  0,  0,  0,  0,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10,  0,  0,  0,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10,  0,  0,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10,  0,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10,  0,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10,  0,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,  0,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,  0, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, left, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hcen, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, rigt, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, top, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vce, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, bot, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, left, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hcen, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, top, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vce, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, bot, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 14),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 16),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 18),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 22),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 26),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 32),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 40),
        //() => testFunc(nan, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", couri, 60),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 14),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 16),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 18),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 22),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 26),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 32),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 40),
        //() => testFunc(nan, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", couri, 60),
        //() => testFunc(200, nan, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, hstr, left, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, hstr, hcen, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, hstr, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, left, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, left, left, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, left, hcen, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, left, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, hcen, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, hcen, left, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, hcen, hcen, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, hcen, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, rigt, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, rigt, left, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, rigt, hcen, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(200, nan, rigt, rigt, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vst, top, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vst, vce, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vst, bot, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, top, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, top, top, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, top, vce, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, top, bot, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vce, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vce, top, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vce, vce, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, vce, bot, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, bot, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, bot, top, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, bot, vce, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
        //() => testFunc(nan, 100, hstr, hstr, bot, bot, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),

        
        
        () => testFunc(300, 100, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "123456789 123456789 123456789 123456789 123456789 123456789 ", arial, 12),
        () => testFunc(300, 100, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "", arial, 12),
        () => testFunc(300, 100, hstr, hstr, vst, vst, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, "Test Text", arial, 12),
      };
    }


    private Action testFunc(
      double width, 
      double height, 
      HorizontalAlignment horizontalControlAlignment, 
      HorizontalAlignment horizontaControllAlignment, 
      VerticalAlignment verticalControlAlignment, 
      VerticalAlignment verticalContentAlignment, 
      double marginLeft, 
      double borderLeft, 
      double paddingLeft, 
      double paddingRight, 
      double borderRight, 
      double marginRight, 
      double marginTop, 
      double borderTop, 
      double paddingTop, 
      double paddingBottom, 
      double borderBottom, 
      double marginBottom, 
      string text, 
      FontFamily fontFamily,
      double fontSize) 
    {
      TestCustomControlSampleTraced.Width = width; 
      TestCustomControlSampleTraced.Height = height; 
      TestCustomControlSampleTraced.HorizontalAlignment = horizontalControlAlignment; 
      TestCustomControlSampleTraced.HorizontalContentAlignment = horizontaControllAlignment; 
      TestCustomControlSampleTraced.VerticalAlignment = verticalControlAlignment; 
      TestCustomControlSampleTraced.VerticalContentAlignment = verticalContentAlignment; 
      TestCustomControlSampleTraced.Margin = new Thickness(marginLeft, marginTop, marginLeft, marginBottom); 
      TestCustomControlSampleTraced.BorderThickness = new Thickness(borderLeft, borderTop, borderRight, borderBottom); 
      TestCustomControlSampleTraced.Padding = new Thickness(paddingLeft, paddingTop, paddingRight, paddingBottom); 
      TestCustomControlSampleTraced.textBox.Text = text; 
      TestCustomControlSampleTraced.FontFamily = fontFamily;
      TestCustomControlSampleTraced.FontSize = fontSize;
      return null;
    }
  }
}
