using System;
using System.Collections.Generic;
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
            // 1) Apply EF Migrations
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<AppDbContext, Todolist.Service.Migrations.Configuration>());

            using (var ctx = new AppDbContext())
                ctx.Database.Initialize(force: true);

            // 2) Setup Unity
            var container = new UnityContainer();
            UnityConfig.RegisterComponents(container);

            // 3) Host both services
            StartServiceHost<UserService, IUserService>(container, "http://localhost:8000/UserService");
            StartServiceHost<TaskService, ITaskService>(container, "http://localhost:8001/TaskService");
        }

        private void StartServiceHost<TService, TContract>(IUnityContainer container, string baseAddress)
        {
            var uri = new Uri(baseAddress);
            var host = new UnityServiceHost(container, typeof(TService), uri);

            var smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpGetUrl = uri
            };
            host.Description.Behaviors.Add(smb);

            host.AddServiceEndpoint(typeof(TContract), new BasicHttpBinding(), "");
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            host.Open();
            _hosts.Add(host);
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

        // Debugging via Console
        public void RunAsConsole()
        {
            OnStart(null);
            Console.WriteLine("Service is running... Press Enter to stop.");
            Console.ReadLine();
            OnStop();
        }
    }
}
