using System;

namespace Fractalis
{
    public partial class MainWindow
    {
        private int currentAlgorithm = 0;

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Обработчики кнопок меню

        private void Lgrammare_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentAlgorithm != 0)
            {
                NavigateSomewhere(@"Pages\LGrammarePage.xaml");
                currentAlgorithm = 0;
            }
        }

        private void IFS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentAlgorithm != 1)
            {
                NavigateSomewhere(@"Pages\AffineFractalsPage.xaml");
                currentAlgorithm = 1;
            }
        }

        private void Transform_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentAlgorithm != 2)
            {
                NavigateSomewhere(@"Pages\LToIFSTransformerPage.xaml");
                currentAlgorithm = 2;
            }
        }

        private void itemHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void itemAbout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = new WindowAbout();
            wnd.ShowDialog();
        }

        #endregion

        #region Методы для навигации

        private void NavigateSomewhere(string uriPath)
        {
            FramePages.Navigate(new Uri(uriPath, UriKind.RelativeOrAbsolute));
        }

        #endregion

    }
}
