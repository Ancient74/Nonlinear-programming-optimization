using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Models
{
    public struct ProblemModel
    {
        public SerializableFunctionInputModel functionInputModel;
        public SerializableConstraintItemModel[] constraintModels;
        public bool greaterThanZeroUsed;
        public double[] initials;
        public SolutionModel solutionModel;
        public DateTime timestamp;
    }
}
