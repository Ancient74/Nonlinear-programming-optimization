using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.InitialVariable.VariableItem
{
    public class VariableItemViewModel: NotifiableObject
    {
        private string name;
        private double val;

        public double Value
        {
            get => val;
            set
            {
                if (val == value)
                    return;
                val = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }

    }
}
