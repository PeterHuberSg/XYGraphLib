/**************************************************************************************

XYGraphLib.VisualsFontPanel
===========================

Supports using TraceWPFEvents  

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


namespace XYGraphLib {

  /// <summary>
  /// A VisualsPanel with fonts properties, foreground, background, simple border (no rounded corners) and padding.
  /// VisualsFontPanel inherits from VisualsPanel. It is a FrameworkElement and has the functionality of a Panel, but it does not 
  /// inherit from Panel
  /// </summary>
  public class VisualsFontPanel: VisualsPanel {

    #region Properties
    //      ----------

    /// <summary>
    ///     The DependencyProperty for the Foreground property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      System Font Color
    /// </summary>
    public static readonly DependencyProperty ForegroundProperty =
                TextElement.ForegroundProperty.AddOwner(
                    typeof(VisualsFontPanel),
                    new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
                        FrameworkPropertyMetadataOptions.Inherits));


    /// <summary>
    ///     An brush that describes the foreground color.
    ///     This will only affect intheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush Foreground {
      get { return (Brush)GetValue(ForegroundProperty); }
      set { SetValue(ForegroundProperty, value); }
    }


    /// <summary>
    ///     The DependencyProperty for the FontFamily property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      System Dialog Font
    /// </summary>
    public static readonly DependencyProperty FontFamilyProperty =
                TextElement.FontFamilyProperty.AddOwner(
                    typeof(VisualsFontPanel),
                    new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily,
                        FrameworkPropertyMetadataOptions.Inherits));


    /// <summary>
    ///     The font family of the desired font.
    ///     This will only affect intheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.Font)]
    public FontFamily FontFamily {
      get { return (FontFamily)GetValue(FontFamilyProperty); }
      set { SetValue(FontFamilyProperty, value); }
    }


    /// <summary>
    ///     The DependencyProperty for the FontSize property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      System Dialog Font Size
    /// </summary>
    public static readonly DependencyProperty FontSizeProperty =
                TextElement.FontSizeProperty.AddOwner(
                    typeof(VisualsFontPanel),
                    new FrameworkPropertyMetadata(SystemFonts.MessageFontSize,
                        FrameworkPropertyMetadataOptions.Inherits));


    /// <summary>
    ///     The size of the desired font.
    ///     This will only affect intheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public double FontSize {
      get { return (double)GetValue(FontSizeProperty); }
      set { SetValue(FontSizeProperty, value); }
    }


    /// <summary>
    ///     The DependencyProperty for the FontStretch property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      FontStretches.Normal
    /// </summary>
    public static readonly DependencyProperty FontStretchProperty
            = TextElement.FontStretchProperty.AddOwner(typeof(VisualsFontPanel),
                new FrameworkPropertyMetadata(TextElement.FontStretchProperty.DefaultMetadata.DefaultValue,
                    FrameworkPropertyMetadataOptions.Inherits));


    /// <summary>
    ///     The stretch of the desired font.
    ///     This will only affect intheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public FontStretch FontStretch {
      get { return (FontStretch)GetValue(FontStretchProperty); }
      set { SetValue(FontStretchProperty, value); }
    }


    /// <summary>
    ///     The DependencyProperty for the FontStyle property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      System Dialog Font Style
    /// </summary>
    public static readonly DependencyProperty FontStyleProperty =
                TextElement.FontStyleProperty.AddOwner(
                    typeof(VisualsFontPanel),
                    new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle,
                        FrameworkPropertyMetadataOptions.Inherits));


    /// <summary>
    ///     The style of the desired font.
    ///     This will only affect intheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public FontStyle FontStyle {
      get { return (FontStyle)GetValue(FontStyleProperty); }
      set { SetValue(FontStyleProperty, value); }
    }


    /// <summary>
    ///     The DependencyProperty for the FontWeight property.
    ///     Flags:              Can be used in style rules
    ///     Default Value:      System Dialog Font Weight
    /// </summary>
    public static readonly DependencyProperty FontWeightProperty =
                TextElement.FontWeightProperty.AddOwner(
                    typeof(VisualsFontPanel),
                    new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight,
                        FrameworkPropertyMetadataOptions.Inherits));


    /// <summary>
    ///     The weight or thickness of the desired font.
    ///     This will only affect intheriting controls who use the property
    ///     as a parameter. On other controls, the property will do nothing.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public FontWeight FontWeight {
      get { return (FontWeight)GetValue(FontWeightProperty); }
      set { SetValue(FontWeightProperty, value); }
    }
    #endregion
  }
}

