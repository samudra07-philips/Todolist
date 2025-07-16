
using System.Linq;
using todolist_mvvm.Bussiness_Layer.Helpers;
using todolist_mvvm.Data;
using todolist_mvvm.model;

namespace Todolist.Services.Repositories
{
    public class UserRepository1 : IUserRepository
    {
        private readonly AppDbContext _ctx;

        public UserRepository1(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public bool ValidateUser(string username, string plainPassword)
        {
            var user = _ctx.Users
                .SingleOrDefault(u => u.Username == username);
            if (user == null) return false;
            return PasswordHasher.VerifyPassword(plainPassword, user.PasswordHash);
        }


        public void CreateUser(string username, string plainPassword)
        {
            var hash = PasswordHasher.HashPassword(plainPassword);
            _ctx.Users.Add(new User
            {
                Username = username,
                PasswordHash = hash
            });
            _ctx.SaveChanges();
        }

    }
}
