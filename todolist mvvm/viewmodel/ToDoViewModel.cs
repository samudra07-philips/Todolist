using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using todolist_mvvm.Data;
using todolist_mvvm.model;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class ToDoViewModel : BaseViewModel, IRefreshablePage
    {
        public ObservableCollection<Tasks> LowPriorityTasks { get; } =
            new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> MediumPriorityTasks { get; } =
            new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> HighPriorityTasks { get; } =
            new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> CriticalPriorityTasks { get; } =
            new ObservableCollection<Tasks>();

        private Tasks selectedTask;
        public Tasks SelectedTask
        {
            get => selectedTask;
            set
            {
                if (selectedTask != value)
                {
                    selectedTask = value;
                    OnPropertyChanged(nameof(SelectedTask));
                    OpenTaskDetailsCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand OpenTaskDetailsCommand { get; }
        public RelayCommand AddCommand { get; }

        public ToDoViewModel()
        {
            LoadTasks();
            OpenTaskDetailsCommand = new RelayCommand(OpenTaskDetails, _ => SelectedTask != null);
            AddCommand = new RelayCommand(ExecuteAdd, _ => true);
        }

        public void RefreshContent()
        {
            // Clear existing so UI sees removals
            LowPriorityTasks.Clear();
            MediumPriorityTasks.Clear();
            HighPriorityTasks.Clear();
            CriticalPriorityTasks.Clear();
            LoadTasks();
        }

        private void LoadTasks()
        {
            using (var context = new AppDbContext())
            {
                var tasks = context
                    .Tasks.Where(t => !t.IsCompleted) // show only pending; adjust as needed
                    .ToList();

                foreach (var task in tasks)
                {
                    switch (task.Priority)
                    {
                        case TaskPriority.Low:
                            LowPriorityTasks.Add(task);
                            break;
                        case TaskPriority.Medium:
                            MediumPriorityTasks.Add(task);
                            break;
                        case TaskPriority.High:
                            HighPriorityTasks.Add(task);
                            break;
                        case TaskPriority.Critical:
                            CriticalPriorityTasks.Add(task);
                            break;
                    }
                }
            }
        }

        private void ExecuteAdd(object parameter)
        {
            if (parameter is Page page)
            {
                var hostWindow = Window.GetWindow(page);
                if (hostWindow != null)
                {
                    var addWindow = new Addtaskwindow { Owner = hostWindow };
                    // After adding, Addtaskwindow should call RefreshContent on owner page VM
                    addWindow.ShowDialog();
                    // Once closed, reload lists
                    RefreshContent();
                }
                else
                {
                    MessageBox.Show(
                        "Host window not found.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void OpenTaskDetails(object parameter)
        {
            if (SelectedTask == null)
                return;

            
            var detailsWindow = new TaskDetails(selectedTask);
            
            detailsWindow.DataContext = new TaskDetailsViewModel(SelectedTask, detailsWindow);
            detailsWindow.Owner = Window.GetWindow(
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            );
            
            detailsWindow.ShowDialog();
            RefreshContent();
        }
    }
}
