using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpotifyEditor.Helpers
{
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute;
        
        public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter = null)
        {
            if (_canExecute != null)
            {
                return _canExecute((T)parameter);
            }
            return true;//Make this false after implemeting canExecutes
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
