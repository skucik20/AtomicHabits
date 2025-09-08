using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApp.Wpf.ViewModels.Shared
{
    public class ToastWindowViewModel : BaseViewModel
    {

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        public event Action RequestClose;

        public ToastWindowViewModel(string title, string message, int durationSeconds = 3)
        {
            Title = title;
            Message = message;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(durationSeconds) };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                RequestClose?.Invoke();
            };
            timer.Start();
        }
    }
}
