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

        private void passboxsignup_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void checkpassboxsignup_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
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
