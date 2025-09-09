using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy BubblesAnimationView.xaml
    /// </summary>
    public partial class BubblesAnimationView : UserControl
    {
        private readonly Random _rand = new Random();

        public BubblesAnimationView()
        {
            InitializeComponent();
            // Timer co 500 ms generuje nowy bąbelek
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => CreateBubble();
            timer.Start();
        }

        private void CreateBubble()
        {
            double size = _rand.Next(20, 80); // średnica
            double startX = _rand.Next(0, (int)BubblesCanvas.ActualWidth);

            var ellipse = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(Color.FromArgb(
                    (byte)_rand.Next(50, 150), // przezroczystość
                    (byte)_rand.Next(100, 255),
                    (byte)_rand.Next(100, 255),
                    (byte)_rand.Next(255)
                ))
            };

            Canvas.SetLeft(ellipse, startX);
            Canvas.SetTop(ellipse, BubblesCanvas.ActualHeight);

            BubblesCanvas.Children.Add(ellipse);

            // Animacja w górę
            var moveAnim = new DoubleAnimation
            {
                From = BubblesCanvas.ActualHeight,
                To = -size,
                Duration = TimeSpan.FromSeconds(_rand.Next(5, 12)),
                FillBehavior = FillBehavior.Stop
            };

            // Animacja zanikania
            var fadeAnim = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = moveAnim.Duration,
                FillBehavior = FillBehavior.Stop
            };

            // Event po zakończeniu usuwa bąbelek z canvasa
            moveAnim.Completed += (s, e) => BubblesCanvas.Children.Remove(ellipse);

            ellipse.BeginAnimation(Canvas.TopProperty, moveAnim);
            ellipse.BeginAnimation(OpacityProperty, fadeAnim);
        }
    }
}
