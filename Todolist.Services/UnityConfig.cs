using Microsoft.Practices.Unity;
using todolist_mvvm.Data;
using Unity;
using Unity.Lifetime;
using Unity.Wcf;

namespace Todolist.Services
{
    public static class UnityConfig
    {
        public static void RegisterComponents(IUnityContainer container)
        {
           
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ITaskService, TaskService>();

        }

        public static IUnityContainer GetContainer()
        {
            var container = new UnityContainer();
            RegisterComponents(container);
            return container;
        }
    }
}

}
