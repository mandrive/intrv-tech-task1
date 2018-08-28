using System.IO;
using System.Web.Hosting;

namespace WebApi.Services
{
    public interface IDirectoryService
    {
        bool Exists(string path);
        DirectoryInfo CreateDirectory(string path);
        string MapPath(string path);
    }

    public class DirectoryService : IDirectoryService
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public string MapPath(string path)
        {
            return HostingEnvironment.MapPath(path);
        }
    }
}