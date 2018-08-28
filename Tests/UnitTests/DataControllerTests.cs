using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using NSubstitute;
using WebApi.Controllers;
using WebApi.DAL;
using WebApi.DAL.Entities;
using WebApi.Models;
using Xunit;

namespace Tests.UnitTests
{
    public class DataControllerTests
    {
        [Fact]
        public async Task ShouldReturnStatusCodeCreated_WhenSaveToDbPasses()
        {
            //Arrange
            var mockDbFactory = Substitute.For<IDbFactory>();
            var mockContext = Substitute.For<DatabaseContext>();
            var data = new List<Request>();
            var mockSet = Substitute.For<DbSet<Request>, IQueryable<Request>, IDbAsyncEnumerable<Request>>()
                .SetupData(data);

            mockContext.Requests.Returns(mockSet);
            mockDbFactory.Create().Returns(mockContext);
            
            var controller = new DataController(mockDbFactory);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            //Act
            var result = await controller.Post((new List<RequestModel>()).ToArray());
            var actionResult = await result.ExecuteAsync(CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.Created, actionResult.StatusCode);
        }
    }
}
