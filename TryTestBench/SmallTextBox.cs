using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;


namespace TryTestBench {

  /// <summary>
  /// When user clicks on the TextBox, all text gets immediately selected
  /// </summary>
  public class SmallTextBox: TextBox {
    public SmallTextBox() {
      PreviewMouseDown += new MouseButtonEventHandler(textBox_PreviewMouseDown);
      GotFocus += new RoutedEventHandler(textBox_GotFocus);
      SelectionChanged += new RoutedEventHandler(textBox_SelectionChanged);
    }


    bool isMouseDown;


    void textBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
      isMouseDown = true;
    }


    bool hasGotFocused;


    void textBox_GotFocus(object sender, RoutedEventArgs e) {
      if (isMouseDown) {
        //user clicked with mouse on TextBox. Wait for the SelectionChanged event to select all the text
        isMouseDown = false;
        hasGotFocused = true;
      } else {
        //user used Tab key, which does not change the selection and the SelectionChanged event will not get fired.
        SelectAll();
      }
    }

    void textBox_SelectionChanged(object sender, RoutedEventArgs e) {
      if (hasGotFocused) {
        hasGotFocused = false;
        SelectAll();
      }
    }

  }
}
