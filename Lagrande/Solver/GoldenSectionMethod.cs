using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Solver
{
    public class GoldenSectionMethod
    {
        private double eps;
        private double alpha1, alpha2;
        public double Eps
        {
            get { return eps; }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentException();
                eps = value;
            }
        }
        public GoldenSectionMethod(double eps = 0.000000000000001)
        {
            Eps = eps;
            alpha1 = (3d - Math.Sqrt(5)) / 2d;//0.382
            alpha2 = (Math.Sqrt(5) - 1) / 2d;//0.618
        }

        public double FindExtremum(Func<double, double> func, double from, double to)
        {
            double a = from, b = to;
            double x1, x2;

            x1 = a + alpha1 * (b - a);
            x2 = a + alpha2 * (b - a);

            double f1 = func(x1), f2 = func(x2);

            double k = 0;
            while (true)
            {
                k++;
                if (f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    x1 = a + alpha1 * (b - a);
                    f2 = f1;
                    f1 = func(x1);
                }
                else if (f1 > f2)
                {
                    a = x1;
                    x1 = x2;
                    x2 = a + alpha2 * (b - a);
                    f1 = f2;
                    f2 = func(x2);
                }
                else if (f1 == f2)
                {
                    return x1;//f1==f2==min
                }
                if (Math.Abs(b - a) < Eps)
                {
                    double res = (b + a) / 2;
                    return res;
                }
            }
        }
    }
}
