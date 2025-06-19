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
        public AppDbContext()
            : base("name=DefaultConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique(); 

            
            modelBuilder.Entity<Tasks>().Property(t => t.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Tasks>().Property(t => t.Priority).IsRequired();

            modelBuilder.Entity<Tasks>().Property(t => t.Description).HasMaxLength(500);

            
            modelBuilder
                .Entity<Tasks>()
                .HasRequired(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}
