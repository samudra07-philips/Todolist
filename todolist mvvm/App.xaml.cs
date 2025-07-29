using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Unity;
using Todolist.Mvvm;               // for WpfUnityConfig
using todolist_mvvm.viewmodel;
using Todolist.Services;     // for MainWindowViewModel

namespace todolist_mvvm
{
    public partial class App : Application
    {
        private Mutex _mutex;
        private const string MutexName = "Samudratodolist_Mutex";
        public static IUnityContainer container { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            // 1) Enforce single instance
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);
            if (!createdNew)
            {
                ActivateExistingWindow();
                Shutdown();
                return;
            }
            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                Exception ex = (Exception)args.ExceptionObject;
                MessageBox.Show("Unhandled exception: " + ex.Message);
            };

            DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show("Dispatcher exception: " + args.Exception.Message);
                args.Handled = true;
            };
            base.OnStartup(e);

            // 2) Build your Unity container via WpfUnityConfig
            container = WpfUnityConfig.RegisterComponents();

            // 3) Resolve your MainWindowViewModel (root VM)
            var mainVm = container.Resolve<MainWindowViewModel>();

            // Resolve the IUserService dependency from the Unity container
            var userService = container.Resolve<IUserService>();

            // Pass the resolved userService to the Mainwindow constructor
            var mainWindow = new Mainwindow(userService)
            {
                DataContext = mainVm
            };
            MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void ActivateExistingWindow()
        {
            try
            {
                var current = Process.GetCurrentProcess();
                var other = Process
                    .GetProcessesByName(current.ProcessName)
                    .FirstOrDefault(p => p.Id != current.Id);

                if (other == null) return;
                var h = other.MainWindowHandle;
                if (IsIconic(h)) ShowWindow(h, SW_RESTORE);
                SetForegroundWindow(h);
            }
            catch
            {
                // ignore any errors in activation
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            _mutex = null;
            base.OnExit(e);
        }

        #region Win32 P/Invoke for single-instance activation

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        #endregion
    }
}
