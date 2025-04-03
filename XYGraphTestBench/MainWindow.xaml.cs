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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using XYGraphTestBench;


namespace XYGraphLib {

  /// <summary>
  /// MainWindow for visual testing of Charts. It cotains buttons allowing the user to open test windows for the
  /// various chart types
  /// </summary>
  public partial class MainWindow: Window {
    public MainWindow() {
      InitializeComponent();

      LegendXButton.Click += LegendXButton_Click;
      LegendXDateButton.Click += LegendXDateButton_Click;
      LegendScrollerXButton.Click += LegendScrollerXButton_Click;
      LegendScrollerXStringButton.Click += LegendScrollerXStringButton_Click;
      LegendYButton.Click += LegendYButton_Click;
      LegendScrollerYButton.Click += LegendScrollerYButton_Click;
      Chart1Plot1X1YLegendButton.Click += Chart1Plot1X1YLegendButton_Click;
      Chart2Plots1X2YLegendsButton.Click += Chart2Plots1X2YLegendsButton_Click;
      Chart4Plots1X4YLegendsButton.Click += Chart4Plots1X4YLegendsButton_Click;
      Chart1Plot1XString1YButton.Click += Chart1Plot1XString1YButton_Click;
      ValuesPanelButton.Click += ValuesPanelButton_Click;

      CustomControlSampleButton.Click += CustomControlSampleButton_Click;
      SizeBindingButton.Click += SizeBindingButton_Click;
      TestButton.Click += TestButton_Click;
      XControlButton.Click += XControlButton_Click;

      //add _ and a, b, c ... at the start of every Button Text to support Alt key
      IEnumerator logicalChildren = LogicalChildren;
      logicalChildren.MoveNext();
      StackPanel stackPanel = (StackPanel)logicalChildren.Current;
      char buttonChar = 'a';
      foreach (var item in stackPanel.Children) {
        Button button = (Button)item;
        if (button!=null) {
          string contentString = (string)button.Content;
          if (contentString!=null) {
            if (buttonChar=='z'+1) {
              buttonChar = '0';
            }  else if (buttonChar=='9'+1) {
              buttonChar = 'a';
            }
            contentString = contentString.Replace("_", "");
            contentString = "_" + buttonChar++ + ":  " + contentString;
            button.Content = contentString;
          }
        }
      }
    }


    void LegendXButton_Click(object sender, RoutedEventArgs e) {
      LegendXWindow.Show(this);
    }


    void LegendXDateButton_Click(object sender, RoutedEventArgs e) {
      LegendXDateWindow.Show(this);
    }


    void LegendScrollerXButton_Click(object sender, RoutedEventArgs e) {
      LegendScrollerXWindow.Show(this);
    }


    void LegendScrollerXStringButton_Click(object sender, RoutedEventArgs e) {
      LegendScrollerXStringWindow.Show(this);
    }


    void LegendYButton_Click(object sender, RoutedEventArgs e) {
      LegendYWindow.Show(this);
    }


    void LegendScrollerYButton_Click(object sender, RoutedEventArgs e) {
      YLegendScrollerWindow.Show(this);
    }


    void Chart1Plot1X1YLegendButton_Click(object sender, RoutedEventArgs e) {
      Chart1Plot1X1YWindow.Show(this);
    }


    void Chart2Plots1X2YLegendsButton_Click(object sender, RoutedEventArgs e) {
      Chart2Plots1X2YLegendsWindow.Show(this);
    }


    void Chart4Plots1X4YLegendsButton_Click(object sender, RoutedEventArgs e) {
      Chart4Plots1X4YLegendsWindow.Show(this);
    }


    void Chart1Plot1XString1YButton_Click(object sender, RoutedEventArgs e) {
      Chart1Plot1XString1YWindow.Show(this);
    }


    private void ValuesPanelButton_Click(object sender, RoutedEventArgs e) {
      ValuesPanelWindow.Show(this);
    }


    //Test buttons
    //------------

    void CustomControlSampleButton_Click(object sender, RoutedEventArgs e) {
      CustomControlSampleWindow.Show(this);
    }


    void SizeBindingButton_Click(object sender, RoutedEventArgs e) {
      TestSizeBindingWindow.Show(this);
    }

    
    void TestButton_Click(object sender, RoutedEventArgs e) {
      TestWindow.Show(this);
    }


    void XControlButton_Click(object sender, RoutedEventArgs e) {
      XControlWindow.Show(this);
    }
  }
}
