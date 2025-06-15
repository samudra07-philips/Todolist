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
using System.Windows.Shapes;
using todolist_mvvm.viewmodel;

namespace todolist_mvvm.view
{
    /// <summary>
    /// Interaction logic for Addtaskwindow.xaml
    /// </summary>
    public partial class Addtaskwindow : Window,IRefreshablePage
    {
        public Addtaskwindow()
        {
            InitializeComponent();
        }
        public void RefreshContent()
        {
            Taskname.Text= string.Empty;
            Add_descripton.Text= string.Empty;

        }
        private void Taskname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(Taskname.Text))
                Taskname.Background.Opacity = 1;
            else Taskname.Background.Opacity = 0;
        }

        private void Add_descripton_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Add_descripton.Text))
                Add_descripton.Background.Opacity = 1;
            else
                Add_descripton.Background.Opacity = 0;
        }

        private void addtaskbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
