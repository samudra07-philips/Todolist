using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using todolist_mvvm.Data;
using todolist_mvvm.view;
using todolist_mvvm.model;
namespace todolist_mvvm.viewmodel
{
    public class LoginPageViewModel : BaseViewModel
    {
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
                    LoginCommand.RaiseCanExecuteChanged();
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
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand LoginCommand { get; }
        public RelayCommand SignUpCommand { get; }

        public LoginPageViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            SignUpCommand = new RelayCommand(ExecuteSignUp);
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void ExecuteLogin(object parameter)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show(
                    "Invalid Credentials! Please fill in all fields.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            using (var context = new AppDbContext())
            {
           
                string passwordHash = HashPassword(Password);

                var user = context.Users.FirstOrDefault(u =>
                    u.Username == Username && u.PasswordHash == passwordHash
                );

                if (user == null)
                {
                    MessageBox.Show(
                        "Invalid username or password!",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }
            }

            
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
                page.NavigationService.Navigate(new Signup());
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
