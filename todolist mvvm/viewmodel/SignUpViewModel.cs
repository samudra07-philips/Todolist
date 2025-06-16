using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using todolist_mvvm.view;
namespace todolist_mvvm.viewmodel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string username;
        private string password;
        private string confirmpassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                    SignupCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                    SignupCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Confirmpassword
        {
            get => confirmpassword;
            set
            {
                if (confirmpassword != value)
                {
                    confirmpassword = value;
                    OnPropertyChanged(nameof(Confirmpassword));
                    SignupCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand SignupCommand { get; }

        public SignUpViewModel()
        {
            SignupCommand = new RelayCommand(Execute);
        }

        //private bool CanExecute(object parameter)
        //{
        //    return !string.IsNullOrEmpty(Username) &&
        //           !string.IsNullOrEmpty(Password) &&
        //           !string.IsNullOrEmpty(Confirmpassword) &&
        //           Password == Confirmpassword;
        //}

        private void Execute(object parameter)
        {
            if (string.IsNullOrEmpty(Username) ||
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Confirmpassword))
            {
                MessageBox.Show(
                    "Invalid Credentials! Please fill in all fields.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (Password != Confirmpassword)
            {
                MessageBox.Show(
                    "Passwords do not match!",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (parameter is Page page)
            {
                MessageBox.Show(
                    "Your account has been successfully created!\n Please log in to your account!",
                    "Account Created",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                page.NavigationService.Navigate(new LoginPage());
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}