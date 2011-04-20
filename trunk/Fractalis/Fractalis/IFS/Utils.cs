using System;

namespace Fractalis.IFS
{
    public static class Utils
    {
        /// <summary>
        /// Сравниваем даблы на равность с небольшой погрешностью (2 цифры после запятой должны совпадать)
        /// </summary>
        public static bool AlmostEqual(double a, double b)
        {
            return AlmostEqual(a, b, 3);
        }

        /// <summary>
        /// Сравниваем даблы на равность с небольшой погрешностью (digit цифр после запятой должны совпадать)
        /// </summary>       
        public static bool AlmostEqual(double a, double b, int digit)
        {
            double error = Math.Abs(a - b);
            double maxExpectedError = 1/Math.Pow(10, digit);

            return error < maxExpectedError;
        }
    }
}
