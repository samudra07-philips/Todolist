using System;
using System.ServiceProcess;

namespace Todolist.ServiceHost
{
    static class Program
    {
        static void Main(string[] args)
        {
            var service = new TodolistWindowsService();

            if (Environment.UserInteractive)
            {
              
                service.RunAsConsole();
            }
            else
            {
               
                ServiceBase.Run(service);
            }
        }
    }
}
