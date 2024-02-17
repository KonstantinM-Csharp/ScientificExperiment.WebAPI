using Microsoft.AspNetCore.Http;
using Moq;
using ScientificExperiment.WebAPI.Models;
using ScientificExperiment.WebAPI.Services;

namespace ScientificExperiment.Tests.Services
{
    public class ModelBuilderServiceTests
    {
        [Fact]
        public async Task GetFileModelFromData_Returns_Correct_FileModel()
        {
            // Arrange
            var service = new ModelBuilderService();

            var file = new Mock<IFormFile>();
            file.Setup(f => f.FileName).Returns("test.csv");
            var author = "John Doe";
            var creationDateTime = DateTime.Now;

            // Act
            var fileModel = await service.GetFileModelFromFileData(file.Object, author,creationDateTime);

            // Assert
            Assert.NotNull(fileModel);
            Assert.Equal("test.csv", fileModel.FileName);
            Assert.Equal("John Doe", fileModel.Author);
            Assert.Equal(creationDateTime, fileModel.CreationDateTime);
        }

        [Fact]
        public async Task ConvertCsvLine_Returns_Correct_ValueModel()
        {
            // Arrange
            var service = new ModelBuilderService();
            var csvLine = "2022-03-20_09-18-17;2000;1600,472";
            var delimiter = ";";

            // Act
            var valueModel = await service.ConvertCsvLine(csvLine, delimiter);

            // Assert
            Assert.NotNull(valueModel);
            Assert.Equal(DateTime.ParseExact("2022-03-20_09-18-17", "yyyy-MM-dd_HH-mm-ss", null), valueModel.StartDateTime);
            Assert.Equal(2000, valueModel.WorkTime);
            Assert.Equal(1600.472, valueModel.Indicator);
            Assert.Null(valueModel.Errors);
        }

        [Fact]
        public async Task CalculationResult_Returns_Correct_ResultModel()
        {
            // Arrange
            var service = new ModelBuilderService();
            var valueModels = new List<ValueModel>
            {
            new ValueModel { FileName = "test.csv", StartDateTime = DateTime.Parse("16.02.2024 10:10:00"), WorkTime = 60, Indicator = 3.14 },
            new ValueModel { FileName = "test.csv", StartDateTime = DateTime.Parse("16.02.2024 10:15:00"), WorkTime = 45, Indicator = 2.71 }
            // Add more ValueModels as needed
            };

            // Act
            var result = await service.CalculationResult(valueModels);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test.csv", result.FileName);
            Assert.Equal(TimeSpan.Parse("10:10:00"), result.FirstTime);
            Assert.Equal(TimeSpan.Parse("10:15:00"), result.LastTime);
            Assert.Equal(60, result.MaxTimeWork);
            Assert.Equal(45, result.MinTimeWork);
            Assert.Equal(52.5, result.AverageTimeWork);
            Assert.Equal(2.925, result.AverageIndicator);
            Assert.Equal(2.925, result.MedianByIndicator);
            Assert.Equal(3.14, result.MaxIndicator);
            Assert.Equal(2.71, result.MinIndicator);
            Assert.Equal(2, result.CountOfExperiments);
        }
    }
}
