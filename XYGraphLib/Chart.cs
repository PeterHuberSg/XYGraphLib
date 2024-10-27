/**************************************************************************************

XYGraphLib.Chart
================

Base class for all Chart classes, which display the dataPoints of some DataSeries in a graphic together with legends.

Written 2014-2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/

// 
// ┌─────────────┬─────────┐
// │             │Legend   │
// │ PlotArea    │Scroller │
// │             │Y        │
// ├─────────────┼─────────┤              
// │Legend       │  100 %  │ ← Total Reset ZoomButton
// │Scroller     ├────┬────┤              
// │X            │ +  │ -  │ ← Total ZoomButtons, they zoom in and out for X and Y simultaneously
// └─────────────┴────┴────┘
// 
// PlotArea: where line graphic gets painted
// LegendScroller: can be used to zoom in & out and scroll, displays also the legend and 
//                 ZoomButtons for that axis
// Total ZoomButtons: allow to zoom in and out both for x and y axis at the same time.
// 
// 
// A chart can hold several PlotAreas and LegendScrollers at the same time. XYGraphLib provides
// some preconfigured charts:
// Chart1Plot1X1YLegend: 1 PlotArea, 1 LegendScrollerX, 1 LegendScrollerY. See sample above
//
// Chart2Plots1X2YLegends:
// ┌────────────────┬────────────────────┐
// │ PlotArea0      │ LegendScrollerY0   │
// ├────────────────┼────────────────────┤
// │ PlotArea1      │ LegendScrollerY1   │
// ├────────────────┼────────────────────┤
// │LegendScrollerX │ Total Zoom Buttons │
// └────────────────┴────────────────────┘
//
// Chart4Plots1X4YLegends:
// ┌────────────────┬────────────────────┐
// │ PlotArea0      │ LegendScrollerY0   │
// ├────────────────┼────────────────────┤
// │ PlotArea1      │ LegendScrollerY1   │
// ├────────────────┼────────────────────┤
// │ PlotArea2      │ LegendScrollerY2   │
// ├────────────────┼────────────────────┤
// │ PlotArea3      │ LegendScrollerY3   │
// ├────────────────┼────────────────────┤
// │XLegendScroller │ Total Zoom Buttons │
// └────────────────┴────────────────────┘
//
//you can create your own combination by inheriting from Chart


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CustomControlBaseLib;


namespace XYGraphLib {
  /// <summary>
  /// Base class for all Chart classes, which display the dataPoints of some DataSeries in a graphic together with legends.
  /// </summary>
  public abstract class Chart: CustomControlBase, IZoom {


    #region Events
    //      ------

    /// <summary>
    /// Called when a renderer gets created. Useful for tracing WPF events
    /// </summary>
    public event Action<Renderer>? RendererCreated;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Constructor
    /// </summary>
    public Chart() {
      IsEnabled = false;
    }
    #endregion


    #region Add control methods for Constructor
    //      -----------------------------------

    protected readonly List<PlotArea> PlotAreas = new();
    protected readonly List<LegendScrollerY> LegendScrollerYs = new();
    protected readonly List<LegendScrollerX> LegendScrollerXs = new();
    protected readonly List<IZoom> Zoomers = new();


    /// <summary>
    /// Add PlotArea to Control
    /// </summary>
    protected PlotArea Add(PlotArea newPlotArea) {
      PlotAreas.Add(newPlotArea);
      AddChild(newPlotArea);
      return newPlotArea;
    }


    /// <summary>
    /// Add LegendScrollerX to control
    /// </summary>
    protected LegendScrollerX Add(LegendScrollerX newLegendScrollerX) {
      LegendScrollerXs.Add(newLegendScrollerX);
      Zoomers.Add(newLegendScrollerX);
      newLegendScrollerX.VerticalAlignment = System.Windows.VerticalAlignment.Top;
      newLegendScrollerX.ZoomStateChanged += legendScroller_ZoomStateChanged;
      AddChild(newLegendScrollerX);
      return newLegendScrollerX;
    }


    /// <summary>
    /// Add LegendScrollerY to control
    /// </summary>
    protected LegendScrollerY Add(LegendScrollerY newLegendScrollerY) {
      LegendScrollerYs.Add(newLegendScrollerY);
      Zoomers.Add(newLegendScrollerY);
      newLegendScrollerY.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      newLegendScrollerY.ZoomStateChanged += legendScroller_ZoomStateChanged;
      AddChild(newLegendScrollerY);
      return newLegendScrollerY;
    }


    protected ZoomButton? TotalZoomInButton;
    protected ZoomButton? TotalZoomOutButton;
    protected Button? TotalZoom100Button;


    /// <summary>
    /// Add ZoomButtons to Control
    /// </summary>
    protected void AddZoomButtons() {
      Brush strokeBrush = Brushes.DarkSlateGray;
      TotalZoomInButton = new ZoomButton(true, strokeBrush) {
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center
      };
      TotalZoomInButton.Click += totalZoomInButton_Click;
      AddChild(TotalZoomInButton);

      TotalZoomOutButton = new ZoomButton(false, strokeBrush) {
        IsEnabled = false,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center
      };
      TotalZoomOutButton.Click += totalZoomOutButton_Click;
      AddChild(TotalZoomOutButton);

      TotalZoom100Button = new Button {
        Content = "100%",
        IsEnabled = false
      };
      TotalZoom100Button.Click += totalZoom100Button_Click;
      AddChild(TotalZoom100Button);
    }
    #endregion


    #region Fill Data
    //      ---------

    /*----------------------------------------------------------------------------------------------------------
    A line chart can display data as they are produced in a measurement. A measurement is taken at one point of 
    time and might cover several values. FillData() takes IEnumerable<TRecord> newRecords, which gives easy 
    access to all values at one particular time. A line graph displays only one value of each measurement, for 
    example one line for the first values and another line for the second values. FillData() stores all first 
    values together in DataSeries as an array, then all second values in another array to make the life easier of 
    LegendX and Renderers.

    IEnumerable<TRecord> newRecords => double[][,]? DataSeries
    ----------------------------------------------------------------------------------------------------------*/

    protected double[][,]? DataSeries;
    SerieStyleEnum[]? serieStyle;
    protected string? XName;
    protected string? XUnit;


    /// <summary>
    /// Updates graphic with new data series 
    /// </summary>
    public virtual void FillData<TRecord>(
      IEnumerable<TRecord> records,
      SerieSetting<TRecord>[] serieSettings,
      string? xName = null,
      string? xUnit = null,
      Func<TRecord, string>? stringGetter = null) 
    {
      DataSeries = new double[serieSettings.Length][,];
      serieStyle = new SerieStyleEnum[serieSettings.Length];
      XName = xName;
      XUnit = xUnit;
      int recordsCount = records.Count();
      double[]? dataExtracted = null;
      serieSettings[0].Getter(records.First(), 0, ref dataExtracted);
      int dimensionCount = dataExtracted.Length;
      for (int dataSeriesIndex = 0; dataSeriesIndex < DataSeries.Length; dataSeriesIndex++) {
        DataSeries[dataSeriesIndex] = new double[recordsCount, dimensionCount];
        serieStyle[dataSeriesIndex] = serieSettings[dataSeriesIndex].SerieStyle;
      }

      int recordIndex = 0;
      foreach (TRecord record in records) {
        for (int dataSerieIndex = 0; dataSerieIndex<serieSettings.Length; dataSerieIndex++) {
          SerieSetting<TRecord> serieSetting = serieSettings[dataSerieIndex];
          serieSetting.Getter(record, dataSerieIndex, ref dataExtracted);
          for (int dimensionIndex = 0; dimensionIndex < dataExtracted.Length; dimensionIndex++) {
            DataSeries[dataSerieIndex][recordIndex, dimensionIndex] = dataExtracted[dimensionIndex];
          }
        }
        recordIndex++;
      }

      //handle x legends with strings
      //the code is here and not in the loop above because LegendXString is seldom used
      var legendXString = LegendScrollerXs[0].Legend as LegendXString;
      if (stringGetter is null) {
        if (legendXString is not null)  
        throw new ArgumentException("When a LegendXString is used, the stringGetter argument in FillData() cannot be null.");

      } else { 
        if (legendXString is null) throw new NotSupportedException(
          "stringGetter should only be defined when LegendScrollerXs[0].Legend is a LegendXString, but it was " +
          $"a '{LegendScrollerXs[0].Legend.GetType().Name}'.");

        var legendXStrings = new string[recordsCount];
        recordIndex = 0;
        foreach (TRecord record in records) {
          legendXStrings[recordIndex++] = stringGetter(record);
        }
        legendXString.LegendStrings = legendXStrings;
      }

      InvalidateMeasure(); //InvalidateVisual() does not force Measure()
      InvalidateVisual();
      IsEnabled = true;
    }
    #endregion


    #region Renderers
    //      ---------

    bool isArea2Expected = false;
    Brush? areaLineStrokeBrush;
    double areaLineStrokeThickness;
    Brush? areaLineFillBrush;
    double[,]? areaLine1DataSerie;


    /// <summary>
    /// returns a new renderer based on serieSetting
    /// </summary>
    protected Renderer? CreateGraphRenderer<TRecord>(int serieIndex, SerieSetting<TRecord> serieSetting) {

      if (isArea2Expected && serieSetting.SerieStyle!=SerieStyleEnum.area2) {
        throw new Exception($"SerieStyle[{serieIndex}] '{serieSetting.SerieStyle}, {(int)serieSetting.SerieStyle}' should be area2 because the previous data series had style area1.");
      }
      //get stroke brush or default brush
      Brush strokeBrush;

      if (serieSetting.StrokeBrush is null) {
        //use default brushes
        strokeBrush = serieIndex switch {
          0 => Brushes.LightGreen,
          1 => Brushes.LightBlue,
          2 => Brushes.LightGray,
          3 => Brushes.LightGray,
          4 => Brushes.Black,
          _ => Brushes.Red
        };
      } else {
        strokeBrush = serieSetting.StrokeBrush;
      }

      //get fill brush or use transparent version of stroke brush
      Brush? fillBrush = null;
      if (serieSetting.SerieStyle==SerieStyleEnum.line) {
        fillBrush = serieSetting.FillBrush;
      } else if (serieSetting.SerieStyle==SerieStyleEnum.area1) {
        if (serieSetting.FillBrush is null) {
          if (strokeBrush is SolidColorBrush strokeSolidColorBrush) {
            Color strokeColor = strokeSolidColorBrush.Color;
            fillBrush = new SolidColorBrush(Color.FromArgb(128, strokeColor.R, strokeColor.G, strokeColor.B));
          } else {
            fillBrush = new SolidColorBrush(Color.FromArgb(128, 240, 240, 240));
          }
        } else {
          fillBrush = serieSetting.FillBrush;
        }
      }

      switch (serieSetting.SerieStyle) {
      case SerieStyleEnum.line:
        return new Renderer1Line(strokeBrush, serieSetting.StrokeThickness, fillBrush, new double[][,] { DataSeries![serieIndex] }, serieSetting.Name, 
          serieSetting.Unit);

      case SerieStyleEnum.area1:
        isArea2Expected = true;
        areaLineStrokeBrush = strokeBrush;
        areaLineStrokeThickness = serieSetting.StrokeThickness;
        areaLineFillBrush = fillBrush;
        areaLine1DataSerie = DataSeries![serieIndex];
        return null;

      case SerieStyleEnum.area2:
        if (!isArea2Expected) {
          throw new Exception($"SerieStyle[{serieIndex}] '{serieSetting.SerieStyle}, {(int)serieSetting.SerieStyle}' should be preceded by serie with style area1.");
        }
 
        isArea2Expected = false;
        double[][,] areaLineDataSeries = { areaLine1DataSerie!, DataSeries![serieIndex] };
        return new Renderer2Lines(areaLineStrokeBrush!, areaLineStrokeThickness, areaLineFillBrush!, areaLineDataSeries,
          serieSetting.Name, serieSetting.Unit);

      default:
        throw new Exception($"SerieStyle[{serieIndex}] '{serieSetting.SerieStyle}, {(int)serieSetting.SerieStyle}' not supported.");
      }
    }


    protected RendererNotes CreateNotesRenderer(IEnumerable<ChartNote> chartNotes, FontDefinition[] fontDefinitions) {
      RendererNotes rendererNotes = new(chartNotes, this, fontDefinitions);
      return rendererNotes;
    }

    
    protected void AddRenderer(Renderer renderer, PlotArea plotArea, LegendScrollerX? legendScrollerX, LegendScrollerY? legendScrollerY) {
      plotArea.AddRenderer(renderer);
      legendScrollerX?.AddRenderer(renderer);
      legendScrollerY?.AddRenderer(renderer);
      RendererCreated?.Invoke(renderer);
    }
    #endregion


    #region Zoom
    //      ----

    /// <summary>
    /// Is ZoomOut() possible ?
    /// </summary>
    public bool CanZoomIn { get; private set; }


    /// <summary>
    /// Is ZoomOut() possible ?
    /// </summary>
    public bool CanZoomOut { get; private set; }


    /// <summary>
    /// Raised when CanZoomIn or CanZoomOut have changed
    /// </summary>
    public event Action<IZoom>? ZoomStateChanged;


    /// <summary>
    /// Zooms in one step (showing more details) in all directions for all plot areas
    /// </summary>
    public void ZoomIn(){
      if (CanZoomIn) {
        foreach (IZoom zoomer in Zoomers) {
          zoomer.ZoomIn();
        }
      }
    }


    /// <summary>
    /// Zooms in one step out (showing less details) all directions for all plot areas
    /// </summary>
    public void ZoomOut(){
      if (CanZoomOut) {
        foreach (IZoom zoomer in Zoomers) {
          zoomer.ZoomOut();
        }
      }
    }


    /// <summary>
    /// Zooms out as much as possible, i.e. shows complete graphic / data
    /// </summary>
    public void ZoomReset(){
      if (CanZoomOut) {
        foreach (IZoom zoomer in Zoomers) {
          zoomer.ZoomReset();
        }
      }
    }


    private void legendScroller_ZoomStateChanged(object sender) {
      bool canZoomOut = false;
      bool canZoomIn = false;
      foreach (IZoom zoomer in Zoomers) {
        if (zoomer.CanZoomIn) {
          canZoomIn = true;
        }
        if (zoomer.CanZoomOut) {
          canZoomOut = true;
        }
      }
      if (CanZoomIn!=canZoomIn || CanZoomOut!=canZoomOut) {
        CanZoomIn = canZoomIn;
        CanZoomOut = canZoomOut;
        TotalZoom100Button!.IsEnabled = TotalZoomOutButton!.IsEnabled =  canZoomOut;
        TotalZoomInButton!.IsEnabled =  canZoomIn;
        ZoomStateChanged?.Invoke(this);
      }
    }


    void totalZoomInButton_Click(object sender, RoutedEventArgs e) {
      ZoomIn();
    }


    void totalZoomOutButton_Click(object sender, RoutedEventArgs e) {
      ZoomOut();
    }


    void totalZoom100Button_Click(object sender, RoutedEventArgs e) {
      ZoomReset();
    }
    #endregion  
  }
}
