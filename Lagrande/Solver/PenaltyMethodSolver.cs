using Lagrande.Models;
using Lagrande.Solver.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Solver
{
    public class PenaltyMethodSolver
    {
        private FunctionInputModel functionInputModel;
        private ConstraintItemModel[] constraintModels;
        private double[] initialX;

        private struct FunctionAndConstraints
        {
            public Func<double[], double> func;
            public Func<double[], double, double> penalty;
        }


        public PenaltyMethodSolver(FunctionInputModel functionInputModel, ConstraintItemModel[] constraintModels, double[] initialX)
        {
            this.functionInputModel = functionInputModel;
            this.constraintModels = constraintModels;
            this.initialX = initialX;
        }

        public SolutionModel Solve()
        {
            var functions = GetFunctionAndConstraints();
            var target = GetTargetFunction(functions);
            var currentX = initialX.Select(x => x).ToArray();
            var prevX = initialX.Select(x => x).ToArray();
            double r = 1;
            double betta = 2;
            while(functions.penalty(currentX, r) >= 0.00001 && r <= 10000000000000)
            {
                Func<double[], double> currnet = (x) =>
                {
                    return target(x, r);
                };
                var gradient = Derevation.Gradient(currnet);
                var currentGrad = gradient(currentX);

                Func<double, double> lambdaFunc = (l) =>
                {
                    var arg = currentX.Subtract(currentGrad.Select(g => g * l).ToArray());
                    return currnet(arg);
                };
                prevX = currentX;
                double lambda = Minimize(lambdaFunc, 0, 10);
                currentX = currentX.Subtract(currentGrad.Select(g => g * lambda).ToArray());
                r *= betta;
            }


            List<SolutionModelItem> result = new List<SolutionModelItem>();
            for (int i = 0; i < currentX.Length; i++)
            {
                result.Add(new SolutionModelItem()
                {
                    value = currentX[i],
                    variableName = "x" + (i + 1).ToString()
                });
                functionInputModel.arguments[i].setArgumentValue(currentX[i]);
            }
            return new SolutionModel() { result = functionInputModel.expression.calculate(), variables = result.ToArray() };
        }

        private double Minimize(Func<double, double> func, double from, double to)
        {
            var gsm = new GoldenSectionMethod();
            return gsm.FindExtremum(func, from, to);
        }

        private Func<double[], double, double> GetTargetFunction(FunctionAndConstraints functions)
        {
            return (x, r) =>
            {
                return functions.func(x) + functions.penalty(x, r);
            };
        }

        private FunctionAndConstraints GetFunctionAndConstraints()
        {
            Func<double[], double> func = (x) =>
            {
                for (int i = 0; i < x.Length; i++)
                {
                    functionInputModel.arguments[i].setArgumentValue(x[i]);
                }
                return functionInputModel.expression.calculate();
            };
            Func<double[], double> extremumFunc = func;
            if (functionInputModel.extremumType == ExtremumType.Max)
            {
                extremumFunc = (x) => -func(x);
            }
            var constraints = constraintModels.Select(item =>
            {
                Func<double[], double> f = (double[] x) =>
                {
                    for (int i = 0; i < x.Length; i++)
                    {
                        item.arguments[i].setArgumentValue(x[i]);
                    }
                    return item.expression.calculate() - item.leftCoef;
                };
                return (constr: item, func: f);
            }).ToArray();

            Func<double[], double>[] penalties = constraints.Select(item => GetPenaltyFunction(item.func, item.constr)).ToArray();

            return new FunctionAndConstraints
            {
                func = extremumFunc,
                penalty = (x, r) => {
                    return r * penalties.Sum(item => item(x));
                }
            };
        }

        private Func<double[], double> GetPenaltyFunction(Func<double[], double> inner, ConstraintItemModel model)
        {
            if (model.constrainType == ConstrainType.Equal)
            {
                return (x) =>
                {
                    var res = inner(x);
                    return res * res;
                };
            }
            else
            {
                return (x) =>
                {
                    var res = inner(x);
                    return res > 0 ? 0 : res * res;
                };
            }
        }
    }
}
