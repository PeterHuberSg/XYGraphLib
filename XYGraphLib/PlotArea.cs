//Todo: Check if Actual width is used properly or RenderSize.Width should be used ?

using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;

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

    #region Properties
    //      ----------

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
                new FrameworkPropertyMetadata(
                    Brushes.White,
                    FrameworkPropertyMetadataOptions.AffectsRender));


    /// <summary>
    /// Number of Renderers registered with PlotArea
    /// </summary>
    public int RendererCount { get { return renderers.Count; } }

    
    /// <summary>
    /// Raised when renderer gets added to PlotArea. Is used to adde WPF tracing to renderer.
    /// </summary>
    public event Action<Renderer> RendererAdded;


    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Default Constructor
    /// </summary>
    public PlotArea(): this(null) {}


    /// <summary>
    /// This constructor allows to give the PlotArea a name straight away. This is helpful for WPF event tracing.
    /// </summary>
    public PlotArea(string plotAreaName) {
      if (plotAreaName!=null) {
        Name = plotAreaName;
      }
      ClipToBounds = true;
      //SnapsToDevicePixels = true;//'
    }
    #endregion


    #region Methods
    //      -------


    List<Renderer> renderers = new List<Renderer>();


    /// <summary>
    /// Removes all Renderers from PlotArea
    /// </summary>
    public void ClearRenderers() {
      renderers.Clear();
      if (Visuals.Count>firstRendererVisual) {
        TraceWpf.Line("PlotArea.ClearRenderers(), remove Visuals");
        //remove all renderer visuals, but leave background visual
        Visuals.RemoveRange(firstRendererVisual, Visuals.Count-firstRendererVisual);
      } else {
        TraceWpf.Line("PlotArea.ClearRenderers(), no Visuals");
      }
    }


    /// <summary>
    /// Adds one renderer to PlotArea
    /// </summary>
    /// <param name="renderer"></param>
    public void AddRenderer(Renderer renderer) {
      TraceWpf.Line("PlotArea.AddRenderer()");
      renderers.Add(renderer);
      renderer.RenderingRequested += renderer_RenderingRequested;
      //When a new renderer gets added, first the legends has to be calculated again, which might change the width of the legend and
      //in consequence also the width of the Plotarea
      isRenderingNeeded = true;
      InvalidateVisual();
      //////if (Visuals.Count<firstRendererVisual) {
      //////  //Background not added yet => onRender will get executed later, which will add the visuals for the Renderer;
      //////  //nothing to do now
      //////} else {
      //////  //Background Visual exists already. Other Visuals can be added.
      //////  Visuals.Add(renderer.Render(ActualWidth, ActualHeight));
      //////}

      if (RendererAdded!=null) RendererAdded(renderer);
    }


    /// <summary>
    /// Replaces the old Visual created by this Renderer with a new one
    /// </summary>
    void renderer_RenderingRequested(Renderer renderer) {
      if (Visuals.Count<=firstRendererVisual) {
        TraceWpf.Line("PlotArea.RenderingRequested(" + renderer.RendererId + "): delayed Visual");
        //Background not added yet => onRender will get executed later, which will add the visuals for the Renderer;
        //nothing to do now
      } else {
        TraceWpf.Line("PlotArea.RenderingRequested(" + renderer.RendererId + "): Visual updated");
        int rendererIndex = renderers.IndexOf(renderer);
        if (rendererIndex==-1) throw new Exception("RenderingRequested: renderer '" + renderer + "' not found in renderers (Count: " + renderers.Count + ").");

        int visualIndex = firstRendererVisual + rendererIndex;
        Visuals.RemoveAt(visualIndex);
        Visuals.Insert(visualIndex, renderer.CreateVisual(ActualWidth, ActualHeight));
      }
    }
    #endregion

  
    #region Overrides
    //      ---------

    protected override Size MeasureOverride(Size availableSize) {
      //use all available size, unless size is infinit, then use 0
      Size desiredSize = availableSize;
      if (double.IsInfinity(desiredSize.Width)) desiredSize.Width=0;
      if (double.IsInfinity(desiredSize.Height)) desiredSize.Height=0;
      return desiredSize;
    }


    protected override Size ArrangeOverride(Size finalSize) {
      //use all available size
      return finalSize;
    }


    const int firstRendererVisual = 1;
    double oldActualWidth = double.NaN;
    double oldActualHeight = double.NaN;
    bool isRenderingNeeded;


    protected override void OnRender(DrawingContext drawingContext) {
      if (double.IsNaN(ActualWidth) ||double.IsNaN(ActualHeight)) return;

      if (!isRenderingNeeded && oldActualWidth==ActualWidth && oldActualHeight==ActualHeight) return;

      //Size has changed. Recreate all Visuals
      oldActualWidth = ActualWidth;
      oldActualHeight = ActualHeight;
      isRenderingNeeded = false;

      Visuals.Clear();
      Visuals.Add(crerateBackGroundVisual());
      foreach (Renderer renderer in renderers) {
        //////var xGridLineRenderer = renderer as XGridLineRenderer;
        //////if (xGridLineRenderer!=null) {
        //////  if (double.IsNaN(xGridLineRenderer.MinDisplayValueY) || double.IsNaN(xGridLineRenderer.MaxDisplayValueY)){
        //////    xGridLineRenderer.SetDisplayValueRangeY(xGridLineRenderer.YLegendScroller.MinDisplayValue, xGridLineRenderer.YLegendScroller.MaxDisplayValue);
        //////  }
        //////}
        Visuals.Add(renderer.CreateVisual(ActualWidth, ActualHeight));
      }
      TraceWpf.Line("PlotArea.OnRender: " + renderers.Count + " Renderer Visuals recreated");
    }


    private Visual crerateBackGroundVisual() {
      DrawingVisual drawingVisual = new DrawingVisual();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
        drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
      }
      return drawingVisual;
    }
    #endregion
  }
}

/*    
    private void AddRulerdrawingContext(DrawingContext drawingContext) {
      StreamGeometry streamGeometry = new StreamGeometry();
      using (StreamGeometryContext streamGeometryContext = streamGeometry.Open()) {
        streamGeometryContext.BeginFigure(new Point(1200, 20), isFilled: false, isClosed: false);
        streamGeometryContext.LineTo(new Point(0, 20), isStroked: true, isSmoothJoin: false);
        streamGeometryContext.LineTo(new Point(0, 0), isStroked: true, isSmoothJoin: false);
        for (int i = 0; i < 60; i++) {
          streamGeometryContext.LineTo(new Point(i*20, 20), isStroked: false, isSmoothJoin: false);
          streamGeometryContext.LineTo(new Point(i*20, 0), isStroked: true, isSmoothJoin: false);
        }
      }
      streamGeometry.Freeze();
      drawingContext.DrawGeometry(Brushes.DarkGray, new Pen(Brushes.DarkGoldenrod, 1), streamGeometry);
    }


    private void AddStreamGeometry(DrawingContext drawingContext) {
      StreamGeometry streamGeometry = new StreamGeometry();
      using (StreamGeometryContext streamGeometryContext = streamGeometry.Open()) {
        streamGeometryContext.BeginFigure(new Point(0, 0), isFilled: false, isClosed: false);
        streamGeometryContext.LineTo(new Point(FullWidth, FullHeight), isStroked: true, isSmoothJoin: false);
        streamGeometryContext.LineTo(new Point(FullWidth, 0), isStroked: true, isSmoothJoin: false);
        streamGeometryContext.LineTo(new Point(1000, 0), isStroked: true, isSmoothJoin: false);
      }
      streamGeometry.Freeze();
      drawingContext.DrawGeometry(Brushes.LightGoldenrodYellow, new Pen(Brushes.DarkGoldenrod, 1), streamGeometry);
    }


    private void AddHorizontalStreamGeometry(DrawingContext drawingContext) {
      StreamGeometry streamGeometry = new StreamGeometry();
      double yStep = FullHeight / 20;
      using (StreamGeometryContext streamGeometryContext = streamGeometry.Open()) {
        for (int i = 0; i < 21; i++) {
          streamGeometryContext.BeginFigure(new Point(0, i*yStep), isFilled : false, isClosed : false);
          streamGeometryContext.LineTo(new Point(FullWidth, i*yStep), isStroked : true, isSmoothJoin : false);
        }
      }
      streamGeometry.Freeze();
      drawingContext.DrawGeometry(Brushes.DarkBlue, new Pen(Brushes.Gray, 0.5), streamGeometry);
    }


    private void AddRectangle(DrawingContext drawingContext) {
      // Create a rectangle and draw it in the DrawingContext.
      Rect rect = new Rect(new System.Windows.Point(160, 100), new System.Windows.Size(320, 80));
      drawingContext.DrawRectangle(System.Windows.Media.Brushes.LightBlue, (System.Windows.Media.Pen)null, rect);
    }


    private void AddText(DrawingContext drawingContext, string text) {
      Typeface typeface = new Typeface(new FontFamily("Arial"),
                                       FontStyles.Normal,
                                       FontWeights.Normal,
                                       FontStretches.Normal);

      GlyphTypeface glyphTypeface;
      if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
        throw new InvalidOperationException("No glyphtypeface found");

      double size = 40;

      ushort[] glyphIndexes = new ushort[text.Length];
      double[] advanceWidths = new double[text.Length];

      double totalWidth = 0;

      for (int n = 0; n < text.Length; n++) {
        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
        glyphIndexes[n] = glyphIndex;

        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
        advanceWidths[n] = width;

        totalWidth += width;
      }

      Point origin = new Point(50, 200);

      GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size,
          glyphIndexes, origin, advanceWidths, null, null, null, null,
          null, null);

      drawingContext.DrawGlyphRun(Brushes.Black, glyphRun);

      double y = origin.Y;
      drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(origin.X, y),
          new Point(origin.X + totalWidth, y));

      y -= (glyphTypeface.Baseline * size);
      drawingContext.DrawLine(new Pen(Brushes.Green, 1), new Point(origin.X, y),
          new Point(origin.X + totalWidth, y));

      y += (glyphTypeface.Height * size);
      drawingContext.DrawLine(new Pen(Brushes.Blue, 1), new Point(origin.X, y),
          new Point(origin.X + totalWidth, y));

    }



    private Visual CreateDrawingVisualText() {
      DrawingVisual drawingVisual = new DrawingVisual();
      DrawingContext drawingContext = drawingVisual.RenderOpen();



      Typeface typeface = new Typeface(new FontFamily("Arial"),
                                      FontStyles.Italic,
                                      FontWeights.Normal,
                                      FontStretches.Normal);

      GlyphTypeface glyphTypeface;
      if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
        throw new InvalidOperationException("No glyphtypeface found");

      string text = "Hello, world!";
      double size = 40;

      ushort[] glyphIndexes = new ushort[text.Length];
      double[] advanceWidths = new double[text.Length];

      double totalWidth = 0;

      for (int n = 0; n < text.Length; n++) {
        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
        glyphIndexes[n] = glyphIndex;

        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
        advanceWidths[n] = width;

        totalWidth += width;
      }

      Point origin = new Point(50, 50);

      GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size,
          glyphIndexes, origin, advanceWidths, null, null, null, null,
          null, null);

      drawingContext.DrawGlyphRun(Brushes.Black, glyphRun);

      double y = origin.Y;
      drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(origin.X, y),
          new Point(origin.X + totalWidth, y));

      y -= (glyphTypeface.Baseline * size);
      drawingContext.DrawLine(new Pen(Brushes.Green, 1), new Point(origin.X, y),
          new Point(origin.X + totalWidth, y));

      y += (glyphTypeface.Height * size);
      drawingContext.DrawLine(new Pen(Brushes.Blue, 1), new Point(origin.X, y),
          new Point(origin.X + totalWidth, y));

      // Persist the drawing content.
      drawingContext.Close();

      return drawingVisual;
    }


    private DrawingVisual CreateDrawingVisualRectangle() {
      DrawingVisual drawingVisual = new DrawingVisual();

      // Retrieve the DrawingContext in order to create new drawing content.
      DrawingContext drawingContext = drawingVisual.RenderOpen();

      // Create a rectangle and draw it in the DrawingContext.
      Rect rect = new Rect(new System.Windows.Point(160, 100), new System.Windows.Size(320, 80));
      drawingContext.DrawRectangle(System.Windows.Media.Brushes.LightBlue, (System.Windows.Media.Pen)null, rect);

      // Persist the drawing content.
      drawingContext.Close();

      return drawingVisual;
    }
    */
