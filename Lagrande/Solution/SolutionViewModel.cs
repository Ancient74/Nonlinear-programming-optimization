using Lagrande.Models;
using Lagrande.Solution.SolutionItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Solution
{
    public class SolutionViewModel: NotifiableObject
    {
        private List<SolutionItemViewModel> solutionList;
        private double result;


        public List<SolutionItemViewModel> SolutionList
        {
            get => solutionList;
            set
            {
                if (solutionList == value)
                    return;
                solutionList = value;
                OnPropertyChanged();
            }
        }

        public double Result
        {
            get => result;
            set
            {
                if (result == value)
                    return;
                result = value;
                OnPropertyChanged();
            }
        }


        public void ApplyModel(ProblemModel model)
        {
            SolutionList = model.solutionModel.variables.Select(item => new SolutionItemViewModel() { Solution = item.value, VariableName = item.variableName }).ToList();
            Result = model.solutionModel.result;
        }

        public void ApplyModel(SolutionModel model)
        {
            SolutionList = model.variables.Select(item => new SolutionItemViewModel() { Solution = item.value, VariableName = item.variableName }).ToList();
            Result = model.result;
        }

        public void Clear()
        {
            SolutionList = new List<SolutionItemViewModel>();
        }
    }
}
