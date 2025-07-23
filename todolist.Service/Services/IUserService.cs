using System.ServiceModel;

namespace Todolist.Services
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract] bool Login(string username, string password);
        [OperationContract] void Signup(string username, string password);
    }
}
