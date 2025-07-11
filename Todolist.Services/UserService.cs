using System;
using System.Collections.Generic;
using System.Linq;
using todolist_mvvm.Bussiness_Layer.Helpers;
using todolist_mvvm.Data;
using todolist_mvvm.model;
namespace Todolist.Services
{
    public class UserService : IUserService
    {
        public bool Login(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Username == username);
                return user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash);
            }
        }

        public void Signup(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                var hash = PasswordHasher.HashPassword(password);
                db.Users.Add(new User { Username = username, PasswordHash = hash });
                db.SaveChanges();
            }
        }
    }
}
