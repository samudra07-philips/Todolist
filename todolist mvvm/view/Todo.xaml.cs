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
    /// Interaction logic for Todo.xaml
    /// </summary>
    public partial class Todo : Page
    {
        public Todo()
        {
            InitializeComponent();
        }

        private void addtask_Click(object sender, RoutedEventArgs e)
        {
            Addtaskwindow a = new Addtaskwindow();
            Mainwindow m = Application.Current.MainWindow as Mainwindow;
            double l = m.Left + (m.Width - a.Width) / 2;
            double t = m.Top + (m.Height - a.Height) / 2;
            a.Left = l;
            a.Top = t;
            if (a.Content is IRefreshablePage)
            {
                a.RefreshContent();
            }
            a.ShowDialog();
        }
    }
}
