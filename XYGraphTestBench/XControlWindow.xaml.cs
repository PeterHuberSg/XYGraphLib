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
using WpfTestbench;


namespace XYGraphLib {

  /// <summary>
  /// Testbench Window for XControl
  /// </summary>
  public partial class XControlWindow: Window {


    /// <summary>
    /// Creates and opens a new XControlWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      new XControlWindow { Owner = ownerWindow }.Show();
    }


    public XControlWindow() {
      InitializeComponent();
    }
  }
}
