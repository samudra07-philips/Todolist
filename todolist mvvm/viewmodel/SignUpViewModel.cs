using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Todolist.Services;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class SignUpViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _confirmpassword;
        private readonly IUserService _userService;
        public string Username
        {
            get => _username;
            set
            {   
                if (_username != value)
                {
                    
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                    SignupCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    SignupCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Confirmpassword
        {
            get => _confirmpassword;
            set
            {
                if (_confirmpassword != value)
                {
                    _confirmpassword = value;
                    OnPropertyChanged(nameof(Confirmpassword));
                    SignupCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand SignupCommand { get; }

        public SignUpViewModel(IUserService userService)
        {
            SignupCommand = new RelayCommand(Execute);
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private void Execute(object parameter)
        {
            if (
                string.IsNullOrEmpty(Username)
                || string.IsNullOrEmpty(Password)
                || string.IsNullOrEmpty(Confirmpassword)
            )
            {
                MessageBox.Show("Invalid Credentials! Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Username.Length < 5 || Username.Length > 20)
            {
                MessageBox.Show("Username must be between 5 and 20 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(Username, @"^[a-zA-Z0-9_]*$"))
            {
                MessageBox.Show("Invalid username! Only alphanumeric characters and underscores are allowed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Password.Length < 8 || Password.Length > 15)
            {
                MessageBox.Show("Password must be 8–15 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Password != Confirmpassword)
            {
                MessageBox.Show("Passwords do not match!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                // Call through WCF to create the user (repository will hash & save)
                _userService.Signup(Username, Password);
                ((IClientChannel)_userService).Close();

                MessageBox.Show(
                    "Your account has been successfully created!\nPlease log in to your account!",
                    "Account Created",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                if (parameter is Page page)
                {
                    Username = string.Empty;
                    Password = string.Empty;
                    Confirmpassword = string.Empty;

                
                    page.NavigationService.Navigate(new LoginPage(_userService));
                }
            }
           
            catch (Exception ex)
            {
                ((IClientChannel)_userService).Abort();
                MessageBox.Show(
                    $"An unexpected error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }


        
    }
}
