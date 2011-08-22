using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fractalis.Base;
using Fractalis.IFS;
using Microsoft.Win32;

namespace Fractalis.Pages
{
    /// <summary>
    /// Interaction logic for AffineFractalsPage.xaml
    /// </summary>
    public partial class AffineFractalsPage
    {
        private readonly FractalLibrary currentLibrary = new FractalLibrary();

        /// <summary>
        /// Цвет фрактала. Задается в ColorPicker.
        /// </summary>
        public Color FractalColor
        {
            get { return (Color)GetValue(FractalColorProperty); }
            set { SetValue(FractalColorProperty, value); }
        }
        public static readonly DependencyProperty FractalColorProperty = DependencyProperty.Register(
            "FractalColor", typeof(Color), typeof(AffineFractalsPage),
            new FrameworkPropertyMetadata(Colors.Brown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                          SelectedColorPropertyChanged));

        public AffineFractalsPage()
        {
            InitializeComponent();
            DataContext = this;
            FractalColorPicker.DataContext = this;
            FractalColor = Colors.Black;
        }

        #region Generate fractal

        private void ButtonFractal_Click(object sender, RoutedEventArgs e)
        {
            string error = TryGenerateFractal();
            if (!String.IsNullOrEmpty(error))
            {
                MessageBox.Show(error);
            }
        }

        public string TryGenerateFractal()
        {
            if (String.IsNullOrEmpty(Functions.Text))
            {
                return "Не указаны необходимые параметры";
            }

            int depth = String.IsNullOrEmpty(Depth.Text) ? 3 : Convert.ToInt32(Depth.Text);
            double size = Math.Min(FractalisImagePanel.ActualWidth, FractalisImagePanel.ActualHeight);

            IFS.IFS ifsAlgo = null;
            if (DifsAlgo.IsChecked.Value)
            {
                ifsAlgo = new DIFS(DifsIsLine.IsChecked.Value);
            }
            else
            {
                ifsAlgo = new RIFS();
            }
            ifsAlgo.ParseAndTranslateFunctions(Functions.Text, size);
            FractalisImage.Source = ifsAlgo.GetAttractor((int)size, depth, FractalColor);

            return null;
        }

        #endregion

        #region events

        private void LibraryLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog
                              {
                                  DefaultExt = "*.ilb|ilb",
                                  Filter = "IFS lib(*.ilb)|*.ilb",
                                  FilterIndex = 1,
                                  RestoreDirectory = true,
                                  Title = "Загрузить библиотеку IFS фракталов"
                              };

                bool? dr = ofd.ShowDialog();
                if (dr.Value)
                {
                    currentLibrary.LoadLibrary(ofd.FileName, typeof(IFSFractalisInfo));
                    ToolsGrid.DataContext = currentLibrary;
                    FractalSelector.SelectedIndex = 0;
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
                currentLibrary.LoadLibrary(@".\Library\IFsamples.ilb", typeof(IFSFractalisInfo));
                ToolsGrid.DataContext = currentLibrary;
            }
            catch (Exception ex)
            {
                Microsoft.Windows.Controls.MessageBox.Show(ex.Message);
            }
        }

        private static void SelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = (AffineFractalsPage)d;
            page.TryGenerateFractal();
        }

        private void FractalSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveImageHelper.SaveImage((BitmapSource)FractalisImage.Source);
        }

        #endregion
    }
}