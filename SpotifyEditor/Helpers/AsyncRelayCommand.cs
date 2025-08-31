    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpotifyEditor.Helpers
{
    
    public class AsyncRelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Func<object,Task> _execute;
        private readonly Predicate<object>? _canExecute;

        public AsyncRelayCommand(Func<object, Task> execute, Predicate<object>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter = null)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;//Make this false after implemeting canExecutes
        }

        public async void Execute(object parameter)
        {
            await _execute(parameter);
        }
    
    }
}
