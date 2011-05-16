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
            if (currentAlgorithm == 1)
            {
                NavigateBack(@"LGrammarePage.xaml");
                currentAlgorithm = 0;
            }
        }

        private void IFS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentAlgorithm == 0)
            {
                NavigateForward(@"AffineFractalsPage.xaml");
                currentAlgorithm = 1;
            }
        }

        #region Методы для навигации

        private void NavigateForward(string uriPath)
        {
            if (FramePages.CanGoForward)
            {
                FramePages.GoForward();
            }
            else
            {
                FramePages.Navigate(new Uri(uriPath, UriKind.RelativeOrAbsolute));
            }
        }

        private void NavigateBack(string uriPath)
        {
            if (FramePages.CanGoBack)
            {
                FramePages.GoBack();
            }
            else
            {
                FramePages.Navigate(new Uri(uriPath, UriKind.RelativeOrAbsolute));
            }
        }

        #endregion
    }
}
