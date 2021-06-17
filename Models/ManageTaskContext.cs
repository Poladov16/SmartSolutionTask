using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSTask.Models
{
    public class ManageTaskContext : IdentityDbContext<User,UserRole,int>
    {
        public ManageTaskContext(DbContextOptions<ManageTaskContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            //modelBuilder.Entity<User>()
            //    .HasOne(x => x.Task)
            //    .WithMany(x=>x.AssignedUsers)
            //    .HasForeignKey(x=>x.TaskId);

            //modelBuilder.Entity<ManageTask>().HasMany(x => x.AssignedUsers);
        }

        public DbSet<ManageTask> ManageTasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
