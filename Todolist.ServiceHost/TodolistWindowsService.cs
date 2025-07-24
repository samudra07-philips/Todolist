using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceProcess;
using Unity;
using Unity.Wcf;
using Todolist.Services;
using Todolist.Services.Data;          
using System.Data.Entity;            

namespace Todolist.ServiceHost
{
    public class TodolistWindowsService : ServiceBase
    {
        private readonly List<ServiceHostBase> _hosts = new List<ServiceHostBase>();

        protected override void OnStart(string[] args)
        {
            // 1) Apply EF Migrations at startup
            Database.SetInitializer(
              new MigrateDatabaseToLatestVersion<AppDbContext,
                  Todolist.Service.Migrations.Configuration>());
            using (var ctx = new AppDbContext())
                ctx.Database.Initialize(force: true);

            // 2) Build Unity container and register types
            var container = new UnityContainer();
            UnityConfig.RegisterComponents(container);

            // 3) Self‑host the WCF services
            var userHost = new UnityServiceHost(container, typeof(UserService), new Uri("http://localhost:8000/UserService"));
            userHost.AddServiceEndpoint(typeof(IUserService), new BasicHttpBinding(), "");
            userHost.Open();
            _hosts.Add(userHost);

            var taskHost = new UnityServiceHost(container, typeof(TaskService), new Uri("http://localhost:8001/TaskService"));
            taskHost.AddServiceEndpoint(typeof(ITaskService), new BasicHttpBinding(), "");
            taskHost.Open();
            _hosts.Add(taskHost);

        }

        protected override void OnStop()
        {
            foreach (var host in _hosts)
            {
                try { host.Close(); }
                catch { host.Abort(); }
            }
            _hosts.Clear();
        }

        // For console debugging
        public void RunAsConsole()
        {
            OnStart(null);
            Console.WriteLine("Service running. Press Enter to stop.");
            Console.ReadLine();
            OnStop();
        }
    }
}
