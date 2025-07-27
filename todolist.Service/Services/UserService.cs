using System.ServiceModel;
using Todolist.Services.Repositories;

namespace Todolist.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public bool Login(string u, string p) => _repo.ValidateUser(u, p);
        public void Signup(string u, string p) => _repo.CreateUser(u, p);
    }
}
