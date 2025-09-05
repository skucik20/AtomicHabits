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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MainVindowView.xaml
    /// </summary>
    public partial class MainVindowView : Window
    {
        private readonly Random _rand = new Random();
        private readonly Polyline _wave1;
        private readonly Polyline _wave2;
        private readonly DispatcherTimer _timer;
        private double _offset = 0;
        public MainVindowView()
        {
            InitializeComponent();
            // Timer co 500 ms generuje nowy bąbelek
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => CreateBubble();
            timer.Start();


            //Waves
            _wave1 = new Polyline
            {
                Stroke = Brushes.DeepSkyBlue,
                StrokeThickness = 2,
                Opacity = 0.6
            };

            _wave2 = new Polyline
            {
                Stroke = Brushes.MediumPurple,
                StrokeThickness = 3,
                Opacity = 0.4
            };

            WaveCanvas.Children.Add(_wave1);
            WaveCanvas.Children.Add(_wave2);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(30); // ok. 33 FPS
            _timer.Tick += (s, e) => DrawWaves();
            _timer.Start();



        }
        private void DrawWaves()
        {
            double width = WaveCanvas.ActualWidth;
            double height = WaveCanvas.ActualHeight;
            if (width == 0 || height == 0) return;

            _wave1.Points.Clear();
            _wave2.Points.Clear();

            int points = 200; // liczba próbek
            double step = width / points;

            for (int i = 0; i <= points; i++)
            {
                double x = i * step;
                // fala 1
                double y1 = height / 2 + 30 * Math.Sin((x + _offset) * 0.02);
                // fala 2 (przesunięta fazowo)
                double y2 = height / 2 + 50 * Math.Sin((x + _offset) * 0.015 + Math.PI / 2);

                _wave1.Points.Add(new Point(x, y1));
                _wave2.Points.Add(new Point(x, y2));
            }

            _offset += 2; // prędkość przesuwania
        }

        private void CreateBubble()
        {
            double size = _rand.Next(20, 80); // średnica
            double startX = _rand.Next(0, (int)ParticleCanvas.ActualWidth);

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
            Canvas.SetTop(ellipse, ParticleCanvas.ActualHeight);

            ParticleCanvas.Children.Add(ellipse);

            // Animacja w górę
            var moveAnim = new DoubleAnimation
            {
                From = ParticleCanvas.ActualHeight,
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
            moveAnim.Completed += (s, e) => ParticleCanvas.Children.Remove(ellipse);

            ellipse.BeginAnimation(Canvas.TopProperty, moveAnim);
            ellipse.BeginAnimation(OpacityProperty, fadeAnim);
        }
    }
}
