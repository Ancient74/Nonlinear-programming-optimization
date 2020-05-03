using Lagrande.Models;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lagrande.FunctionInput
{
    public class FunctionInputViewModel : NotifiableObject
    {
        private string functionString;
        private string selectedActionString;
        private ExtremumType selectedExtremumType;
        private int numberOfVariables;
        private Argument[] arguments;
        private bool canSolve;

        public event Action SolveClicked;
        public event Action<int> NumberOfVariablesChanged;

        public string FunctionString
        {
            get => functionString;
            set
            {
                if (functionString == value)
                    return;
                functionString = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }

        public bool CanSolve
        {
            get => canSolve;
            set
            {
                if (canSolve == value)
                    return;
                canSolve = value;
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }

        public string SelectedAction
        {
            get => selectedActionString;
            set
            {
                if (selectedActionString == value)
                    return;
                selectedActionString = value;
                selectedExtremumType = value == ExtremumTypes[0] ? ExtremumType.Min : ExtremumType.Max;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }

        public bool ButtonEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(selectedActionString) && GetExpression().checkSyntax() && CanSolve;
            }
        }

        public List<string> ExtremumTypes { get; } = new List<string> { "Минимизациия", "Максимизация" };
        public int NumberOfVariables
        {
            get => numberOfVariables;
            set
            {
                if (numberOfVariables == value)
                    return;
                numberOfVariables = value;
                arguments = new Argument[numberOfVariables];
                for (int i = 1; i <= numberOfVariables; i++)
                {
                    arguments[i - 1] = new Argument("x" + i.ToString(), 0);
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonEnabled));
                NumberOfVariablesChanged?.Invoke(value);
            }
        }

        public ICommand ButtonClickCommand { get; }

        public FunctionInputViewModel()
        {
            NumberOfVariables = 1;
            ButtonClickCommand = new CallbackCommand((arg) =>
             {
                 SolveClicked?.Invoke();
             });
        }

        public Expression GetExpression()
        {
            return new Expression(functionString, arguments);
        }

        public FunctionInputModel GetModel()
        {
            return new FunctionInputModel { expression = GetExpression(), extremumType = selectedExtremumType, arguments = arguments };
        }

        public void ApplyModel(ProblemModel model)
        {
            NumberOfVariables = model.functionInputModel.numberOfVariables;
            FunctionString = model.functionInputModel.expression;
            SelectedAction = model.functionInputModel.extremumType == ExtremumType.Max ? ExtremumTypes[1] : ExtremumTypes[0];
        }
    }
}
