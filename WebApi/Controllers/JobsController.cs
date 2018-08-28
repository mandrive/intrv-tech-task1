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
        private readonly IDirectoryService directoryService;

        public JobsController(IRequestsXmlSerializer serializer, IDirectoryService directoryService)
        {
            this.serializer = serializer;
            this.directoryService = directoryService;
        }

        [HttpGet]
        [Route("saveFiles")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var destPath = directoryService.MapPath("~/App_Data");
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