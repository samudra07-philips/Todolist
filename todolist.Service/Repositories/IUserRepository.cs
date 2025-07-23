namespace Todolist.Services.Repositories
{
    public interface IUserRepository
    {
        bool ValidateUser(string username, string plainPassword);
        void CreateUser(string username, string plainPassword);
    }
}
