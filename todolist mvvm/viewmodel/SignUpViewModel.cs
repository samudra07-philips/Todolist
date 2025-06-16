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
                username = value;
                OnPropertyChanged(nameof(username));
                Signupcommand.RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                Signupcommand.RaiseCanExecuteChanged();
            }
        }
        public string Confirmpassword
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Confirmpassword));
                Signupcommand.RaiseCanExecuteChanged();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand Signupcommand { get; }

        public SignUpViewModel()
        {
            Signupcommand = new RelayCommand(Execute);
        }

        public bool Canexecute(object parameter)
        {
            bool cansignup = true;
            if (
                string.IsNullOrEmpty(Username)
                || string.IsNullOrEmpty(Password)
                || string.IsNullOrEmpty(Confirmpassword)
            )
                cansignup = false;
            return cansignup;
        }
        public bool Aresamepass(object parameter)
        {
            bool samepasswords = true;
            if(Password!=Confirmpassword)samepasswords = false;
            return samepasswords;
        }
        public void Execute(object parameter) {

            if (!Canexecute(null))
            {
                MessageBox.Show(
                    "Invalid Credentials! Please fill in all fields.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }
            if (!Aresamepass(null))
            {
                MessageBox.Show(
                    "Passwords do not match!",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            if( parameter is Page page)
            {
                MessageBox.Show(
                    "Your account has been successfully created!\n Please log in to your acount!",
                    "Account Created",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                page.NavigationService.Navigate(new LoginPage());
            }
        }
    }
}
