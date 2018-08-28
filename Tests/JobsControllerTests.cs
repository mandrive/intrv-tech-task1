using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Controllers;
using WebApi.Services;
using Xunit;

namespace Tests.UnitTests
{
    public class JobsControllerTests
    {
        private IRequestsXmlSerializer SetupMockRequestXmlSerializer(string path = null)
        {
            var mockXmlSerializer = Substitute.For<IRequestsXmlSerializer>();
            mockXmlSerializer.SerializeDataAsync(path ?? TestUtils.TempPath).Returns(Task.CompletedTask);

            return mockXmlSerializer;
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldReturnStatusCodeOk_WhenControllerCalled()
        {
            var mockDirectoryService = TestUtils.SetupMockDirectoryService();
            var mockFileService = Substitute.For<IFileService>();

            var mockXmlSerializer = SetupMockRequestXmlSerializer();

            var controller = new JobsController(mockXmlSerializer, mockDirectoryService);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            var result = await controller.Get();
            var actionResult = await result.ExecuteAsync(CancellationToken.None);

            Assert.Equal(HttpStatusCode.OK, actionResult.StatusCode);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldReturnStatusCodeInternalServerError_WhenExceptionOccured()
        {
            var mockDirectoryService = TestUtils.SetupMockDirectoryService();
            var mockFileService = Substitute.For<IFileService>();

            var mockXmlSerializer = SetupMockRequestXmlSerializer();
            mockXmlSerializer.SerializeDataAsync(TestUtils.TempPath).Returns(x => { throw new Exception("Exception"); });

            var controller = new JobsController(mockXmlSerializer, mockDirectoryService);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            var result = await controller.Get();
            var actionResult = await result.ExecuteAsync(CancellationToken.None);

            Assert.Equal(HttpStatusCode.InternalServerError, actionResult.StatusCode);
        }
    }
}
