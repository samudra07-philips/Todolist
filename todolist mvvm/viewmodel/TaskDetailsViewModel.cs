using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using todolist_mvvm.Data;
using todolist_mvvm.model;

namespace todolist_mvvm.viewmodel
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        private Tasks task;
        private bool isEditing;
        private readonly Window window;

        public Tasks Task
        {
            get => task;
            set
            {
                task = value;
                OnPropertyChanged(nameof(Task));
            }
        }

        // True when editing; controls Save button and TextBox read-only state
        public bool IsEditing
        {
            get => isEditing;
            private set
            {
                if (isEditing != value)
                {
                    isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                    OnPropertyChanged(nameof(IsReadOnly));
                    SaveCommand.RaiseCanExecuteChanged();
                    EditCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // TextBoxes are read-only when not editing
        public bool IsReadOnly => !IsEditing;

        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand MarkCompletedCommand { get; }

        // Parameterless ctor not used; we use the one below
        public TaskDetailsViewModel() { }

        // Constructor takes the selected task and the window instance
        public TaskDetailsViewModel(Tasks selectedTask, Window window)
        {
            Task = selectedTask;
            this.window = window;

            EditCommand = new RelayCommand(_ => BeginEdit(), _ => !IsEditing);
            SaveCommand = new RelayCommand(_ => Save(), _ => IsEditing);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => true);
            MarkCompletedCommand = new RelayCommand(_ => MarkCompleted(), _ => !Task.IsCompleted);
        }

        private void BeginEdit()
        {
            IsEditing = true;
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Task.Name))
            {
                MessageBox.Show(
                    "Title cannot be empty.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            using (var context = new AppDbContext())
            {
                var toUpdate = context.Tasks.Find(Task.Id);
                if (toUpdate != null)
                {
                    toUpdate.Name = Task.Name;
                    toUpdate.Description = Task.Description;
                    // Priority unchanged here
                    context.SaveChanges();
                }
            }
            IsEditing = false;
            MessageBox.Show(
                "Changes saved.",
                "Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void Delete()
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete this task?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result != MessageBoxResult.Yes)
                return;

            using (var context = new AppDbContext())
            {
                var toDelete = context.Tasks.Find(Task.Id);
                if (toDelete != null)
                {
                    context.Tasks.Remove(toDelete);
                    context.SaveChanges();
                }
            }
            // Close window; parent will refresh after ShowDialog
            window.Close();
        }

        private void MarkCompleted()
        {
            var result = MessageBox.Show(
                "Mark this task as completed?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result != MessageBoxResult.Yes)
                return;

            using (var context = new AppDbContext())
            {
                var toMark = context.Tasks.Find(Task.Id);
                if (toMark != null)
                {
                    toMark.IsCompleted = true;
                    toMark.CompletedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
            // Close window; parent will refresh after ShowDialog
            window.Close();
        }
       
    }
}
