using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Models
{
    public struct SolutionModelItem
    {
        public string variableName;
        public double value;
    }

    public struct SolutionModel
    {
        public SolutionModelItem[] variables;
        public double result;
    }
}
