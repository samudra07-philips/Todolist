using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using todolist_mvvm.Bussiness_Layer;
using todolist_mvvm.Data;
using todolist_mvvm.model;

using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class ToDoViewModel : BaseViewModel, IRefreshablePage
    {
        // Task collections
        public ObservableCollection<Tasks> DisplayedLowPriorityTasks { get; private set; } =
            new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> DisplayedMediumPriorityTasks { get; private set; } =
            new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> DisplayedHighPriorityTasks { get; private set; } =
            new ObservableCollection<Tasks>();
        public ObservableCollection<Tasks> DisplayedCriticalPriorityTasks { get; private set; } =
            new ObservableCollection<Tasks>();

        private List<Tasks> allLowPriorityTasks = new List<Tasks>();
        private List<Tasks> allMediumPriorityTasks = new List<Tasks>();
        private List<Tasks> allHighPriorityTasks = new List<Tasks>();
        private List<Tasks> allCriticalPriorityTasks = new List<Tasks>();

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

        // Commands
        public RelayCommand OpenTaskDetailsCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand OpenHistoryPage { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand SearchCommand { get; }

        // Constructor
        public ToDoViewModel()
        {
            LoadTasks();
            OpenTaskDetailsCommand = new RelayCommand(
                param => OpenTaskDetails(param as Tasks),
                param => SelectedTask != null
            );

            AddCommand = new RelayCommand(ExecuteAdd, _ => true);
            SearchCommand = new RelayCommand(_ => ExecuteSearch());
            OpenHistoryPage = new RelayCommand(OpenHistory);
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        public void RefreshContent()
        {
            ClearDisplayedTasks();
            SearchQuery = string.Empty;
            LoadTasks();
        }

        private void ClearDisplayedTasks()
        {
            DisplayedLowPriorityTasks.Clear();
            DisplayedMediumPriorityTasks.Clear();
            DisplayedHighPriorityTasks.Clear();
            DisplayedCriticalPriorityTasks.Clear();
        }

        private void LoadTasks()
        {
            using (var context = new AppDbContext())
            {
                var tasks = context.Tasks.Where(t => t.UserId == CurrentUser.Id && !t.IsCompleted).ToList();

                allLowPriorityTasks = tasks.Where(t => t.Priority == TaskPriority.Low).ToList();
                allMediumPriorityTasks = tasks
                    .Where(t => t.Priority == TaskPriority.Medium)
                    .ToList();
                allHighPriorityTasks = tasks.Where(t => t.Priority == TaskPriority.High).ToList();
                allCriticalPriorityTasks = tasks
                    .Where(t => t.Priority == TaskPriority.Critical)
                    .ToList();
            }

            UpdateDisplayedTasks();
        }

        private void UpdateDisplayedTasks(string query = null)
        {
            UpdateTaskCollection(DisplayedLowPriorityTasks, allLowPriorityTasks, query);
            UpdateTaskCollection(DisplayedMediumPriorityTasks, allMediumPriorityTasks, query);
            UpdateTaskCollection(DisplayedHighPriorityTasks, allHighPriorityTasks, query);
            UpdateTaskCollection(DisplayedCriticalPriorityTasks, allCriticalPriorityTasks, query);
        }

        private void UpdateTaskCollection(
            ObservableCollection<Tasks> displayedCollection,
            List<Tasks> allTasks,
            string query
        )
        {
            displayedCollection.Clear();
            var filteredTasks = string.IsNullOrWhiteSpace(query)
                ? allTasks
                : allTasks.Where(t =>
                    t.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                );

            foreach (var task in filteredTasks)
            {
                displayedCollection.Add(task);
            }
        }

        private string searchQuery;
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                ExecuteSearch();
            }
        }

        private void ExecuteSearch()
        {
            UpdateDisplayedTasks(SearchQuery);

            // If no tasks match the query, display all tasks again
            if (
                DisplayedLowPriorityTasks.Count == 0
                && DisplayedMediumPriorityTasks.Count == 0
                && DisplayedHighPriorityTasks.Count == 0
                && DisplayedCriticalPriorityTasks.Count == 0
            )
            {
                ClearDisplayedTasks();
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
                    addWindow.ShowDialog();
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


        public void OpenHistory(object parameter)
        {
            if (parameter is Page page)
            {
                page.NavigationService.Navigate(new Historypage());
            }
        }

        private void ExecuteLogout(object parameter)
        {
            CurrentUser.Clear();

            if (Application.Current.MainWindow is Mainwindow main && main.MainFrame != null)
            {
                var nav = main.MainFrame.NavigationService;
                while (nav.RemoveBackEntry() != null) { }
                main.MainFrame.Content = new LoginPage();
            }
            else if (parameter is Page page && page.NavigationService != null)
            {
                var nav = page.NavigationService;
                while (nav.RemoveBackEntry() != null) { }
                var frame = Window.GetWindow(page)?.FindName("MainFrame") as Frame;
                frame?.Navigate(new LoginPage());
            }
        }
    }
}
