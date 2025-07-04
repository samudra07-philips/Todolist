﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using todolist_mvvm.view;
using todolist_mvvm.viewmodel;

namespace todolist_mvvm
{
    /// <summary>
    /// Interaction logic for mainwindow.xaml
    /// </summary>
    public partial class Mainwindow : Window
    {
        public Mainwindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(MainFrame);
            MainFrame.Navigate(new LoginPage());
        }
    }

}
