using System;
using UsingBrushes.Factories;
using UsingBrushes.Interfaces.Factories;
using UsingBrushes.Models;

namespace UsingBrushes.ViewModels
{
    public class GradientStopViewModel: BindableViewModelBase
    {
        private readonly IColourInfoFactory _colourInfoFactory = new ColourInfoFactory();
        private const double DefaultOffset = 0.5D;


        public event EventHandler OnGradientStopPropertiesChanged;

        public GradientStopViewModel()
        {
            Colour = _colourInfoFactory.Create();
            Offset = DefaultOffset;
        }

        public GradientStopViewModel(string colourName, double offset)
        {
            Colour = _colourInfoFactory.Create(colourName);
            Offset = offset;
        }

        public GradientStopViewModel(ColourInfo colour, double offset)
        {
            Colour = colour;
            Offset = offset;
        }


        private ColourInfo _colour;
        public ColourInfo Colour
        {
            get { return _colour; }
            set
            {
                SetProperty(ref _colour, value);
                RaiseOnGradientStopPropertiesChanged();
            }
        }

        private double _offset;
        public double Offset
        {
            get { return _offset; }
            set
            {
                SetProperty(ref _offset, value);
                RaiseOnGradientStopPropertiesChanged();
            }
        }


        private void RaiseOnGradientStopPropertiesChanged()
        {
            if (OnGradientStopPropertiesChanged != null)
                OnGradientStopPropertiesChanged(this, EventArgs.Empty);
        }
    }
}
