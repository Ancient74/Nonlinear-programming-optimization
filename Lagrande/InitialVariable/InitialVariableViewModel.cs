using Lagrande.InitialVariable.VariableItem;
using Lagrande.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.InitialVariable
{
    public class InitialVariableViewModel: NotifiableObject
    {
        private int numberOfVariables;
        private List<VariableItemViewModel> variables = new List<VariableItemViewModel>();
        public List<VariableItemViewModel> Variables
        {
            get => variables;
            set
            {
                if (variables == value)
                    return;
                variables = value;
                OnPropertyChanged();
            }
        }
        public int NumberOfVariables
        {
            get => numberOfVariables;
            set
            {
                if (numberOfVariables == value)
                    return;
                int diff = value - numberOfVariables;
                if (diff > 0)
                {
                    var newList = new List<VariableItemViewModel>(variables);
                    for (int i = 0; i< diff; i++)
                    {
                        var item = new VariableItemViewModel
                        {
                            Name = "x" + (newList.Count + 1).ToString(),
                            Value = 0
                        };
                        newList.Add(item);
                    }
                    Variables = newList;
                }
                else
                {
                    var newList = new List<VariableItemViewModel>(variables);
                    for (int i = 0; i < -diff; i++)
                    {
                        if (newList.Count > 0)
                        {
                            newList.Remove(newList.Last());
                        }
                    }
                    Variables = newList;

                }
                numberOfVariables = value;
                OnPropertyChanged();
            }
        }

        public double[] GetModel()
        {
            return Variables.Select(item => item.Value).ToArray();
        }

        public void ApplyModel(ProblemModel model)
        {
            NumberOfVariables = model.functionInputModel.numberOfVariables;
            for (int i = 0; i < model.initials.Length; i++)
            {
                var current = model.initials[i];
                Variables[i].Value = current;
            }
        }
        
    }
}
