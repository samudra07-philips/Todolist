using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace todolist_mvvm.viewmodel
{
    public class AddTaskWindowViewModel : BaseViewModel
    {
       
        private string title;
        private string description;
        private readonly Window window;

        public RelayCommand AddNewTask { get; }
        public AddTaskWindowViewModel()
        {
            
        }
        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Description);
        }

        private void Execute(object parameter)
        {
            if (window is IRefreshablePage refreshable)
            {
                refreshable.RefreshContent();
            }

            window?.Close();
        }

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

        public AddTaskWindowViewModel(Window window)
        {
            this.window = window;
            AddNewTask = new RelayCommand(Execute, CanExecute);
        }

        
    }
}
