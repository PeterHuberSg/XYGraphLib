/**************************************************************************************

XYGraphLib.SerieSetting
=======================

Stores the parameters of a LineGraph data serie 

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
using System.Windows.Media;


namespace XYGraphLib {

  /// <summary>
  /// How serie should get displayed in the graph
  /// </summary>
  public enum SerieStyleEnum {
    line,
    area1,
    area2
  }


  /// <summary>
  /// Takes any dataRecord and returns the x and y values in dataExtracted. When GetterDoubleDouble() gets called
  /// for the first time, dataExtracted can be null, but must return a properly sized array. 
  /// </summary>
  public delegate void GetterIndexDoubleDouble<TRecord>(TRecord dataRecord, int index, [NotNull] ref double[]? dataExtracted);


  /// <summary>
  /// Stores the parameters of a LineGraph data serie
  /// </summary>
  public class SerieSetting<TRecord> {
    public GetterIndexDoubleDouble<TRecord> Getter { get; set; }
    public SerieStyleEnum SerieStyle { get; set; }
    public int Group { get; set; }


    /// <summary>
    /// Name used in crosshair for y value
    /// </summary>
    public string? Name { get; set; }


    /// <summary>
    /// Measurement unit used in crosshair for y value
    /// </summary>
    public string? Unit { get; set; }
    public Brush? StrokeBrush { get; set; }
    public double StrokeThickness { get { return strokeThickness; } set { strokeThickness = value; } }
    double strokeThickness = 1;
    public Brush? FillBrush { get; set; }


    /// <summary>
    /// Constructor
    /// </summary>
    public SerieSetting(
      GetterIndexDoubleDouble<TRecord> getter,
      SerieStyleEnum serieStyle,
      Brush? strokeBrush = null,
      double strokeThickness = 1,
      Brush? fillBrush = null,
      string? name = null,
      string? unit = null,
      int group = 0)
    {
      Getter = getter;
      SerieStyle = serieStyle;
      Group = group;
      Name = name;
      Unit = unit;
      StrokeBrush = strokeBrush;
      StrokeThickness = strokeThickness;
      FillBrush = fillBrush; 
    }


    public override string ToString() {
      return 
        "SerieStyle: " + SerieStyle +
        "; Group: " + Group +
        "; StrokeBrush: " + SerieSetting<TRecord>.toString(StrokeBrush) +
        "; StrokeThickness: " + StrokeThickness +
        "; FillBrush: " + SerieSetting<TRecord>.toString(FillBrush);
    }


    private static string toString(Brush? brush) {
      if (brush is null) return "null";

      return brush is SolidColorBrush solidColorBrush ? solidColorBrush.Color.ToString() : brush.ToString();
    }
  }
}
