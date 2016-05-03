using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UsingBrushes.Factories;
using UsingBrushes.Interfaces.Factories;

namespace UsingBrushes.ViewModels
{
    public class RadialGradientBrushViewModel : BindableViewModelBase
    {

        private readonly IColourInfoFactory _colourInfoFactory = new ColourInfoFactory();
        private const string DefaultFirstColourName = "Blue";
        private const string DefaultSecondColourName = "White";
        private const double DefaultFirstOffset = 1.0D;
        private const double DefaultSecondOffset = 0.0D;


        public event EventHandler OnRadialGradientBrushPropertiesChanged;

        public RadialGradientBrushViewModel()
        {
            _header = "Set Radial Gradient Properties";

            _radiusX = 1.0D;
            _radiusY = 1.0D;
            _centreX = 0.5D;
            _centreY = 0.5D;
            _originX = 0.7D;
            _originY = 0.3D;

            //  Create default GradientStops
            _gradientStops = new ObservableCollection<GradientStopViewModel>();
            /*
             * Add a CollectionChanged handler to the Observable collection to allow a
             * handler to be added to the item being added (GradientStopViewModel) which
             * is used to propogate changes in the UI to the view model and back up
             * to the UI, allowing the UI to redraw the LinearGradientBrush and show
             * the changes as they are made.
             */            
            _gradientStops.CollectionChanged += GradientStops_OnCollectionChanged;

            _gradientStops.Add(new GradientStopViewModel(DefaultFirstColourName, DefaultFirstOffset));
            _gradientStops.Add(new GradientStopViewModel(DefaultSecondColourName, DefaultSecondOffset));

        }

        private string _header;
        public string Header
        {
            get { return _header; }
            private set
            {
                SetProperty(ref _header, value);
            }
        }

        /*
         * Raduis X and Y .
         */
        private double _radiusX;
        public Double RadiusX
        {
            get { return _radiusX; }
            set
            {
                SetProperty(ref _radiusX, value);
                RaiseOnRadialGradientBrushPropertiesChanged();
            }
        }
        private double _radiusY;
        public Double RadiusY
        {
            get { return _radiusY; }
            set
            {
                SetProperty(ref _radiusY, value);
                RaiseOnRadialGradientBrushPropertiesChanged();
            }
        }

        /*
         * Centre point of fill area
         */
        private double _centreX;
        public double CentreX
        {
            get { return _centreX; }
            set
            {
                SetProperty(ref _centreX, value);
                RaiseOnRadialGradientBrushPropertiesChanged();
            }
        }
        private double _centreY;
        public double CentreY
        {
            get { return _centreY; }
            set
            {
                SetProperty(ref _centreY, value);
                RaiseOnRadialGradientBrushPropertiesChanged();
            }
        }
        public Point Centre
        {
            get { return new Point(_centreX, _centreY); }
        }

        /*
         * Gradient Origin
         */
        private double _originX;
        public double OriginX
        {
            get { return _originX; }
            set
            {
                SetProperty(ref _originX, value);
                RaiseOnRadialGradientBrushPropertiesChanged();
            }
        }
        private double _originY;
        public double OriginY
        {
            get { return _originY; }
            set
            {
                SetProperty(ref _originY, value);
                RaiseOnRadialGradientBrushPropertiesChanged();
            }
        }
        public Point GradientOrigin
        {
            get { return new Point(_originX, _originY); }
        }


        /*
         * Gradient Stops
         * 
         */
        private ObservableCollection<GradientStopViewModel> _gradientStops;
        public ObservableCollection<GradientStopViewModel> GradientStops
        {
            get { return _gradientStops; }
            set { SetProperty(ref _gradientStops, value); }
        }


        /*
         * Private Methods
         */
        private void RaiseOnRadialGradientBrushPropertiesChanged()
        {
            if (OnRadialGradientBrushPropertiesChanged != null)
                OnRadialGradientBrushPropertiesChanged(this, new EventArgs());
        }


        /*
         * Event Handlers
         * 
         */
        /*
         * Handle the CollectionChanged event for the GradientStops collection, 
         * add a handler for the GradientStopsViewModel.OnGradientStopsPropertyChanged
         * event for the gradientstop being added to the collection.
         * This ensures that changes to the UI for this gradientstop will be
         * reflected in the UI.
         */
        private void GradientStops_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;

            //  Add an event handler to each item being added: There should
            //  only be one such item as this collection is bound to the 
            //  ItemSource of the datagrid.
            foreach (GradientStopViewModel item in e.NewItems)
            {
                item.OnGradientStopPropertiesChanged += OnGradientStopPropertiesChanged;
            }
        }


        private void OnGradientStopPropertiesChanged(object sender, EventArgs eventArgs)
        {
            /*
             * Buble the Properties changed event up to the parent.  It's used only for triggering 
             * the code above to update the Brush and force a redraw of the shape and brush.
             * Nothing is passed through the eventArgs, so we can just call the method to
             * Raise the propertieschanged event for this class.
             */
             RaiseOnRadialGradientBrushPropertiesChanged();
        }

    }
}
