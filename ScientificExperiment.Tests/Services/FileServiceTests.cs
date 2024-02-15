using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Moq;
using ScientificExperiment.Tests.Database;
using ScientificExperiment.WebAPI.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApi_CSV.Services;

namespace WebApi.Tests.Services
{
    public class FileServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public FileServiceTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public async Task GetFileId_Returns_Correct_Id()
        {

            var files = new List<DAL.Entities.File>
             {
                new DAL.Entities.File { Id = 1, FileName = "file1.csv" },
                new DAL.Entities.File { Id = 2, FileName = "file2.csv" }
             };
            _fixture.Context.Files.AddRange(files);
            _fixture.Context.SaveChanges(); // Сохраняем изменения в базе данных

            var fileService = new FileService(_fixture.Context);

            // Act
            var fileId = await fileService.GetFileId("file1.csv");

            // Assert
            Assert.Equal(1, fileId);
        }

        [Fact]
        public async Task GetFileName_Returns_Correct_FileName()
        {
            var files = new List<DAL.Entities.File>
            {
            new DAL.Entities.File { Id = 3, FileName = "file3.csv" },
            new DAL.Entities.File { Id = 4, FileName = "file4.csv" }
            };

            _fixture.Context.Files.AddRange(files);
            _fixture.Context.SaveChanges();

            var fileService = new FileService(_fixture.Context);

            // Act
            var fileName = await fileService.GetFileName(3);

            // Assert
            Assert.Equal("file3.csv", fileName);
        }
    }
}
