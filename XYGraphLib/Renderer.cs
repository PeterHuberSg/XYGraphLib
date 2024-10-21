/**************************************************************************************

XYGraphLib.Renderer
===================

Creates a Visual for the PlotArea to display.

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


namespace XYGraphLib {


  /// <summary>
  /// Creates a Visual for the PlotArea to display. Inherit from this class if no dataserie is linked to this Renderer. Rendering something on the
  /// screen usually includes using x- and y-Pixel values as coordinates. The first 2 Dimensions of a DataPoint (DimensionX and DimensionY) are
  /// used for this purpose. TranslateValueXYToPoint() provides this conversion.
  /// </summary>
  public abstract class Renderer {
    //                       y ←MinValueY
    //                       y 
    //            ←--Width---→                     
    //          ↑ ┌──────────┐ ←MinDisplayValueY
    //          | |          |                     
    //   Height | | PlotArea |                     
    //          | |          |                      
    // xxxxxxxx ↓ └──────────┘xxxxxxxxxxxxxxxx ←MaxDisplayValueY
    // ↑          ↑          y               ↑                      
    // ┊          ┊          y ←MaxValueY    ┊            
    // ┊          ┊          ↑               ┊  
    // MinIndex   ┊          MaxDisplayIndex ┊  
    //            MinDisplayIndex            MaxIndex


    #region Properties
    //      ----------

    //Dimensions
    //----------
    //A dataSerie consists of dataPoints. A dataPoint has usually at least 2 values. In a time serie, each dataPoint has a date and double for that date. A 
    //dataPoint can have more than 2 values. In a bubble chart, showing circles for region and products, there are 3 values per dataPoint (region, product, double).
    //There could be more data assigned to a dataPoint, like colour, tooltip and more. Each value of a dataPoint is a dimension. Usually the first value (index: 0) is the 
    //x-coordinate and the second value (index: 1) is the y-coordinate.
    //
    //It would be nice to use an enumeration for dimensions, but that would make it difficult for inheritors to add more dimensions. Therefore, they are defined
    //as integer constant. Inheritors should use negative numbers for their dimensions to avoid conflicts with future versions of Renderer.

    /// <summary>
    /// First value in a DataPoint, usually x-axis
    /// </summary>
    public const int DimensionX = 0;
    /// <summary>
    /// Second value in a DataPoint, usually y-axis
    /// </summary>
    public const int DimensionY = 1;
    /// <summary>
    /// Third value in a DataPoint, could be z-axis or any other helpful value like colour
    /// </summary>
    public const int DimensionZ = 2;
    /// <summary>
    /// Fourth value in a DataPoint, can be any helpful value like colour
    /// </summary>
    public const int DimensionA = 3;
    /// <summary>
    /// Fifth value in a DataPoint, can be any helpful value like colour
    /// </summary>
    public const int DimensionB = 4;
    /// <summary>
    /// Sixth value in a DataPoint, can be any helpful value like colour
    /// </summary>
    public const int DimensionC = 5;


    public static readonly int[] DimensionMapX = new int[] { DimensionX };
    public static readonly int[] DimensionMapY = new int[] { DimensionY };
    public static readonly int[] DimensionMapXY = new int[] { DimensionX, DimensionY };
    public static readonly int[] DimensionMapXYZ = new int[] { DimensionX, DimensionY, DimensionZ };


    /// <summary>
    /// A Dimension indicates which value of a DataPoint should be used. A LegendScroller controls which value range for a particular
    /// Dimension the Renderer should display. The mapping between Legend Dimension and the Min/Max-values of the Renderer is not
    /// 1 to 1. Example: A y-grid-line renderer needs only 1 Min/Max pair, which has the index 0, but the dimension of the x-axis legend
    /// is 1 (DimensionY). To find the proper Min/Max pair, one has to search in the DimensionMap the legend's Dimension value. The index 
    /// of the legend's Dimension value entry in the DimensionMap is the index to be used to access Min- and Max-Values.
    public readonly int[] DimensionMap;


    /// <summary>
    /// Lowest value displayed in renderer per dimension.
    /// </summary>
    public double[] MinDisplayValues { get; protected set; }


    /// <summary>
    /// Highest value displayed in renderer per dimension
    /// </summary>
    public double[] MaxDisplayValues { get; protected set; }


    /// <summary>
    /// Called by LegendScroller.DisplayRangeChanged event to set MinDisplayValue and MaxDisplayValue of Renderer
    /// </summary>
    public void DisplayValueChanged(Legend legend) {
      int dimension = legend.Dimension;
      SetDisplayValueRange(dimension, legend.DisplayValue, legend.DisplayValue + legend.DisplayValueRange, legend);
    }


    /// <summary>
    /// Sets Min and Max MaxDisplayIndex in one call. This allows to render the visual only once, even if
    /// both values change. Use the predefined Renderer.DimensionX etc. values for dimension
    /// </summary>
    public void SetDisplayValueRange(int dimension, double min, double max, object source) {
      int  dimensionMapIndex;
      for (dimensionMapIndex = 0; dimensionMapIndex<DimensionMap.Length; dimensionMapIndex++) {
        if (DimensionMap[dimensionMapIndex]==dimension) break;
 
      }
      if (dimensionMapIndex>=DimensionMap.Length){
        string dimensionMapString = "";
        for (dimensionMapIndex = 0; dimensionMapIndex<DimensionMap.Length; dimensionMapIndex++) {
          dimensionMapString = dimensionMapString + "[" + dimensionMapIndex + "]: " + DimensionMap[dimensionMapIndex] + "; ";
        }
        throw new Exception("Cannot find Dimension " + dimension + " in DimensionMap " + dimensionMapString + 
          ". Renderer: " + GetType() + "; Source: " + source.GetType() + ";");
      }

      if (MinDisplayValues[dimensionMapIndex]==min && MaxDisplayValues[dimensionMapIndex]==max) return;

      MinDisplayValues[dimensionMapIndex] = min;
      MaxDisplayValues[dimensionMapIndex] = max;
      RenderingRequested?.Invoke(this);
    }


    #region Rendering
    //      ---------
    /// <summary>
    /// Renderer Id. Used to differentiate the WPF events of multiply renderers
    /// </summary>
    public int RendererId { get; private set; }


    /// <summary>
    /// Raised if rendering is needed
    /// </summary>
    public event Action<Renderer>? RenderingRequested;

    
    /// <summary>
    /// Rendering completed. Used for WPF event tracing
    /// </summary>
    public event Action<Renderer>? Rendered;
    #endregion
    #endregion


    #region Constructor
    //      -----------

    static int nextRendererId = 0;
    protected Pen StrokePen;


    public Renderer(Brush? strokeBrush, double strokeThickness, int[] dimensionMap) {
      RendererId = nextRendererId++;
      StrokePen = new Pen(strokeBrush, strokeThickness);
      DimensionMap = dimensionMap;

      int dimensionCount = DimensionMap.Length;
      MinDisplayValues = new double[dimensionCount];
      MaxDisplayValues = new double[dimensionCount];

      for (int dimensionIndex = 0; dimensionIndex<dimensionCount; dimensionIndex++) {
        MinDisplayValues[dimensionIndex] = double.NaN;
        MaxDisplayValues[dimensionIndex] = double.NaN;
      }
    }
    #endregion


    #region Methods
    //      -------
  
    /// <summary>
    /// Multiplier used to convert an x-value into an x-pixel;
    /// </summary>
    protected double ScaleX;


    /// <summary>
    /// Multiplier used to convert an y-value into an y-pixel;
    /// </summary>
    protected double ScaleY;


    private int dimensionXIndex = int.MinValue;
    private int dimensionYIndex = int.MinValue;


    /// <summary>
    /// Renders the chart graph to a Visual. The graphic gets scaled to the available height and width displaying only 
    /// values between minValueDisplayX and minValueDisplayX. The actual values get cropped between minDisplayValueY 
    /// and maxDisplayValueY.
    /// </summary>
    public Visual CreateVisual(double width, double height) {
      DrawingVisual drawingVisual = new();

      bool areMinMaxDefined = !double.IsNaN(MinDisplayValues[0]) && !double.IsNaN(MaxDisplayValues[0]);
      if (MinDisplayValues.Length>1) {
        areMinMaxDefined = areMinMaxDefined && !double.IsNaN(MinDisplayValues[0]) && !double.IsNaN(MaxDisplayValues[0]);
      }
      if (!areMinMaxDefined || double.IsNaN(width) || double.IsNaN(height)  || width<=0 || height<=0) {
        string message = "Renderer" + RendererId + "(): empty Visual returned";
        if (DimensionX<MinDisplayValues.Length) {
          message += ", MinDisplayValueX: " + MinDisplayValues[DimensionX] + ", MaxDisplayValueX: " + MaxDisplayValues[DimensionX];
        }
        if (DimensionY<MinDisplayValues.Length) {
          message += ", MinDisplayValueY: " + MinDisplayValues[DimensionY] + ", MaxDisplayValueY: " + MaxDisplayValues[DimensionY] + "";
        }
        TraceWpf.Line(message);
        return drawingVisual; //return an empty Visual, not null
      }

      if (DimensionMap[0] == DimensionX) {
        //if there is a DimensionX in DimensionMap, it must be the first entry per convention
        dimensionXIndex = 0;
        double differenceX = MaxDisplayValues[DimensionX] - MinDisplayValues[DimensionX];
        ScaleX = differenceX==0 ? width : width / differenceX;
      } else {
        //DimensionX not used
        dimensionXIndex = int.MinValue;
        ScaleX = double.NaN;
      }

      if (DimensionMap[0] == DimensionY) {
        //if the DimensionMap has only 1 entry, only this one has to be checked
        dimensionYIndex = 0;
      } else if (DimensionMap.Length>1 && DimensionMap[1] == DimensionY) {
        //if the DimensionMap has more than 1 entry, DimensionY is by convention the second entry in DimensionMap
        dimensionYIndex = 1;
      }
      if (dimensionYIndex>int.MinValue) {
        double differenceY = MaxDisplayValues[dimensionYIndex] - MinDisplayValues[dimensionYIndex];
        ScaleY = differenceY==0 ? height : height / differenceY;
      } else {
        //DimensionY not used
        dimensionYIndex = int.MinValue;
        ScaleY = double.NaN;
      }

      using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
        OnCreateVisual(drawingContext, width, height, drawingVisual);
      }

      Rendered?.Invoke(this);
      return drawingVisual;
    }


    /// <summary>
    /// Overwritten by inheritor to generate the actual graphic.
    /// </summary>
    protected abstract void OnCreateVisual(DrawingContext drawingContext, double width, double height, DrawingVisual drawingVisual);


    /// <summary>
    /// Translate the x and y value of a data-point into screen pixel. The x and y values get scaled according to
    /// displayRange and available pixel width or height. The y-pixel is inverted (yPixel = height-yPixel) to accommodate for
    /// screen coordinates. The values get limited to plus/minus 10 times the width or height
    /// </summary>
    protected Point TranslateValueXYToPoint(double[,] dataSerie, int dataPointIndex, double width, double height) {
      return TranslateValueXYToPoint(dataSerie[dataPointIndex, DimensionX], dataSerie[dataPointIndex, DimensionY], width, height);
    }


    protected Point TranslateValueXYToPoint(double x, double y, double width, double height) {
      double xPixel;
      if (dimensionXIndex>int.MinValue) {
        xPixel = ScaleX * (x - MinDisplayValues[dimensionXIndex]);
        if (xPixel>10*width) {
          xPixel = 10*width;
        } else if (xPixel<-10*width) {
          xPixel = -10*width;
        }
      } else {
        xPixel = double.NaN;
      }

      double yPixel;
      if (dimensionYIndex>int.MinValue) {
        yPixel = ScaleY * (y - MinDisplayValues[dimensionYIndex]);
        if (yPixel>10*height) {
          yPixel = 10*height;
        } else if (yPixel<-10*height) {
          yPixel = -10*height;
        }
        yPixel = height - yPixel;
      } else {
        yPixel = double.NaN;
      }

      return new Point(xPixel, yPixel);
    }


    public override string ToString() {
      string minMaxString = "";
      for (int dimensionMapIndex = 0; dimensionMapIndex < DimensionMap.Length; dimensionMapIndex++) {
        int dimension = DimensionMap[dimensionMapIndex];
        minMaxString = minMaxString + Environment.NewLine + "DimensionMap[" + dimensionMapIndex + "]: " + DimensionToString(dimension) + ";" +
          "MinDisplay: " + MinDisplayValues[dimensionMapIndex] + "; " +
          "MaxDisplay: " + MaxDisplayValues[dimensionMapIndex] + "; ";
      }
      return "RendererId: " + RendererId + minMaxString;
    }


    /// <summary>
    /// Converts an integer dimension into "DimensionX, 0" string
    /// </summary>
    public static string DimensionToString(int dimension) {
      if (dimension<3) {
        return "Dimension" + ('X' + dimension) + ", " + dimension;
      } else {
        return "Dimension" + ('A' + dimension-3) + ", " + dimension;
      }
    }
    #endregion
  }
}
