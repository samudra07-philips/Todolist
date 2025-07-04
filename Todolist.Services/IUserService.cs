using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Todolist.Services
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract] bool Login(string username, string password);
        [OperationContract] void Signup(string username, string password);
    }
}
