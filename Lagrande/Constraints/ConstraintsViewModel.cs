using Lagrande.Constraints.ConstraintItem;
using Lagrande.Models;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lagrande.Constraints
{
    public class ConstraintsViewModel : NotifiableObject
    {
        private List<ConstraintItemViewModel> constraints = new List<ConstraintItemViewModel>();
        private int numberOfVariables;
        private bool isValid;
        private bool greaterThanZero;

        public event Action<bool> ValidChanged;

        public bool IsValid
        {
            get => isValid;
            set
            {
                if (isValid == value)
                    return;
                isValid = value;
                ValidChanged?.Invoke(value);
            }
        }

        public bool GreaterThanZero
        {
            get => greaterThanZero;
            set
            {
                if (greaterThanZero == value)
                    return;
                greaterThanZero = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfVariables
        {
            get => numberOfVariables;
            set
            {
                numberOfVariables = value;
                Constraints.ForEach(c => c.NumberOfVariables = value);
            }
        }

        public List<ConstraintItemViewModel> Constraints
        {
            get => constraints;
            set
            {
                if (constraints == value)
                    return;
                constraints = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand RemoveItemCommand { get; }

        public ConstraintsViewModel()
        {
            AddCommand = new CallbackCommand(OnAdd);
            RemoveCommand = new CallbackCommand(OnRemove);
            RemoveItemCommand = new CallbackCommand(OnRemoveItem);
        }

        private void OnAdd(object obj)
        {
            List<ConstraintItemViewModel> newList = new List<ConstraintItemViewModel>(Constraints);
            var item = new ConstraintItemViewModel();
            item.NumberOfVariables = NumberOfVariables;
            newList.Add(item);
            item.ValidChanged += OnValidChanged;
            Constraints = newList;
        }

        private void OnValidChanged(bool obj)
        {
            bool valid = true;
            Constraints.ForEach(item =>
            {
                valid = valid && item.IsValid;
            });
            IsValid = valid;
        }

        private void OnRemove(object obj)
        {
            if (Constraints.Count == 0)
            {
                return;
            }
            List<ConstraintItemViewModel> newList = new List<ConstraintItemViewModel>(Constraints);
            var item = newList.Last();
            item.ValidChanged -= OnValidChanged;
            newList.Remove(item);
            Constraints = newList;
        }

        private void OnRemoveItem(object obj)
        {
            if (!(obj is ConstraintItemViewModel))
                return;
            var item = obj as ConstraintItemViewModel;
            List<ConstraintItemViewModel> newList = new List<ConstraintItemViewModel>(Constraints);
            item.ValidChanged -= OnValidChanged;
            newList.Remove(item);
            Constraints = newList;
        }

        public void Clear()
        {
            Constraints = new List<ConstraintItemViewModel>();
        }

        public ConstraintItemModel[] GetModel()
        {
            var constr = Constraints.Select(item => item.GetModel()).ToList();
            if (!GreaterThanZero)
                return constr.ToArray();

            for (int i = 0; i < NumberOfVariables; i++)
            {
                Argument[] arguments = new Argument[NumberOfVariables];
                for (int j = 0; j < NumberOfVariables; j++)
                {
                    arguments[j] = new Argument("x" + (j + 1).ToString(), 0);
                }
                constr.Add(new ConstraintItemModel()
                {
                    leftCoef = 0,
                    expression = new Expression(arguments[i].getArgumentName(), arguments),
                    arguments = arguments,
                    constrainType = ConstrainType.GreaterThan
                });
            }
            return constr.ToArray();
        }

        public void ApplyModel(ProblemModel model)
        {
            Clear();
            int constraintsNumber = model.greaterThanZeroUsed ? model.constraintModels.Length - model.functionInputModel.numberOfVariables : model.constraintModels.Length;
            for (int i = 0; i < constraintsNumber; i++)
            {
                var current = model.constraintModels[i];
                AddCommand.Execute(null);
                Constraints[i].FunctionExpressionString = current.expression;
                Constraints[i].LeftCoef = current.leftCoef;
                Constraints[i].NumberOfVariables = current.numberOfVariables;
            }
            GreaterThanZero = model.greaterThanZeroUsed;
        }
    }
}
