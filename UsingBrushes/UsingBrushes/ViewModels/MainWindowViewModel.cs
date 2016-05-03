using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UsingBrushes.Data;
using UsingBrushes.Extensions;
using UsingBrushes.Interfaces.Data;
using UsingBrushes.Models;

namespace UsingBrushes.ViewModels
{
    public class MainWindowViewModel : BindableViewModelBase
    {
        #region Fields

        private readonly IDataManager _manager;     //  Accesses the data


        #endregion


        public MainWindowViewModel()
        {
            _manager = new DataManager();
            
            //  Load the Data
            
            //  Define the Shapes that can be shown
            _shapesNames = _manager.GetShapeNames();
            /*
             * Leave this null check out and the datacontext throws an error in the designtime editor
             * "Value cannot be null, Parameter Name: Source", when the XAML references this
             * ViewModel.
             */
            if (_shapesNames != null)
                _selectedShapeName = _shapesNames.FirstOrDefault();
            ToggleVisibility(_selectedShapeName);


            //  Define Colours that can be used
            //var colours = typeof (Colors).GetRuntimeProperties();
            //_coloursCollection = colours.ToObservableCollection();
            _coloursCollection = _manager.GetColors();


            //  Define the SolidBrush properties, which also set the SelectedColour
            _solidBrushProperties = new SolidBrushViewModel();
            //  Attach ColorChanged event handler
            _solidBrushProperties.OnSolidColorBrushColorChanged += SolidBrushPropertiesOnOnSolidColorBrushColorChanged;

            //  Define the LinearGradientBrush properties
            _linearGradientBrushProperties = new LinearGradientBrushViewModel();
            _linearGradientBrushProperties.OnLinearGradientBrushPropertiesChanged += LinearGradientBrushPropertiesOnOnLinearGradientBrushPropertiesChanged;

            //  Define the RadialGradientBrush properties
            //  Todo: Instantiate the brush view model class and attach the event handler
            _radialGradientBrushProperties = new RadialGradientBrushViewModel();
            _radialGradientBrushProperties.OnRadialGradientBrushPropertiesChanged += RadialGradientBrushPropertiesOnOnRadialGradientBrushPropertiesChanged;

            //  Define the Brushes that can be used
            _brushNames = _manager.GetBrushNames();
            _selectedBrushName = _brushNames.FirstOrDefault();
            _selectedBrush = CreateBrush(_selectedBrushName);   // Requires the Brush Properties to be set
            ToggleBrushPropertiesVisibility(_selectedBrushName);
        }


        #region Properties

        /* The Shapes ComboBox stuff
         */
        private readonly ObservableCollection<string> _shapesNames;
        public ObservableCollection<string> ShapesNames
        {
            get { return _shapesNames; }
        }

        private string _selectedShapeName;
        public string SelectedShapeName
        {
            get { return _selectedShapeName; }
            set
            {
                SetProperty(ref _selectedShapeName, value);

                //  Swap to the selected shape for displaying
                ToggleVisibility(_selectedShapeName);
            }
        }

        /* 
         * Visibility for the shapes, used to toggles the shape
         * currently visible within the ViewBox
         */
        private bool _rectangleIsVisible;
        public bool RectangleIsVisible
        {
            get { return _rectangleIsVisible; }
            private set
            {
                SetProperty(ref _rectangleIsVisible, value);
            }
        }

        private bool _ellipseIsVisible;
        public bool EllipseIsVisible
        {
            get { return _ellipseIsVisible; }
            private set
            {
                SetProperty(ref _ellipseIsVisible, value);
            }
        }

        /* 
         * Brush Combobox and selected Brush properties
         */
        private ObservableCollection<string> _brushNames;
        public ObservableCollection<string> BrushNames
        {
            get { return _brushNames; }
        }

        private string _selectedBrushName;
        public string SelectedBrushName
        {
            get { return _selectedBrushName; }
            set
            {
                SetProperty(ref _selectedBrushName, value);

                /*  
                 * Trigger the logic to create the appropriate brush 
                 * and update the selectedBrush itself.
                 */
                //  Update the selected Brush with the correct properties
                this.SelectedBrush = CreateBrush(_selectedBrushName);

                //  show the properties of the selected brush
                ToggleBrushPropertiesVisibility(_selectedBrushName);
            }
        }

        private Brush _selectedBrush;
        public Brush SelectedBrush
        {
            get { return _selectedBrush; }
            private set
            {
                SetProperty(ref _selectedBrush, value);
            }
        }


        /* 
         * Colours for the Colour selection ComboBoxes, so that
         * we only have on instance of them (256 pre-defined colours).
         */
        private ObservableCollection<ColourInfo> _coloursCollection;
        public ObservableCollection<ColourInfo> ColoursCollection
        { get { return _coloursCollection; } }




        /*
         * Properties, ViewModels for the properties of each
         * Brush Type.
         */
        private SolidBrushViewModel _solidBrushProperties;
        public SolidBrushViewModel SolidBrushProperties
        {
            get { return _solidBrushProperties; }
            set { SetProperty(ref _solidBrushProperties, value);}
        }

        private bool _isSolidBrushPropertiesVisible;
        public bool IsSolidBrushPropertiesVisible
        {
            get { return _isSolidBrushPropertiesVisible; }
            private set { SetProperty(ref _isSolidBrushPropertiesVisible, value);}
        }

        private void SolidBrushPropertiesOnOnSolidColorBrushColorChanged(object sender, EventArgs eventArgs)
        {
            this.SelectedBrush = CreateBrush(_selectedBrushName);
        }


        private LinearGradientBrushViewModel _linearGradientBrushProperties;
        public LinearGradientBrushViewModel LinearGradientBrushProperties
        {
            get { return _linearGradientBrushProperties; }
            set { SetProperty(ref _linearGradientBrushProperties, value); }
        }

        private bool _isLinearGradientBrushPropertiesVisible;
        public bool IsLinearGradientBrushPropertiesVisible
        {
            get { return _isLinearGradientBrushPropertiesVisible; }
            private set { SetProperty(ref _isLinearGradientBrushPropertiesVisible, value);}
        }

        private void LinearGradientBrushPropertiesOnOnLinearGradientBrushPropertiesChanged(object sender, EventArgs eventArgs)
        {
            this.SelectedBrush = CreateBrush(_selectedBrushName);
        }


        private RadialGradientBrushViewModel _radialGradientBrushProperties;

        public RadialGradientBrushViewModel RadialGradientBrushProperties
        {
            get { return _radialGradientBrushProperties; }
            set { SetProperty(ref _radialGradientBrushProperties, value); }
        }

        private bool _isRadialGradientBrushPropertiesVisible;

        public bool IsRadialGradientBrushPropertiesVisible
        {
            get { return _isRadialGradientBrushPropertiesVisible; }
            private set { SetProperty(ref _isRadialGradientBrushPropertiesVisible, value); }
        }
        private void RadialGradientBrushPropertiesOnOnRadialGradientBrushPropertiesChanged(object sender, EventArgs eventArgs)
        {
            this.SelectedBrush = CreateBrush(_selectedBrushName);
        }




        #endregion





        private void ToggleVisibility(string shapeName)
        {
            switch (shapeName)
            {
                case  "Rectangle":
                    this.RectangleIsVisible = true;
                    this.EllipseIsVisible = false;
                    break;
                case "Ellipse":
                    this.RectangleIsVisible = false;
                    this.EllipseIsVisible = true;
                    break;
                default:
                    this.RectangleIsVisible = true;
                    this.EllipseIsVisible = false;
                    break;
            }
        }

        private void ToggleBrushPropertiesVisibility(string brushName)
        {
            if (brushName == typeof(SolidColorBrush).Name)
            {
                this.IsSolidBrushPropertiesVisible = true;
                this.IsLinearGradientBrushPropertiesVisible = false;
                this.IsRadialGradientBrushPropertiesVisible = false;
            }
            if (brushName == typeof (LinearGradientBrush).Name)
            {
                this.IsSolidBrushPropertiesVisible = false;
                this.IsLinearGradientBrushPropertiesVisible = true;
                this.IsRadialGradientBrushPropertiesVisible = false;
            }
            if (brushName == typeof (RadialGradientBrush).Name)
            {
                this.IsSolidBrushPropertiesVisible = false;
                this.IsLinearGradientBrushPropertiesVisible = false;
                this.IsRadialGradientBrushPropertiesVisible = true;
            }
        }


        private Brush CreateBrush(string brushName)
        {
            if (brushName == typeof (SolidColorBrush).Name)
            {
                var brush = new SolidColorBrush
                {
                    Color = _solidBrushProperties.SelectedColor.Color
                };
                return brush;
            }

            if (brushName == typeof (LinearGradientBrush).Name)
            {
                var brush = new LinearGradientBrush();
                brush.StartPoint = _linearGradientBrushProperties.StartPoint;
                brush.EndPoint = _linearGradientBrushProperties.EndPoint;

                foreach (var gradientStopViewModel in _linearGradientBrushProperties.GradientStops)
                {
                    brush.GradientStops.Add(new GradientStop(gradientStopViewModel.Colour.Color, gradientStopViewModel.Offset));
                }
                /*
                 * Well: I've just spent 2 to 3 hours trying to understand why I couln't
                 * change a SolidColorBrush to a LinearGradientBrush in code and bind the
                 * Fill property of the shape to it.  Lots of methods of transforming one
                 * to the other, using XAMl animations and storyboards but not in code.
                 * 
                 * Then, the light bulb moment, I hadn't added the GradientStops collection
                 * to the brush, therefore it hadn't a clue how to render it so it 
                 * rendered nothing.  Immediately I added the GradientStops it rendered
                 * perfectly.  
                 * 
                 * !!! WHAT A BLOOMIN' IDIOT.  NUMPTY OF THE FIRST ORDER.  BAWHIED!!!
                 *
                 * Edit:
                 * Code now refactored to use the underlying properties of the GradientStopViewModel
                 * which is part of the LinearGradientBrushViewModel, but comment and old code left 
                 * to remind me what an idiot I was!!!!!
                 */
                //brush.GradientStops = gradientStops;

                return brush;
            }

            if (brushName == typeof(RadialGradientBrush).Name)
            {
                /*
                 * Let's now set up a RadialGradientBrush, just to prove the logic works
                 * 
                 * 
                 */
                //  Done: refactor this method to use the actual class and factory.
                var brush = new RadialGradientBrush
                {
                    RadiusX = _radialGradientBrushProperties.RadiusX,
                    RadiusY = _radialGradientBrushProperties.RadiusY,
                    GradientOrigin = _radialGradientBrushProperties.GradientOrigin,
                    Center = _radialGradientBrushProperties.Centre
                };
                foreach (var gradientStop in _radialGradientBrushProperties.GradientStops)
                {
                    brush.GradientStops.Add(new GradientStop(gradientStop.Colour.Color, gradientStop.Offset));
                }

                return brush;
            }

            return new SolidColorBrush(Colors.Blue);
        }

    }
}
