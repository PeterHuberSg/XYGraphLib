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
  /// Stores the parameters of a LineGraph data serie
  /// </summary>
  public class SerieSetting {
    public SerieStyleEnum SerieStyle { get; set; }
    public int Group { get; set; }
    public Brush StrokeBrush { get; set; }
    public double StrokeThickness { get{return strokeThickness;} set{strokeThickness = value;} }
      double strokeThickness = 1;
    public Brush? FillBrush { get; set; }


    /// <summary>
    /// constructor without FillBrush, which will be null
    /// </summary>
    public SerieSetting(
      SerieStyleEnum newSerieStyle,
      int newGroup,
      Brush newStrokeBrush,
      double newStrokeThickness) :
      this(newSerieStyle, newGroup, newStrokeBrush, newStrokeThickness, null) { }


    /// <summary>
    /// constructor without Group, which will be 0
    /// </summary>
    public SerieSetting(
      SerieStyleEnum newSerieStyle,
      Brush newStrokeBrush,
      double newStrokeThickness,
      Brush? newFillBrush):
      this(newSerieStyle, 0, newStrokeBrush, newStrokeThickness, newFillBrush) { }


    /// <summary>
    /// constructor without Group (=0) nor FillBrush (=null)
    /// </summary>
    public SerieSetting(
      SerieStyleEnum newSerieStyle,
      Brush newStrokeBrush,
      double newStrokeThickness) :
      this(newSerieStyle, 0, newStrokeBrush, newStrokeThickness, null) { }


    /// <summary>
    /// constructor with all parameters
    /// </summary>
    public SerieSetting( 
      SerieStyleEnum newSerieStyle,
      int newGroup,
      Brush newStrokeBrush,
      double newStrokeThickness,
      Brush? newFillBrush) 
    {
      SerieStyle = newSerieStyle;
      Group = newGroup;
      StrokeBrush = newStrokeBrush;
      StrokeThickness = newStrokeThickness;
      FillBrush = newFillBrush; 
    }


    public override string ToString() {
      return 
        "SerieStyle: " + SerieStyle +
        "; Group: " + Group +
        "; StrokeBrush: " + toString(StrokeBrush) +
        "; StrokeThickness: " + StrokeThickness +
        "; FillBrush: " + toString(FillBrush);
    }


    private string toString(Brush? brush) {
      if (brush is null) return "null";

      if (brush is not SolidColorBrush solidColorBrush) return brush.ToString();

      return solidColorBrush.Color.ToString();
    }
  }
}
