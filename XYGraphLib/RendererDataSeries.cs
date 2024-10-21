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
  /// Creates a Visual for the PlotArea to display. Inherit from this class if dataserie(s) are linked to this Renderer.
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
    public readonly bool[] IsDimensionSorted;


    /// <summary>
    /// DataSeries contains the data displayed by the Renderer.
    /// </summary>
    protected readonly double[][,] DataSeries;


    /// <summary>
    /// Name for Y value displayed in crosshair
    /// </summary>
    protected readonly string? YName;


    /// <summary>
    /// Measurement unit for Y value displayed in crosshair
    /// </summary>
    protected readonly string? YUnit;
    #endregion


    #region Constructor
    //      -----------

    public RendererDataSeries(Brush strokeBrush, double strokeThickness, int[] dimensionMap, double[][,] dataSeries,
      string? yName, string? yUnit) :
      base(strokeBrush, strokeThickness, dimensionMap)
    {
      YName = yName;
      YUnit = yUnit;
      int dimensionCount = DimensionMap.Length;
      if (dimensionCount!=dataSeries[0].GetLength(1)) {
        throw new Exception("Renderer was set up with " + dimensionCount + " dimensions. The DataPoints in the DataSeries should have the same number of values, but" + 
        "the dataPoints in dataSeries[0] have " + dataSeries[0].GetLength(1) + " values.");
      }

      DataSeries = dataSeries;

      //find min and max value within dataSeries and check if it is sorted
      IsDimensionSorted = new bool[dimensionCount];
      MinValues = new double[dimensionCount];
      MaxValues = new double[dimensionCount];
      for (int dimensionIndex = 0; dimensionIndex<IsDimensionSorted.Length; dimensionIndex++) {
			  IsDimensionSorted[dimensionIndex] = true;
        MinValues[dimensionIndex] = double.MaxValue;
        MaxValues[dimensionIndex] = double.MinValue;
      }

      foreach (double[,] dataSerie in dataSeries){
        int dataSerieLength = dataSerie.GetLength(0);
        for (int dataPointIndex = 0; dataPointIndex < dataSerieLength; dataPointIndex++) {
          for (int dimensionIndex = 0; dimensionIndex < dimensionCount; dimensionIndex++) {
            if (MinValues[dimensionIndex]>dataSerie[dataPointIndex, dimensionIndex]) {
              MinValues[dimensionIndex] = dataSerie[dataPointIndex, dimensionIndex];
            }
            if (MaxValues[dimensionIndex]<dataSerie[dataPointIndex, dimensionIndex]) {
              MaxValues[dimensionIndex] = dataSerie[dataPointIndex, dimensionIndex];
            }
            if (dataPointIndex>0 && IsDimensionSorted[dimensionIndex]) {
              if (dataSerie[dataPointIndex-1, dimensionIndex]>dataSerie[dataPointIndex, dimensionIndex]) {
                //this value is smaller than the previous, i.e. the values are not sorted ascending
                IsDimensionSorted[dimensionIndex] = false;
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
        minMaxString = minMaxString + Environment.NewLine + "DimensionMap[" + dimensionMapIndex + "]: " + DimensionToString(dimension) + ";" +
          "Min: " + MinValues[dimensionMapIndex] + "; " +
          "MinDisplay: " + MinDisplayValues[dimensionMapIndex] + "; " +
          "MaxDisplay: " + MaxDisplayValues[dimensionMapIndex] + "; " +
          "Max: " + MaxValues[dimensionMapIndex] + "; ";
      }
      return "RendererId: " + RendererId + minMaxString;
    }
    #endregion
  }
}