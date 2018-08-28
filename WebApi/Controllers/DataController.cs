using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.DAL;
using WebApi.DAL.Entities;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        private readonly IDbFactory dbFactory;

        public DataController(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] RequestModel[] models)
        {
            try
            {
                using (var db = dbFactory.Create())
                {
                    foreach (var model in models)
                    {
                        db.Requests.Add(new Request
                        {
                            Index = model.Index,
                            Visits = model.Visits,
                            Name = model.Name,
                            Date = model.Date
                        });
                    }

                    await db.SaveChangesAsync();
                }

                return StatusCode(System.Net.HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}