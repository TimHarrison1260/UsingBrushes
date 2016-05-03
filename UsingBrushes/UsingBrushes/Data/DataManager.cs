using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UsingBrushes.Interfaces.Data;
using UsingBrushes.ViewModels;
using UsingBrushes.Extensions;
using UsingBrushes.Factories;
using UsingBrushes.Interfaces.Factories;
using UsingBrushes.Models;

namespace UsingBrushes.Data
{
    public  class DataManager : IDataManager
    {
        private readonly ObservableCollection<ShapeViewModel> _shapeCollection;
        private readonly ObservableCollection<Brush> _brushesCollection;

        private readonly IColourInfoFactory _colourFactory;


        public DataManager()
        {
            _shapeCollection = SetShapeData();

            _brushesCollection = SetBrushData();

            _colourFactory = new ColourInfoFactory();
        }


        public ObservableCollection<ShapeViewModel> GetShapeViewModels()
        {
            return _shapeCollection;
        }

        public ObservableCollection<string> GetShapeNames()
        {
            /*
             * Defined and use a Generic extension method of type IEnumerable<T> to 
             * convert the collection to an ObservableCollection of the same type T.
             * 
             * This avoids an invalid cast from the IEnumerable<T> resulting from the
             * LINQ Select method.
             */
            var result =  _shapeCollection.Select(s => s.Name).ToObservableCollection();
            return result;
        }


        public ObservableCollection<string> GetBrushNames()
        {
            var brushNames = _brushesCollection.Select(b => b.GetType().Name).ToObservableCollection();
            return brushNames;
        }


        public ObservableCollection<ColourInfo> GetColors()
        {
            //  See sample code from Course: Using Blend to Design UI
            //var colours = typeof (Colors).GetRuntimeProperties().Select(c => new ColourInfo
            //{
            //    Name = c.Name,
            //    Color = (Color) c.GetValue(null)
            //});

            var colours = typeof (Colors).GetRuntimeProperties().Select(c => _colourFactory.Create(c.Name));

            return colours.ToObservableCollection();
        }




        /*
         * Private methods to create the data itself
         * 
         */

        private ObservableCollection<ShapeViewModel> SetShapeData()
        {
            var shapes = new ObservableCollection<ShapeViewModel>();

            var rectangle = new ShapeViewModel("Rectangle");
            shapes.Add(rectangle);

            var ellipse = new ShapeViewModel("Ellipse");
            shapes.Add(ellipse);

            return shapes;
        }

        private ObservableCollection<Brush> SetBrushData()
        {
            var brushes = new ObservableCollection<Brush>
            {
                new SolidColorBrush(),
                new LinearGradientBrush(),
                new RadialGradientBrush()
            };

            return brushes;
        }

    }
}
