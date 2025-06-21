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
using todolist_mvvm.Bussiness_Layer;
namespace todolist_mvvm.view
{
    ///<summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page, IRefreshablePage
    {
        public void RefreshContent()
        {
            if (DataContext is LoginPageViewModel viewModel)
            {
                viewModel.Username=string.Empty;
                viewModel.Password=string.Empty;
            }
            textboxsample.Text = string.Empty;
            passboxsample.Password = string.Empty;
        }
        public LoginPage()
        {
            InitializeComponent();
            CurrentUser.Clear();
            
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passboxsample.Password))
                passboxsample.Background.Opacity = 1;
            else
                passboxsample.Background.Opacity = 0;
         
            if (DataContext is LoginPageViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void textboxsample_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textboxsample.Text))
                textboxsample.Background.Opacity = 1;
            else
                textboxsample.Background.Opacity = 0;
        }
    }
}
