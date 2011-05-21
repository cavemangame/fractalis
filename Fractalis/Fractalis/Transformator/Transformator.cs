using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fractalis.IFS;
using Fractalis.LGrammaire;

namespace Fractalis.Transformator
{
    /// <summary>
    /// Трансформатор фракталов из L в IFS и обратно
    /// </summary>
    public static class Transformator
    {
        #region L to IFS

        public static List<AffineMap> LToIFSTransform(FractalisInfo lFractal)
        {
            string fWord = CorrectAxiom(lFractal);
            int r;
            var bigIFSList = ComputeIFS(fWord, lFractal.Angle, out r);
            foreach (var map in bigIFSList)
            {
                map.M = map.M.Mul(1/(double)r);
            }
            return bigIFSList;
        }

        /// <summary>
        /// Считаем IFS и отношение r
        /// </summary>
        /// <param name="word">F правило</param>
        /// <param name="angle">угол поворота</param>
        /// <param name="r">параметр, показывающий отношение длин F на соседних шагах</param>
        /// <returns></returns>
        private static List<AffineMap> ComputeIFS(string word, double angle, out int r)
        {
            throw new NotImplementedException();
        }

        private static string CorrectAxiom(FractalisInfo lFractal)
        {
            // конвертим l описание с тем, чтобы в аксиоме было просто F
            // TODO: b тоже надо бы
            string fRule = lFractal.FRule;
            return lFractal.Axiom.Replace("F", fRule);
        }

        #endregion
    }
}
