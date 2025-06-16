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
    public partial class Signup : Page, IRefreshablePage
    {
        public Signup()
        {
            InitializeComponent();
        }

        public void RefreshContent()
        {   if (DataContext is SignUpViewModel viewmodel)
            {
                textboxsignup.Text = string.Empty;
                passboxsignup.Password = string.Empty;
                checkpassboxsignup.Password = string.Empty;
            }
            textboxsignup.Text = string.Empty;
            passboxsignup.Password = string.Empty;
            checkpassboxsignup.Password = string.Empty;
        }

        private void textboxsignup_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textboxsignup.Text))
                textboxsignup.Background.Opacity = 1;
            else
                textboxsignup.Background.Opacity = 0;
        }

        private void passboxsignup_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passboxsignup.Password))
                passboxsignup.Background.Opacity = 1;
            else
                passboxsignup.Background.Opacity = 0;
            if(DataContext is SignUpViewModel viewModel)
            {
                viewModel.Password=((PasswordBox)sender).Password;
            }

        }

        private void checkpassboxsignup_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(checkpassboxsignup.Password))
                checkpassboxsignup.Background.Opacity = 1;
            else
                checkpassboxsignup.Background.Opacity = 0;

            if (DataContext is SignUpViewModel viewModel)
            {
                viewModel.Confirmpassword = ((PasswordBox)sender).Password;
            }
        }

        
    }
}
