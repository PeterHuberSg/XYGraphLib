using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace XYGraphLib {

  /// <summary>
  /// Creates a Visual for the PlotArea to display. Inherit from this class if dataserie(s) are linked to this Renderer.
  /// </summary>
  public abstract class RendererDataSeries: Renderer {
    // 
    //                       + MinValueY
    //            <--Width---> 
    //          ^ +----------+ MinDisplayValueY
    //          | |          | 
    //   Height | | PlotArea | 
    //          | |          |  
    // :. . . . v +----------+ MaxDisplayValueY.+
    // :          :          .                  :
    // :          :          + MaxValueY        :
    // :          :          :                  :
    // MinIndex   :          MaxDisplayIndex    :
    //            MinDisplayIndex               MaxIndex
    //

    #region Properties
    //      ----------

    /// <summary>
    /// Lowest value stored in the datarecords per dimension
    /// </summary>
    public readonly double[] MinValues;


    /// <summary>
    /// Highest value stored in the datarecords per dimension
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
    #endregion


    #region Constructor
    //      -----------

    public RendererDataSeries(Brush strokeBrush, double strokeThickness, int[] dimensionMap, double[][,] dataSeries) :
      base(strokeBrush, strokeThickness, dimensionMap)
    {
      int dimensionCount = DimensionMap.Length;
      if (dimensionCount!=dataSeries[0].GetLength(1)) {
        throw new Exception("Renderer was set up with " + dimensionCount + " dimensions. The DataPoints in the DataSeries should have the same number of values, but" + 
        "the dataPoints in dataSeries[0] have " + dataSeries[0].GetLength(1) + " values.");
      }

      IsDimensionSorted = new bool[dimensionCount];
      MinValues = new double[dimensionCount];
      MaxValues = new double[dimensionCount];
      for (int dimensionIndex = 0; dimensionIndex<IsDimensionSorted.Length; dimensionIndex++) {
			  IsDimensionSorted[dimensionIndex] = true;
        MinValues[dimensionIndex] = double.MaxValue;
        MaxValues[dimensionIndex] = double.MinValue;
      }

      DataSeries = dataSeries;
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