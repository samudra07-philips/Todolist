

namespace Todolist.Services.Repositories
{
    public interface IUserRepository
    {
        bool ValidateUser(string username, string password);
        void CreateUser(string username, string password);
    }
}

