using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UsingBrushes.Factories;
using UsingBrushes.Interfaces.Factories;
using UsingBrushes.Models;

namespace UsingBrushes.ViewModels
{
    public class LinearGradientBrushViewModel : BindableViewModelBase
    {
        private readonly IColourInfoFactory _colourInfoFactory = new ColourInfoFactory();
        private const string DefaultFirstColourName = "Blue";
        private const string DefaultSecondColourName = "White";
        private const double DefaultFirstOffset = 0.0D;
        private const double DefaultSecondOffset = 1.0D;


        public event EventHandler OnLinearGradientBrushPropertiesChanged;

        public LinearGradientBrushViewModel()
        {
            _header = "Set Linear Gradient Properties";

            //_startPoint = new Point(0.0, 0.0);
            _startPointX = 0.0D;
            _startPointY = 0.0D;
            //_endPoint = new Point(1.0, 1.0);
            _endPointX = 1.0D;
            _endPointY = 1.0D;

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
            //_gradientStops.Add(CreateGradientStopAndAssignHandler(DefaultFirstColourName, DefaultFirstOffset));
            _gradientStops.Add(new GradientStopViewModel(DefaultSecondColourName, DefaultSecondOffset));
            //_gradientStops.Add(CreateGradientStopAndAssignHandler(DefaultSecondColourName, DefaultSecondOffset));

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
         * Can bind OK to the Point.X and Point.Y and update the screen if the
         * coordinates are changed, but two way binding on the X and Y components
         * doesn't appear to trigger the property setter.  Instead, create Double
         * proerties for the X and Y components, which DO respond, and allow the
         * Point to be retrieved by creating a new instand of Point on the Getter.
         */
        private double _startPointX;
        public Double StartPointX
        {
            get { return _startPointX; }
            set
            {
                SetProperty(ref _startPointX, value);
                RaiseOnLinearGradientBrushPropertiesChanged();
            }
        }
        private double _startPointY;
        public Double StartPointY
        {
            get { return _startPointY; }
            set
            {
                SetProperty(ref _startPointY, value);
                RaiseOnLinearGradientBrushPropertiesChanged();
            }
        }
        public Point StartPoint
        {
            get { return new Point(_startPointX, _startPointY); }
        }

        private double _endPointX;
        public double EndPointX
        {
            get { return _endPointX; }
            set
            {
                SetProperty(ref _endPointX, value);
                RaiseOnLinearGradientBrushPropertiesChanged();
            }
        }
        private double _endPointY;
        public double EndPointY
        {
            get { return _endPointY; }
            set
            {
                SetProperty(ref _endPointY, value);
                RaiseOnLinearGradientBrushPropertiesChanged();
            }
        }
        public Point EndPoint
        {
            get { return new Point(_endPointX, _endPointY); }
        }


        private ObservableCollection<GradientStopViewModel> _gradientStops;
        public ObservableCollection<GradientStopViewModel> GradientStops
        {
            get { return _gradientStops; }
            set { SetProperty(ref _gradientStops, value); }
        }


        //private GradientStopViewModel _selectGradientStop;

        //public GradientStopViewModel SelectedGradientStop
        //{
        //    get { return _selectGradientStop; }
        //    set
        //    {
        //        SetProperty(ref _selectGradientStop, value);
        //        //  May need to rais the PropertiesChanged event, to trigger a UI update in the Parent

        //    }
        //}

        //private int _selectedGradientStopIndex;
        //public int SelectedGradientStopIndex
        //{
        //    get { return _selectedGradientStopIndex; }
        //    set { SetProperty(ref _selectedGradientStopIndex, value); }
        //}




        /*
         * Private Methods
         * 
         */
        private void RaiseOnLinearGradientBrushPropertiesChanged()
        {
            if (OnLinearGradientBrushPropertiesChanged != null)
                OnLinearGradientBrushPropertiesChanged(this, new EventArgs());
        }


        //  Create instance of viewModel and attach an event handler.  Each is included within the collection.
        //
        //  No longer needed as the event handler is added in the ObservableCollection.CollectionChanged event
        //
        //private GradientStopViewModel CreateGradientStopAndAssignHandler(string colourName, double offset)
        //{
        //    var gradientStop = new GradientStopViewModel(colourName, offset);
        //    //  Assign handler
        //    gradientStop.OnGradientStopPropertiesChanged += OnGradientStopPropertiesChanged;
        //    return gradientStop;
        //}



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

            //var action = e.Action;
            //var newItems = e.NewItems;
            //var newStartIndex = e.NewStartingIndex;
            //var oldItems = e.OldItems;
            //var oldStartIndex = e.OldStartingIndex;
        }


        private void OnGradientStopPropertiesChanged(object sender, EventArgs eventArgs)
        {
            /*
             * Buble the Properties changed event up to the parent.  It's used only for triggering 
             * the code above to update the Brush and force a redraw of the shape and brush.
             * Nothing is passed through the eventArgs, so we can just call the method to
             * Raise the propertieschanged event for this class.
             */
             RaiseOnLinearGradientBrushPropertiesChanged();
        }


    }
}
