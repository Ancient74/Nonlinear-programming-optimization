using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lagrande
{
    public class CallbackCommand : ICommand
    {
        private Action<object> callback;
        public event EventHandler CanExecuteChanged;

        public CallbackCommand(Action<object> callback)
        {
            this.callback = callback;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            callback?.Invoke(parameter);
        }
    }
}
