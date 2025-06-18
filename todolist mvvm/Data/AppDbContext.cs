using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using todolist_mvvm.model;
namespace todolist_mvvm.Data
{
    public class AppDbContext : DbContext
    {
        // The base constructor will read "DefaultConnection" from App.config
        public AppDbContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Ensure Username is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique(); // EF6.2+ supports HasIndex
        }
    }
}
