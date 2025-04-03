/**************************************************************************************

XYGraphLib.RendererDataSeries
=============================

Creates a Visual for a series of data to be displayed in the PlotArea.

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
  /// Creates a Visual for the PlotArea to display. Inherit from this class if the this Renderer uses DataSerie(s).
  /// </summary>
  public abstract class RendererDataSeries: Renderer {
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


    /// <summary>
    /// Data needed by Renderer to display one y value.
    /// </summary>
    /// <param name="Name">Name for Y value displayed in crosshair</param>
    /// <param name="Format">Crosshair uses Format to convert the y data value to a string. Y is a double and the conversion is:
    /// X.ToString(XFormat)</param>
    /// <param name="Unit">Measurement unit for Y value displayed in crosshair</param>
    /// <param name="Values">Values to be displayed by Renderer</param>
    public readonly record struct YSerie(
      string? Name,
      string? Format,
      string? Unit,
      double[,] Values
    );




    #region Properties
    //      ----------

    /// <summary>
    /// Lowest value stored in the data-records per dimension
    /// </summary>
    public readonly double[] MinValues;


    /// <summary>
    /// Highest value stored in the data-records per dimension
    /// </summary>
    public readonly double[] MaxValues;


    /// <summary>
    /// If the values of one dimension are sorted ascending, the drawing speed can get optimised when displaying only a subset
    /// </summary>
    public readonly bool[] IsYSerieSorted;


    /// <summary>
    /// YSeries contains the data displayed by the Renderer. YSerie.Values contains all the (x,y) values of 1 line.
    /// A renderer displaying an area needs 2 border lines and therefore 2 YSeries.
    /// </summary>
    protected internal readonly YSerie[] YSeries;
    #endregion


    #region Constructor
    //      -----------

    public RendererDataSeries(
      Brush strokeBrush, 
      double strokeThickness, 
      int[] dimensionMap,
      YSerie[] ySeries) :
      base(strokeBrush, strokeThickness, dimensionMap)
    {
      var ySeriesDimensionCount = ySeries[0].Values.GetLength(1);
      var dimensionCount = DimensionMap.Length;
      if (dimensionCount!=ySeriesDimensionCount) {
        throw new Exception("Renderer was set up with " + DimensionMap.Length + " dimensions. The DataPoints in the DataSeries should have the same number of dimensions, but" + 
        "the dataPoints in dataSeries[0] have " + ySeriesDimensionCount + " dimensions.");
      }

      YSeries = ySeries;

      //find min and max value within dataSeries and check if it is sorted
      IsYSerieSorted = new bool[ySeriesDimensionCount];
      MinValues = new double[ySeriesDimensionCount];
      MaxValues = new double[ySeriesDimensionCount];
      for (int serieIndex = 0; serieIndex<IsYSerieSorted.Length; serieIndex++) {
			  IsYSerieSorted[serieIndex] = true;
        MinValues[serieIndex] = double.MaxValue;
        MaxValues[serieIndex] = double.MinValue;
      }

      //foreach (double[,] dataSerie in dataSeries){
      //  int dataSerieLength = dataSerie.GetLength(0);
      //  for (int dataPointIndex = 0; dataPointIndex < dataSerieLength; dataPointIndex++) {
      //    for (int dimensionIndex = 0; dimensionIndex < dimensionCount; dimensionIndex++) {
      //      if (MinValues[dimensionIndex]>dataSerie[dataPointIndex, dimensionIndex]) {
      //        MinValues[dimensionIndex] = dataSerie[dataPointIndex, dimensionIndex];
      //      }
      //      if (MaxValues[dimensionIndex]<dataSerie[dataPointIndex, dimensionIndex]) {
      //        MaxValues[dimensionIndex] = dataSerie[dataPointIndex, dimensionIndex];
      //      }
      //      if (dataPointIndex>0 && IsDimensionSorted[dimensionIndex]) {
      //        if (dataSerie[dataPointIndex-1, dimensionIndex]>dataSerie[dataPointIndex, dimensionIndex]) {
      //          //this value is smaller than the previous, i.e. the values are not sorted ascending
      //          IsDimensionSorted[dimensionIndex] = false;
      //        }
      //      }
      //    }
      //  }
      //}
      foreach (var ySerie in ySeries) {
        var valuesLength = ySerie.Values.GetLength(0);
        for (int dataPointIndex = 0; dataPointIndex<valuesLength; dataPointIndex++) {
          for (int dimensionIndex = 0; dimensionIndex<ySeriesDimensionCount; dimensionIndex++) {
            if (MinValues[dimensionIndex]>ySerie.Values[dataPointIndex, dimensionIndex]) {
              MinValues[dimensionIndex] = ySerie.Values[dataPointIndex, dimensionIndex];
            }
            if (MaxValues[dimensionIndex]<ySerie.Values[dataPointIndex, dimensionIndex]) {
              MaxValues[dimensionIndex] = ySerie.Values[dataPointIndex, dimensionIndex];
            }
            if (dataPointIndex>0 && IsYSerieSorted[dimensionIndex]) {
              if (ySerie.Values[dataPointIndex-1, dimensionIndex]>ySerie.Values[dataPointIndex, dimensionIndex]) {
                //this value is smaller than the previous, i.e. the values are not sorted ascending
                IsYSerieSorted[dimensionIndex] = false;
              }
            }
          }
        }
      }
    }
    #endregion


    #region Methods
    //      -------

    public override string ToString() {
      string minMaxString = "";
      for (int dimensionMapIndex = 0; dimensionMapIndex < DimensionMap.Length; dimensionMapIndex++) {
        int dimension = DimensionMap[dimensionMapIndex];
        minMaxString = minMaxString + Environment.NewLine + $"DimensionMap[{dimensionMapIndex}]: {DimensionToString(dimension)};" +
          $"Min: {MinValues[dimensionMapIndex]}; " +
          $"MinDisplay: {MinDisplayValues[dimensionMapIndex]}; " +
          $"MaxDisplay: {MaxDisplayValues[dimensionMapIndex]}; " +
          $"Max: {MaxValues[dimensionMapIndex]}; ";
      }
      return $"RendererId: {RendererId}" + minMaxString;
    }
    #endregion
  }
}