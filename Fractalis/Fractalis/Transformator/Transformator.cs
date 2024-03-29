﻿using System;
using System.Collections.Generic;
using Fractalis.Base;
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

        public static List<AffineMap> LToIFSTransform(LFractalisInfo lFractal, out double xr, out double yr)
        {
            string fWord = CorrectAxiom(lFractal);
            BoundingRectangle br;
            var bigIFSList = ComputeIFS(fWord, lFractal.Angle, out xr, out yr, out br);
            var smallIFSList = new List<AffineMap>();

            var maxr = Math.Max(xr, yr);
            // тут сжимаем/растягиваем по осям и распологаем получившийся фрактал так,
            // чтобы он распологался как фракталы из модуля IFS (сверху вниз и слева направо)
            var brMatrix = new Matrix(2, new[,] { { br.X0 / xr, br.X1 / xr }, { br.Y0 / yr, br.Y1 / yr } });
            var eMatrix = new Matrix(2, new double[,] { { 1, 0 }, { 0, 1 } });
            foreach (var map in bigIFSList)
            {
                map.M = map.M.Mul(1 / maxr);
                map.e /= maxr;
                map.f /= maxr;
                smallIFSList.Add(CoordinateTranslator.Translate(map, brMatrix, eMatrix));
            }
            return smallIFSList;
        }

        /// <summary>
        /// Считаем IFS и отношение r
        /// </summary>
        /// <param name="word">F правило</param>
        /// <param name="angle">угол поворота</param>
        /// <param name="r">параметр, показывающий отношение длин F на соседних шагах</param>
        /// <param name="boundingRectangle"></param>
        /// <returns></returns>
        private static List<AffineMap> ComputeIFS(string word, double angle, out double xr, out double yr, out BoundingRectangle boundingRectangle)
        {
            var maps = new List<AffineMap>();
            double x0 = 0, y0 = 0, x1 = 0, y1 = 0;
            double xMax = 0, xMin = 0, yMax = 0, yMin = 0;
            Double curAngle = 0;
            var branches = new Stack<TurtleStep>();

            // сканим слово, записывая все образы отрезка [0, 1] (ветка F) и вычисляя границы прогона
            for (int i = 0; i < word.Length; i++)
            {
                switch (word[i])
                {
                    case '+':
                        curAngle += angle;
                        break;
                    case '-':
                        curAngle -= angle;
                        break;
                    case 'F':
                        x1 = x0 + Math.Cos(GetRadian(curAngle));
                        y1 = y0 + Math.Sin(GetRadian(curAngle));
                        maps.Add(GetMapByLine(x0, y0, curAngle));
                        x0 = x1;
                        y0 = y1;
                        break;
                    case 'b':
                        x0 = x0 + Math.Cos(GetRadian(curAngle));
                        y0 = y0 + Math.Sin(GetRadian(curAngle));
                        break;
                    case '[':
                        branches.Push(new TurtleStep { X0 = x0, Y0 = y0, Angle = curAngle });
                        break;
                    case ']':
                        if (branches.Count > 0)
                        {
                            TurtleStep old = branches.Pop();
                            x0 = old.X0;
                            y0 = old.Y0;
                            curAngle = old.Angle;
                        }
                        break;
                    default:
                        // skip Nth step of add rules (minimal errors)
                        break;
                }
                if (xMin > x0)
                    xMin = x0;
                if (yMin > y0)
                    yMin = y0;
                if (xMax < x0)
                    xMax = x0;
                if (yMax < y0)
                    yMax = y0;
            }

            xr = xMax - xMin; // транслируется отрезок (0,1) в [xMin, xMax]
            yr = yMax - yMin; // транслируется отрезок (0,1) в [yMin, yMax]
            boundingRectangle = new BoundingRectangle { X0 = xMin, X1 = xMax, Y0 = yMin, Y1 = yMax };
            return maps;
        }

        private static double GetRadian(double grad)
        {
            return grad*Math.PI/180;
        }

        /// <summary>
        /// Генерируем аффинное отображение, которое переводит отрезок [(0,0),(1,0)] в отрезок [(x0, y0), (x1, y1)]
        /// angle для облегчения подсчетов
        /// </summary>
        private static AffineMap GetMapByLine(double x0, double y0, double angle)
        {
            // M = матрица поворота на angle, v - сдвиг (х0, у0)
            return new AffineMap
                          {
                              a = Math.Cos(GetRadian(angle)),
                              b = (-Math.Sin(GetRadian(angle))),
                              c = Math.Sin(GetRadian(angle)),
                              d = Math.Cos(GetRadian(angle)),
                              e = x0,
                              f = y0
                          };
        }

        private static string CorrectAxiom(LFractalisInfo lFractal)
        {
            // конвертим l описание с тем, чтобы в аксиоме было просто F
            // TODO: b тоже надо бы
            string fRule = lFractal.FRule;
            return lFractal.Axiom.Replace("F", fRule);
        }

        #endregion
    }
}
