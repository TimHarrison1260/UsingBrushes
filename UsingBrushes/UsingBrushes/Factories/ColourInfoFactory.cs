using System.Linq;
using System.Windows.Media;
using UsingBrushes.Interfaces.Factories;
using UsingBrushes.Models;

namespace UsingBrushes.Factories
{
    public class ColourInfoFactory : IColourInfoFactory
    {
        private const string DefaultColourName = "Blue";

        public ColourInfo Create()
        {
            return this.Create(DefaultColourName);
        }

        public ColourInfo Create(string colourName)
        {
            var thisColour = string.IsNullOrWhiteSpace(colourName) ? DefaultColourName : colourName;

            var thisColourPropertyInfo = typeof(Colors).GetProperties().FirstOrDefault(c => c.Name == thisColour);
            if (thisColourPropertyInfo==null) return new ColourInfo();

            var colourInfo = new ColourInfo
            {
                Name = thisColourPropertyInfo.Name,
                Color = (Color) thisColourPropertyInfo.GetValue(null)
            };
            
            return colourInfo;
        }

    }
}
