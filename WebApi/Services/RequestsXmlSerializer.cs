using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebApi.DAL;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IRequestsXmlSerializer
    {
        Task SerializeDataAsync(string destFolder);
    }

    public class RequestsXmlSerializer : IRequestsXmlSerializer
    {
        private readonly IDbFactory dbFactory;
        private readonly IDirectoryService directory;
        private readonly IFileService file;

        public RequestsXmlSerializer(IDbFactory dbFactory, IDirectoryService directory, IFileService file)
        {
            this.dbFactory = dbFactory;
            this.directory = directory;
            this.file = file;
        }

        public Task SerializeDataAsync(string destFolder)
        {
            return Task.Run(() =>
            {
                using (var db = dbFactory.Create())
                {
                    foreach (var request in db.Requests)
                    {
                        var fileName = request.Date.ToString("yyyy-MM-dd") + ".xml";

                        if (!directory.Exists(destFolder))
                        {
                            directory.CreateDirectory(destFolder);
                        }

                        var fullPath = Path.Combine(destFolder, fileName);

                        if (file.Exists(fullPath))
                        {
                            file.Delete(fullPath);
                        }

                        using (var stream = file.Create(fullPath))
                        {
                            var xmlModel = new XmlRequestModel
                            {
                                Index = request.Index,
                                Content = new XmlRequestContentModel
                                {
                                    Date = request.Date,
                                    Name = request.Name,
                                    Visits = request.Visits
                                }
                            };

                            var serializer = new XmlSerializer(typeof(XmlRequestModel));
                            serializer.Serialize(stream, xmlModel);
                        }
                    }
                }
            });
            
        }
    }
}