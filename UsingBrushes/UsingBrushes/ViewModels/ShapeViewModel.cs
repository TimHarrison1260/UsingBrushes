using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingBrushes.ViewModels
{
    public class ShapeViewModel: BindableViewModelBase
    {

        public ShapeViewModel(string name)
        {
            this.Name = name;
        }


        public string Name { get; private set; }



    }
}
