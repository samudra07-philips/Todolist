using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;

using Todolist.Services;
using Todolist.Services.Contracts;
using todolist_mvvm.Bussiness_Layer;
using todolist_mvvm.model;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class ToDoViewModel : BaseViewModel, IRefreshablePage
    {
        private readonly Func<ITaskService> _taskServiceFactory;
        private readonly IUserService _userService;

        public ObservableCollection<Tasks> DisplayedLowPriorityTasks { get; }
            = new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> DisplayedMediumPriorityTasks { get; }
            = new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> DisplayedHighPriorityTasks { get; }
            = new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> DisplayedCriticalPriorityTasks { get; }
            = new ObservableCollection<Tasks>();

        private List<TaskDto> _allTasks = new List<TaskDto>();

        private Tasks _selectedTask;
        public Tasks SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask != value)
                {
                    _selectedTask = value;
                    OnPropertyChanged(nameof(SelectedTask));
                    OpenTaskDetailsCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand OpenTaskDetailsCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand OpenHistoryPage { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand SearchCommand { get; }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                ExecuteSearch();
            }
        }

        public ToDoViewModel(Func<ITaskService> taskServiceFactory)
        {
            _taskServiceFactory = taskServiceFactory ?? throw new ArgumentNullException(nameof(taskServiceFactory));

            OpenTaskDetailsCommand = new RelayCommand(
                _ => OpenTaskDetails(),
                _ => SelectedTask != null);

            AddCommand = new RelayCommand(_ => ExecuteAdd());
            SearchCommand = new RelayCommand(_ => ExecuteSearch());
            OpenHistoryPage = new RelayCommand(_ => OpenHistory());
            LogoutCommand = new RelayCommand(_ => ExecuteLogout());

            // Initial load
            LoadTasks();
        }

        public void RefreshContent()
        {
            SearchQuery = string.Empty;
            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                // Directly use the task service without a using statement
                var taskService = _taskServiceFactory();
                var dtos = taskService.GetPendingTasks(CurrentUser.Id).ToList();
                _allTasks = dtos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _allTasks = new List<TaskDto>();
            }

            UpdateDisplayedTasks();
        }

        private void UpdateDisplayedTasks(string query = null)
        {
            DisplayedLowPriorityTasks.Clear();
            DisplayedMediumPriorityTasks.Clear();
            DisplayedHighPriorityTasks.Clear();
            DisplayedCriticalPriorityTasks.Clear();

            IEnumerable<TaskDto> filtered = string.IsNullOrWhiteSpace(query)
                ? _allTasks
                : _allTasks.Where(t => t.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var dto in filtered)
            {
                var model = new Tasks
                {
                    Id = dto.Id,
                    Name = dto.Title,
                    Description = dto.Description,
                    IsCompleted = dto.IsCompleted,
                    Priority = (TaskPriority)dto.Priority, // assuming dto.Priority is same enum
                };

                switch (model.Priority)
                {
                    case TaskPriority.Low:
                        DisplayedLowPriorityTasks.Add(model);
                        break;
                    case TaskPriority.Medium:
                        DisplayedMediumPriorityTasks.Add(model);
                        break;
                    case TaskPriority.High:
                        DisplayedHighPriorityTasks.Add(model);
                        break;
                    case TaskPriority.Critical:
                        DisplayedCriticalPriorityTasks.Add(model);
                        break;
                }
            }
        }

        private void ExecuteSearch() => UpdateDisplayedTasks(SearchQuery);

        private void ExecuteAdd()
        {
            var hostWindow = Application.Current.Windows
                                .OfType<Window>()
                                .FirstOrDefault(w => w.IsActive);
            if (hostWindow == null)
            {
                MessageBox.Show("Host window not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addWindow = new Addtaskwindow { Owner = hostWindow };
            addWindow.ShowDialog();
            RefreshContent();
        }

        private void OpenTaskDetails()
        {
            if (SelectedTask == null) return;

            var taskService = _taskServiceFactory();

            // Convert SelectedTask to TaskDto for TaskDetails
            var dto = new TaskDto
            {
                Id = SelectedTask.Id,
                Title = SelectedTask.Name, // Use Name if that's the property holding the title
                Description = SelectedTask.Description,
                IsCompleted = SelectedTask.IsCompleted,
                Priority = (Todolist.Services.Data.TaskPriority)(int)SelectedTask.Priority, // Explicit conversion added
                UserId = SelectedTask.UserId,
                CompletedAt = SelectedTask.CompletedAt
            };

            var detailsWindow = new TaskDetails(dto, taskService);

            // Pass a TaskDetailsViewModel that uses the TaskDto, not Tasks
            detailsWindow.DataContext = new TaskDetailsViewModel(taskService, dto, detailsWindow);

            detailsWindow.Owner = Application.Current.Windows
                                 .OfType<Window>()
                                 .FirstOrDefault(w => w.IsActive);
            detailsWindow.ShowDialog();
            RefreshContent();
        }

        public void OpenHistory()
        {
            var mainFrame = Application.Current.MainWindow
                              .FindName("MainFrame") as System.Windows.Controls.Frame;
            mainFrame?.Navigate(new Historypage());
        }

        private void ExecuteLogout()
        {
            CurrentUser.Clear();
            var main = Application.Current.MainWindow as Mainwindow;
            if (main?.MainFrame != null)
            {
                while (main.MainFrame.RemoveBackEntry() != null) { }
                main.MainFrame.Navigate(new LoginPage(_userService));
            }
        }
    }
}
