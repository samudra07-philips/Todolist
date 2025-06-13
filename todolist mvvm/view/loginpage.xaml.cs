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
    ///<summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page,IRefreshablePage
    {
        public void RefreshContent()
        {
            textboxsample.Text = string.Empty;
            passboxsample.Password = string.Empty;
            
        }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void textboxsample_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textboxsample.Text.Length == 0) textboxsample.Background.Opacity = 1;
            else textboxsample.Background.Opacity = 0;
        }
        private void passboxsample_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passboxsample.Password.Length == 0)
                passboxsample.Background.Opacity = 1;
            else
                passboxsample.Background.Opacity = 0;
        }

       
        private void Signedup(object sender, RoutedEventArgs e)
        {
            
            this.NavigationService.Navigate(new Signup());
            

        }

        private void Logedin(object sender, RoutedEventArgs e)

        {
            if (string.IsNullOrEmpty(textboxsample.Text) || string.IsNullOrEmpty(passboxsample.Password))
            {
                MessageBox.Show("Invalid Credentials! Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.NavigationService.Navigate(new Todo());
        }

    }
}
