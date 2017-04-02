using System.Windows;
using System.Windows.Forms;

namespace ambi_gui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonPreview_OnClick(object sender, RoutedEventArgs e)
        {
            var pixels = DisplaysPanel.PixelCount;

            const int width = 60;
            const int height = 60;

            var delta = Util.UnscaledBounds.Width / pixels;
            var deltaLeft = Util.UnscaledBounds.Left + Util.UnscaledBounds.Width / 2 / pixels - width/2.0;

            for (var i = 0; i < pixels; i++)
            {
                var ambiWindow = new AmbiPixelWindow
                {
                    Width = width,
                    Height = height,
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Left = deltaLeft + i * delta,
                    Top = Util.UnscaledBounds.Bottom - height * 2
                };
                
                ambiWindow.Show();
            }
        }
    }
}
