using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using Todolist.Services.Contracts;
using Todolist.Services;// ITaskService, TaskDto
using todolist_mvvm.Bussiness_Layer;
using todolist_mvvm.model;

namespace todolist_mvvm.viewmodel
{
    public class AddTaskWindowViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;
        private readonly Window _window;

        private string _title;
        private string _description;
        private string _selectedPriority;

        public RelayCommand AddNewTask { get; }

        public List<string> Priorities { get; } =
            new List<string> { "Low", "Medium", "High", "Critical" };

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                    AddNewTask.RaiseCanExecuteChanged();
                }
            }
        }
        public AddTaskWindowViewModel()
        {
            
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                    AddNewTask.RaiseCanExecuteChanged();
                }
            }
        }

        public string SelectedPriority
        {
            get => _selectedPriority;
            set
            {
                if (_selectedPriority != value)
                {
                    _selectedPriority = value;
                    OnPropertyChanged(nameof(SelectedPriority));
                    AddNewTask.RaiseCanExecuteChanged();
                }
            }
        }

        public AddTaskWindowViewModel(ITaskService taskService, Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

            AddNewTask = new RelayCommand(ExecuteAdd, CanExecuteAdd);
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }

        private bool CanExecuteAdd(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Title)
                && !string.IsNullOrWhiteSpace(SelectedPriority);
        }

        private void ExecuteAdd(object parameter)
        {
            if (!Enum.TryParse<TaskPriority>(SelectedPriority, out var priority))
            {
                MessageBox.Show(
                    "Invalid priority selected. Please select a valid priority.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            var dto = new TaskDto
            {
                UserId = CurrentUser.Id,
                Title = Title,
                Description = Description,
                Priority = (Todolist.Services.Data.TaskPriority)priority
            };

            try
            {
                // Send to service
                _taskService.AddTask(dto);
                ((IClientChannel)_taskService).Close();
            }
            catch (Exception ex)
            {
                ((IClientChannel)_taskService).Abort();
                MessageBox.Show($"Error creating task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Tell the owning page to refresh
            if (_window.Owner is IRefreshablePage refreshable)
            {
                refreshable.RefreshContent();
            }
            else if (_window.Content is Frame frame && frame.Content is IRefreshablePage page)
            {
                page.RefreshContent();
            }

            // Close the dialog
            _window.Close();
        }
    }
}
