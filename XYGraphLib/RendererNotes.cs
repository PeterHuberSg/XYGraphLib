using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;


namespace XYGraphLib {


  public struct ChartNote {
    public readonly double[] Values;
    public readonly string Note;
    public int FontDefinitionId;
    public readonly int ListId;
    public readonly int ItemId;


    public ChartNote(double[] values, string note, int fontDefinitionId = 0, int listId = 0, int itemId = 0) {
      Values = values;
      Note = note;
      FontDefinitionId = fontDefinitionId;
      ListId = listId;
      ItemId = itemId;
    }


    public override string ToString() {
      string valuesString = null;
      foreach (double value in Values) {
        if (valuesString==null) {
          valuesString = "Values[";
        } else {
          valuesString += ", ";
        }
        valuesString += value;
      }
      return valuesString + "] Note: " + Note + "; FontDefinitionId: " + FontDefinitionId + "; ListId: " + ListId + "; ItemId: " + ItemId + ";";
    }
  }


  public struct FontDefinition {
    public readonly Brush FontBrush;
    public readonly FontFamily FontFamily;
    public readonly double? FontSize;
    public readonly FontStretch? FontStretch;
    public readonly FontStyle? FontStyle;
    public readonly FontWeight? FontWeight;


    public FontDefinition(Brush fontBrush, FontFamily fontFamily, double? fontSize, FontStretch? fontStretch, FontStyle? fontStyle, FontWeight? fontWeight) {
      FontBrush = fontBrush;
      FontFamily = fontFamily;
      FontSize = fontSize;
      FontStretch = fontStretch;
      FontStyle = fontStyle;
      FontWeight = fontWeight;
    }


    public override string ToString() {
      return "FontBrush: " + FontBrush??"null" + "; FontFamily: " + FontFamily??"null" + "; FontSize: " + FontSize??"null" + 
        "; FontStretch: " + FontStretch??"null" + "; FontStyle: " + FontStyle??"null" + "; FontWeight: " + FontWeight??"null" + ";";
    }
  }


  public class RendererNotes: Renderer {

    #region Constructor
    //      -----------

    ChartNote[] chartNotes;
    Chart chart; //for the font values
    FontDefinition[] fontDefinitions;


    public RendererNotes(IEnumerable<ChartNote> chartNotes, Chart chart, FontDefinition[] fontDefinitions):
      base(null, 0, DimensionMapXY) 
    {
      this.chart = chart;
      if (fontDefinitions==null) {
        this.fontDefinitions = new FontDefinition[] {new FontDefinition(null, null, null, null, null, null) };
      } else {
        this.fontDefinitions = fontDefinitions;
      }
      this.chartNotes = chartNotes.OrderBy(x => x.Values[0]).ToArray();
      foreach (ChartNote chartNote in chartNotes) {
        if (chartNote.FontDefinitionId<0 || chartNote.FontDefinitionId>=fontDefinitions.Length) {
          System.Diagnostics.Debugger.Break();
          throw new Exception("FontStyleId " + chartNote.FontDefinitionId + " should be between 0 and " + fontDefinitions.Length + ". ChartNote: " + chartNote);
        }
      }
    }
    #endregion


    #region Methods
    //      -------

    GlyphDrawer[] glyphDrawers;
    FontFamily fontFamilytracked;
    double fontSizeTracked = double.MinValue;

    /// <summary>
    /// Renders the notes to the drawingContext. The notes gets  scaled to the available height and width displaying only 
    /// values between minDisplayValueX and maxDisplayValueX, if the x-values are sorted.
    /// </summary>
    protected override void OnCreateVisual(DrawingContext drawingContext, double width, double height) {
      if (glyphDrawers==null || fontFamilytracked!=chart.FontFamily || fontSizeTracked!=chart.FontSize) {
        fontFamilytracked = chart.FontFamily;
        fontSizeTracked = chart.FontSize;
        if (glyphDrawers==null) {
          glyphDrawers = new GlyphDrawer[fontDefinitions.Length];
        }
        for (int fontDefinitionIndex = 0; fontDefinitionIndex < fontDefinitions.Length; fontDefinitionIndex++) {
          FontDefinition fontDefinition = fontDefinitions[fontDefinitionIndex];
          glyphDrawers[fontDefinitionIndex] = new GlyphDrawer(
            fontDefinition.FontFamily ?? chart.FontFamily,
            fontDefinition.FontStyle.HasValue ? fontDefinition.FontStyle.Value : chart.FontStyle,
            fontDefinition.FontWeight.HasValue ? fontDefinition.FontWeight.Value : chart.FontWeight,
            fontDefinition.FontStretch.HasValue ? fontDefinition.FontStretch.Value : chart.FontStretch);
        }
      }

      double minDisplayValueX = MinDisplayValues[DimensionX];
      double maxDisplayValueX = MaxDisplayValues[DimensionX];
      double minDisplayValueY = MinDisplayValues[DimensionY];
      double maxDisplayValueY = MaxDisplayValues[DimensionY];
      int chartNotesLength = chartNotes.Length;
      for (int chartNotesIndex = 0; chartNotesIndex<chartNotesLength; chartNotesIndex++) {
        ChartNote chartNote = chartNotes[chartNotesIndex];
        double chartNotesX = chartNote.Values[0];
        if (chartNotesX<minDisplayValueX) {
          continue;
        }
        double chartNotesY = chartNote.Values[1];
        FontDefinition fontDefinition = fontDefinitions[chartNote.FontDefinitionId];
        if ((minDisplayValueY<=chartNotesY && chartNotesY<=maxDisplayValueY) || double.IsInfinity(chartNotesY)) {
          Point chartNotePoint = TranslateValueXYToPoint(chartNotesX, chartNotesY, width, height);
          if (double.IsPositiveInfinity(chartNotesY)) {
            //draw at top
            const double fontOffset = 1.3; //need to move the letters a bit down
            chartNotePoint.Y = fontOffset * (fontDefinition.FontSize.HasValue ? fontDefinition.FontSize.Value : chart.FontSize);
          } else if (double.IsPositiveInfinity(chartNotesY)) {
            //draw at bottom
            chartNotePoint.Y = height;
          }
          glyphDrawers[chartNote.FontDefinitionId].Write(drawingContext, chartNotePoint, chartNote.Note,
            fontDefinition.FontSize.HasValue ? fontDefinition.FontSize.Value : chart.FontSize, fontDefinition.FontBrush ?? chart.Foreground);
        }

        if (chartNotesX>maxDisplayValueX) {
          break;
        }
      }
    }
    #endregion
  }
}
