using System;
using System.ServiceModel;
using System.Windows;
using Todolist.Services.Contracts;
using Todolist.Services;
using todolist_mvvm.model;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;
        private readonly Window _window;

        private TaskDto _taskDto;
        private bool _isEditing;
        private Tasks selectedTask;
        private TaskDetails detailsWindow;

        public TaskDto TaskDto
        {
            get => _taskDto;
            set { _taskDto = value; OnPropertyChanged(nameof(TaskDto)); }
        }

        public bool IsEditing
        {
            get => _isEditing;
            private set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                    OnPropertyChanged(nameof(IsReadOnly));
                    SaveCommand.RaiseCanExecuteChanged();
                    EditCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsReadOnly => !IsEditing;

        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand MarkCompletedCommand { get; }

        public TaskDetailsViewModel(ITaskService taskService, TaskDto selectedTask, Window window)
        {

            EditCommand = new RelayCommand(_ => BeginEdit(), _ => !IsEditing);
            SaveCommand = new RelayCommand(_ => SaveAsync(), _ => IsEditing);
            DeleteCommand = new RelayCommand(_ => DeleteAsync(), _ => true);
            MarkCompletedCommand = new RelayCommand(_ => MarkCompletedAsync(), _ => !TaskDto.IsCompleted);
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _window = window ?? throw new ArgumentNullException(nameof(window));

            TaskDto = selectedTask;

        }

        public TaskDetailsViewModel(Tasks selectedTask, TaskDetails detailsWindow)
        {
            this.selectedTask = selectedTask;
            this.detailsWindow = detailsWindow;
        }

        private void BeginEdit() => IsEditing = true;

        private void SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(TaskDto.Title))
            {
                MessageBox.Show("Title cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Map TaskDto back to itself and call Update
                _taskService.UpdateTask(TaskDto);
                ((IClientChannel)_taskService).Close();
            }
            catch (Exception ex)
            {
                ((IClientChannel)_taskService).Abort();
                MessageBox.Show($"Error saving task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsEditing = false;
            MessageBox.Show("Changes saved.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteAsync()
        {
            var result = MessageBox.Show("Are you sure you want to delete this task?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                _taskService.DeleteTask(TaskDto.Id);
                ((IClientChannel)_taskService).Close();
            }
            catch (Exception ex)
            {
                ((IClientChannel)_taskService).Abort();
                MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _window.Close();
        }

        private void MarkCompletedAsync()
        {
            var result = MessageBox.Show("Mark this task as completed?",
                                         "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                _taskService.MarkTaskComplete(TaskDto.Id);
                ((IClientChannel)_taskService).Close();
            }
            catch (Exception ex)
            {
                ((IClientChannel)_taskService).Abort();
                MessageBox.Show($"Error marking complete: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _window.Close();
        }
    }
}
