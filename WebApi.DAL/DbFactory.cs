namespace WebApi.DAL
{
    public interface IDbFactory
    {
        DatabaseContext Create();
    }

    public class DbFactory : IDbFactory
    {
        public DatabaseContext Create()
        {
            return new DatabaseContext();
        }
    }
}
