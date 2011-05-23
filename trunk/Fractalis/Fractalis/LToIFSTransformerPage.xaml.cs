using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fractalis.LGrammaire;
using Microsoft.Win32;

namespace Fractalis
{
    /// <summary>
    /// Interaction logic for LToIFSTransformerPage.xaml
    /// </summary>
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
                currentLibrary.LoadLibrary("..\\..\\..\\Library\\samples.flb");
                ToolsGrid.DataContext = currentLibrary;
            }
            catch (Exception ex)
            {
                Microsoft.Windows.Controls.MessageBox.Show(ex.Message);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void ButtonTransform_Click(object sender, RoutedEventArgs e)
        {
            int r;
            var ifsFunctions = Transformator.Transformator.LToIFSTransform(
                currentLibrary.Fractals[FractalSelector.SelectedIndex], out r);
            RValue.Text = @"Отношение F на соседних уровнях: " + r;

            var sb = new StringBuilder();
            foreach (var map in ifsFunctions)
            {
                sb.AppendLine(String.Format("{0:0.###};{1:0.###};{2:0.###};{3:0.###};{4:0.###};{5:0.###}",
                    map.a, map.b, map.c, map.d, map.e, map.f));
            }
            IFSRules.Text = sb.ToString();
        }

        #endregion


    }
}
