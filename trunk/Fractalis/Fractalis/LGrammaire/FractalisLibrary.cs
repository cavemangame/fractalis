using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;

namespace Fractalis.LGrammaire
{
    /// <summary>
    /// Описание библиотеки фракталов
    /// </summary>
    public class FractalisLibrary : INotifyPropertyChanged
    {
        #region Vars

        private string name;

        private string author;

        private List<FractalisInfo> fractals;

        #endregion


        #region Properties

        public string Name
        {
            get { return name; }
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

        public List<FractalisInfo> Fractals
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
                    sw.WriteLine(info.Axiom);
                    sw.WriteLine(info.FRule);
                    sw.WriteLine(info.BRule);
                    sw.WriteLine(info.AddRules);
                    sw.WriteLine(info.Angle);
                    sw.WriteLine(info.BeginAngle);
                    sw.WriteLine(info.Depth);
                    sw.WriteLine();
                }
                sw.Flush();
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
                    Fractals = new List<FractalisInfo>();
                    while (!sr.EndOfStream)
                    {
                        Fractals.Add(new FractalisInfo
                                         {
                                             Name = sr.ReadLine(),
                                             Axiom = sr.ReadLine(),
                                             FRule = sr.ReadLine(),
                                             BRule = sr.ReadLine(),
                                             AddRules = sr.ReadLine(),
                                             Angle = Convert.ToDouble(sr.ReadLine()),
                                             BeginAngle = Convert.ToDouble(sr.ReadLine()),
                                             Depth = Convert.ToInt32(sr.ReadLine())
                                         });
                        sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при загрузке библиотеки фракталов: " + e.Message);
            }
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