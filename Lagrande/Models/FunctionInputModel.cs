using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Models
{
    public struct FunctionInputModel
    {
        public Expression expression;
        public Argument[] arguments;
        public ExtremumType extremumType;
    }

    public struct SerializableFunctionInputModel
    {
        public string expression;
        public int numberOfVariables;
        public ExtremumType extremumType;
    }
}
