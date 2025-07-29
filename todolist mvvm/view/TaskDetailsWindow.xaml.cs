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
using todolist_mvvm.model;
using todolist_mvvm.viewmodel;
using Todolist.Services;
using Todolist.Services.Contracts;

namespace todolist_mvvm.view
{
    /// <summary>
    /// Interaction logic for TaskDetails.xaml
    /// </summary>
    public partial class TaskDetails : Window
    {
        public TaskDetails(TaskDto taskDto, ITaskService taskService)
        {
            InitializeComponent();
            DataContext = new TaskDetailsViewModel(taskService, taskDto, this);
        }
    }
}
