using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Todolist.Services;
using Unity.Wcf;

public class UnityServiceHostFactory : ServiceHostFactory
{
    protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
    {
        var container = UnityConfig.GetContainer();
        return new UnityServiceHost(container, serviceType, baseAddresses);
    }
}

