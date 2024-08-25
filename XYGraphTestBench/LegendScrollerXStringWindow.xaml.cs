using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XYGraphLib;


namespace XYGraphTestBench {
  /// <summary>
  /// Interaction logic for LegendScrollerXStringWindow.xaml
  /// </summary>
  public partial class LegendScrollerXStringWindow: Window {

    /// <summary>
    /// Creates and opens a new LegendScrollerXStringWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      new LegendScrollerXStringWindow{Owner = ownerWindow}.Show();
    }


    public LegendScrollerXStringWindow() {
      InitializeComponent();
    }
  }
}
