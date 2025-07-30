using System;
using System.ServiceModel;
using Unity;
using Unity.Lifetime;
using Todolist.Services;
using todolist_mvvm.viewmodel;

namespace Todolist.Mvvm
{
    public static class WpfUnityConfig
    {
        public static IUnityContainer RegisterComponents()
        {
            var container = new UnityContainer();

            // --- 1) Register IUserService via ChannelFactory ---
            container.RegisterFactory<IUserService>(c =>
            {
                var binding = new BasicHttpBinding();
                var address = new EndpointAddress("http://localhost:8000/UserService");
                var factory = new ChannelFactory<IUserService>(binding, address);
                return factory.CreateChannel();
            }, new TransientLifetimeManager());

            // --- 2) Register ITaskService ---
            container.RegisterFactory<ITaskService>(c =>
            {
                var binding = new BasicHttpBinding();
                var address = new EndpointAddress("http://localhost:8001/TaskService");
                var factory = new ChannelFactory<ITaskService>(binding, address);
                return factory.CreateChannel();
            }, new TransientLifetimeManager());

            // --- 3) Register ViewModels ---
            container.RegisterType<LoginPageViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<SignUpViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<HistoryViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<TaskDetailsViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ToDoViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<AddTaskWindowViewModel>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}
