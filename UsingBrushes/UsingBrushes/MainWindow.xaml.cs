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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UsingBrushes.ViewModels;

namespace UsingBrushes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GradientStopsDataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            //  0: ComboBox (TemplateColumn), 1: TextColumn
            var columnIndex = e.Column.DisplayIndex;

            //  Only let ComboBox column through
            if (columnIndex != 0) return;



            var columnheader = e.Column.Header;
            
            var thisContentPresenter = e.EditingElement as ContentPresenter;
            var thisTemplate = thisContentPresenter.ContentTemplate;
            var combobox = thisTemplate.LoadContent();

            var dataContext = e.Row.DataContext as GradientStopViewModel;
            var currentItem = e.Row.Item as GradientStopViewModel;
            var currentColour = currentItem.Colour.Name;

            var currentOffset = dataContext.Offset;

            var obj = sender.GetType();

        }

        private void GradientStopsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void GradientStopsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void GradientStopsDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
    }
}
