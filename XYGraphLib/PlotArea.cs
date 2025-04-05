/**************************************************************************************

XYGraphLib.PlotArea
===================

Controls area where the graphics get drawn.

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
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Windows.Input;
using System.Collections;
using System.Reflection;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using static XYGraphLib.RendererDataSeries;
using System.Data;


/*
 Helpful Links
 ------------- 
 
 Control Authoring Overview: http://msdn.microsoft.com/en-us/library/ms745025(v=vs.100).aspx
 Create a WPF Custom Control, Part 1: http://www.codeproject.com/Articles/49797/Create-a-WPF-Custom-Control-Part-1
 Create a WPF Custom Control, Part 2: http://www.codeproject.com/Articles/49802/Create-a-WPF-Custom-Control-Part-2
 Steps to Create a new Custom Control: http://www.abhisheksur.com/2011/07/writing-reusable-custom-control-in-wpf.html
 
 WPF Graphics Rendering Overview: http://msdn.microsoft.com/en-us/library/ms748373(v=vs.100).aspx
 Shapes and Basic Drawing in WPF Overview: http://msdn.microsoft.com/en-us/library/ms747393.aspx
 Using DrawingVisual Objects: http://msdn.microsoft.com/en-us/library/ms742254(v=vs.100).aspx
 
 WPF Tutorial : Drawing Visual: http://tarundotnet.wordpress.com/2011/05/19/wpf-tutorial-drawing-visual/
 GlyphRun and So Forth: http://smellegantcode.wordpress.com/2008/07/03/glyphrun-and-so-forth/
 2,000 Things You Should Know About WPF: http://wpf.2000things.com/
  */


namespace XYGraphLib {


  /// <summary>
  /// Controls Height and Width of the area where the graphics get drawn. It also manages all graphic renderers, calls a renderer
  /// when it needs to update its graphic visual. Visuals are used because they have less overhead and they permit to render with
  /// going first through measure and arrange.
  /// </summary>
  public class PlotArea: VisualsFrameworkElement {
    //The PlotArea paints its visual children in this sequence:
    //Background: always there
    //Renderers: count varies
    //visuals added if mouse is within PlotArea
    //Crosshair: vertical line at nearest x-value to mouse x position
    //ValuesPanel: Is a Border, not just a Visual and needs to be added to the logical tree

    //Visuals contain only rendering instructions, but no support for events, mouse, etc.
    //ValuesPanel inherits indirectly from Visual, i.e. is a Visual. Since it also a FrameworkElement,
    //it must get added to the logical tree and Measured() and Arranged().


    #region Properties
    //      ----------

    /// <summary>
    /// LegendScrollerY used by this PlotArea
    /// </summary>
    public readonly LegendScrollerY LegendScrollerY;


    /// <summary>
    /// LegendScrollerX used by this PlotArea
    /// </summary>
    public LegendScrollerX? LegendScrollerX { get; internal set; }


    /// <summary>
    /// Brush used to paint the background of PlotArea
    /// </summary>
    public Brush Background {
      get { return (Brush)GetValue(BackgroundProperty); }
      set { SetValue(BackgroundProperty, value); }
    }


    /// <summary>
    /// The DependencyProperty definition for Background.
    /// </summary>
    public static readonly DependencyProperty BackgroundProperty =
      Panel.BackgroundProperty.AddOwner(typeof(PlotArea),
      new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Should the PlotArea display a Crosshair next to the mouse pointer ?
    /// </summary>
    public bool IsShowCrosshair {
      get { return (bool)GetValue(IsShowCrosshairProperty); }
      set { SetValue(IsShowCrosshairProperty, value); }
    }


    /// <summary>
    /// The DependencyProperty definition for IsShowCrosshair.
    /// </summary>
    public static readonly DependencyProperty IsShowCrosshairProperty =
      DependencyProperty.RegisterAttached(
        "IsShowCrosshair", // Property name
        typeof(bool), // Property type
        typeof(PlotArea), // Property owner
        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Should the PlotArea display a ValuesPanel next to the mouse pointer ?
    /// </summary>
    public bool IsShowValuesPanel {
      get { return (bool)GetValue(IsShowValuesPanelProperty); }
      set { SetValue(IsShowValuesPanelProperty, value); }
    }


    /// <summary>
    /// The DependencyProperty definition for IsShowValuesPanel.
    /// </summary>
    public static readonly DependencyProperty IsShowValuesPanelProperty =
      DependencyProperty.RegisterAttached(
        "IsShowValuesPanel", // Property name
        typeof(bool), // Property type
        typeof(PlotArea), // Property owner
        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// ValuesPanel uses XName to name the x data value
    /// </summary>
    public string? XName;


    /// <summary>
    /// ValuesPanel uses XFormat to convert x data value to a string. Often x is a double and the conversion is:
    /// new DateTime((long)X).ToString(XFormat)
    /// </summary>
    public string? XFormat;


    /// <summary>
    /// ValuesPanel uses XUnit to display the x data value with a measurement unit
    /// </summary>
    public string? XUnit;


    /// <summary>
    /// Copy of Chart.XValues, 
    /// </summary>
    public double[]? XValues;


    /// <summary>
    /// Copy of Chart.LegendXString.LegendStrings, 
    /// </summary>
    public string[]? LegendXStrings;


    /// <summary>
    /// Number of Renderers registered with PlotArea
    /// </summary>
    public int RendererCount { get { return renderers.Count; } }

    
    /// <summary>
    /// Raised when renderer gets added to PlotArea. Is used to added WPF tracing to renderer.
    /// </summary>
    public event Action<Renderer>? RendererAdded;
    #endregion


    #region Constructor
    //      -----------

    static int plotAreaIndex;
    readonly Crosshair crosshair;
    ValuesPanel? valuesPanel;


    /// <summary>
    /// Default constructor
    /// </summary>
    public PlotArea(
      LegendScrollerY? legendScrollerY, 
      string? plotAreaName = null, 
      Pen? crosshairPen = null, 
      string? xName = null,
      string? xFormat = null,
      string? xUnit = null) 
    {
      this.LegendScrollerY = legendScrollerY ?? new LegendScrollerY();
      Name = plotAreaName ?? $"PlotArea{plotAreaIndex++}";
      crosshairPen = crosshairPen??new() {Brush=Brushes.DimGray, DashStyle=DashStyles.Dash, Thickness=1};
      crosshair = new Crosshair(crosshairPen);
      XName = xName;
      XFormat = xFormat;
      XUnit = xUnit;

      ClipToBounds = true;

      MouseEnter += PlotArea_MouseEnter;
      MouseMove += PlotArea_MouseMove;
      MouseLeave += PlotArea_MouseLeave;
    }
    #endregion


    #region Events
    //      ------

    bool isMouseEnter = false;//is only true when mouse enters PlotArea, but gets false when mouse moves within PlotArea
    bool isMouseInsidePlotArea = false;//is true when mouse is within PlotArea
    Point mousePosition;
    const int distanceFromCrosshair = 10;
    int existingXValueIndex = int.MinValue;
    double existingXValue = double.MinValue; //existingXValue = XValues[existingXValueIndex]
    double crosshairX = double.MinValue; //x coordinate of crosshair


    private void PlotArea_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
      if (!IsShowCrosshair && !IsShowValuesPanel) return;

      isMouseEnter = true;
      isMouseInsidePlotArea = true;
      mousePosition = Mouse.GetPosition(this);
      (existingXValueIndex, existingXValue, crosshairX) = findNearestExisting(mousePosition);
      InvalidateVisual();

      if (valuesPanel is null && rendererDataSeriesList.Count>0) {
        //create new valuesPanel
        var rowCount = 1;//first row is for x (date)
        foreach (var rendererDataSerie in rendererDataSeriesList) {
          rowCount += rendererDataSerie.YSeries.Length;
        }
        var rowConfigs = new ValuesPanel.RowConfig[rowCount];
        rowValueStrings = new string[rowCount];
        var rowIndex = 0;
        rowConfigs[rowIndex++] = new ValuesPanel.RowConfig(
          null,
          XName,
          XUnit);

        foreach (var rendererDataSerie in rendererDataSeriesList) {
          foreach (var ySerie in rendererDataSerie.YSeries) {
            rowConfigs[rowIndex++] = new ValuesPanel.RowConfig(
              rendererDataSerie.StrokePen.Brush,
              ySerie.Name,
              ySerie.Unit);
          }
        }
        valuesPanel = new ValuesPanel(rowConfigs) {
          VerticalAlignment = VerticalAlignment.Top,
          HorizontalAlignment = HorizontalAlignment.Left
        };
      }

      if (IsShowValuesPanel) {
        //add valuesPanel to logical and visual tree
        if (Visuals.Contains(valuesPanel)) {
          System.Diagnostics.Debugger.Break();//we should never come here, because PlotArea_MouseLeave should have removed valuesPanel
        } else {
          Visuals.Add(valuesPanel);
          AddLogicalChild(valuesPanel); //equivalent to Child.Parent = this;
        }
        updateValuesPanel(existingXValueIndex, mousePosition);
      }


    }


    private void PlotArea_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
      if (!IsShowCrosshair && !IsShowValuesPanel) return;

      isMouseEnter = false;
      isMouseInsidePlotArea = true;
      //lookup xValue nearest to mouse position
      mousePosition = Mouse.GetPosition(this);
      (var newValueIndex, existingXValue, crosshairX) = findNearestExisting(mousePosition);
      if (existingXValueIndex!=newValueIndex) {
        existingXValueIndex = newValueIndex;

        if (IsShowValuesPanel) {
          updateValuesPanel(existingXValueIndex, mousePosition);
        }
        InvalidateVisual();
      }
    }


    bool isValuesPanelMeasurementNeeded = false;


    private void updateValuesPanel(int existingValueIndex, Point mousePosition) {
      var rowIndex = 0;
      var date = new DateTime(((long)existingXValue)*10000);
      if (LegendXStrings is null) {
        rowValueStrings![rowIndex++] = XFormat is null ? date.ToString("dd.MM.yyyy" + Environment.NewLine + "hh:mm:ss") : date.ToString(XFormat);
      } else {
        rowValueStrings![rowIndex++] = LegendXStrings[existingValueIndex];
      }

      foreach (var rendererDataSerie in rendererDataSeriesList) {
        foreach (var ySerie in rendererDataSerie.YSeries) {
          rowValueStrings[rowIndex++] = ySerie.Format is null
            ? ySerie.Values[existingValueIndex, 1].ToString()
            : ySerie.Values[existingValueIndex, 1].ToString(ySerie.Format);
        }
      }

      //fill ValuesPanel with data
      valuesPanel!.Update(rowValueStrings);
      isValuesPanelMeasurementNeeded = true;
    }


    private void PlotArea_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
      if (!IsShowCrosshair && !IsShowValuesPanel) return;

      isMouseEnter = false;
      isMouseInsidePlotArea = false;
      existingXValueIndex = int.MinValue;
      existingXValue = double.MinValue;
      crosshairX = double.MinValue;
      if (IsShowValuesPanel) {
        RemoveLogicalChild(valuesPanel); //equivalent to Child.Parent = null;
        Visuals.Remove(valuesPanel);
      }
      InvalidateVisual();
    }
    #endregion


    #region Methods
    //      -------

    const int firstRendererVisual = 1; //at 0 is the background renderer
    readonly List<Renderer> renderers = new();


    /// <summary>
    /// Removes all Renderers from PlotArea and add backs grid line renderers
    /// </summary>
    public void ResetRenderers() {
      renderers.Clear();
      rendererDataSeriesList.Clear();
      if (Visuals.Count>firstRendererVisual) {
        //remove all renderer visuals, but leave background visual
        Visuals.RemoveRange(firstRendererVisual, Visuals.Count-firstRendererVisual);
      }

      //add back grid lines in case the chart stays empty
      AddRenderer(new RendererGridLineX(LegendScrollerY, Brushes.DarkGray, 1));
      AddRenderer(new RendererGridLineY(LegendScrollerX!, Brushes.DarkGray, 1));
    }


    /// <summary>
    /// used by Crosshair to find y values based on cursor x position
    /// </summary>
    readonly List<RendererDataSeries> rendererDataSeriesList = new();
    bool isRendererAdded;


    /// <summary>
    /// Adds one renderer to PlotArea
    /// </summary>
    /// <param name="renderer"></param>
    public void AddRenderer(Renderer renderer) {
      renderers.Add(renderer);
      renderer.Add(this);
      if (renderer is not RendererGridLineX) {
        //only RendererGridLineX doesn't need LegendScrollerX !
        LegendScrollerX!.AddRenderer(renderer);
      }
      if (renderer is not RendererGridLineY) {
        //only RendererGridLineY doesn't need LegendScrollerY !
        LegendScrollerY.AddRenderer(renderer);
      }
      //When a new renderer gets added, first the legends have to be calculated again, which might change the width of the legend and
      //in consequence also the width of the Plot-area
      isRendererAdded = true;
      InvalidateVisual();

      RendererAdded?.Invoke(renderer);

      //setup references to the renderers crosshair needs to display the x and y values at the cursor x location.
      if (renderer is RendererDataSeries rendererDataSeries) rendererDataSeriesList.Add(rendererDataSeries);
    }


    /// <summary>
    /// Replaces the old Visual created by this Renderer with a new one
    /// </summary>
    internal void UpdateVisual(Renderer renderer) {
      if (Visuals.Count<=firstRendererVisual) {
        //Background not added yet => onRender will get executed later, which will add the visuals for the Renderer;
        //nothing to do now
      } else {
        int rendererIndex = renderers.IndexOf(renderer);
        if (rendererIndex==-1) throw new Exception($"UpdateVisual: renderer '{renderer}' not found in renderers (Count: " + renderers.Count + ").");

        int visualIndex = firstRendererVisual + rendererIndex;
        Visuals.RemoveAt(visualIndex);
        Visuals.Insert(visualIndex, renderer.CreateVisual(ActualWidth, ActualHeight));
      }
    }
    #endregion


    #region Overrides
    //      ---------

    protected override Size MeasureOverride(Size availableSize) {
      if (RendererCount==0) {
        //empty graphic. Add at least the grid lines
        AddRenderer(new RendererGridLineX(LegendScrollerY, Brushes.DarkGray, 1));
        AddRenderer(new RendererGridLineY(LegendScrollerX!, Brushes.DarkGray, 1));
      }

      //use all available size, unless size is infinite, then use 0
      Size desiredSize = availableSize;
      if (double.IsInfinity(desiredSize.Width)) desiredSize.Width = 0;
      if (double.IsInfinity(desiredSize.Height)) desiredSize.Height = 0;
      return desiredSize;
    }


    string[]? rowValueStrings;


    //int logicalChildrenCount() {
    //  var childrenCount = 0;
    //  while (LogicalTreeHelper.GetChildren(this).GetEnumerator().MoveNext()) childrenCount++;
    //  return childrenCount;
    //}


    bool isLeft;
    bool isTop;
    double valuesPanelLeft = int.MinValue;
    double valuesPanelTop = int.MinValue;


    protected override Size ArrangeOverride(Size finalSize) {
      if (isValuesPanelMeasurementNeeded) {
        isValuesPanelMeasurementNeeded = false;
        valuesPanel!.Measure(finalSize);

        if (isMouseEnter) {
          //mouse just entered PlotArea
          isLeft = crosshairX<finalSize.Width/2;
          isTop = mousePosition.Y<finalSize.Height/2;

        } else {
          //mouse moves inside PlotArea
          //if valuesPanel is displayed left of the cursor, there comes a time when there is no more enough space left of
          //the cursor to display the valuesPanel and valuesPanel needs to be displayed right of the cursor.
          if (isLeft) {
            if (crosshairX>finalSize.Width - valuesPanel!.DesiredSize.Width - 2*distanceFromCrosshair) {
              isLeft = false;
            }
          } else {
            if (crosshairX<valuesPanel!.DesiredSize.Width + 2*distanceFromCrosshair) {
              isLeft = true;
            }
          }
          if (isTop) {
            if (mousePosition.Y>finalSize.Height - valuesPanel.DesiredSize.Height - 2*distanceFromCrosshair) {
              isTop = false;
            }
          } else {
            if (mousePosition.Y<valuesPanel.DesiredSize.Height + 2*distanceFromCrosshair) {
              isTop = true;
            }
          }
        }

        //adjust left position of valuesPanel
        if (valuesPanel.DesiredSize.Width>ActualWidth - 2*distanceFromCrosshair) {
          valuesPanelLeft = distanceFromCrosshair;
        } else if (isLeft) {
          valuesPanelLeft = crosshairX + distanceFromCrosshair;
          if (valuesPanelLeft + valuesPanel.DesiredSize.Width + distanceFromCrosshair > ActualWidth) {
            valuesPanelLeft = ActualWidth - distanceFromCrosshair - valuesPanel.DesiredSize.Width;
            isLeft = false;//prevents isLeft from 
          }
        } else {
          valuesPanelLeft = crosshairX - distanceFromCrosshair - valuesPanel.DesiredSize.Width;
          if (valuesPanelLeft - distanceFromCrosshair < 0) {
            valuesPanelLeft = distanceFromCrosshair;
            isLeft = true;
          }
        }

        //adjust top position of valuesPanel
        if (valuesPanel.DesiredSize.Height>ActualHeight - 2*distanceFromCrosshair) {
          valuesPanelTop = distanceFromCrosshair;
        } else if (isTop) {
          valuesPanelTop = mousePosition.Y + distanceFromCrosshair;
          if (valuesPanelTop + valuesPanel.DesiredSize.Height + distanceFromCrosshair > ActualHeight) {
            valuesPanelTop = ActualHeight - distanceFromCrosshair - valuesPanel.DesiredSize.Height;
            isTop = false;//prevents isTop from 
          }
        } else {
          valuesPanelTop = mousePosition.Y - distanceFromCrosshair - valuesPanel.DesiredSize.Height;
          if (valuesPanelTop - distanceFromCrosshair < 0) {
            valuesPanelTop = distanceFromCrosshair;
            isTop = true;
          }
        }
        valuesPanel?.Arrange(new Rect(new Point(valuesPanelLeft, valuesPanelTop), valuesPanel.DesiredSize));
      }

      //use all available size
      return finalSize;
    }


    double oldActualWidth = double.NaN;
    double oldActualHeight = double.NaN;
    DrawingVisual? crosshairVisual;


    protected override void OnRender(DrawingContext drawingContext) {
      if (double.IsNaN(ActualWidth) ||double.IsNaN(ActualHeight)) return;//not ready for rendering yet

      if (isRendererAdded || oldActualWidth!=ActualWidth || oldActualHeight!=ActualHeight) {
        //Size has changed. Recreate all Visuals
        oldActualWidth = ActualWidth;
        oldActualHeight = ActualHeight;
        isRendererAdded = false;

        Visuals.Clear();
        Visuals.Add(createBackGroundVisual());
        foreach (Renderer renderer in renderers) {
          Visuals.Add(renderer.CreateVisual(ActualWidth, ActualHeight));
        }
      }

      if (IsShowCrosshair) {
        //handle crosshair 
        if (crosshairVisual is not null) {
          Visuals.Remove(crosshairVisual);
          crosshairVisual = null;
        }
        if (isMouseInsidePlotArea && IsShowCrosshair) {
          //draw  crosshair
          var crosshairX = (existingXValue - LegendScrollerX!.DisplayValue) * ActualWidth / LegendScrollerX!.DisplayValueRange;
          crosshairVisual = crosshair.CreateVisual(crosshairX, ActualWidth, ActualHeight);
          Visuals.Add(crosshairVisual);
        }
      }
    }


    (int valueIndex, double valueX, double crosshairX) findNearestExisting(Point mousePosition) {
      var valueIndex = int.MinValue;
      var valueX = LegendScrollerX!.DisplayValue + mousePosition.X / ActualWidth * LegendScrollerX!.DisplayValueRange;
      if (valueX<XValues![0]) {
        valueIndex = 0;
        valueX = XValues[0];
      } else if (valueX>XValues[^1]) {
        valueIndex = XValues.Length - 1;
        valueX = XValues[^1];
      } else {
        var low = 0;
        var high = XValues.Length - 1;
        double mediumValue;
        int mediumIndex;
        bool hasFoundValue = false;
        while (low<=high) {
          mediumIndex = low + ((high - low) >> 1);
          mediumValue = XValues[mediumIndex];
          if (mediumValue==valueX) {
            //found exact value
            valueIndex = mediumIndex;
            hasFoundValue = true;
            break;
          }

          if (mediumValue<valueX) {
            low = mediumIndex + 1;
          } else {
            high = mediumIndex - 1;
          }
        }

        if (!hasFoundValue) {
          //no exact value found. Return the nearest value
          //note: low > high when loop exits
          var higherValue = XValues[low];
          var highDif = higherValue - valueX;
          var lowerValue = XValues[high];
          var lowerDif = valueX - lowerValue;
          if (highDif<lowerDif) {
            valueIndex = low;
            valueX = higherValue;
          } else {
            valueIndex = high;
            valueX = lowerValue;
          }
        }
      }
      var crosshairX = (valueX - LegendScrollerX!.DisplayValue) * ActualWidth / LegendScrollerX!.DisplayValueRange;
      return (valueIndex, valueX, crosshairX);
    }


    private Visual createBackGroundVisual() {
      DrawingVisual drawingVisual = new();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
        drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
      }
      return drawingVisual;
    }
    #endregion
  }
}

