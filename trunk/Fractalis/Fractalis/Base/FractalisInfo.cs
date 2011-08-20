using System.IO;

namespace Fractalis.Base
{
    /// <summary>
    /// Базовые свойства фрактала
    /// </summary>
    public abstract class FractalisInfo
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сохранение данных фрактала в поток
        /// </summary>
        public abstract void SaveItself(StreamWriter sw);

        /// <summary>
        /// Загрузка данных фрактала из потока
        /// </summary>
        public abstract void LoadItself(StreamReader sr);
    }
}
