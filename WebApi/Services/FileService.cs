using System.IO;

namespace WebApi.Services
{
    public interface IFileService
    {
        bool Exists(string path);
        void Delete(string path);
        Stream Create(string path);
    }

    public class FileService : IFileService
    {
        public Stream Create(string path)
        {
            return File.Create(path);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}