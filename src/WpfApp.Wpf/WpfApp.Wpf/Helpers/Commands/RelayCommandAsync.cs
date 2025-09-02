using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp.Wpf.Helpers.Commands
{
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool>? _canExecute;

        public RelayCommandAsync(Func<object, Task> execute, Func<object, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter!) ?? true;

        public async void Execute(object? parameter)
        {
            await _execute(parameter!);
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
