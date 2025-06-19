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
    public partial class Todo : Page, IRefreshablePage
    {
        public Todo()
        {
            InitializeComponent();
            DataContext = new ToDoViewModel();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ToDoViewModel vm && vm.OpenTaskDetailsCommand.CanExecute(null))
            {
                vm.OpenTaskDetailsCommand.Execute(null);
            }
        }

        public void RefreshContent()
        {
            if (DataContext is ToDoViewModel vm)
            {
                vm.SearchQuery = string.Empty;
            }
        }
    }
}
