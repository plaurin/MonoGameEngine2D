using System;
using System.Windows.Input;

namespace GameNavigator
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> executeAction;
        private readonly Func<object, bool> canExecuteFunc;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc = null)
        {
            this.executeAction = executeAction;
            this.canExecuteFunc = canExecuteFunc;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecuteFunc == null || this.canExecuteFunc.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            this.executeAction.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null) handler.Invoke(this, new EventArgs());
        }
    }
}