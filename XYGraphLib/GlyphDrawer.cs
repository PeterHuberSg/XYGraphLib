//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Media;
//using System.Windows;


//namespace XYGraphLib {

//  /// <summary>
//  /// Draws glyphs to a DrawingContext. From the font information in the constructor, GlyphDrawer creates and stores the GlyphTypeface, which
//  /// is used everytime for the drawing of the string.
//  /// </summary>
//  public class GlyphDrawer {

//    Typeface typeface;
//    public GlyphTypeface GlyphTypeface {
//      get { return glyphTypeface; }
//    }
//    GlyphTypeface glyphTypeface;


//    public GlyphDrawer(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch) {
//      typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
//      if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
//        throw new InvalidOperationException("No glyphtypeface found");
//    }
    

//    /// <summary>
//    /// Writes a string to a DrawingContext, using the GlyphTypeface stored in the GlyphDrawer.
//    /// </summary>
//    /// <param name="drawingContext"></param>
//    /// <param name="origin"></param>
//    /// <param name="text"></param>
//    /// <param name="size">same unit like FontSize: (em)</param>
//    /// <param name="brush"></param>
//    public void Write(DrawingContext drawingContext, Point origin, string text, double size, Brush brush) {
//      if (text==null) return;

//      ushort[] glyphIndexes = new ushort[text.Length];
//      double[] advanceWidths = new double[text.Length];

//      double totalWidth = 0;

//      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
//        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
//        glyphIndexes[charIndex] = glyphIndex;

//        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
//        advanceWidths[charIndex] = width;

//        totalWidth += width;
//      }

//      GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size, glyphIndexes, origin, advanceWidths, null, null, null, null, null, null);

//      drawingContext.DrawGlyphRun(brush, glyphRun);
//    }


//    /// <summary>
//    /// Writes a string to a DrawingContext, using the GlyphTypeface stored in the GlyphDrawer. The text will be right alligned. The
//    /// last character will be at Origin, all other characters in front.
//    /// </summary>
//    /// <param name="drawingContext"></param>
//    /// <param name="origin"></param>
//    /// <param name="text"></param>
//    /// <param name="size">same unit like FontSize: (em)</param>
//    /// <param name="brush"></param>
//    public void WriteRightAligned(DrawingContext drawingContext, Point origin, string text, double size, Brush brush) {

//      ushort[] glyphIndexes = new ushort[text.Length];
//      double[] advanceWidths = new double[text.Length];

//      double totalWidth = 0;

//      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
//        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
//        glyphIndexes[charIndex] = glyphIndex;

//        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
//        advanceWidths[charIndex] = width;

//        totalWidth += width;
//      }

//      Point newOrigin = new Point(origin.X - totalWidth, origin.Y);
//      GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size, glyphIndexes, newOrigin, advanceWidths, null, null, null, null, null, null);

//      drawingContext.DrawGlyphRun(brush, glyphRun);
//    }


//    /// <summary>
//    /// Returns the length of the text using the GlyphTypeface stored in the GlyphDrawer. 
//    /// </summary>
//    /// <param name="text"></param>
//    /// <param name="size">same unit like FontSize: (em)</param>
//    /// <returns></returns>
//    public double GetLength(string text, double size) {
//     double length = 0;

//      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
//        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
//        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
//        length += width;
//      }
//      return length;
//    }
//  }
//}
