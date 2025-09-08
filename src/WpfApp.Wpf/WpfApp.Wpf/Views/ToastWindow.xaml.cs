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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System;
using System.Windows.Threading;

namespace WpfApp.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window
    {
        public ToastWindow(string title, string message, int durationSeconds = 3)
        {
            InitializeComponent();

            TitleText.Text = title;
            MessageText.Text = message;

            Loaded += (s, e) =>
            {

                var app = Application.Current.Windows
                            .OfType<MainVindowView>()
                            .FirstOrDefault();

                // Pozycjonowanie w prawym dolnym rogu ekranu

                var workingArea = SystemParameters.WorkArea;
                Left = app.Left + app.ActualWidth - Width - 10;
                Top = app.Top + app.ActualHeight - Height - 10;

                // Timer do automatycznego zamknięcia
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(durationSeconds) };
                timer.Tick += (s2, e2) => { Close(); };
                timer.Start();
            };
        }
    }
}
