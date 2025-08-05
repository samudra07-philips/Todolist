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
            try
            {
                // Set EF initializer to migrate or create DB/tables as needed
                Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<AppDbContext, Todolist.Service.Migrations.Configuration>());

                using (var ctx = new AppDbContext())
                {
                    ctx.Database.Initialize(force: true);
                }

                var container = new UnityContainer();
                UnityConfig.RegisterComponents(container);

                StartServiceHost<UserService, IUserService>(container, "http://localhost:8000/UserService");
                StartServiceHost<TaskService, ITaskService>(container, "http://localhost:8001/TaskService");
            }
            catch (Exception ex)
            {
                // Log to Event Viewer or a file
                System.Diagnostics.EventLog.WriteEntry(
                    "TodolistWindowsService",
                    $"Database initialization failed: {ex.Message}\n{ex.StackTrace}",
                    System.Diagnostics.EventLogEntryType.Error);

                throw; // Optionally rethrow to stop the service
            }
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

        public void RunAsConsole()
        {
            try
            {
                OnStart(null);
                Console.WriteLine("Service running. Press Ctrl + C or Enter to stop.");

                // This will keep the service alive indefinitely unless Enter is pressed
                while (true)
                {
                    var input = Console.ReadLine();
                    if (input != null)
                        break;
                }

                OnStop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Service crashed with error:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine(); // Pause so you can read the error
            }
        }


    }
}
