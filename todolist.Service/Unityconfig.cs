using Unity;
using Unity.Lifetime;
using Todolist.Services.Data;
using Todolist.Services.Repositories;

namespace Todolist.Services
{
    public static class UnityConfig
    {
        public static void RegisterComponents(IUnityContainer c)
        {
            c.RegisterType<AppDbContext>(new HierarchicalLifetimeManager());
            c.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            c.RegisterType<ITaskRepository, TaskRepository>(new HierarchicalLifetimeManager());
            c.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            c.RegisterType<ITaskService, TaskService>(new HierarchicalLifetimeManager());
        }
    }
}
