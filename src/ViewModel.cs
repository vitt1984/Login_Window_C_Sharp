/*
 * Created by SharpDevelop.
 * User: Vittorio
 * Date: 18/11/2014
 * Time: 21:58
 * 
 * 
 */
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Test2
{	
	/// <summary>
	/// View Model containing the busines logic of the application
	/// </summary>
	
	public class ViewModel : INotifyPropertyChanged
	{
		// Model, handling authentication
		private Model _model;
		
		// Commands
	    private ICommand _loginCommand;
	    private ICommand _resetCommand;
	    
		// Bound Properties
		private string _windowTitle;
		private string _currentUsername;
		private string _authenticationMessage;
        private Brush _gridBackground;
        
        // Constructor
	    
		public ViewModel()
		{
			_model = new Model();
			_windowTitle = "Login";
			_authenticationMessage = "Please Login";
			_loginCommand = new RelayCommand(new Action<object>(AttemptLogin));
			_resetCommand = new RelayCommand(new Action<object>(ResetBoxes));
			_gridBackground = new SolidColorBrush(Colors.White);
		}
		
		// Actions
		
		// Parameters: object that will be casted to PasswordBox
		// Does: calls models for authentication.
		//       if the answer is positive, sets the AuthenticationMessage to Login successful
		//       else sets the AuthenticationMessage to Login failed, clears the TextBox and the PasswordBox  
		//       and shows a quick animation

        public void AttemptLogin(object obj)
        {
        	if (_model.authenticate(_currentUsername,((PasswordBox)obj).Password))
        	{    		
        		AuthenticationMessage = "Login successful";
        	}
        	else
        	{
        		Username = "";
        		AuthenticationMessage = "Login failed";
                ((PasswordBox)obj).Clear();
                ColorAnimation animation;
		        animation = new ColorAnimation();
				animation.From = (Color?)ColorConverter.ConvertFromString("#00EE0000");
		        animation.To = Colors.White;
		        animation.Duration = new Duration(TimeSpan.FromSeconds(1));
		        GridBackground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        	}
        }

		// Parameters: object that will be casted to PasswordBox
		// Does: clears the TextBox and the PasswordBox  
		
        public void ResetBoxes(object obj)
        {
            Username = "";
            ((PasswordBox)obj).Clear();
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string propertyName)
        {
        	PropertyChangedEventHandler handler = PropertyChanged;
        	if (handler != null)
        	{
        		handler(this, new PropertyChangedEventArgs(propertyName));
        	}
        }
        #endregion
        
		// Bound properties

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand;
            }
            set
            {
        		if (this._loginCommand == value) return;
                _loginCommand = value;
            }
        }
        
        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand;
            }
            set
            {
        		if (this._resetCommand == value) return;
                _resetCommand = value;
            }
        }
	
        public string WindowTitle
        {
        	get {
        		return this._windowTitle;
        	}
        	set {
        		if (this._windowTitle == value) return;
        		this._windowTitle = value;
        		OnPropertyChanged("WindowTitle");
        	}
        }
        
        public string Username
        {
        	get {
        		return this._currentUsername;
        	}
        	set {
        		if (this._currentUsername == value) return;
        		this._currentUsername = value;
        		OnPropertyChanged("Username");
        	}
        }
        
        public string AuthenticationMessage
        {
        	get {
        		return this._authenticationMessage;
        	}
        	set {
        		if (this._authenticationMessage == value) return;
        		this._authenticationMessage = value;
        		OnPropertyChanged("AuthenticationMessage");
        	}
        }

		public Brush GridBackground {
			get {
				return _gridBackground;
			}
			set {
        		if (this._gridBackground == value) return;
        		this._gridBackground = value;
        		OnPropertyChanged("GridBackground");
			}
		}
	}
}
