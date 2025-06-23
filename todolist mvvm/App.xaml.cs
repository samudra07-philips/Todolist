using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using todolist_mvvm;
namespace todolist_mvvm
{
   
    public partial class App : Application
    {
        private Mutex _mutex;
        private const string MutexName = "Samudratodolist_Mutex"; 

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);
            if (!createdNew)
            {
                ActivateExistingWindow();
                Shutdown();
                return;
            }

            base.OnStartup(e);
            // normal startup: show MainWindow etc.
            var main = new Mainwindow();
            MainWindow = main;
            main.Show();
        }

        private void ActivateExistingWindow()
        {
            try
            {
                var currentProc = Process.GetCurrentProcess();
                // Find other process with same name
                var other = Process.GetProcessesByName(currentProc.ProcessName)
                                   .FirstOrDefault(p => p.Id != currentProc.Id);
                if (other != null)
                {
                    var handle = other.MainWindowHandle;
                    if (handle != IntPtr.Zero)
                    {
                        // If minimized, restore
                        if (IsIconic(handle))
                            ShowWindow(handle, SW_RESTORE);
                        // Bring to front
                        SetForegroundWindow(handle);
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            _mutex = null;
            base.OnExit(e);
        }

        // P/Invoke for window activation:
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }

}
