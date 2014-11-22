using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] ints = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            Solver solver = new Solver(ints, 20, 10);
            Console.WriteLine(solver.GetMin());
            Console.ReadKey();
        }
    }
}
