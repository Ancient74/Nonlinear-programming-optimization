using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagrande.Solver.Utils
{
    public static class ArrayUtils
    {
        public static double[] Subtract(this double[] arr, double[] other) 
        {
            var newArr = new double[arr.Length];
            for(int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i] - other[i];
            }
            return newArr;
        }
    }
}
