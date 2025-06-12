using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace todolist_mvvm.view
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void textboxsignup_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void passboxsignup_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
        private void passboxsignup_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordPlaceholder.Visibility = Visibility.Hidden;
        }

        private void passboxsignup_LostFocus(object sender, RoutedEventArgs e)
        {
            passwordPlaceholder.Visibility = string.IsNullOrEmpty(passboxsignup.Password)
                  ? Visibility.Visible : Visibility.Hidden;
        }
        private void checkpassboxsignup_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void checkpassboxsignup_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void checkpassboxsignup_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (textboxsignup.Text == "Enter your name" || passboxsignup.Password=="" || checkpassboxsignup.Password=="")
            {
                MessageBox.Show("Invalid Credentials! Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (passboxsignup.Password == checkpassboxsignup.Password)
            {
                MessageBox.Show("Your account has been successfully created!\n Please log in to your acount!", "Account Created", MessageBoxButton.OK, MessageBoxImage.Information);
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(new LoginPage());
                }
                else if (Application.Current.MainWindow is Mainwindow mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new LoginPage());
                }
            }
            else if (passboxsignup.Password != checkpassboxsignup.Password)
            {
                MessageBox.Show("Passwords do not match!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                MessageBox.Show("Invalid Credentials", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void RemoveText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter your name" || textBox.Text == "Enter your password"|| textBox.Text == "Confirm your password")
            {
                textBox.Text = "";
            }
        }
        private void AddText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "textboxsignup")
                {
                    textBox.Text = "Enter your name";
                }
                else if (textBox.Name == "passboxsignup")
                {
                    textBox.Text = "Enter your password";
                }
                else if (textBox.Name == "checkpassboxsignup")
                {
                    textBox.Text = "Confirm your password";
                }
            }
        }


    }
}
