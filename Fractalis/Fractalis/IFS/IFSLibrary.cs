using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.ComponentModel;

namespace Fractalis.IFS
{
    /// <summary>
    /// Библиотека фракталов, записанных в виде аффинных преобразований
    /// </summary>
    public class IFSLibrary : INotifyPropertyChanged
    {
        #region Vars

        private string name;

        private string author;

        private List<IFSInfo> fractals;

        #endregion


        #region Properties

        public string Name
        {
            get { return name;}
            set
            {
                if (value != name)
                {
                    this.name = value;
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
                    this.author = value;
                    OnPropertyChanged("Author");
                }
            }
        }

        public List<IFSInfo> Fractals
        {
            get { return fractals; }
            set
            {

                this.fractals = value;
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
                    sw.WriteLine(info.Name);
                    sw.WriteLine(info.Depth);
                    sw.WriteLine(info.Rules);
                }
                sw.Flush();
                sw.Close();
            }
        }

        public void LoadLibrary(string filePath)
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
                    Fractals = new List<IFSInfo>();
                    while (!sr.EndOfStream)
                    {
                        var info = new IFSInfo()
                                       {
                                           Name = sr.ReadLine(),
                                           Depth = Convert.ToInt32(sr.ReadLine()),
                                       };

                        var sb = new StringBuilder();
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            if (String.IsNullOrEmpty(s))
                                break;
                            sb.Append(s + Environment.NewLine);
                        }
                        info.Rules = sb.ToString();
                        Fractals.Add(info);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при загрузке библиотеки фракталов: " + e.Message);
            }
        }

        public IFSInfo ContainsFractal(string name)
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
