using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Models
{
    public struct ConstraintItemModel
    {
        public Expression expression;
        public Argument[] arguments;
        public double leftCoef;
        public ConstrainType constrainType;
    }

    public enum ConstrainType
    {
        Equal, GreaterThan
    }

    public struct SerializableConstraintItemModel
    {
        public string expression;
        public int numberOfVariables;
        public double leftCoef;
    }
}
