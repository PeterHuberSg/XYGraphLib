using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace XYGraphLib {
  /// <summary>
  /// Interaction logic for TestWindow.xaml
  /// </summary>
  public partial class TestWindow: Window {


    /// <summary>
    /// Creates and opens a new LegendScrollerXWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      TestWindow newTestWindow = new TestWindow();
      newTestWindow.Owner = ownerWindow;
      newTestWindow.Show();
    }

    
    public TestWindow() {
      InitializeComponent();

      Loaded += TestWindow_Loaded;
      MouseDoubleClick += TestWindow_MouseDoubleClick;
    }


    void TestWindow_Loaded(object sender, RoutedEventArgs e) {
      //LegendX legendX = new LegendX();
      //MainGrid.Children.Add(legendX);

      //LegendScrollerX legendScrollerX = new LegendScrollerX();
      //MainGrid.Children.Add(legendScrollerX);

      //LegendY legendY = new LegendY();
      //MainGrid.Children.Add(legendY);

      FrameworkElement testControl;
      //testControl = new LegendScrollerX();
      //testControl = new Chart1Plot1X1YLegend();
      //testControl = new Chart2Plots1X2YLegendsTraced();
      //testControl = new LegendXDate();
      //MainGrid.Children.Add(testControl);

      //testSomething();
      testChart();
    }


    void TestWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
      System.Diagnostics.Debugger.Break();
    }


    private void testSomething() {
      //DockPanel ScrollbarDockPanel = new DockPanel();
      //MainGrid.Children.Add(ScrollbarDockPanel);

    }


    private void testChart() {
      var chart = new Chart1Plot1X1YLegend(new PlotArea(), new LegendScrollerX(new LegendXDate()), new LegendScrollerY());
      //var chart = new Chart1Plot1X1YLegend(new PlotArea(), new LegendScrollerX(new LegendXDate()), new LegendScrollerY(), new Grid());
      //var chart = new Chart1Plot1X1YLegend(new PlotArea(), new LegendScrollerX(), new LegendScrollerY(), new Grid());
      //var chart = new Chart1Plot1X1YLegend();
      MainGrid.Children.Add(chart);
    }

  }
}
