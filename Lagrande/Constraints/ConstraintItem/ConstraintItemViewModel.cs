using Lagrande.Models;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lagrande.Constraints.ConstraintItem
{
    public class ConstraintItemViewModel: NotifiableObject
    {
        private string functionExpressionString;
        private Argument[] arguments;
        private int numberOfVariables;
        private double leftCoef;

        public event Action<bool> ValidChanged;

        public int NumberOfVariables
        {
            get => numberOfVariables;
            set
            {
                if (numberOfVariables == value)
                    return;
                numberOfVariables = value;
                arguments = new Argument[value];
                for (int i = 1; i <= value; i++)
                {
                    arguments[i - 1] = new Argument("x" + i.ToString());
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged(nameof(StarVisibility));
                ValidChanged?.Invoke(IsValid);
            }
        }

        public double LeftCoef
        {
            get => leftCoef;
            set
            {
                if (leftCoef == value)
                    return;
                leftCoef = value;
                OnPropertyChanged();
            }
        }

        public Visibility StarVisibility
        {
            get => IsValid ? Visibility.Hidden : Visibility.Visible;
        }

        public bool IsValid
        {
            get
            {
                return new org.mariuszgromada.math.mxparser.Expression(functionExpressionString, arguments).checkSyntax();
            }
        }
        public string FunctionExpressionString
        {
            get => functionExpressionString;
            set
            {
                if (functionExpressionString == value)
                    return;
                functionExpressionString = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged(nameof(StarVisibility));
                ValidChanged?.Invoke(IsValid);
            }
        }

        public ConstraintItemViewModel()
        {
            NumberOfVariables = 1;
            LeftCoef = 0;
        }

        public ConstraintItemModel GetModel()
        {
            return new ConstraintItemModel
            {
                expression = new org.mariuszgromada.math.mxparser.Expression(FunctionExpressionString, arguments),
                arguments = arguments,
                leftCoef = LeftCoef,
                constrainType = ConstrainType.Equal
            };
        }
    }
}
