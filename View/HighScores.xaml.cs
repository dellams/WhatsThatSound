using DevExpress.UI.Xaml.Layout;
using WhatsThatSound.Data;

namespace WhatsThatSound.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HighScores : DXPage
    {
        public HighScores()
        {
            InitializeComponent();
            grid.Loaded += grid_Loaded;
        }

        void grid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            grid.ItemsSource = DataSource.GetHighScores();
        }
    }
}
