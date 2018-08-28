using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
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
            var data = new List<Request>();
            var models = new List<RequestModel>()
            {
                new RequestModel()
                {
                    Index = 1,
                    Visits = 1,
                    Name = "test",
                    Date = DateTime.Now
                }
            };
            
            var mockDbFactory = TestUtils.PrepareDbFactoryWithMockedRequestsSet(data);
            
            var controller = new DataController(mockDbFactory);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            //Act
            var result = await controller.Post(models.ToArray());
            var actionResult = await result.ExecuteAsync(CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.Created, actionResult.StatusCode);
            Assert.Single(data);
        }

        [Fact]
        public async Task ShouldReturnStatusCodeInternalServerError_WhenExceptionOccured()
        {
            //Arrange
            var data = new List<Request>();
            var models = new List<RequestModel>()
            {
                new RequestModel()
                {
                    Index = 1,
                    Visits = 1,
                    Name = "test",
                    Date = DateTime.Now
                }
            };

            var mockDbFactory = TestUtils.PrepareDbFactoryWithMockedRequestsSet(data, true);

            var controller = new DataController(mockDbFactory);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            //Act
            var result = await controller.Post(models.ToArray());
            var actionResult = await result.ExecuteAsync(CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, actionResult.StatusCode);
            Assert.Empty(data);
        }
    }
}
