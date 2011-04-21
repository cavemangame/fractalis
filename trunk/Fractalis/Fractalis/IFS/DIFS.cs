using System.Collections.Generic;

namespace Fractalis.IFS
{
    /// <summary>
    /// Реализация детерминированного СИФ
    /// </summary>
    public class DIFS
    {
        public List<AffineMap> Axioms { get; set; }

        // TODO: generalize Matrix and Vector
        public Matrix GetAttractor(Matrix start, int screenSize, int depth)
        {
            Matrix S = new Matrix(start.Dim);
            Matrix T = start.Copy();

            for (int k = 0; k < depth; k++)
            {
                for (int i = 0; i < start.Dim; i++)
                {
                    for (int j = 0; j < start.Dim; j++)
                    {
                        if (T[i, j] == 1)
                        {
                            for (int l = 0; l < Axioms.Count; l++)
                            {
                                double ii = Axioms[l].a*i + Axioms[l].b*j + Axioms[l].e + 1;
                                if (1 <= ii && ii <= start.Dim)
                                {
                                    double jj = Axioms[l].c * i + Axioms[l].d * j + Axioms[l].f + 1;
                                    if (1 <= jj && jj <= start.Dim)
                                    {
                                        S[(int)ii, (int)jj] = 1;
                                    }
                                }
                            }
                        }
                    }

                }
                T = S.Copy();
            }
            return T;
        }

    }
}
