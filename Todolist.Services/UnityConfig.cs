using todolist_mvvm.Data;       // your EF DbContext namespace
using Todolist.Services.Repositories; // the new repository interfaces & impls
using Unity;
using Unity.Lifetime;
using Todolist.Services.Contracts;
namespace Todolist.Services
{
    public static class UnityConfig
    {
        /// <summary>
        /// Call once at host startup to register ALL dependencies.
        /// </summary>
        public static void RegisterComponents(IUnityContainer container)
        {
            // 1) EF DbContext: one per WCF operation
            container.RegisterType<AppDbContext>(
                new HierarchicalLifetimeManager());

            // 2) Repositories: one per operation, auto-disposed
            container.RegisterType<IUserRepository,UserRepository1>(
                new HierarchicalLifetimeManager());
            container.RegisterType<ITaskRepository, TaskRepository>(
                new HierarchicalLifetimeManager());

            // 3) WCF services themselves: one instance per call
            container.RegisterType<IUserService, UserService>(
                new HierarchicalLifetimeManager());
            container.RegisterType<ITaskService, TaskService>(
                new HierarchicalLifetimeManager());

            // …register any other services, validators, loggers here…
        }
    }
}
