using Lagrande.Constraints;
using Lagrande.FunctionInput;
using Lagrande.History;
using Lagrande.InitialVariable;
using Lagrande.Models;
using Lagrande.Solution;
using Lagrande.Solver;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Input;

namespace Lagrande
{
    public class MainWindowViewModel
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private readonly string historyFileName = "history.json";

        public FunctionInputViewModel FunctionInputViewModel { get; set; } = new FunctionInputViewModel();
        public ConstraintsViewModel ConstraintsViewModel { get; set; } = new ConstraintsViewModel();
        public SolutionViewModel SolutionViewModel { get; set; } = new SolutionViewModel();
        public InitialVariableViewModel InitialVariableViewModel { get; set; } = new InitialVariableViewModel();
        public HistoryViewModel HistoryViewModel { get; set; } = new HistoryViewModel();

        public ICommand LoadedCommand { get; }


        public MainWindowViewModel()
        {
            LoadedCommand = new CallbackCommand(LoadHistory);

            FunctionInputViewModel.SolveClicked += OnSolveClicked;
            FunctionInputViewModel.NumberOfVariablesChanged += OnNumberOfVariablesChanged;
            ConstraintsViewModel.ValidChanged += OnConstraintsViewModelValidChanged;
            HistoryViewModel.HistoryChanged += OnHistoryChanged;
            HistoryViewModel.ModelSelected += OnModelSelected;

            //FunctionInputViewModel.NumberOfVariables = 9;
            //FunctionInputViewModel.FunctionString = "(300*x1+400*x2+500*x3+500*x4+500*x5+700*x6+600*x7+700*x8+900*x9)-0.1*(0.1*x1^2-0.1*x1*x2+0.5*x1*x3-0.2*x2*x1+0.8*x2^2+0.1*x2*x3+0.7*x3*x1+0.9*x3*x2+0.6*x3^2+0.3*x4^2+0.4*x4*x5+0.6*x4*x6-0.1*x5*x4+0.7*x5^2-0.2*x5*x6+0.4*x6*x4+0.8*x6*x5+0.4*x6^2+0.2*x7^2+0.5*x7*x8+0.7*x7*x9+0.1*x8*x7+0.8*x8^2-0.1*x8*x9+0.3*x9*x7+0.8*x9*x8+0.8*x9^2)";
            //FunctionInputViewModel.SelectedAction = FunctionInputViewModel.ExtremumTypes[1];

            //ConstraintsViewModel.AddCommand.Execute(null);
            //ConstraintsViewModel.AddCommand.Execute(null);
            //ConstraintsViewModel.AddCommand.Execute(null);

            //ConstraintsViewModel.GreaterThanZero = true;

            //ConstraintsViewModel.Constraints[0].FunctionExpressionString = "x1 + x2 + x3";
            //ConstraintsViewModel.Constraints[0].LeftCoef = 1;

            //ConstraintsViewModel.Constraints[1].FunctionExpressionString = "x4 + x5 + x6";
            //ConstraintsViewModel.Constraints[1].LeftCoef = 1;

            //ConstraintsViewModel.Constraints[2].FunctionExpressionString = "x7 + x8 + x9";
            //ConstraintsViewModel.Constraints[2].LeftCoef = 1;

            //InitialVariableViewModel.Variables.ForEach(item => item.Value = 0);
        }

        private void OnModelSelected(ProblemModel obj)
        {
            FunctionInputViewModel.ApplyModel(obj);
            ConstraintsViewModel.ApplyModel(obj);
            InitialVariableViewModel.ApplyModel(obj);
            SolutionViewModel.ApplyModel(obj);
        }

        private void OnHistoryChanged(List<ProblemModel> obj)
        {
            SaveToJson(obj);
        }

        private void OnConstraintsViewModelValidChanged(bool obj)
        {
            FunctionInputViewModel.CanSolve = obj;
        }

        private void OnNumberOfVariablesChanged(int obj)
        {
            ConstraintsViewModel.NumberOfVariables = obj;
            InitialVariableViewModel.NumberOfVariables = obj;
        }

        private void OnSolveClicked()
        {
            var funcModel = FunctionInputViewModel.GetModel();
            var constraints = ConstraintsViewModel.GetModel();
            var initial = InitialVariableViewModel.GetModel();
            var solver = new PenaltyMethodSolver(funcModel, constraints, initial);
            var solutionModel = solver.Solve();
            SolutionViewModel.ApplyModel(solutionModel);

            var problemModel = CreateProblemModel(funcModel, constraints, initial, solutionModel, ConstraintsViewModel.GreaterThanZero);
            HistoryViewModel.Add(problemModel);
        }

        private void LoadHistory(object param)
        {
            if (!File.Exists(historyFileName))
            {
                using (var stream = File.Create(historyFileName))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("[]");
                    }
                }
            }
            var json = File.ReadAllText(historyFileName);
            HistoryViewModel.ApplyModel(serializer.Deserialize<List<ProblemModel>>(json));
        }

        private ProblemModel CreateProblemModel(FunctionInputModel funcModel, ConstraintItemModel[] constraintModels, double[] initials, SolutionModel solutionModel, bool greaterThanZeroUsed)
        {
            return new ProblemModel
            {
                functionInputModel = new SerializableFunctionInputModel()
                {
                    expression = funcModel.expression.getExpressionString(),
                    numberOfVariables = funcModel.arguments.Length,
                    extremumType = funcModel.extremumType,
                },
                constraintModels = constraintModels.Select(item => new SerializableConstraintItemModel()
                {
                    expression = item.expression.getExpressionString(),
                    leftCoef = item.leftCoef,
                    numberOfVariables = item.arguments.Length
                }).ToArray(),
                initials = initials,
                solutionModel = solutionModel,
                timestamp = DateTime.Now,
                greaterThanZeroUsed = greaterThanZeroUsed
            };
        }
        private void SaveToJson(List<ProblemModel> history)
        {
            var json = serializer.Serialize(history);
            File.WriteAllText(historyFileName, json);
        } 
    }
}
