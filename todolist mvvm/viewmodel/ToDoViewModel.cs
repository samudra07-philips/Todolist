using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using todolist_mvvm.view;

namespace todolist_mvvm.viewmodel
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
        public RelayCommand AddCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ToDoViewModel()
        {
            AddCommand = new RelayCommand(Execute,CanExecute);
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {

            if (parameter is Page page)
            {
                var hostWindow = Window.GetWindow(page);

                if (hostWindow != null)
                {
                    var addTaskWindow = new Addtaskwindow
                    {
                        Owner = hostWindow
                    };

                   
                    //addTaskWindow.Left = hostWindow.Left + (hostWindow.Width - addTaskWindow.Width) / 2;
                    //addTaskWindow.Top = hostWindow.Top + (hostWindow.Height - addTaskWindow.Height) / 2;

                    addTaskWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Host window not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
