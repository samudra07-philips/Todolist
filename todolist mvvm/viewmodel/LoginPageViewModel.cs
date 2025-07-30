using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using Todolist.Services;   // for IUserService, LoginRequest, LoginResponse
using todolist_mvvm.Bussiness_Layer;
using todolist_mvvm.model;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                    LoginCommand?.RaiseCanExecuteChanged();
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
                    LoginCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public LoginPageViewModel()
        {
            
        }
        public RelayCommand LoginCommand { get; }
        public RelayCommand SignUpCommand { get; }

        // Constructor now takes IUserService via DI
        public LoginPageViewModel(IUserService userService)
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            SignUpCommand = new RelayCommand(ExecuteSignUp);
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private bool CanExecuteLogin(object parameter) =>
            !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        private void ExecuteLogin(object parameter)
        {
            int CurrentId;
            try
            {
                CurrentId = _userService.Login(Username, Password);

                ((IClientChannel)_userService).Close();
            }
            catch (Exception ex)
            {
                ((IClientChannel)_userService).Abort();
                MessageBox.Show($"Service error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CurrentId==0)
            {
                MessageBox.Show("Invalid username or password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else CurrentUser.SetUser(CurrentId, Username); //need to be checked later

            if (parameter is Page page)
            {
                page.NavigationService.Navigate(new Todo());
            }

        }

        private void ExecuteSignUp(object parameter)
        {
            if (parameter is Page page)
            {
                Username = string.Empty;
                Password = string.Empty;
                page.NavigationService.Navigate(new Signup(_userService)); // Pass the required IUserService instance
            }
        }
    }
}
