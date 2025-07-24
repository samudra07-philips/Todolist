using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;            
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using Todolist.Services;
using Todolist.Services.Data;          
using Unity;
using Unity.Wcf;

namespace Todolist.ServiceHost
{
    public class TodolistWindowsService : ServiceBase
    {
        private readonly List<ServiceHostBase> _hosts = new List<ServiceHostBase>();

        protected override void OnStart(string[] args)
        {
            // 1) Apply EF Migrations at startup
            Database.SetInitializer(
              new MigrateDatabaseToLatestVersion<
                  AppDbContext,
                  Todolist.Service.Migrations.Configuration>());
            using (var ctx = new AppDbContext())
                ctx.Database.Initialize(force: true);

            // 2) Build Unity container
            var container = new UnityContainer();
            UnityConfig.RegisterComponents(container);

            // For UserService
            var userBase = new Uri("http://localhost:8000/UserService");
            var userHost = new UnityServiceHost(container, typeof(UserService), userBase);

            // Add metadata behavior
            var smb1 = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpGetUrl = userBase
            };
            userHost.Description.Behaviors.Add(smb1);

            // Add service endpoint
            userHost.AddServiceEndpoint(typeof(IUserService),
                                        new BasicHttpBinding(),
                                        "");

            // Add MEX endpoint
            userHost.AddServiceEndpoint(typeof(IMetadataExchange),
                                        MetadataExchangeBindings.CreateMexHttpBinding(),
                                        "mex");

            userHost.Open();
            _hosts.Add(userHost);

            // For TaskService
            var taskBase = new Uri("http://localhost:8001/TaskService");
            var taskHost = new UnityServiceHost(container, typeof(TaskService), taskBase);

            // Add metadata behavior
            var smb2 = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpGetUrl = taskBase
            };
            taskHost.Description.Behaviors.Add(smb2);

            // Add service endpoint
            taskHost.AddServiceEndpoint(typeof(ITaskService),
                                        new BasicHttpBinding(),
                                        "");

            // Add MEX endpoint
            taskHost.AddServiceEndpoint(typeof(IMetadataExchange),
                                        MetadataExchangeBindings.CreateMexHttpBinding(),
                                        "mex");

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



    
