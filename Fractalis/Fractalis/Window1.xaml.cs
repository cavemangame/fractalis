using System;

namespace Fractalis
{
    public partial class Window1
    {
        #region Constructor

        public Window1()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        private void Lgrammare_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FramePages.Navigate(new Uri(@"LGrammarePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void IFS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FramePages.Navigate(new Uri(@"AffineFractalsPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
