using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;

namespace Fractalis.Base
{
    /// <summary>
    /// Библиотека фракталов, записанных в виде аффинных преобразований
    /// </summary>
    public class FractalLibrary : INotifyPropertyChanged
    {
        #region Vars

        private string name;

        private string author;

        private List<FractalisInfo> fractals;

        #endregion


        #region Properties

        public string Name
        {
            get { return name;}
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Author
        {
            get { return author; }
            set
            {
                if (value != author)
                {
                    author = value;
                    OnPropertyChanged("Author");
                }
            }
        }

        public List<FractalisInfo> Fractals
        {
            get { return fractals; }
            set
            {

                fractals = value;
                OnPropertyChanged("Fractals");
            }
        }

        #endregion


        #region Save and Load

        public void SaveLibrary(string filePath)
        {
            using (var sw = new StreamWriter(filePath))
            {
                // инфа о самой библиотеке
                sw.WriteLine(Name);
                sw.WriteLine(Author);
                sw.WriteLine();

                // список фракталов
                foreach (var info in Fractals)
                {
                    info.SaveItself(sw);
                }
                sw.Flush();
                sw.Close();
            }
        }

        public void LoadLibrary(string filePath, Type fractalGenType)
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    // инфа о самой библиотеке
                    Name = sr.ReadLine();
                    Author = sr.ReadLine();

                    sr.ReadLine();

                    // список фракталов
                    Fractals = new List<FractalisInfo>();
                    while (!sr.EndOfStream)
                    {
                        FractalisInfo info = null;
                        if (fractalGenType == typeof(IFSFractalisInfo))
                            info = new IFSFractalisInfo();
                        else
                            info = new LFractalisInfo();
                        info.LoadItself(sr);
                        Fractals.Add(info);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при загрузке библиотеки фракталов: " + e.Message);
            }
        }

        public FractalisInfo ContainsFractal(string name)
        {
            foreach (var ifsInfo in Fractals)
            {
                if (name == ifsInfo.Name)
                {
                    return ifsInfo;
                }
            }
            return null;
        }

        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}