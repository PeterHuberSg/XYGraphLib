using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
  /// Interaction logic for ValuesPanelWindow.xaml
  /// </summary>
  public partial class ValuesPanelWindow: Window {

    /// <summary>
    /// Creates and opens a new ValuesPanelWindow
    /// </summary>
    public static void Show(Window ownerWindow) {
      new ValuesPanelWindow { Owner = ownerWindow }.Show();
    }


    readonly DateTime today;
    readonly List<TextBox> valueTextBoxes;
    readonly List<string> valueStrings;

    public ValuesPanelWindow() {
      InitializeComponent();

      today = DateTime.Today;
      valueTextBoxes = [];
      valueStrings = [];

      DataPointsCount1RadioButton.Click += DataPointsCountRadioButton_Click;
      DataPointsCount2RadioButton.Click += DataPointsCountRadioButton_Click;
      DataPointsCount3RadioButton.Click += DataPointsCountRadioButton_Click;
      DataPointsCount4RadioButton.Click += DataPointsCountRadioButton_Click;
      DataPointsCount5RadioButton.Click += DataPointsCountRadioButton_Click;

      setupDataValuesGrid(1);
    }

    private void DataPointsCountRadioButton_Click(object sender, RoutedEventArgs e) {
      var radioButton = (RadioButton)sender;
      var dataValuesCount = int.Parse((string)radioButton.Content);
      setupDataValuesGrid(dataValuesCount);
    }


    ValuesPanel testValuesPanel;
    Brush?[] serieBrushes = [null, Brushes.Blue, Brushes.Green, Brushes.Red, Brushes.Gray];


    [MemberNotNull(nameof(testValuesPanel))]
    private void setupDataValuesGrid(int dataValuesCount) {
      //remove existing textboxes
      for (int childIndex = DataValuesGrid.Children.Count-1; childIndex>=0; childIndex--) {
        if (DataValuesGrid.Children[childIndex] is Label) break;

        DataValuesGrid.Children.RemoveAt(childIndex);
      }
      valueTextBoxes.Clear();
      valueStrings.Clear();

      //create new textboxes
      var rowConfigs = new ValuesPanel.RowConfig[dataValuesCount];
      var letter = 'A';
      for (int rowConfigsIndex = 0; rowConfigsIndex < dataValuesCount; rowConfigsIndex++) {
        var row = rowConfigsIndex + 1;
        createTextBox(row, 0, rowConfigsIndex.ToString(), Brushes.AliceBlue);
        var label = row==2 ? null : new string(letter++, row);
        createTextBox(row, 1, label);
        var valueString = (rowConfigsIndex+1).ToString();
        createTextBox(row, 2, valueString);
        var unit = "u"+row.ToString();
        createTextBox(row, 3, unit);

        rowConfigs[rowConfigsIndex] = new (serieBrushes[rowConfigsIndex], label, unit);
      }
      testValuesPanel = new ValuesPanel(rowConfigs);
      testValuesPanel.Update(valueStrings); ;
      TestValuesPanelGrid.Children.Clear();
      TestValuesPanelGrid.Children.Add(testValuesPanel);
    }


    private void createTextBox(int row, int column, string? text = null, Brush? background = null) {
      var textBox = new TextBox {};
      //, Text = dataValuesIndex.ToString(), Background = Brushes.AliceBlue 
      if (text is not null) {
        textBox.Text = text;
      }
      if (background is not null) {
        textBox.Background = background;
      }
      if (column==0) {
        //ID
        textBox.IsReadOnly = true;
      } else {
        //other columns
        textBox.Tag = row-1;
        if (column==2) {
          valueTextBoxes.Add(textBox);
          valueStrings.Add(textBox.Text);
        }
        
      }
      textBox.TextChanged += TextBox_TextChanged;
      Grid.SetRow(textBox, row);
      Grid.SetColumn(textBox, column);
      DataValuesGrid.Children.Add(textBox);
    }


    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
      var textBox = (TextBox)sender;
      var row = (int)textBox.Tag;
      //var dataPointValue = testValuesPanel.DataValues[row];
      //switch (column) {
      //case 0: dataPointValue = new DataPointValue(dataPointValue.Date, dataPointValue.Value, dataPointValue.Color, textBox.Text, dataPointValue.Unit); break;
      //case 1: dataPointValue = new DataPointValue(dataPointValue.Date, textBox.Text, dataPointValue.Color, dataPointValue.Label, dataPointValue.Unit); break;
      //case 2: dataPointValue = new DataPointValue(dataPointValue.Date, dataPointValue.Value, dataPointValue.Color, dataPointValue.Label, textBox.Text); break;
      //default: throw new NotSupportedException();
      //}
      //testValuesPanel.DataValues[row] = dataPointValue;
      valueStrings[row] = textBox.Text;
      testValuesPanel.Update(valueStrings);
    }
  }
}
