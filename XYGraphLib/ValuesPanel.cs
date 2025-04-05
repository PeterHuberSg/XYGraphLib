using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static XYGraphLib.ValuesPanel;


namespace XYGraphLib {


  /// <summary>
  /// Displays the series' values closest to the X position of the mouse in a PlotArea
  /// </summary>
  public class ValuesPanel: Border {
    /*
    ValuesPanel displays one row for the x value and a row for the y value of each series

    The ValuesPanel displays always a column for values, but can have 3 columns: 
    ┌──────┬─────────┬────┐
    │Label │Value    │Unit│
    └──────┴─────────┴────┘

    The first row is for the x value, which is usually a date
    ┌──────┬─────────┬────┐
    │Date  │20.1.2000│    │
    ├──────┼─────────┼────┤
    │Value1│       90│CHF │
    ├──────┼─────────┼────┤
    │Value2│      130│SGD │
    └──────┴─────────┴────┘
    */

    /// <summary>
    /// Contains the information how to display one row in the ValuesPanel. 
    /// </summary>
    public record RowConfig(
      Brush? Color = null,
      string? Label = null,
      string? Unit = null);


    #region Properties
    //      ----------

    public RowConfig[] RowConfigs { get; private set; }
    #endregion


    #region Constructor
    //      -----------

    enum columnEnum {Label, Value, Unit};//a row can contain up to 3 TextBoxes

    readonly bool valuesPanelHasLabels;
    readonly bool valuesPanelHasUnits;
    readonly int colCount;
    readonly TextBox[] valueTextBoxes;
    readonly Grid grid;


    public ValuesPanel(RowConfig[] rowConfigs) {
      RowConfigs = rowConfigs;
      valueTextBoxes = new TextBox[RowConfigs.Length];

      BorderBrush = Brushes.DarkGray;
      BorderThickness = new Thickness(2);
      grid = new Grid();
      Child = grid;

      //grid.RowDefinitions.Add(new RowDefinition());//date
      valuesPanelHasLabels = false;
      valuesPanelHasUnits = false;
      for (int rowIndex = 0; rowIndex<RowConfigs.Length; rowIndex++) {
        grid.RowDefinitions.Add(new RowDefinition());
        var yConfig = RowConfigs[rowIndex];
        if (yConfig.Label is not null) {
          valuesPanelHasLabels = true;
        }
        if (yConfig.Unit is not null) {
          valuesPanelHasUnits = true;
        }
      }
      colCount = 1;//there is always a y value
      if (valuesPanelHasLabels) colCount++;
      if (valuesPanelHasUnits) colCount++;

      for (var columnIndex = 0; columnIndex<colCount; columnIndex++) {
        grid.ColumnDefinitions.Add(new ColumnDefinition());
      }

      for (int rowIndex = 0; rowIndex<RowConfigs.Length; rowIndex++) {
        var rowConfig = RowConfigs[rowIndex];
        if (valuesPanelHasLabels) {
          createTextBox(rowIndex, columnEnum.Label, rowConfig.Label, rowConfig.Color);
        }
        createTextBox(
          rowIndex, 
          columnEnum.Value, 
          null, 
          RowConfigs[rowIndex].Label is null ? rowConfig.Color : null);
        if (valuesPanelHasUnits) {
          createTextBox(rowIndex, columnEnum.Unit, rowConfig.Unit);
        }
      }
    }


    readonly Brush backgroundBrush = new SolidColorBrush(Color.FromArgb(0x80, 0xE0, 0xE0, 0xE0));
    readonly Brush backgroundBrushValue = new SolidColorBrush(Color.FromArgb(0x80, 0xF0, 0xF0, 0xF0));


    private void createTextBox(
      int rowIndex, 
      columnEnum column, 
      string? text = null, 
      Brush? color = null) 
    {
      var textBox = new TextBox {
        IsReadOnly = true,
        HorizontalContentAlignment = column switch {
          columnEnum.Label => HorizontalAlignment.Right,
          columnEnum.Value => HorizontalAlignment.Right,
          columnEnum.Unit => HorizontalAlignment.Left,
          _ => throw new NotSupportedException(),
        }
      };
      if (text is not null) {
        textBox.Text = text;
      }
      if (color is not null) {
        textBox.Foreground = color;
      }
      if (column==columnEnum.Label) {
        textBox.FontWeight = FontWeights.Bold;
      }
      if (column==columnEnum.Value) {
        textBox.Background = backgroundBrushValue;
        valueTextBoxes[rowIndex] = textBox;
        if (rowIndex==0) {
          textBox.AcceptsReturn = true;
        }
      } else {
        textBox.Background = backgroundBrush;
      }
      grid.Children.Add(textBox);
      Grid.SetRow(textBox, rowIndex);
      Grid.SetColumn(textBox, (int)column);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Updates the value column. The first row is for x (date) and the other rows for the ys.
    /// </summary>
    public void Update(IList<string> values) {
      if (values.Count!=RowConfigs.Length) 
        throw new FormatException($"values should have a length of {RowConfigs.Length} but is {values.Count} long. ");

      for (int valuesIndex = 0; valuesIndex<values.Count; valuesIndex++) {
        var newValue = values[valuesIndex];
        //var format = valueFormats[valuesIndex];
        valueTextBoxes[valuesIndex]!.Text = newValue;
      }
    }
    #endregion
  }
}
