using Lagrande.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.History.HistoryItem
{
    public class HistoryItemViewModel
    {
        public string Expression { get; set; }
        public int NumberOfConstraints { get; set; }

        public int NumberOfVariables { get; set; }

        public DateTime Timestamp { get; set; }

        public string GreaterThanZero { get; set; }

        public double Result { get; set; }

        public string Action { get; set; }

        public ProblemModel Model { get; }

        public HistoryItemViewModel(ProblemModel model)
        {
            bool greaterThanZero = model.greaterThanZeroUsed;
            Expression = model.functionInputModel.expression;
            NumberOfVariables = model.functionInputModel.numberOfVariables;
            NumberOfConstraints = greaterThanZero ? model.constraintModels.Length - NumberOfVariables : model.constraintModels.Length;
            GreaterThanZero = greaterThanZero ? "Да" : "Нет";
            Action = model.functionInputModel.extremumType == ExtremumType.Max ? "Максимизация" : "Минимизация";
            Timestamp = model.timestamp;
            Result = model.solutionModel.result;
            Model = model;
        }
    }
}
