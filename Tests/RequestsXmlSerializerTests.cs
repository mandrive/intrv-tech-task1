﻿using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DAL.Entities;
using WebApi.Services;
using Xunit;

namespace Tests.UnitTests
{
    public class RequestsXmlSerializerTests : IDisposable
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldSerializeAllRequestsFromDb_WhenDbHasAnyRequestsStored()
        {
            var memStreams = new List<MemoryStream>();
            var dbData = new List<Request>()
            {
                new Request { Index = 1, Name = "Test1", Visits = 1, Date = DateTime.Now },
                new Request { Index = 2, Name = "Test2", Visits = 2, Date = DateTime.Now },
            };
            var dbfactory = TestUtils.PrepareDbFactoryWithMockedRequestsSet(dbData);
            var mockDirService = TestUtils.SetupMockDirectoryService();
            var mockFileService = Substitute.For<IFileService>();
            
            mockFileService.Create(Arg.Any<string>()).Returns(x =>
            {
                memStreams.Add(new MemoryStream());
                return memStreams.Last();
            });

            var xmlSerializer = new RequestsXmlSerializer(dbfactory, mockDirService, mockFileService);

            await xmlSerializer.SerializeDataAsync(TestUtils.TempPath);

            Assert.NotEmpty(memStreams);
            Assert.Equal(dbData.Count, memStreams.Count);
            mockFileService.Received(2).Create(Arg.Any<string>());
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldSerializeAllRequestsFromDbAnCreateXmlFiles_WhenDbHasAnyRequestsStored()
        {
            var dbData = new List<Request>()
            {
                new Request { Index = 1, Name = "Test1", Visits = 1, Date = DateTime.Now },
                new Request { Index = 2, Name = "Test2", Visits = 2, Date = DateTime.Now.AddDays(1) },
            };

            var dbfactory = TestUtils.PrepareDbFactoryWithMockedRequestsSet(dbData);

            var xmlSerializer = new RequestsXmlSerializer(dbfactory, new DirectoryService(), new FileService());

            await xmlSerializer.SerializeDataAsync(TestUtils.TempPathXmlDir);
            var resultFiles = Directory.GetFiles(TestUtils.TempPathXmlDir);

            Assert.Equal(dbData.Count(), Directory.GetFiles(TestUtils.TempPathXmlDir).Count());
            Assert.True(resultFiles.All(x => dbData.Any(record => (record.Date.ToString("yyyy-MM-dd") + ".xml") == Path.GetFileName(x))));
        }

        public void Dispose()
        {
            if (Directory.Exists(TestUtils.TempPathXmlDir))
            {
                Directory.Delete(TestUtils.TempPathXmlDir, true);
            }
        }
    }
}
