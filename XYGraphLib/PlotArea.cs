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
    //Crosshair: added if mouse is within PlotArea 

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
      new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));


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

    readonly Crosshair crosshair;


    ///// <summary>
    ///// Default Constructor
    ///// </summary>
    //public PlotArea(): this(null) {}


    /// <summary>
    /// This constructor allows to give the PlotArea a name straight away. This is helpful for WPF event tracing.
    /// </summary>
    public PlotArea(string? plotAreaName = null, Pen? crosshairPen = null) {
      if (plotAreaName!=null) {
        Name = plotAreaName;
      }
      crosshair = new Crosshair(crosshairPen);
      ClipToBounds = true;


      MouseEnter += PlotArea_MouseEnter;
      MouseMove += PlotArea_MouseMove;
      MouseLeave += PlotArea_MouseLeave;
    }
    #endregion


    #region Events
    //      ------

    private void PlotArea_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
    }


    private void PlotArea_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
      this.InvalidateVisual();
    }


    private void PlotArea_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
      this.InvalidateVisual();
    }
    #endregion


    #region Methods
    //      -------

    readonly List<Renderer> renderers = new();


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
    /// used by Crosshair to calculate cursor position to x value
    /// </summary>
    LegendScrollerX? legendScrollerX;


    /// <summary>
    /// used by Crosshair to find y values based on cursor x position
    /// </summary>
    List<RendererDataSeries> rendererDataSeriesList = new();


    /// <summary>
    /// Adds one renderer to PlotArea
    /// </summary>
    /// <param name="renderer"></param>
    public void AddRenderer(Renderer renderer) {
      TraceWpf.Line("PlotArea.AddRenderer()");
      renderers.Add(renderer);
      renderer.RenderingRequested += renderer_RenderingRequested;
      //When a new renderer gets added, first the legends have to be calculated again, which might change the width of the legend and
      //in consequence also the width of the Plot-area
      isRendererAdded = true;
      InvalidateVisual();

      RendererAdded?.Invoke(renderer);

      //remember the renderers crosshair needs to display the x and y values at the cursor x location.
      //if (renderer is RendererGridLineY rendererGridLineY) {
      //  legendScrollerX = rendererGridLineY.LegendScrollerX;
      //}
      switch (renderer) {
      case RendererGridLineY rendererGridLineY: legendScrollerX = rendererGridLineY.LegendScrollerX; break;
      case RendererDataSeries rendererDataSeries: rendererDataSeriesList.Add(rendererDataSeries); break;
      default: break; 
      }
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
      //use all available size, unless size is infinite, then use 0
      Size desiredSize = availableSize;
      if (double.IsInfinity(desiredSize.Width)) desiredSize.Width = 0;
      if (double.IsInfinity(desiredSize.Height)) desiredSize.Height = 0;
      return desiredSize;
    }


    protected override Size ArrangeOverride(Size finalSize) {
      //use all available size
      return finalSize;
    }


    const int firstRendererVisual = 1; //at 0 is the background renderer
    double oldActualWidth = double.NaN;
    double oldActualHeight = double.NaN;
    bool isRendererAdded;


    protected override void OnRender(DrawingContext drawingContext) {
      if (double.IsNaN(ActualWidth) ||double.IsNaN(ActualHeight)) return;//not ready for rendering yet

      if (isRendererAdded || oldActualWidth!=ActualWidth || oldActualHeight!=ActualHeight){
        //Size has changed. Recreate all Visuals
        oldActualWidth = ActualWidth;
        oldActualHeight = ActualHeight;
        isRendererAdded = false;

        Visuals.Clear();
        Visuals.Add(createBackGroundVisual());
        foreach (Renderer renderer in renderers) {
          Visuals.Add(renderer.CreateVisual(ActualWidth, ActualHeight));
        }
        TraceWpf.Line("PlotArea.OnRender: " + renderers.Count + " Renderer Visuals recreated");
      }

      //handle crosshair
      var backGroundAndRendererCount = renderers.Count + 1;
      if (Visuals.Count>backGroundAndRendererCount) {
        Visuals.RemoveAt(backGroundAndRendererCount);
      }
      if (IsMouseOver && IsShowCrosshair) {
        TraceWpf.Line("PlotArea.OnRender:P draw  crosshair");
        //draw  crosshair
        var mousePosition = Mouse.GetPosition(this);
        var xValue = legendScrollerX!.DisplayValue + mousePosition.X / ActualWidth * legendScrollerX!.DisplayValueRange;
        Visuals.Add(crosshair.CreateVisual(mousePosition.X, ActualWidth, ActualHeight));
      }
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

