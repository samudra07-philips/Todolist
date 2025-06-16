using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace todolist_mvvm.viewmodel
{
    public class MainWindowViewModel
    {
        public ICommand BackCommand { get; }
        public ICommand RefreshCommand { get; }

        private Frame _mainFrame;

        public MainWindowViewModel() { }

        public MainWindowViewModel(Frame mainFrame)
        {
            _mainFrame = mainFrame;
            BackCommand = new RelayCommand(Back, CanGoBack);
            RefreshCommand = new RelayCommand(Refresh, CanRefresh);
        }

        private bool CanGoBack(object parameter)
        {
            return _mainFrame.CanGoBack;
        }

        private void Back(object parameter)
        {
            _mainFrame.GoBack();
        }

        private bool CanRefresh(object parameter)
        {
            return _mainFrame.Content is IRefreshablePage;
        }

        private void Refresh(object parameter)
        {
            if (_mainFrame.Content is IRefreshablePage refreshablePage)
            {
                refreshablePage.RefreshContent();
            }
        }
    }
}
