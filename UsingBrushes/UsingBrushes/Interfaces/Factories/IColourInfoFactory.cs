using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingBrushes.Models;

namespace UsingBrushes.Interfaces.Factories
{
    public interface IColourInfoFactory
    {
        ColourInfo Create();
        ColourInfo Create(string colourName);
    }
}
