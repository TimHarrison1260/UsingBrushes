using System.Collections.ObjectModel;
using UsingBrushes.Models;
using UsingBrushes.ViewModels;

namespace UsingBrushes.Interfaces.Data
{
    public interface IDataManager
    {
        ObservableCollection<ShapeViewModel> GetShapeViewModels();

        ObservableCollection<string> GetShapeNames();

        ObservableCollection<string> GetBrushNames();

        ObservableCollection<ColourInfo> GetColors();
    }
}
