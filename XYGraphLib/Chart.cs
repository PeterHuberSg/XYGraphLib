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


    #region Properties
    //      ----------

    protected readonly SerieSetting[]? SerieSettings;
    protected readonly SerieStyleEnum[]? SerieStyle;
    protected readonly double[][,]? DataSeries;
    #endregion


    #region Constructor
    //      -----------


    /// <summary>
    /// Constructor
    /// </summary>
    public Chart(SerieSetting[]? serieSettings) {
      SerieSettings = serieSettings;
      if (serieSettings is not null) {
        DataSeries = new double[serieSettings.Length][,];
        SerieStyle = new SerieStyleEnum[serieSettings.Length];
        for (int dataSeriesIndex = 0; dataSeriesIndex < serieSettings.Length; dataSeriesIndex++) {
          SerieStyle[dataSeriesIndex] = serieSettings[dataSeriesIndex].SerieStyle;
        }
      }

      IsEnabled = false;
    }
    #endregion


    #region Add to control methods for Constructor
    //      --------------------------------------

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
    /// Add PlotArea to Control
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



    ///// <summary>
    ///// Updates graphic with new data series 
    ///// </summary>
    //public virtual void FillData(IEnumerable<TRecord> newRecords, SerieSetting<TRecord>[] newSerieSettings) {
    //  DataSeries = new double[newSerieSettings.Length][,];
    //  serieStyle = new SerieStyleEnum[newSerieSettings.Length];
    //  //Groups = new int[newSerieSettings.Length];
    //  int recordsCount = newRecords.Count();
    //  double[] firstDataPoint = newSerieSettings[0].Getter(newRecords.First());
    //  int dimensionCount = firstDataPoint.Length;
    //  for (int dataSeriesIndex = 0; dataSeriesIndex < DataSeries.Length; dataSeriesIndex++) {
    //    DataSeries[dataSeriesIndex] = new double[recordsCount, dimensionCount];
    //    serieStyle[dataSeriesIndex] = newSerieSettings[dataSeriesIndex].SerieStyle;
    //    //Groups[dataSeriesIndex] = newSerieSettings[dataSeriesIndex].Group;
    //  }

    //  int recordIndex = 0;
    //  foreach (TRecord record in newRecords) {
    //    for (int dataSerieIndex = 0; dataSerieIndex < newSerieSettings.Length; dataSerieIndex++) {
    //      SerieSetting<TRecord> serieSetting = newSerieSettings[dataSerieIndex];
    //      double[] dataPoint = serieSetting.Getter(record);
    //      for (int dimensionIndex = 0; dimensionIndex < dataPoint.Length; dimensionIndex++) {
    //        DataSeries[dataSerieIndex][recordIndex, dimensionIndex] = dataPoint[dimensionIndex];
    //      }
    //    }
    //    recordIndex++;
    //  }

    //  InvalidateMeasure(); //InvalidateVisual() does not force Measure()
    //  InvalidateVisual();
    //  IsEnabled = true;
    //}

    /// <summary>
    /// Updates graphic with new data series 
    /// </summary>
    public virtual void FillData<TRecord>(IEnumerable<TRecord> records, Func<TRecord, double[]>[] valueReaders) {
      if (SerieSettings is null) throw new Exception("FillData() needs serieSettings which must be provided in the constructor.)");
      
      int recordsCount = records.Count();
      double[] firstDataPoint = valueReaders[0](records.First());
      int dimensionCount = firstDataPoint.Length;
      for (int dataSeriesIndex = 0; dataSeriesIndex < DataSeries!.Length; dataSeriesIndex++) {
        DataSeries[dataSeriesIndex] = new double[recordsCount, dimensionCount];
      }

      int recordIndex = 0;
      foreach (TRecord record in records) {
        for (int dataSerieIndex = 0; dataSerieIndex < SerieSettings.Length; dataSerieIndex++) {
          double[] dataPoint = valueReaders[dataSerieIndex](record);
          for (int dimensionIndex = 0; dimensionIndex < dataPoint.Length; dimensionIndex++) {
            DataSeries[dataSerieIndex][recordIndex, dimensionIndex] = dataPoint[dimensionIndex];
          }
        }
        recordIndex++;
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
    protected Renderer? CreateGraphRenderer(int serieIndex, SerieSetting serieSetting) {

      if (isArea2Expected && serieSetting.SerieStyle!=SerieStyleEnum.area2) {
        throw new Exception(string.Format("SerieStyle[{0}] '{1}, {2}' should be area2 because the previous data series had style area1.", serieIndex, serieSetting.SerieStyle, (int)serieSetting.SerieStyle));
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
        double[][,] lineDataSeries = {DataSeries![serieIndex] };
        return new Renderer1Line(strokeBrush, serieSetting.StrokeThickness, fillBrush, lineDataSeries);

      case SerieStyleEnum.area1:
        isArea2Expected = true;
        areaLineStrokeBrush = strokeBrush;
        areaLineStrokeThickness = serieSetting.StrokeThickness;
        areaLineFillBrush = fillBrush;
        areaLine1DataSerie = DataSeries![serieIndex];
        return null;

      case SerieStyleEnum.area2:
        if (!isArea2Expected) {
          throw new Exception(string.Format("SerieStyle[{0}] '{1}, {2}' should be preceded by serie with style area1.", serieIndex, serieSetting.SerieStyle, (int)serieSetting.SerieStyle));
        }
        isArea2Expected = false;
        //double[][,]()
        double[][,] areaLineDataSeries = { areaLine1DataSerie!, DataSeries![serieIndex] };
        return new Renderer2Lines(areaLineStrokeBrush!, areaLineStrokeThickness, areaLineFillBrush!, areaLineDataSeries);

      default:
        throw new Exception(string.Format("SerieStyle[{0}] '{1}, {2}' not supported.", serieIndex, serieSetting.SerieStyle, (int)serieSetting.SerieStyle));
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
