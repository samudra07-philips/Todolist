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
using todolist_mvvm.viewmodel;

namespace todolist_mvvm.view
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Page,IRefreshablePage
    {
        public Signup()
        {
            InitializeComponent();
        }
        public void RefreshContent()
        {
            textboxsignup.Text = string.Empty;
            passboxsignup.Password = string.Empty;
            checkpassboxsignup.Password = string.Empty;
        }
        private void textboxsignup_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textboxsignup.Text)) textboxsignup.Background.Opacity = 1;
            else textboxsignup.Background.Opacity = 0;

        }
        private void passboxsignup_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passboxsignup.Password.Length == 0)
               passboxsignup.Background.Opacity = 1;
            else
                passboxsignup.Background.Opacity = 0;
        }
        private void checkpassboxsignup_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (checkpassboxsignup.Password.Length == 0)
                checkpassboxsignup.Background.Opacity = 1;
            else
               checkpassboxsignup.Background.Opacity = 0;
        }
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textboxsignup.Text) || string.IsNullOrEmpty(passboxsignup.Password) || string.IsNullOrEmpty(checkpassboxsignup.Password))
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
       }


}

