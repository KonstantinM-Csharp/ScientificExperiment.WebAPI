using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_CSV.Services;

namespace WebApi.Tests.Services
{
    public class FileServiceTests
    {
        [Fact]
        public async Task GetFileId_Returns_Correct_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new DataContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Files.Add(new DAL.Entities.File { Id = 3, FileName = "test.txt" });
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(options))
            {
                var service = new FileService(context);

                // Act
                var fileId = await service.GetFileId("test.txt");

                // Assert
                Assert.Equal(3, fileId);
            }
        }

        [Fact]
        public async Task GetFileName_Returns_Correct_Name()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using (var context = new DataContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Files.Add(new DAL.Entities.File { Id = 4, FileName = "test.txt" });
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(options))
            {
                var service = new FileService(context);

                // Act
                var fileName = await service.GetFileName(1);

                // Assert
                Assert.Equal("test.txt", fileName);
            }
        }
    }
}
