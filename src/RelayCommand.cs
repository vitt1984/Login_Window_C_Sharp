/*
 * Created by SharpDevelop.
 * User: Vittorio
 * Date: 18/11/2014
 * Time: 23:07
 * 
 */
using System;
using System.Linq;
using System.Windows.Input;

namespace Test2
{
	/// <summary>
    /// Relay Command, to invoke delegates in the ViewModel
	/// </summary> 
	
    class RelayCommand : ICommand
    {
        private readonly Action<object> _action;

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
        	_action(parameter);
        }

        #endregion
    }
}