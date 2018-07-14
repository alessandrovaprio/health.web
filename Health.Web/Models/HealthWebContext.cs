using System;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace Health.Web.Models
{
    public class HealthWebContext : DbContext
    {
        /*public HealthWebContext(DbContextOptions<HealthWebContext> options): base(options)
        {
            
        }*/

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=health_web;user=root;password=Vprlsn90");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });

            /* modelBuilder.Entity<Book>(entity =>
             {
                 entity.HasKey(e => e.ISBN);
                 entity.Property(e => e.Title).IsRequired();
                 entity.HasOne(d => d.Publisher)
                   .WithMany(p => p.Books);
             });*/
        }
    }
}
