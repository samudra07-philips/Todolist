using System.Data.Entity;
using System.Threading.Tasks;

namespace Todolist.Services.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("TodolistConnection") { }

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
