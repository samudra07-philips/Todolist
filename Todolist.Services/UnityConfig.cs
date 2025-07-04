using Microsoft.Practices.Unity;
using todolist_mvvm.Data;
using Unity;
using Unity.Lifetime;
using Unity.Wcf;

namespace Todolist.Services
{
    public class UnityConfig
    {
        private IUnityContainer _container;

        public UnityConfig()
        {
            _container = new UnityContainer();
            RegisterComponents(_container);
        }

        private void RegisterComponents(IUnityContainer container)
        {
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ITaskService, TaskService>();
        }

        public IUnityContainer GetContainer()
        {
            return _container;
        }
    }
}


