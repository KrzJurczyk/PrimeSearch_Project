using System;
using System.Windows.Input;

namespace PrimeSearch.UI.Utils
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;
        private bool _canBeExecuted = true;

        public DelegateCommand(Action action)
        {
            this._action = action;
        }

        public bool CanExecute(object parameter)
        {
            return CanBeExecuted;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
        }

        public bool CanBeExecuted
        {
            get { return _canBeExecuted; }
            set
            {
                _canBeExecuted = value; 
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IDisposable RunWithDisabled()
        {
            return new DisposableEnabler(this);
        }

        public event EventHandler CanExecuteChanged;

        private class DisposableEnabler : IDisposable
        {
            private readonly DelegateCommand _command;

            public DisposableEnabler(DelegateCommand command)
            {
                _command = command;
                _command.CanBeExecuted = false;
            }

            public void Dispose()
            {
                _command.CanBeExecuted = true;
            }
        }
    }
}