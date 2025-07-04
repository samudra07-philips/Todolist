using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Todolist.Services;
using Unity.Wcf;

namespace Todolist.Services
{
    public class CustomUnityServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var unityConfig = new UnityConfig();
            var container = unityConfig.GetContainer();
            return new UnityServiceHost(container, serviceType, baseAddresses);
        }
    }
}

