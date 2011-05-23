using System;

namespace Fractalis
{
    public partial class Window1
    {
        private int currentAlgorithm = 0;

        #region Constructor

        public Window1()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        private void Lgrammare_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentAlgorithm != 0)
            {
                NavigateSomewhere(@"LGrammarePage.xaml");
                currentAlgorithm = 0;
            }
        }

        private void IFS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentAlgorithm != 1)
            {
                NavigateSomewhere(@"AffineFractalsPage.xaml");
                currentAlgorithm = 1;
            }
        }

        private void Transform_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentAlgorithm != 2)
            {
                NavigateSomewhere(@"LToIFSTransformerPage.xaml");
                currentAlgorithm = 2;
            }
        }

        #region Методы для навигации

        private void NavigateSomewhere(string uriPath)
        {
            FramePages.Navigate(new Uri(uriPath, UriKind.RelativeOrAbsolute));
        }

        #endregion
    }
}
