using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfCapture
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action Action { get; }
        public RelayCommand(Action action)
        {
            this.Action = action;
        }
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => this.Action();
    }
}
