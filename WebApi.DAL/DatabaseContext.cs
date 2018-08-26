using System.Data.Entity;
using WebApi.DAL.Entities;

namespace WebApi.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=LocalDb")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DatabaseContext>());  
        }

        public virtual DbSet<Request> Requests { get; set; }
    }
}
