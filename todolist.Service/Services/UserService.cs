using System.ServiceModel;
using Todolist.Services.Repositories;


namespace Todolist.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public int Login(string username, string password)
        {
            // 1) Validate credentials
            if (!_repo.ValidateUser(username, password))
                return 0;

            // 2) Fetch the user to get its Id
            var user = _repo.GetByUsername(username);
            return user?.Id ?? 0;
        }
        public void Signup(string u, string p) => _repo.CreateUser(u, p);
    }
}
