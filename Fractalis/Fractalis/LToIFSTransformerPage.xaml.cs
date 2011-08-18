using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Fractalis.LGrammaire;
using Microsoft.Win32;
using System.IO;
using Fractalis.IFS;

namespace Fractalis
{
    public partial class LToIFSTransformerPage : Page
    {
        private readonly FractalisLibrary currentLibrary = new FractalisLibrary();
        
        public LToIFSTransformerPage()
        {
            InitializeComponent();
        }

        #region events

        private void LibraryLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    DefaultExt = "*.flb|flb",
                    Filter = "fractalis lib(*.flb)|*.flb",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    Title = "Загрузить библиотеку"
                };

                bool? dr = ofd.ShowDialog();
                if (dr.Value)
                {
                    currentLibrary.LoadLibrary(ofd.FileName);
                    ToolsGrid.DataContext = currentLibrary;
                }
            }
            catch (Exception ex)
            {
                Microsoft.Windows.Controls.MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                currentLibrary.LoadLibrary("..\\..\\Library\\samples.flb");
                ToolsGrid.DataContext = currentLibrary;
            }
            catch (Exception ex)
            {
                Microsoft.Windows.Controls.MessageBox.Show(ex.Message);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new SaveFileDialog
                {
                    DefaultExt = "*.ilb|ilb",
                    Filter = "IFS lib(*.ilb)|*.ilb",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    Title = "Сохранить в библиотеку IFS фракталов"
                };

                bool? dr = ofd.ShowDialog();
                if (dr.Value)
                {
                    if (!File.Exists(ofd.FileName))
                    {
                        SaveIFSToNewLibrary(ofd);
                    }
                    else
                    {
                        SaveIFSToOldLibrary(ofd);
                    }
                }
            }
            catch (Exception ex)
            {
                Microsoft.Windows.Controls.MessageBox.Show(ex.Message);
            }
        }

        private void ButtonTransform_Click(object sender, RoutedEventArgs e)
        {
            double r;
            BoundingRectangle fr;
            var ifsFunctions = Transformator.Transformator.LToIFSTransform(
                currentLibrary.Fractals[FractalSelector.SelectedIndex], out r);
            RValue.Text = @"Отношение F на соседних уровнях: " + r;
            var sb = new StringBuilder();
            foreach (var map in ifsFunctions)
            {
                sb.Append(String.Format("{0:0.###};{1:0.###};{2:0.###};{3:0.###};{4:0.###};{5:0.###}\n",
                    map.a, map.b, map.c, map.d, map.e, map.f));
            }
            IFSRules.Text = sb.ToString();
        }

        /// <summary>
        /// Создать новую библиотеку и добавить туда фрактал
        /// </summary>
        private void SaveIFSToNewLibrary(SaveFileDialog ofd)
        {
            // новая библиотека
            IFSLibrary newLibrary = new IFSLibrary();
            newLibrary.Author = "Fractalis toolbox";
            newLibrary.Name = ofd.SafeFileName;

            // добавляем вычисленный фрактал в виде набора IFS
            IFSInfo fractal = new IFSInfo();
            fractal.Name = FractalSelector.Text;
            fractal.Depth = Convert.ToInt32(Depth.Value);
            fractal.Rules = IFSRules.Text;
            newLibrary.Fractals = new List<IFSInfo>() {fractal};
            newLibrary.SaveLibrary(ofd.FileName);
        }

        /// <summary>
        /// Сохранить фрактал в существующую библиотеку
        /// Если уже есть с таким именем - то он будет перезаписан
        /// </summary>
        /// <param name="ofd"></param>
        private void SaveIFSToOldLibrary(SaveFileDialog ofd)
        {
            IFSLibrary oldLibrary = new IFSLibrary();
            oldLibrary.LoadLibrary(ofd.FileName);
            IFSInfo oldFractal = oldLibrary.ContainsFractal(FractalSelector.Text);
            if (oldFractal == null)
            {
                IFSInfo fractal = new IFSInfo();
                fractal.Name = FractalSelector.Text;
                fractal.Depth = Convert.ToInt32(Depth.Value);
                fractal.Rules = IFSRules.Text;
                oldLibrary.Fractals.Add(fractal);
            }
            else
            {
                oldFractal.Depth = Convert.ToInt32(Depth.Value);
                oldFractal.Rules = IFSRules.Text;
            }
            oldLibrary.SaveLibrary(ofd.FileName);
        }


        #endregion


    }
}
