using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using WebApi.DAL;
using WebApi.DAL.Entities;
using WebApi.Services;

namespace Tests.UnitTests
{
    public class TestUtils
    {
        public static string TempPath = Path.GetTempPath();

        public static IDbFactory PrepareDbFactoryWithMockedRequestsSet(IList<Request> data, bool dbCreatorShouldThrowException = false)
        {
            var mockDbFactory = Substitute.For<IDbFactory>();
            var mockContext = Substitute.For<DatabaseContext>();
            var mockSet = Substitute.For<DbSet<Request>, IQueryable<Request>, IDbAsyncEnumerable<Request>>()
                .SetupData(data);

            mockContext.Requests.Returns(mockSet);
            if (!dbCreatorShouldThrowException)
            {
                mockDbFactory.Create().Returns(mockContext);
            }
            else
            {
                mockDbFactory.Create().Returns(x => { throw new Exception("Exception"); });
            }

            return mockDbFactory;
        }



        public static IDirectoryService SetupMockDirectoryService(string path = null)
        {
            var mockDirectoryService = Substitute.For<IDirectoryService>();
            mockDirectoryService.MapPath(Arg.Any<string>()).ReturnsForAnyArgs(path ?? TempPath);
            mockDirectoryService.Exists(Arg.Any<string>()).Returns(true);

            return mockDirectoryService;
        }

    }
}
