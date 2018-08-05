using System;
using System.Windows.Input;

namespace RDPManager.ViewModel
{
    internal class ActionCommand : ICommand
    {
        private readonly Action _action;
        private bool _enabled;

        public event EventHandler CanExecuteChanged;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public ActionCommand(Action action) : this(action, true) { }

        public ActionCommand(Action action, bool enabled)
        {
            this._action = action ?? throw new ArgumentNullException("action", "Action cannot be null");
            this.Enabled = enabled;
        }

        public bool CanExecute(object parameter)
        {
            return Enabled;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
        }
    }
}
