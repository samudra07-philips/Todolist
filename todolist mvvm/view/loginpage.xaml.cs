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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void textboxsample_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }

        private void passboxsample_TextChanged(object sender, RoutedEventArgs e)
        {
          
        }

        private void Signedup(object sender, RoutedEventArgs e)
        {
           
        }

        private void Logedin(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Todo());
        }
        private void RemoveText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter your name" || textBox.Text == "Enter your password")
            {
                textBox.Text = "";
            }
        }
        private void AddText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "textboxsample")
                {
                    textBox.Text = "Enter your name";
                }
                else if (textBox.Name == "passboxsample")
                {
                    textBox.Text = "Enter your password";
                }
            }
        }
    }
}
