using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fractalis.Modules
{
    public enum LLanguage
    {
        /// <summary>
        /// Движение с отрисовкой
        /// </summary>
        Fill,
        /// <summary>
        /// Движение без отрисовки
        /// </summary>
        Blank,
        /// <summary>
        /// Поворот влево
        /// </summary>
        Plus,
        /// <summary>
        /// Поворот вправо
        /// </summary>
        Minus,
        /// <summary>
        /// Начало ветвления
        /// </summary>
        OpenBrace,
        /// <summary>
        /// Окончание ветвления
        /// </summary>
        CloseBrace
    }
}
