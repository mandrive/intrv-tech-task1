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

        public RequestsXmlSerializer(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public Task SerializeDataAsync(string destFolder)
        {
            return Task.Run(() =>
            {
                using (var db = dbFactory.Create())
                {
                    foreach (var request in db.Requests)
                    {
                        var pathCombined = Path.Combine(destFolder, "xml");
                        var fileName = request.Date.ToString("yyyy-MM-dd") + ".xml";

                        if (!Directory.Exists(pathCombined))
                        {
                            Directory.CreateDirectory(pathCombined);
                        }

                        var fullPath = Path.Combine(pathCombined, fileName);

                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }

                        using (var stream = File.Create(fullPath))
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