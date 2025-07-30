
using System.Windows.Controls;
using Todolist.Services;
using todolist_mvvm.viewmodel;
using Unity;

namespace todolist_mvvm.view
{
    /// <summary>
    /// Interaction logic for Historypage.xaml
    /// </summary>
    public partial class Historypage : Page
    {
        public Historypage()
        {
            InitializeComponent();

            // 1) Grab the container
            var container = App.container;

            // 2) Resolve the service
            var taskService = container.Resolve<ITaskService>();

            // 3) Now resolve the VM, passing in the service and this Window
            var vm = new HistoryViewModel(taskService);
            // Alternatively, if you want Unity to construct the VM:
            // var vm = container.Resolve<AddTaskWindowViewModel>(
            //     new ParameterOverride("window", this)
            // );

            DataContext = vm;
        }
    }
}
