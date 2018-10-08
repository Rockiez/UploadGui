using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UploadGui.Commands
{
    public class DelegateCommand:ICommand
    {
        public bool CanExecute(object parameter)
        {
            return CanExecutePre == null || CanExecutePre(parameter);
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteAction == null)
            {
                return;
            }
            this.ExecuteAction(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public Action<object> ExecuteAction { get; set; }
        public Predicate<object> CanExecutePre { get; set;}
    }
}
