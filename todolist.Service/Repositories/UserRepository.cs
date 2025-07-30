using System.Linq;
using Todolist.Services.Data;
using Todolist.Services.Helpers;

namespace Todolist.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _ctx;
        public UserRepository(AppDbContext ctx) => _ctx = ctx;

        public bool ValidateUser(string username, string plainPassword)
        {
            var user = _ctx.Users.SingleOrDefault(u => u.Username == username);
            return user != null && PasswordHasher.VerifyPassword(plainPassword, user.PasswordHash);
        }

        public void CreateUser(string username, string plainPassword)
        {
            var hash = PasswordHasher.HashPassword(plainPassword);
            _ctx.Users.Add(new User { Username = username, PasswordHash = hash });
            _ctx.SaveChanges();
        }
        public User GetByUsername(string username)
        {
            return _ctx.Users.SingleOrDefault(u => u.Username == username);
        }
    }
}
