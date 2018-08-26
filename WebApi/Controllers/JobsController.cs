using System;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Services;

namespace WebApi.Controllers
{
    [RoutePrefix("api/jobs")]
    public class JobsController : ApiController
    {
        private readonly IRequestsXmlSerializer serializer;

        public JobsController(IRequestsXmlSerializer serializer)
        {
            this.serializer = serializer;
        }

        [HttpGet]
        [Route("saveFiles")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var destPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
                await serializer.SerializeDataAsync(destPath);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}