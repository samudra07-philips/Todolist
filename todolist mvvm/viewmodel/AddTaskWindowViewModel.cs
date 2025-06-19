using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using todolist_mvvm.Data;
using todolist_mvvm.model;

namespace todolist_mvvm.viewmodel
{
    public class AddTaskWindowViewModel : BaseViewModel
    {
        private string title;
        private string description;
        private string selectedPriority;
        private readonly Window window;

        public RelayCommand AddNewTask { get; }
        public List<string> Priorities { get; } =
            new List<string> { "Low", "Medium", "High", "Critical" };

        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                    AddNewTask.RaiseCanExecuteChanged();
                }
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                    AddNewTask.RaiseCanExecuteChanged();
                }
            }
        }
        public AddTaskWindowViewModel()
        {
            
        }

        public string SelectedPriority
        {
            get => selectedPriority;
            set
            {
                if (selectedPriority != value)
                {
                    selectedPriority = value;
                    OnPropertyChanged(nameof(SelectedPriority));
                    AddNewTask.RaiseCanExecuteChanged();
                }
            }
        }


        public AddTaskWindowViewModel(Window window)
        {
            this.window = window;
            AddNewTask = new RelayCommand(Execute, CanExecute);
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Title)
                && !string.IsNullOrEmpty(Description)
                && !string.IsNullOrEmpty(SelectedPriority);
        }

        private void Execute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(SelectedPriority))
            {
                MessageBox.Show(
                    "All fields are required. Please fill in all details.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            using (var context = new AppDbContext())
            {
                if (Enum.TryParse<TaskPriority>(SelectedPriority, out var parsedPriority))
                {
                    var task = new Tasks
                    {
                        Name = Title,
                        Description = Description,
                        Priority = parsedPriority,
                        UserId = 1 // Update this logic if dynamic user IDs are needed
                    };

                    context.Tasks.Add(task);
                    context.SaveChanges();

                   

                    if (window is IRefreshablePage refreshable)
                    {
                        refreshable.RefreshContent();
                    }

                    window?.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Invalid priority selected. Please select a valid priority.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }


    }
}
