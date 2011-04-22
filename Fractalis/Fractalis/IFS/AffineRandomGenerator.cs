using System;
using System.Collections.Generic;
using System.Linq;

namespace Fractalis.IFS
{
    /// <summary>
    /// Генерирует рандомное число из заданных, с весами
    /// </summary>
    public class AffineRandomGenerator
    {
        private List<double> probabilities;
        private Random rnd;
        private double norm;

        public AffineRandomGenerator(IEnumerable<AffineMap> maps)
        {
            probabilities = new List<double>();
            rnd = new Random();
            ComputeProbabilities(maps);
        }

        private void ComputeProbabilities(IEnumerable<AffineMap> maps)
        {
            if (maps.Count() == 0)
            {
                return;
            }

            norm = maps.Sum(map => map.Det);
            foreach (var map in maps)
            {
                probabilities.Add(map.Det / norm);
            }
        }

        public double GetNewRandom()
        {
            double seed = rnd.NextDouble() * norm;
            double curPos = 0;
            for (int i=0; i < probabilities.Count; i++)
            {
                curPos += probabilities[i];
                if (curPos > seed)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
