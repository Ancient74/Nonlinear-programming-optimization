using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Solution.SolutionItem
{
    public class SolutionItemViewModel: NotifiableObject
    {
        private string variableName;
        private double solution;

        public string VariableName
        {
            get => variableName;
            set
            {
                if (variableName == value)
                    return;
                variableName = value;
                OnPropertyChanged();
            }
        }

        public double Solution
        {
            get => solution;
            set
            {
                if (solution == value)
                    return;
                solution = value;
                OnPropertyChanged();
            }
        }
    }
}
