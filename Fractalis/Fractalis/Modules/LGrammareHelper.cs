using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fractalis.Modules
{
    /// <summary>
    /// Методы для работы с L языком
    /// </summary>
    public static class LGrammareHelper
    {
        private static char[] lchars = new[] {'F', 'b', '+', '-', '[', ']'};

        /// <summary>
        /// Конвертит строковое представление слова L языка в массив токенов
        /// </summary>
        public static List<LLanguage> ConvertInput(string input)
        {
            var output = new List<LLanguage>();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                switch (c)
                {
                    case 'F':
                        output.Add(LLanguage.Fill);
                        break;
                    case 'b':
                        output.Add(LLanguage.Blank);
                        break;
                    case '+':
                        output.Add(LLanguage.Plus);
                        break;
                    case '-':
                        output.Add(LLanguage.Minus);
                        break;
                    case '[':
                        output.Add(LLanguage.OpenBrace);
                        break;
                    case ']':
                        output.Add(LLanguage.CloseBrace);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Неверный вход. Попался символ '{0}'", c));
                }
            }
            return output;
        }

        /// <summary>
        /// Трассируем вход по правилам level уровней
        /// </summary>
        /// <param name="axiom">аксиома - слово инициализации</param>
        /// <param name="fillRule">правило F</param>
        /// <param name="blankRule">правило b</param>
        /// <param name="addRules">доп правила вида 'X' -> 'FF+X'</param>
        /// <param name="level">глубина разбора</param>
        /// <returns></returns>
        public static string TraceInputRules(string axiom, string fillRule, string blankRule, Dictionary<char, string> addRules, 
            int level)
        {
            string W = axiom;
            while (level > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < W.Length; i++)
                {
                    switch (W[i])
                    {
                        case '+':
                            sb.Append('+');
                            break;
                        case '-':
                            sb.Append('-');
                            break;
                        case '[':
                            sb.Append('[');
                            break;
                        case ']':
                            sb.Append(']');
                            break;
                        case 'F':
                            sb.Append(fillRule);
                            break;
                        case 'b':
                            sb.Append(blankRule);
                            break;
                        default:
                            if (addRules != null && addRules.ContainsKey(W[i]))
                            {
                                sb.Append(addRules[W[i]]);
                                break;
                            }
                            throw new ArgumentException("Неверный вход");
                    }
                }
                level--;
                W = sb.ToString();
            }
            return W;
        }

        /// <summary>
        /// Рисуем фрактал, чтоб наверняка весь влез в BoundingRectangle
        /// </summary>
        /// <param name="word">Слово фрактала</param>
        /// <param name="angle">Угол поворота</param>
        /// <param name="beginAngle">Начальный угол</param>
        /// <param name="rect">BoundingRectangle</param>
        /// <param name="brush">Кисть заливки</param>
        /// <returns>DrawingVisual c фракталом</returns>
        public static DrawingVisual DrawLFractalis(string word, double angle, double beginAngle, BoundingRectangle rect, Brush brush)
        {
            BoundingRectangle oneRect = ComputeFractalisRectangle(word, angle, beginAngle);
            var fractalDiameter = Math.Max(oneRect.X1 - oneRect.X0, oneRect.Y1 - oneRect.Y0);
            var boundDiameter = Math.Min(rect.X1 - rect.X0, rect.Y1 - rect.Y0);
            return DrawLFractalis(word, angle, beginAngle, rect, boundDiameter / fractalDiameter, brush);
        }

        public static DrawingVisual DrawLFractalis(string word, double angle, double beginAngle, BoundingRectangle rect, 
            double zoom, Brush brush)
        {
            double x0 = 0, y0 = 0, x1 = 0, y1 = 0;
            double xMax = 0, xMin = Double.MaxValue, yMax = 0, yMin = Double.MaxValue;
            var branches = new Stack<TurtleStep>();
            var primitiveBorderPen = new Pen(brush, 1.0);
            double curAngle = beginAngle;
            var drawingVisual = new DrawingVisual();
            DrawingContext dc = drawingVisual.RenderOpen();
            var geo = new StreamGeometry();

            using (var gc = geo.Open())
            {
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
                            x1 = x0 + Math.Cos(curAngle) * zoom;
                            y1 = y0 + Math.Sin(curAngle) * zoom;
                            gc.BeginFigure(new Point(x0, y0), false, false);
                            gc.LineTo(new Point(x1, y1), true, false);
                            x0 = x1;
                            y0 = y1;
                            break;
                        case 'b':
                            x0 = x0 + Math.Cos(curAngle) * zoom;
                            y0 = y0 + Math.Sin(curAngle) * zoom;
                            break;
                        case '[':
                            branches.Push(new TurtleStep {X0 = x0, Y0 = y0, Angle = curAngle});
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
            }
            dc.DrawGeometry(brush, primitiveBorderPen, geo);
            dc.Close();

            double xshift = (rect.X1 - rect.X0) - (xMax - xMin);
            xshift = xshift <= 0 ? 0 : xshift/2;
            double yshift = (rect.Y1 - rect.Y0) - (yMax - yMin);
            yshift = yshift <= 0 ? 0 : yshift / 2;

            drawingVisual.Transform = new TranslateTransform(rect.X0-xMin+xshift, rect.Y0-yMin+yshift);

            return drawingVisual;
        }

        public static BoundingRectangle ComputeFractalisRectangle(string word, double angle, double beginAngle)
        {
            double x0 = 0, y0 = 0, x1 = 0, y1 = 0;
            var branches = new Stack<TurtleStep>();
            double xMax = 0, xMin = Double.MaxValue, yMax = 0, yMin = Double.MaxValue;
            double curAngle = beginAngle;
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
                        x1 = x0 + Math.Cos(curAngle);
                        y1 = y0 + Math.Sin(curAngle);
                        x0 = x1;
                        y0 = y1;
                        break;
                    case 'b':
                        x0 = x0 + Math.Cos(curAngle);
                        y0 = y0 + Math.Sin(curAngle);
                        break;
                    case '[':
                        branches.Push(new TurtleStep {X0 = x0, Y0 = y0, Angle = curAngle});
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

            return new BoundingRectangle { X0 = xMin, X1 = xMax, Y0 = yMin, Y1 = yMax };
        }

        public static bool LTextAllowed(String text)
        {
            return Array.TrueForAll(text.ToCharArray(), c => lchars.Contains(c));
        }

        /// <summary>
        /// Парсит строку вида "X=FX; Y=FX+Y", возвращая словарь доп правил
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Dictionary<char, string> ParseAddRules(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            string[] rules = text.Split(';');
            if (rules.Count() == 0)
                return null;

            var addRules = new Dictionary<char, string>();
            foreach (var rule in rules)
            {
                string[] parts = rule.Split('=');
                if (parts.Count() == 2)
                {
                    addRules.Add(parts[0][0], parts[1]);
                }
            }

            return addRules;
        }
    }
}
