using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace ambi_gui.View.Theme
{
    /// <summary>
    ///     Interaction logic for DisplaysPanel.xaml
    /// </summary>
    public partial class DisplaysPanel : UserControl
    {
        public DisplaysPanel()
        {
            InitializeComponent();
        }
        
        private void DisplaysCanvas_OnLoaded(object sender, RoutedEventArgs e)
        {
            var displays = GetDisplayDimensions().ToArray();
            var workingArea = displays.Aggregate(Rect.Union);

            const int margin = 20;
            var scale = Math.Min((DisplaysCanvas.ActualWidth - margin) / workingArea.Width, (DisplaysCanvas.ActualHeight - margin) / workingArea.Height);

            displays = displays.Select(rect => new Rect(
                rect.X * scale,
                rect.Y * scale,
                rect.Width * scale,
                rect.Height * scale
            )).ToArray();

            workingArea = displays.Aggregate(Rect.Union);

            var left = DisplaysCanvas.ActualWidth / 2 - workingArea.Width / 2;
            var top = DisplaysCanvas.ActualHeight / 2 - workingArea.Height / 2;

            foreach (var display in displays)
            {
                var rectangle = new Rectangle
                {
                    Width = display.Width,
                    Height = display.Height,
                    Fill = new SolidColorBrush(WindowsColorizationProvider.GetWindowColorizationColor()),
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.3
                };

                DisplaysCanvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, left);
                Canvas.SetTop(rectangle, top);

                left += display.Width;
                //top += display.Width;
            }
        }

        private static IEnumerable<Rect> GetDisplayDimensions()
        {
            foreach (var screen in Screen.AllScreens)
            {
                var scaleX = SystemParameters.WorkArea.Width / screen.WorkingArea.Width;
                var scaleY = SystemParameters.WorkArea.Height / screen.WorkingArea.Height;

                yield return new Rect(
                    scaleX * screen.Bounds.X,
                    scaleY * screen.Bounds.Y,
                    scaleX * screen.Bounds.Width,
                    scaleY * screen.Bounds.Height
                );
            }
        }
    }
}