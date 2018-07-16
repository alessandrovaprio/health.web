using Health.Web.Models;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;


namespace Health.Web.Models
{
    public class HealthDataContext : DataConnection
    {
        /*public HealthWebContext(DbContextOptions<HealthWebContext> options): base(options)
        {
            
        }*/

        public HealthDataContext(IDataProvider dataProvider, string connectionString)
            : base(dataProvider, connectionString)
        { }

        public ITable<User> Users => GetTable<User>();

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
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

             modelBuilder.Entity<Book>(entity =>
             {
                 entity.HasKey(e => e.ISBN);
                 entity.Property(e => e.Title).IsRequired();
                 entity.HasOne(d => d.Publisher)
                   .WithMany(p => p.Books);
             });
        }*/
    }
}
