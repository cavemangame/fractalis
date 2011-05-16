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
        private readonly List<double> probabilities;
        private readonly Random rnd;
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

            norm = maps.Sum(map => Math.Abs(map.Det));
            Double min = Double.MaxValue;

            foreach (var map in maps)
            {
                var prob = Math.Abs(map.Det)/norm;
                if (min > prob && prob > 0)
                {
                    min = prob;
                }
                probabilities.Add(prob);
            }
            // у вырожденных матриц det == 0, но появиться в генераторе они должны - пожтому пусть мудут как минимальное
            for (int i = 0; i < probabilities.Count; i++)
            {
                if (probabilities[i] == 0)
                {
                    probabilities[i] = min;
                }
            }
            // не забываем пересчитать норму
            norm = probabilities.Sum();
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
