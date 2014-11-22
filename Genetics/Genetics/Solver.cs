using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Genetics
{
    class Solver
    {
        private int[] values;
        private List<bool[]> generation;
        private Random ran;
        private int goodValue;
        private int len;
        private int iterations;

        public Solver(int[] ints, int desiredValue, int iter)
        {
            iterations = iter;
            goodValue = desiredValue;
            values = ints;
            len = ints.Length;
            ran = new Random();
            generation = Enumerable.Range(0, 50).Select(x => new bool[len].Select(e => ran.Next()%2 == 1).ToArray()).ToList();
        }

        private void Generate()
        {
            
        }

        private bool[] Mutate(bool[] parent)
        {
            bool[] res = (bool[])parent.Clone();
            for (int i = 0; i < ran.Next(4); i++)
            {
                var index = ran.Next(parent.Length);
                res[index] = !parent[index];
            }
            return res;
        }

        private List<bool[]> Merge(bool[] a, bool[] b)
        {
            var child1 = new bool[a.Length];
            var child2 = new bool[a.Length];
            var replaceIdx = ran.Next(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                child1[i] = a[i] ^ b[i];
                child2[i] = i > replaceIdx ? b[i] : a[i];
            }
            return new List<bool[]> { child1, child2 };
        }

        public int GetMin()
        {
            for (int i = 0; i < iterations; i++)
            {
                MakeLove();
                GenerateMutants();
                Evolution();
            }
            return generation.Select(GetMinVal).Min();
        }

        public void Evolution()
        {
            generation = generation.Where(IsGoodEnough).ToList();
        }

        public bool IsGoodEnough(bool[] bools)
        {
            int heap1 = 0;
            int heap2 = 0;
            for (int i = 0; i < bools.Length; i++)
            {
                if (bools[i]) heap1 += values[i];
                else heap2 += values[i];
            }
            return Math.Abs(heap1 - heap2) < goodValue;
        }

        public int GetMinVal(bool[] bools)
        {
            int heap1 = 0;
            int heap2 = 0;
            for (int i = 0; i < bools.Length; i++)
            {
                if (bools[i]) heap1 += values[i];
                else heap2 += values[i];
            }
            
            return Math.Abs(heap1 - heap2);
        }

        private void GenerateMutants()
        {
            var chosenOnes = Enumerable.Range(0, (int) generation.Count/10).Select(x => ran.Next(generation.Count));
            foreach (var chosenOne in chosenOnes)
            {
                generation.Add(Mutate(generation[chosenOne]));
            }
        }

        private void MakeLove()
        {
            var lovers =
                Enumerable.Range(0, generation.Count/4)
                    .Select(x => Tuple.Create(ran.Next(generation.Count), ran.Next(generation.Count)));
            foreach (var pair in lovers)
            {
                generation.AddRange(Merge(generation[pair.Item1], generation[pair.Item2]));
            }

        }
    }
}
