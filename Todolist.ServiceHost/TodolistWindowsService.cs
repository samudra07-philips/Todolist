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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Todolist.Service.Migrations.Configuration>());
            using (var ctx = new AppDbContext())
                ctx.Database.Initialize(force: true);

            // 2) Build Unity container
            var container = new UnityContainer();
            UnityConfig.RegisterComponents(container);

            // Start both service hosts
            StartServiceHost<UserService, IUserService>("UserService", 8000, container);
            StartServiceHost<TaskService, ITaskService>("TaskService", 8001, container);
        }

        private void StartServiceHost<TService, TContract>(string path, int port, IUnityContainer container)
            where TService : class
        {
            var baseAddress = new Uri($"http://localhost:{port}/{path}");
            var host = new UnityServiceHost(container, typeof(TService), baseAddress);

            // Ensure metadata behavior is present
            var smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb == null)
            {
                smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpGetUrl = baseAddress
                };
                host.Description.Behaviors.Add(smb);
            }

            // Add main service endpoint
            host.AddServiceEndpoint(typeof(TContract), new BasicHttpBinding(), "");

            // Add MEX endpoint
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
                                    MetadataExchangeBindings.CreateMexHttpBinding(),
                                    "mex");

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
