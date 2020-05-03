using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Solver.Utils
{
    public static class Derevation
    {
        public static Func<double[], double[]> Gradient(Func<double[], double> f)
        {
            double eps = 0.00000001;
            return (x) =>
            {
                List<double> list = new List<double>();
                for (int i = 0; i < x.Count(); i++)
                {
                    var xh = x.Select(y => y).ToArray();
                    xh[i] = xh[i] + eps;
                    list.Add((f(xh) - f(x)) / eps);
                }
                return list.ToArray();
            };
        }
    }
}
