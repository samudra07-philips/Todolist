
using System.Windows;
using System.Windows.Controls;
using Todolist.Services;
using todolist_mvvm.viewmodel;
using Unity;

namespace todolist_mvvm.view
{
    /// <summary>
    /// Interaction logic for Addtaskwindow.xaml
    /// </summary>
    public partial class Addtaskwindow : Window
    {
        public Addtaskwindow()
        {
            InitializeComponent();

            // 1) Grab the container
            var container = App.container;

            // 2) Resolve the service
            var taskService = container.Resolve<ITaskService>();

            // 3) Now resolve the VM, passing in the service and this Window
            var vm = new AddTaskWindowViewModel(taskService, this);
            // Alternatively, if you want Unity to construct the VM:
            // var vm = container.Resolve<AddTaskWindowViewModel>(
            //     new ParameterOverride("window", this)
            // );

            DataContext = vm;
        }


        public void RefreshContent()
        {
            Taskname.Text = string.Empty;
            Add_descripton.Text = string.Empty;
        }

        private void Taskname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Taskname.Text))
                Taskname.Background.Opacity = 1;
            else
                Taskname.Background.Opacity = 0;
        }

        private void Add_descripton_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Add_descripton.Text))
                Add_descripton.Background.Opacity = 1;
            else
                Add_descripton.Background.Opacity = 0;
        }

        
    }
}
