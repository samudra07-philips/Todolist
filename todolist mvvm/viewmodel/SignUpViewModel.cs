using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using todolist_mvvm.Data;
using todolist_mvvm.model;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class SignUpViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _confirmpassword;

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

        public SignUpViewModel()
        {
            SignupCommand = new RelayCommand(Execute);
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

            string passwordHash = HashPassword(Password);

            using (var context = new AppDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (context.Users.Any(u => u.Username == Username))
                        {
                            MessageBox.Show("This username is already taken.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var user = new User { Username = Username, PasswordHash = passwordHash };
                        context.Users.Add(user);
                        context.SaveChanges();

                        transaction.Commit();

                        MessageBox.Show("Your account has been successfully created!\nPlease log in to your account!", "Account Created", MessageBoxButton.OK, MessageBoxImage.Information);

                        if (parameter is Page page)
                        {
                            Username = string.Empty;
                            Password = string.Empty;
                            Confirmpassword = string.Empty;

                            page.NavigationService.Navigate(new LoginPage());
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
