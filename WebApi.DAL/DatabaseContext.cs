using System;
using System.Data.Entity;
using WebApi.DAL.Entities;

namespace WebApi.DAL
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<Request> Requests { get; set; }
    }


    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext() : base("name=LocalDb")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DatabaseContext>());  
        }

        public virtual DbSet<Request> Requests { get; set; }
    }
}
