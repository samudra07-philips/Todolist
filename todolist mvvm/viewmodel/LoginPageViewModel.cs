using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private string username;
        private string password;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand LoginCommand { get; }
        public RelayCommand SignUpCommand { get; }

        public LoginPageViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
            SignUpCommand = new RelayCommand(ExecuteSignUp);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanExecuteLogin(object parameter)
        {
            bool canlogin = !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
            return canlogin;
        }

        private void ExecuteLogin(object parameter)
        {
            if (!CanExecuteLogin(null))
            {
                MessageBox.Show(
                    "Invalid Credentials! Please fill in all fields.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
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
                page.NavigationService.Navigate(new Signup());
            }
        }
    }
}
