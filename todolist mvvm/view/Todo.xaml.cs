using System.Windows.Controls;

using System.Windows.Input;

using Todolist.Services;
using todolist_mvvm.viewmodel;
using Unity;

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

            // 1) Grab the container
            var container = App.container;

            // 2) Resolve the service
            var taskService = container.Resolve<ITaskService>();

            // 3) Now resolve the VM, passing in the service as a factory function
            var vm = new ToDoViewModel(() => taskService);
           

            DataContext = vm;
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
