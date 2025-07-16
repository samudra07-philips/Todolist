using System.ServiceModel;
using Todolist.Services.Repositories;

namespace Todolist.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public bool Login(string username, string password)
        {
            return _repo.ValidateUser(username, password);
        }

        public void Signup(string username, string password)
        {
            _repo.CreateUser(username, password);
        }
    }
}
